// Have been working on things to have zero alpha when walking inside the building
//             things such as open cupboards, doors, fridges etc.
// Issue as of now - 2/7/23: as I enter a building from below, the 'closed' door sprite I go through does no set back to 0.15f
// It does this because I made it so all objects with 0.15f alpha would go to 0 upon entry... except I need this particular door to stay at 0.15f.  



using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomThresholdColliderScript : MonoBehaviour
{
    public BuildingScript building;
    public RoomScript roomAbove;
    public RoomScript roomBelow;
    LevelColliderScript levelColliderScript;
    PlayerMovement playerMovement;
    [SerializeField] GameObject Player;
    private bool aboveCollider;
    private List<float> initialOpenDoorAlpha = new List<float>();
    private List<float> initialClosedDoorAlpha = new List<float>();
    private List<GameObject> openDoorSpriteList = new List<GameObject>();
    private List<GameObject> closedDoorSpriteList = new List<GameObject>();

    private Coroutine closedDoorFadeCoroutine;
    private Coroutine openDoorFadeCoroutine;
    private Coroutine thresholdSortingSequenceCoro;
    private string initialSortingLayer;

    private bool playerIsInDoorway = false;
    private bool playerIsInRoomAbove = false;

    private string[] tagsToExludeEntExt = { "OpenDoor", "AlphaZeroEntExt" };
    


    void Awake()
    { 
        Player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = Player.GetComponent<PlayerMovement>(); 
    }  

    void Start()
    {
        // get the sprites and add them to the corresponding lists
        GetSprites(FindSiblingWithTag("OpenDoor"), openDoorSpriteList);
        GetSprites(FindSiblingWithTag("ClosedDoor"), closedDoorSpriteList);

        for (int i = 0; i < openDoorSpriteList.Count; i++)
        {
            Color initialColorOpen = openDoorSpriteList[i].GetComponent<SpriteRenderer>().color;
            initialOpenDoorAlpha.Add(initialColorOpen.a);
        }

        for (int i = 0; i < closedDoorSpriteList.Count; i++)
        {
            Color initialColorClosed = closedDoorSpriteList[i].GetComponent<SpriteRenderer>().color;
            initialClosedDoorAlpha.Add(initialColorClosed.a);
        }

        for (int i = 0; i < openDoorSpriteList.Count; i++)
        {
            SetTreeAlpha(openDoorSpriteList[i], 0);
        } 

        for (int i = 0; i < closedDoorSpriteList.Count; i++)
        {
            SetTreeAlpha(closedDoorSpriteList[i], initialClosedDoorAlpha[i]);
        }    

        SetDoorState(false, false);
    }

    void OnTriggerEnter2D()
    {
        // initialSortingLayer = Player.GetComponent<SpriteRenderer>().sortingLayerName;


        if (thresholdSortingSequenceCoro != null)
        {
            StopCoroutine(thresholdSortingSequenceCoro);
            SetTreeSortingLater(Player, initialSortingLayer);
        }
        
        playerMovement.motionDirection = "normal";

        if((isPlayerCrossingUp() && isPlayerCrossingLeft()) 
            || !isPlayerCrossingUp() && !isPlayerCrossingLeft())
        {
            playerMovement.fixedDirectionLeft = true;
        }
        else
        {
            playerMovement.fixedDirectionRight = true;
        }


        if (isPlayerCrossingUp())
        {
            aboveCollider = false;
        }    
        else 
        {
            aboveCollider = true; 
        }

        SetPlayerIsInDoorway(true);
    }

    void OnTriggerExit2D()
    {
            // initialSortingLayer = Player.GetComponent<SpriteRenderer>().sortingLayerName;

            playerMovement.fixedDirectionLeft = false;
            playerMovement.fixedDirectionRight = false;

       // Bug avoidance: in case the player is inside and isPlayerInside is false etc. (need to optimise)

        // if (building == null && !playerMovement.isPlayerInside)
        // {
        //     SetTreeAlpha(multiFdObjInside, 1, tagsToExludeEntExt);
        //     SetTreeAlpha(multiFdObjOutside, 0);
        //     multiFdObjOutside.SetActive(false);
        //     GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside = true;
        // }


        if (thresholdSortingSequenceCoro != null)
        {
            StopCoroutine(thresholdSortingSequenceCoro);
            SetTreeSortingLater(Player, initialSortingLayer);
        }

            //ON EXIT CROSSING UP
        if (isPlayerCrossingUp())
        {
            if (roomBelow != null)
            {
                roomBelow.ExitRoom();
            }
            if (roomAbove != null)
            {
                roomAbove.EnterRoom(false, 0f);
            }

            if (this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() != null)
            {
                if (!playerMovement.isPlayerInside && !aboveCollider)
                {
                    //need to make exit building 0.35f here - for back of the building
                    // building.ExitBuilding();
                   
                    // GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside = false;

                    //need to optimise
                    if (roomBelow != null || roomAbove == null)
                    {
                        roomBelow.ExitBuilding();
                    }  
                }
                else if (!aboveCollider)
                {
                    // building.EnterBuilding();
                    
                    // playerMovement.isPlayerInside = true;
                }
            }
            
            aboveCollider = true;
        }
            //ON EXIT CROSSING DOWN
        else
        {
            // initialSortingLayer = Player.GetComponent<SpriteRenderer>().sortingLayerName;

            if (roomAbove != null)
            {
                roomAbove.ExitRoom();
            }


            
            if (this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() != null)
            {
                if (!playerMovement.isPlayerInside && aboveCollider)
                {                    
                    // building.ExitBuilding();

                    // playerMovement.isPlayerInside = false;
                    
                    //need to optimise
                    if (roomBelow != null)
                    {
                        roomBelow.ExitBuilding();
                    }                
                }
                else if (playerMovement.isPlayerInside && aboveCollider)
                {
                    if (roomBelow != null)
                    {
                        roomBelow.EnterRoom(true, 1f);
                    }     
                }
            }
            else if (roomBelow != null)
            {
                roomBelow.EnterRoom(false, 0f);

                if (aboveCollider && playerMovement.isPlayerInside)
                {
                    initialSortingLayer = Player.GetComponent<SpriteRenderer>().sortingLayerName;

                    thresholdSortingSequenceCoro = StartCoroutine(ThresholdLayerSortingSequence(0.5f, Player, "ThresholdSequence"));
                }
            }             
            aboveCollider = false;

        }

        SetPlayerIsInDoorway(false);
    }
    private bool isPlayerCrossingUp()
    {
        return GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().change.y > 0;
    }
    private bool isPlayerCrossingLeft()
    {
        return GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().change.x < 0;
    }

    void SetTreeAlpha(GameObject treeNode, float alpha, string[] tagsToExclude = null) 
    {
        if (treeNode == null) {
            return; // TODO: remove this
        }
        SpriteRenderer sr = treeNode.GetComponent<SpriteRenderer>();
        if (sr != null && (tagsToExclude == null || !Array.Exists(tagsToExclude, element => element == treeNode.tag)))
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
        }
        foreach (Transform child in treeNode.transform) 
        {
            SetTreeAlpha(child.gameObject, alpha, tagsToExclude);
        }
    }  

    IEnumerator treeFade(
      GameObject obj,
      float fadeTo,
      float fadeSpeed) 
    {
        float fadeFrom = obj.GetComponent<SpriteRenderer>().color.a;

        for (float t = 0.0f; t < 1; t += Time.deltaTime) 
        {
            float currentAlpha = Mathf.Lerp(fadeFrom, fadeTo, t * fadeSpeed);
            SetTreeAlpha(obj, currentAlpha);
            yield return null;
        }
    }
    
    static void SetTreeSortingLater(GameObject gameObject, string sortingLayerName)
    {
        if(gameObject.GetComponent<SpriteRenderer>() != null) 
        {
            gameObject.GetComponent<SpriteRenderer>().sortingLayerName = sortingLayerName;
        }
        foreach (Transform child in gameObject.transform)
        {
            RoomThresholdColliderScript.SetTreeSortingLater(child.gameObject, sortingLayerName);
        }
    }  

    IEnumerator ThresholdLayerSortingSequence(
    float waitTime,
    GameObject gameObject,
    string newSortingLayer) 
    {
        initialSortingLayer = FindSiblingWithTag("ClosedDoor").GetComponent<SpriteRenderer>().sortingLayerName;

        SetTreeSortingLater(gameObject, newSortingLayer);

        yield return new WaitForSeconds(waitTime);

        SetTreeSortingLater(gameObject, initialSortingLayer);

        yield return null;
    }
    
    // NOTE: The parent of the threshold collider must be the door parent
    public GameObject FindSiblingWithTag(string tag) 
    {
        foreach (Transform child in this.gameObject.transform.parent)
        {
            if (child.CompareTag(tag))
            {
                return child.gameObject;
            }
        }
        return null;
    }

    private void GetSprites(GameObject root, List<GameObject> spriteList)
    {
        if(root != null)
        {
            Stack<GameObject> stack = new Stack<GameObject>();
            stack.Push(root);

            while (stack.Count > 0)
            {
                GameObject currentNode = stack.Pop();
                SpriteRenderer sr = currentNode.GetComponent<SpriteRenderer>();

                if (sr != null)
                {
                    spriteList.Add(currentNode);
                }

                foreach (Transform child in currentNode.transform)
                {
                    stack.Push(child.gameObject);
                }
            }
        }
    }

    public void SetDoorState(bool playerIsInDoorway, bool playerIsInRoomAbove, bool fade=false) 
    {
        this.playerIsInDoorway = playerIsInDoorway;
        this.playerIsInRoomAbove = playerIsInRoomAbove;

        if (fade) {
            if(this.closedDoorFadeCoroutine != null)
            {
                StopCoroutine(this.closedDoorFadeCoroutine);
            }

            if(this.openDoorFadeCoroutine != null)
            {
                StopCoroutine(this.openDoorFadeCoroutine);
            }

            if (playerIsInDoorway) {
                this.closedDoorFadeCoroutine = StartCoroutine(treeFade(this.FindSiblingWithTag("ClosedDoor"), 0, 3f));
                this.openDoorFadeCoroutine = StartCoroutine(treeFade(this.FindSiblingWithTag("OpenDoor"), 1, 3f));
            }
            else if (playerIsInRoomAbove) {
                this.closedDoorFadeCoroutine = StartCoroutine(treeFade(this.FindSiblingWithTag("ClosedDoor"), 0.15f, 3f));
                this.openDoorFadeCoroutine = StartCoroutine(treeFade(this.FindSiblingWithTag("OpenDoor"), 0, 3f));
            }
            else if (!GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside) {
                this.openDoorFadeCoroutine = StartCoroutine(treeFade(this.FindSiblingWithTag("OpenDoor"), 0, 3f));
            }
            else {
                this.closedDoorFadeCoroutine = StartCoroutine(treeFade(this.FindSiblingWithTag("ClosedDoor"), 1, 3f));
                this.openDoorFadeCoroutine = StartCoroutine(treeFade(this.FindSiblingWithTag("OpenDoor"), 0, 3f));
            }
        } else {
            if (playerIsInDoorway) {
                for (int i = 0; i < closedDoorSpriteList.Count; i++)
                {
                    SetTreeAlpha(closedDoorSpriteList[i], 0);
                }                    
                
                for (int i = 0; i < openDoorSpriteList.Count; i++)
                {
                    SetTreeAlpha(openDoorSpriteList[i], initialOpenDoorAlpha[i]);
                }                    
            }
            else if (playerIsInRoomAbove) {
                for (int i = 0; i < closedDoorSpriteList.Count; i++)
                {
                    SetTreeAlpha(closedDoorSpriteList[i], initialClosedDoorAlpha[i] - ((initialClosedDoorAlpha[i]/100)*85));
                }                           
                
                for (int i = 0; i < openDoorSpriteList.Count; i++)
                {
                    SetTreeAlpha(openDoorSpriteList[i], 0);
                }               
            }
            else if (!GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside) {
                for (int i = 0; i < openDoorSpriteList.Count; i++)
                {
                    SetTreeAlpha(openDoorSpriteList[i], 0);
                }       
            }
            else {
                for (int i = 0; i < closedDoorSpriteList.Count; i++)
                {
                    SetTreeAlpha(closedDoorSpriteList[i], initialClosedDoorAlpha[i]);
                }         

                for (int i = 0; i < openDoorSpriteList.Count; i++)
                {
                    SetTreeAlpha(openDoorSpriteList[i], 0);
                }     
            }
        }


    }

    public void SetPlayerIsInDoorway(bool playerIsInDoorway) 
    {
        this.playerIsInDoorway = playerIsInDoorway;
        SetDoorState(playerIsInDoorway, playerIsInRoomAbove);
    }

    public void SetPlayerIsInRoomAbove(bool playerIsInRoomAbove) 
    {
        this.playerIsInRoomAbove = playerIsInRoomAbove;
        //SetDoorState(playerIsInDoorway, playerIsInRoomAbove, true);
        SetDoorState(playerIsInDoorway, playerIsInRoomAbove, false);
    }
}



