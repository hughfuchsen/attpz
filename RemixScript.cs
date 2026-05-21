using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RemixScript : MonoBehaviour
{
    [Header("Characters")]
    [SerializeField] GameObject pony;
    [SerializeField] GameObject pig;
    [SerializeField] GameObject person;
    [SerializeField] GameObject puppy;
    [SerializeField] GameObject chicken;

    public CameraMovement cam;

    public GameObject chosenPlayer;

    List<GameObject> allCharacters = new List<GameObject>();

    public Vector3 initialPositionPlayer = new Vector3(-40,22,0);
    public Vector3 initialPosition2 = new Vector3(-143,73,0);
    public Vector3 initialPosition3 = new Vector3(65,-31,0);
    // public Vector3 initialPosition4 = new Vector3(65,-31,0);
    // public Vector3 initialPosition5 = new Vector3(65,-31,0);

    public TMP_Text remixText;


    [Header("Soundtrack")]
    int currentTrack= 0;
    public AudioSource[] musicTracks;



    void Start()
    {

        allCharacters.Add(pony);
        allCharacters.Add(pig);
        allCharacters.Add(person);
        // allCharacters.Add(puppy);
        // allCharacters.Add(chicken);
        
        // choose random player
        chosenPlayer =
            allCharacters[Random.Range(0, allCharacters.Count)];
        
        ShowRoundUpText();
       
        // allCharacters[Random.Range(0, 3)];

        // melody.loop = true;
        // melody.Play();

        double startTime = AudioSettings.dspTime + 0.2;

        foreach (AudioSource track in musicTracks)
        {
            track.volume = 0;
            track.PlayScheduled(startTime);
        }

        musicTracks[0].volume = 1;


        RemixAndStartGame();
        GoToAnchorPoint();
    }

    bool isRemixInitiated = false;
    bool remixBusy = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !remixBusy)
        {
            isRemixInitiated = !isRemixInitiated;

            if (isRemixInitiated)
            {
                remixBusy = true;

                SetAllToNpc();

                GoToAnchorPoint();
                remixText.text = "";           
                
            }
            else
            {
                RemixAndStartGame();

                int randomTrack = Random.Range(0, 4);

                SwitchTrack(randomTrack);

                ShowRoundUpText();
            }
        }

        // unlock once everybody settles
        if (remixBusy && CharactersSettled())
        {
            remixBusy = false;
            ShowRemixText();
        }
    }   


    bool CharactersSettled()
    {
        foreach (GameObject character in allCharacters)
        {
            CharacterMovement cm =
                character.GetComponent<CharacterMovement>();

            if (cm.change.magnitude > 0.01f)
            {
                return false;
            }
        }

        return true;
    }
    void RemixAndStartGame()
    {
         // choose random player
        chosenPlayer =
            allCharacters[Random.Range(0, allCharacters.Count)];

        person.GetComponent<CharacterCustomization>().UpdateRandom();
        ApplySharedHueShift(GetHueRemixGroup());
        
        // reset everybody first
        foreach (GameObject character in allCharacters)
        {
            AssignNPC(character);
        }
        ApplyTeleportPositions();

        if (cam != null)
        {
            cam.target = chosenPlayer.transform;
        }

        AssignPlayer(chosenPlayer);

        // Debug.Log("PLAYER IS: " + chosenPlayer.name);

        // reset think loop
        foreach (GameObject character in allCharacters)
        {
            NPCPathFollower pf = character.GetComponent<NPCPathFollower>();
            CharacterMovement cm = character.GetComponent<CharacterMovement>();
            cm.movementSpeed = 65;
            cm.activeCollisions.Clear();
            
            if(pf.goCoroutine != null)
            StopCoroutine(pf.goCoroutine);

            pf.goCoroutine = pf.StartCoroutine(pf.ThinkLoop());
        }

    }
    void SetAllToNpc()
    {
        // reset everybody first
        foreach (GameObject character in allCharacters)
        {
            AssignNPC(character);
        }

        if (cam != null)
        {
            cam.target = chosenPlayer.transform;
        }
        
    }

    void AssignPlayer(GameObject character)
    {
        character.tag = "Player";

        Rigidbody2D rb = character.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        SetLayerRecursively(
            character,
            LayerMask.NameToLayer("Player")
        );

        NPCPathFollower ai =
            character.GetComponent<NPCPathFollower>();

        if (ai != null)
        {
            ai.enabled = false;
        }
    }

    void AssignNPC(GameObject character)
    {
        character.tag = "NPC";

        Rigidbody2D rb = character.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        SetLayerRecursively(
            character,
            LayerMask.NameToLayer("Default")
        );

        NPCPathFollower ai =
            character.GetComponent<NPCPathFollower>();

        if (ai != null)
        {
            ai.enabled = true;
        }
    }

    public void GoToAnchorPoint()
    {
        List<Vector3> otherPositions = new List<Vector3>
        {
            initialPosition2,
            initialPosition3
            // initialPosition4,
            // initialPosition5
        };

        // shuffle positions
        for (int i = 0; i < otherPositions.Count; i++)
        {
            Vector3 temp = otherPositions[i];
            int rand = Random.Range(i, otherPositions.Count);
            otherPositions[i] = otherPositions[rand];
            otherPositions[rand] = temp;
        }

        int index = 0;

        foreach (GameObject character in allCharacters)
        {
            Vector3 homePos;

            if (character == chosenPlayer)
            {
                homePos = initialPositionPlayer;
            }
            else
            {
                homePos = otherPositions[index];
                index++;
            }

            NPCPathFollower ai = character.GetComponent<NPCPathFollower>();
            ai.GoToInitialPosition(homePos);
        }
    }
        // if(cm.currentArea.areaType == AreaType.train)
        // switch (cm.currentArea.areaType)
        // {
        //     case AreaType.train:
        //         foreach (GameObject character in allCharacters)
        //         {
        //             NPCPathFollower ai = character.GetComponent<NPCPathFollower>();
        //             ai.GoToInitialPosition();
        //         }    
        //     break;

        //     case AreaType.platform:
        //         areaType = AreaType.platform;
        //     break;
        // }
    // }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
    public void SwitchTrack(int newTrack)
    {
        musicTracks[currentTrack].volume = 0;

        musicTracks[newTrack].volume = 1;

        currentTrack = newTrack;
    }


    void ApplyTeleportPositions()
    {
        
        List<Vector3> otherPositions = new List<Vector3>
        {
            initialPosition2,
            initialPosition3
            // initialPosition4,
            // initialPosition5
        };

         // shuffle
        for (int i = 0; i < otherPositions.Count; i++)
        {
            int rand = Random.Range(i, otherPositions.Count);

            Vector3 temp = otherPositions[i];
            otherPositions[i] = otherPositions[rand];
            otherPositions[rand] = temp;
        }

        int index = 0;

        foreach (GameObject character in allCharacters)
        {
            CharacterMovement cm = character.GetComponent<CharacterMovement>();

            if (character == chosenPlayer)
            {
                character.transform.position =
                    ApplyAreaOffset(initialPositionPlayer, cm);
            }
            else
            {
                character.transform.position =
                    ApplyAreaOffset(otherPositions[index], cm);

                index++;
            }

            cm.change = Vector3.zero;
        }
    }

    Vector3 ApplyAreaOffset(Vector3 pos, CharacterMovement cm)
    {
        return cm.currentArea.areaType == AreaType.platform
            ? pos
            : pos + new Vector3(0, -60, 0);
    }

    List<GameObject> GetHueRemixGroup()
    {
        List<GameObject> group = new List<GameObject>();

        foreach (GameObject character in allCharacters)
        {
            CharacterAnimation ca = character.GetComponent<CharacterAnimation>();

            if (ca.characterType == CharacterAnimation.CharacterType.Pig ||
                ca.characterType == CharacterAnimation.CharacterType.Pony 
                // ||
                // ca.characterType == CharacterAnimation.CharacterType.Chicken ||
                // ca.characterType == CharacterAnimation.CharacterType.Dog
                )
            {
                group.Add(character);
            }
        }

        return group;
    }

    void ApplySharedHueShift(List<GameObject> characters)
    {
        float hueShift = Random.Range(0f, 1f); // full hue wheel offset

        foreach (GameObject character in characters)
        {
            SpriteRenderer[] sprites =
                character.GetComponentsInChildren<SpriteRenderer>();

            foreach (SpriteRenderer sr in sprites)
            {
                Color col = sr.color;

                float h, s, v;
                Color.RGBToHSV(col, out h, out s, out v);

                h = Mathf.Repeat(h + hueShift, 1f);

                sr.color = Color.HSVToRGB(h, s, v);
            }
        }
    }

    void ShowRoundUpText()
    {
        remixText.text = "Round Up [R]";
    }

    void ShowRemixText()
    {
        remixText.text = "Remix [R]";
    }
}