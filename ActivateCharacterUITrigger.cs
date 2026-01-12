using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCharacterUITrigger : MonoBehaviour
{
    CharacterAnimation myCharacterAnimation;
    public GameObject browseChrctrObj;
    public GameObject createChrctrObj;

    [SerializeField] GameObject backdrop;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject platform;
    [SerializeField] GameObject thnxMessageUI;
    [SerializeField] LoadCSVDataWeb loadCSVData;
    // [SerializeField] LoadCSVData loadCSVData;

    private Coroutine backdropFadeCoroutine;

    private bool fadeFirst;

    public bool playerInRange = false;

    // public List<BuildingScript> buildingScripts = new List<BuildingScript>();






    // Start is called before the first frame update
    void Start () 
    {
        StartCoroutine (LateStart());

        Player = GameObject.FindGameObjectWithTag("Player");

        myCharacterAnimation = Player.GetComponent<CharacterAnimation>();

        backdrop = GameObject.FindGameObjectWithTag("InnerBuildingBackdrop");

        loadCSVData = GameObject.FindGameObjectWithTag("CharacterCustomizationMenu").GetComponent<LoadCSVDataWeb>();
        // loadCSVData = GameObject.FindGameObjectWithTag("CharacterCustomizationMenu").GetComponent<LoadCSVData>();
        thnxMessageUI = GameObject.Find("Thnx Message");
        thnxMessageUI.SetActive(true);

    }

    void Update()
    {
        if (playerInRange)
        {
            if ((Input.GetKeyDown(KeyCode.Space) || 
                        Input.GetKeyDown(KeyCode.JoystickButton0) ||  // A button
                        Input.GetKeyDown(KeyCode.JoystickButton1) ||  // B button
                        Input.GetKeyDown(KeyCode.JoystickButton2)   // X button
                        // Input.GetKeyDown(KeyCode.JoystickButton3)
                        ) && !Player.GetComponent<CharacterMovement>().IsInputFieldFocused())           
                {  
                    if (GameObject.Find("creationUIWeb")?.activeSelf == true)
                    {
                        Player.GetComponent<CharacterCustomization>().UpdateRandom();
                        // Player.GetComponent<CharacterAnimation>().characterSpriteList.Clear;
                        // Player.GetComponent<CharacterAnimation>().initialChrctrColorList.Clear;
                    }
                    else
                    {
                        loadCSVData.DisplayRandomRow();
                        myCharacterAnimation.GetSpritesAndAddToLists(Player, myCharacterAnimation.characterSpriteList, new List<GameObject>(), myCharacterAnimation.initialChrctrColorList, myCharacterAnimation.initialChrctrSpriteTransformList);
                    }
                }
        }
    }

    IEnumerator LateStart() 
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame(); // wait for children to switch off separately
        
        browseChrctrObj.SetActive(false);
        createChrctrObj.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D player)
    {
        if(player.CompareTag("PlayerCollider") && !Player.GetComponent<CharacterMovement>().playerOnBike)
        {
            if(backdropFadeCoroutine != null)
            {
                StopCoroutine(this.backdropFadeCoroutine);
            }

            playerInRange = true;

            backdropFadeCoroutine = StartCoroutine(FadeBackdropAndSetSortingLayersEtc(false, 1f));

            // cameraMovement.smoothing = 3f;
            // cameraMovement.target = characterCuzzyTarget;
        }
    }


    void OnTriggerExit2D(Collider2D player)
    {
        if(player.CompareTag("PlayerCollider"))
        {
            if(backdropFadeCoroutine != null)
            {
                StopCoroutine(this.backdropFadeCoroutine);
            }

            playerInRange = false;

            backdropFadeCoroutine = StartCoroutine(FadeBackdropAndSetSortingLayersEtc(true,0f));
            


            loadCSVData.UpdateAllNPCSDuringPlay();

            // cameraMovement.smoothing = 1000f;
            // cameraMovement.target = player.transform;
            // foreach(BuildingScript building in buildingScripts)
            // {   
            //     building.ExitBuilding(0f, 0f, false);
            // }


        }
    }

    public IEnumerator FadeBackdropAndSetSortingLayersEtc(bool fadeFirst, float alpha)
    {
        // if (backdrop != null)
        // {
            SpriteRenderer sr = backdrop.GetComponent<SpriteRenderer>();

            // Calculate duration based on the alpha difference
            float alphaDiff = Mathf.Abs(sr.color.a - alpha);
            float duration = alphaDiff; // Assuming alpha ranges from 0 to 1
            float elapsedTime = 0.0f;

            if(fadeFirst == false)
            {
                // browseChrctrObj.SetActive(true);

                SetTreeSortingLayer(backdrop, "UI1");
                SetTreeSortingLayer(Player, "UI2");
                SetTreeSortingLayer(platform, "UI2");

                thnxMessageUI.SetActive(false);

                browseChrctrObj.SetActive(true);
                createChrctrObj.SetActive(false);

                while (elapsedTime < duration)
                {
                    elapsedTime += Time.deltaTime;
                    float t = elapsedTime / duration;

                    float nextAlpha = Mathf.Lerp(sr.color.a, alpha, t);
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, nextAlpha);

                    yield return null;
                }

                // yield return new WaitForSeconds(0.1f);



                // yield return new WaitForSeconds(1f);

                // Ensure the final alpha value is set
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
            }
            else
            {
                if(browseChrctrObj != null)
                {
                    browseChrctrObj.SetActive(false);
                    createChrctrObj.SetActive(false);
                }
                while (elapsedTime < duration)
                {
                    elapsedTime += Time.deltaTime;
                    float t = elapsedTime / duration;

                    float nextAlpha = Mathf.Lerp(sr.color.a, alpha, t);
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, nextAlpha);

                    yield return null;
                }

                yield return new WaitForSeconds(0.1f);




                SetTreeSortingLayer(backdrop, "Backdrop");
                SetTreeSortingLayer(Player, "Default");
                SetTreeSortingLayer(platform, "Default");

                thnxMessageUI.SetActive(true);


                // Ensure the final alpha value is set
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);  
            }
        // }
    }

    static void SetTreeSortingLayer(GameObject gameObject, string sortingLayerName)
    {
        if(gameObject.GetComponent<SpriteRenderer>() != null) 
        {
          gameObject.GetComponent<SpriteRenderer>().sortingLayerName = sortingLayerName;
        }
        foreach (Transform child in gameObject.transform)
        {
            ActivateCharacterUITrigger.SetTreeSortingLayer(child.gameObject, sortingLayerName);
        }    
    }
}
