// Have been working on things to have zero alpha when walking inside the building
//             things such as open cupboards, doors, fridges etc.
// Issue as of now - 2/7/23: as I enter a building from below, the 'closed' door sprite I go through does no set back to 0.35f
// It does this because I made it so all objects with 0.35f alpha would go to 0 upon entry... except I need this particular door to stay at 0.35f.  



using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomThresholdColliderScript : MonoBehaviour
{
    public BuildingScript building;
    public RoomScript roomAbove;
    public RoomScript roomBelow;
    LevelThreshColliderScript levelThreshColliderScript;
    PlayerMovement playerMovement;
    [SerializeField] GameObject Player;
    private bool aboveCollider;
    private List<float> initialOpenDoorAlpha = new List<float>();
    private List<float> initialClosedDoorAlpha = new List<float>();
    private List<GameObject> openDoorSpriteList = new List<GameObject>();
    private List<GameObject> closedDoorSpriteList = new List<GameObject>();

    private Coroutine closedDoorFadeCoroutine;
    private Coroutine openDoorFadeCoroutine;
    public Coroutine thresholdSortingSequenceCoro; 
    private string initialSortingLayer;
    private bool playerIsInDoorway = false;
    private bool playerIsInRoomAbove = false;

    private string[] tagsToExludeEntExt = { "OpenDoor", "AlphaZeroEntExt" };
    

    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = Player.GetComponent<PlayerMovement>(); 
        
        // get the sprites and add them to the -> corresponding lists
        GetSprites(FindSiblingWithTag("OpenDoor"), openDoorSpriteList);
        GetSprites(FindSiblingWithTag("ClosedDoor"), closedDoorSpriteList);

        PopulateInitialColorLists();

        SetOpenDoorToZeroAlpha();

        SetClosedDoorToInitialAlpha(); 
        
        SetDoorState(false, false);
    }

    void OnTriggerEnter2D()
    {
        StopAllCoros();

        playerMovement.motionDirection = "normal";

        if (isPlayerCrossingUp())
        {
            aboveCollider = false;
        }
        else
        {
            aboveCollider = true;
        }

        if((isPlayerCrossingUp() && isPlayerCrossingLeft()) // is player going this \ (left diag) way or
        || !isPlayerCrossingUp() && !isPlayerCrossingLeft()) // is player going this / (right diag) way :-)
        {
            playerMovement.fixedDirectionLeft = true; // fix the player in \ left diag way while inside the collider
        }
        else
        {
            playerMovement.fixedDirectionRight = true; // fix the player in / right diag way while inside the collider
        }

        SetClosedDoorToZeroAlpha();
        SetOpenDoorToInitialAlpha();
        SetPlayerIsInDoorway(true);
    }

    void OnTriggerExit2D()
    {
        StopAllCoros();

        playerMovement.fixedDirectionLeft = false;
        playerMovement.fixedDirectionRight = false; // un-fix the player in \/ left/right diag way upon collider exit. 

            //ON EXIT CROSSING UP
        if (isPlayerCrossingUp())
        {
            if (ThisThresholdIsAnEntranceToTheBuildingOrIsStairs())
            {
                if (roomBelow != null && roomAbove == null) // if player is exiting the back of the building
                {
                    roomBelow.ResetRoomPositions(); 
                    roomBelow.ExitRoomAndSetDoorwayInstances(); 
                    SetOpenDoorToZeroAlpha();
                    SetClosedDoorToInitialAlpha();
                }  
                else if (roomAbove != null) // if player is entering the building from the fro
                {
                    // SetClosedDoorToInitialAlpha();
                    roomAbove.EnterRoom(true, 0.3f); 
                }
            }
            else // if this threshold is just the entrance to a room and not a building entrance
            {               
                SetClosedDoorToInitialAlpha(); 
                if (roomBelow != null && roomAbove == null) // if there is no room above the thresh i.e. if 
                                                            // the player is leaving the level and the rooms need to go in their original pos.
                {
                    roomBelow.ResetRoomPositions(); 
                }
                else if (roomAbove != null) 
                {
                    roomAbove.EnterRoom(false, 0f);
                }

                if (roomBelow != null)
                {
                    roomBelow.ExitRoomAndSetDoorwayInstances();
                }
            }
            
            aboveCollider = true;
        }
            
        else //ON EXIT CROSSING DOWN
        {
            if (roomAbove != null)
            {
                roomAbove.ExitRoomAndSetDoorwayInstances();
                SetOpenDoorToZeroAlpha();
                SetClosedDoorToInitialAlpha();
            }

            if (ThisThresholdIsAnEntranceToTheBuildingOrIsStairs())
            {
                if (roomBelow == null && roomAbove != null) // if the player IS inside bulding and ABOVE collider prior to collider exit
                {
                    roomAbove.ResetRoomPositions();
                }   
                else
                if (playerMovement.playerIsInside() && roomBelow != null) // if the player IS inside bulding and ABOVE collider, going down stairs and entering room downstairs
                {
                    roomBelow.EnterRoom(false, 0f);

                    SetClosedDoorToInitialAlpha();
                    for (int i = 0; i < openDoorSpriteList.Count; i++)
                    { 
                        SetTreeAlpha(this.FindSiblingWithTag("OpenDoor"), 0);
                    }  
                }    
            }
            else if (roomBelow != null) // entering a room going down while inside building
            {
                roomBelow.EnterRoom(false, 0f);

                if (playerMovement.playerIsInside() && aboveCollider) // if player is inside 
                {
                    initialSortingLayer = Player.GetComponent<SpriteRenderer>().sortingLayerName;

                    thresholdSortingSequenceCoro = StartCoroutine(ThresholdLayerSortingSequence(0.3f, Player, "ThresholdSequence"));
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


    void SetClosedDoorToZeroAlpha()
    {
        for (int i = 0; i < closedDoorSpriteList.Count; i++)
        {
            SetTreeAlpha(closedDoorSpriteList[i], 0);
        }       
    }
    void SetClosedDoorToInitialAlpha()
    {
        for (int i = 0; i < closedDoorSpriteList.Count; i++)
        {
            SetTreeAlpha(closedDoorSpriteList[i], initialClosedDoorAlpha[i]);
        }       
    }
    void SetOpenDoorToZeroAlpha()
    {
        for (int i = 0; i < openDoorSpriteList.Count; i++)
        {
            SetTreeAlpha(openDoorSpriteList[i], 0);
        }   
    }
    void SetOpenDoorToInitialAlpha()
    {
        for (int i = 0; i < openDoorSpriteList.Count; i++)
        {
            SetTreeAlpha(openDoorSpriteList[i], initialOpenDoorAlpha[i]);
        }   
    }

    bool ThisThresholdIsAnEntranceToTheBuildingOrIsStairs()
    {
            if (this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() != null
            ||  this.transform.parent.GetComponentInChildren<LevelThreshColliderScript>() != null
            || this.transform.parent.GetComponentInChildren<InclineMovement>() != null)
            {
                return true;
            } 
            return false;
    }

    void PopulateInitialColorLists()
    {
                //populate the initialOpenDoorAlpha list
        for (int i = 0; i < openDoorSpriteList.Count; i++)
        {
            Color initialColorOpen = openDoorSpriteList[i].GetComponent<SpriteRenderer>().color;
            initialOpenDoorAlpha.Add(initialColorOpen.a);
        }

        //populate the initialClosedDoorAlpha list
        for (int i = 0; i < closedDoorSpriteList.Count; i++)
        {
            Color initialColorClosed = closedDoorSpriteList[i].GetComponent<SpriteRenderer>().color;
            initialClosedDoorAlpha.Add(initialColorClosed.a);
        }
    }

    void StopAllCoros()
    {
        if(this.closedDoorFadeCoroutine != null)
        {
            StopCoroutine(this.closedDoorFadeCoroutine);
        }

        if(this.openDoorFadeCoroutine != null)
        {
            StopCoroutine(this.openDoorFadeCoroutine);
        }
        if (thresholdSortingSequenceCoro != null)
        {
            StopCoroutine(thresholdSortingSequenceCoro);
            SetTreeSortingLayer(Player, initialSortingLayer);
        }
    }

    IEnumerator treeFade(
    GameObject obj,
    float fadeTo,
    float elapsedTime) 
    {
        float fadeFrom = obj.GetComponent<SpriteRenderer>().color.a;
        float timer = 0.0f;

        while (timer < elapsedTime) 
        {
            float t = timer / elapsedTime;
            float currentAlpha = Mathf.Lerp(fadeFrom, fadeTo, t);
            SetTreeAlpha(obj, currentAlpha);
            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure final state
        SetTreeAlpha(obj, fadeTo);
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
    
    static void SetTreeSortingLayer(GameObject gameObject, string sortingLayerName)
    {
        if(gameObject.GetComponent<SpriteRenderer>() != null) 
        {
            gameObject.GetComponent<SpriteRenderer>().sortingLayerName = sortingLayerName;
        }
        foreach (Transform child in gameObject.transform)
        {
            RoomThresholdColliderScript.SetTreeSortingLayer(child.gameObject, sortingLayerName);
        }
    }  

    IEnumerator ThresholdLayerSortingSequence(
        float waitTime,
        GameObject gameObject,
        string newSortingLayer) 
    {  
        initialSortingLayer = Player.GetComponent<SpriteRenderer>().sortingLayerName; 
        // initialSortingLayer = LayerMask.LayerToName(this.gameObject.layer);

        SetTreeSortingLayer(gameObject, newSortingLayer);

        yield return new WaitForSeconds(waitTime);

        if (Player.GetComponent<SpriteRenderer>().sortingLayerName == newSortingLayer)
        {
            SetTreeSortingLayer(gameObject, initialSortingLayer);
        }

        yield return null;
    }
    
    // NOTE: The parent of the threshold collider must be the door parent

    public GameObject FindSiblingWithTag(string tag, Transform transform = null) 
    {
        if (transform == null)
        {
            foreach (Transform child in this.gameObject.transform.parent)
            {
                if (child.CompareTag(tag))
                {
                    return child.gameObject; 
                } 
                else
                {
                    FindSiblingWithTag(tag, child);
                }       
            }
        }
        else 
        {
            foreach (Transform child in transform)
            {
                if (child.CompareTag(tag))
                {
                    return child.gameObject; //this returns the first sibling that has the corresponding tag
                                            // then, if this group of siblings doesn's contain the tag, the next bit checks their descendants 
                                            //and so on
                }
                else
                {
                    Transform descendant = child.gameObject.transform;
                    if (descendant != null)
                    {
                        foreach (Transform child2 in descendant)
                        {
                            FindSiblingWithTag(tag, child2);
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
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

    public void SetDoorState(bool playerIsInDoorway, bool playerIsInRoomAbove) 
    {
        this.playerIsInDoorway = playerIsInDoorway;
        this.playerIsInRoomAbove = playerIsInRoomAbove;

        if (playerIsInDoorway) 
        {
      
            
        }
        else 
        if (playerIsInRoomAbove) 
        {
            if (roomAbove != null)
            {                    
                for (int i = 0; i < roomAbove.doorsBelow.Count; i++)
                {
                    if (roomAbove.doorsBelow[i].FindSiblingWithTag("ClosedDoor") != null)
                    {
                        this.closedDoorFadeCoroutine = StartCoroutine(treeFade(roomAbove.doorsBelow[i].FindSiblingWithTag("ClosedDoor"), 0.35f, 0.3f));                    
                        SetTreeAlpha(roomAbove.doorsBelow[i].FindSiblingWithTag("OpenDoor"), 0);
                    }
                }
            }  
        }
        else if (playerMovement.playerIsOutside) {
            if(this.FindSiblingWithTag("OpenDoor") != null)
            {
                this.openDoorFadeCoroutine = StartCoroutine(treeFade(this.FindSiblingWithTag("OpenDoor"), 0, 0.3f));
            }
        }
        else // if player is not in room above
        {
            if (roomAbove != null)
            {
                for (int i = 0; i < roomAbove.doorsBelow.Count; i++)
                {
                    for (int j = 0; j < roomAbove.doorsBelow[i].closedDoorSpriteList.Count; j++)
                    {
                        this.closedDoorFadeCoroutine = StartCoroutine(treeFade(roomAbove.doorsBelow[i].closedDoorSpriteList[j], 
                                                                        roomAbove.doorsBelow[i].initialClosedDoorAlpha[j], 0.3f)); 
                        // SetTreeAlpha(roomAbove.doorsBelow[i].closedDoorSpriteList[j], roomAbove.doorsBelow[i].initialClosedDoorAlpha[j]);
                    } 
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
        SetDoorState(playerIsInDoorway, playerIsInRoomAbove);
    }
}



