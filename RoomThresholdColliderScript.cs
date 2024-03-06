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
    LevelThreshColliderScript levelThreshColliderScript;
    PlayerMovement playerMovement;
    [SerializeField] GameObject Player;
    public bool aboveCollider;
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
            SetTreeSortingLater(Player, initialSortingLayer);
        }
        
        playerMovement.motionDirection = "normal";

        if((isPlayerCrossingUp() && isPlayerCrossingLeft()) // is player going this \ (left diag) way or
        || !isPlayerCrossingUp() && !isPlayerCrossingLeft()) // is player going this / (right diag) way :-)
        {
            playerMovement.fixedDirectionLeft = true; // fix the player in \ left diag way while inside the collider
        }
        else
        {
            playerMovement.fixedDirectionRight = true; // fix the player in / right diag way while inside the collider
        }
        // fixing the player's movement inside the collider ensures the correct conditions are met upon collider exit


        // if (isPlayerCrossingUp())
        // {
        //     aboveCollider = false;
        // }    
        // else 
        // {
        //     aboveCollider = true; 
        // }

        SetPlayerIsInDoorway(true);
    }

    void OnTriggerExit2D()
    {
        if(this.closedDoorFadeCoroutine != null)
        {
            StopCoroutine(this.closedDoorFadeCoroutine);
        }

        if(this.openDoorFadeCoroutine != null)
        {
            StopCoroutine(this.openDoorFadeCoroutine);
        }

            playerMovement.fixedDirectionLeft = false;
            playerMovement.fixedDirectionRight = false; // un-fix the player in \/ left/right diag way upon collider exit. 

        if (thresholdSortingSequenceCoro != null)
        {
            StopCoroutine(thresholdSortingSequenceCoro);
            SetTreeSortingLater(Player, initialSortingLayer);
        }

            //ON EXIT CROSSING UP
        if (isPlayerCrossingUp())
        {
            if (this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() != null
            ||  this.transform.parent.GetComponentInChildren<LevelThreshColliderScript>() != null
            || this.transform.parent.GetComponentInChildren<InclineMovement>() != null)
            {
                // if (playerMovement.isPlayerOutside && !aboveCollider)
                // {
                //     //need to make exit building 0.35f here - for back of the building
                //     // building.ExitBuilding();
                   
                //     // GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerOutside = false;


                // }
                // else 

                // if (aboveCollider) 
                // {   
                //     if (roomBelow == null && roomAbove != null) // if the player is at the top of the stairs about to enter the level
                //     {
                //         roomAbove.EnterRoom(false, 0f);
                //     }  
                // }
                // if (!aboveCollider)
                // {
                    if (roomBelow != null && roomAbove == null)
                    {
                        roomBelow.ResetRooms();
                    }  
                    else if (roomAbove != null)
                    {
                        roomAbove.EnterRoom(true, 0.3f);
                    }
                    // building.EnterBuilding();
                    
                    // playerMovement.isPlayerOutside = true;
                // }
            }
            else 
            {                
                if (roomAbove != null)
                {
                    roomAbove.EnterRoom(false, 0f);
                }

                if (roomBelow != null)
                {
                    roomBelow.ExitRoom();
                }
            }
            
            aboveCollider = true;
        }
            
        else //ON EXIT CROSSING DOWN
        {
            if (roomAbove != null)
            {
                roomAbove.ExitRoom();
                for (int i = 0; i < roomAbove.doorsBelow.Count; i++)
                {
                    // Debug.Log("KUFDSHLSFBLSUEDUF");
                    this.closedDoorFadeCoroutine = StartCoroutine(treeFade(roomAbove.doorsBelow[i].FindSiblingWithTag("ClosedDoor"), 1f, 3f));                    
                    // this.openDoorFadeCoroutine = StartCoroutine(treeFade(roomAbove.doorsBelow[i].FindSiblingWithTag("OpenDoor"), 0f, 3f)); 
                    SetTreeAlpha(roomAbove.doorsBelow[i].FindSiblingWithTag("OpenDoor"), 0);
                }  
            }

            if ((this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() != null)
                || this.transform.parent.GetComponentInChildren<LevelThreshColliderScript>() !=null
                || this.transform.parent.GetComponentInChildren<InclineMovement>() != null
               )
            {
                // if (aboveCollider) 
                // {   
                if (roomBelow == null && roomAbove != null) // if the player IS inside bulding and ABOVE collider prior to collider exit
                {
                    roomAbove.ResetRooms();
                }   
                else
                if (!playerMovement.isPlayerOutside && roomBelow != null) // if the player IS inside bulding and ABOVE collider, going down stairs and entering room downstairs
                {
                    roomBelow.EnterRoom(false, 0f);
                }    
                // }

                // if (playerMovement.isPlayerOutside && aboveCollider) /
                // {   
                //     // if (roomBelow != null) 
                //     // {
                //     //     roomBelow.EnterRoom(true, 0f);
                //     // } 
                //     //else 
                //     if (roomBelow == null && roomAbove != null) // exiting building going down
                //     {
                //         roomAbove.ResetRooms();
                //     }                  
                // }
            }
            else if (roomBelow != null) // entering a room going down while inside building
            {
                roomBelow.EnterRoom(false, 0f);

                if (
                    // aboveCollider && 
                    !playerMovement.isPlayerOutside)
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

        for (float t = 0.0f; t < 1f; t += Time.deltaTime) 
        {
            float currentAlpha = Mathf.Lerp(fadeFrom, fadeTo, t * fadeSpeed);
            SetTreeAlpha(obj, currentAlpha);
            yield return null;
        }
    }
    

// IEnumerator treeFade(
//     GameObject obj,
//     float fadeTo,
//     float fadeTime) 
// {
//     float fadeFrom = obj.GetComponent<SpriteRenderer>().color.a;
//     float elapsedTime = 0.0f;

//     while (elapsedTime < fadeTime) 
//     {
//         float t = elapsedTime / fadeTime;
//         float currentAlpha = Mathf.Lerp(fadeFrom, fadeTo, t);
//         SetTreeAlpha(obj, currentAlpha);

//         elapsedTime += Time.deltaTime;
//         yield return null;
//     }

//     // Ensure final alpha is set to fadeTo value
//     SetTreeAlpha(obj, fadeTo);
// }

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
        initialSortingLayer = LayerMask.LayerToName(this.gameObject.layer);

        SetTreeSortingLater(gameObject, newSortingLayer);

        yield return new WaitForSeconds(waitTime);

        SetTreeSortingLater(gameObject, initialSortingLayer);

        yield return null;
    }

    // IEnumerator ThresholdLayerSortingSequence(
    // float waitTime,
    // GameObject gameObject,
    // string newSortingLayer) 
    // {   
    //     initialSortingLayer = FindSiblingWithTag("ClosedDoor", null).GetComponent<SpriteRenderer>().sortingLayerName;

    //     SetTreeSortingLater(gameObject, newSortingLayer);

    //     yield return new WaitForSeconds(waitTime);

    //     SetTreeSortingLater(gameObject, initialSortingLayer);

    //     yield return null;
    // }
    
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

    public void SetDoorState(bool playerIsInDoorway, bool playerIsInRoomAbove, bool fade=false) 
    {
        this.playerIsInDoorway = playerIsInDoorway;
        this.playerIsInRoomAbove = playerIsInRoomAbove;

        if (fade) {
            // if(this.closedDoorFadeCoroutine != null)
            // {
            //     StopCoroutine(this.closedDoorFadeCoroutine);
            // }

            // if(this.openDoorFadeCoroutine != null)
            // {
            //     StopCoroutine(this.openDoorFadeCoroutine);
            // }

            if (playerIsInDoorway) {
                Debug.Log("OOOOOO");
                // this.closedDoorFadeCoroutine = StartCoroutine(treeFade(this.FindSiblingWithTag("ClosedDoor"), 0, 3f));
                // this.openDoorFadeCoroutine = StartCoroutine(treeFade(this.FindSiblingWithTag("OpenDoor"), 1, 3f));                      
                for (int i = 0; i < openDoorSpriteList.Count; i++)
                {
                    SetTreeAlpha(openDoorSpriteList[i], initialOpenDoorAlpha[i]);
                }  
                for (int i = 0; i < closedDoorSpriteList.Count; i++)
                {
                    SetTreeAlpha(closedDoorSpriteList[i], 0);
                }                               
            }
            else if (playerIsInRoomAbove) {
                Debug.Log("RRRRRR");
                // if (aboveCollider)
                // {
                    // for (int i = 0; i < closedDoorSpriteList.Count; i++)
                    // {
                    //     this.closedDoorFadeCoroutine = StartCoroutine(treeFade(closedDoorSpriteList[i], 0.15f, 3f));                    
                    // }  
                    for (int i = 0; i < roomAbove.doorsBelow.Count; i++)
                    {
                        // Debug.Log("KUFDSHLSFBLSUEDUF");
                        this.closedDoorFadeCoroutine = StartCoroutine(treeFade(roomAbove.doorsBelow[i].FindSiblingWithTag("ClosedDoor"), 0.15f, 3f));                    
                        // this.openDoorFadeCoroutine = StartCoroutine(treeFade(roomAbove.doorsBelow[i].FindSiblingWithTag("OpenDoor"), 0f, 3f)); 
                        SetTreeAlpha(roomAbove.doorsBelow[i].FindSiblingWithTag("OpenDoor"), 0);
                    }  
                    // this.closedDoorFadeCoroutine = StartCoroutine(treeFade(this.FindSiblingWithTag("ClosedDoor"), 0.15f, 3f));

                    // this.openDoorFadeCoroutine = StartCoroutine(treeFade(this.FindSiblingWithTag("OpenDoor"), 0, 3f));
                    // for (int i = 0; i < openDoorSpriteList.Count; i++)
                    // {
                    //     SetTreeAlpha(openDoorSpriteList[i], 0);
                    // } 
                // }
                // else
                // {

                // }          
            }
            else if (GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerOutside) {
                this.openDoorFadeCoroutine = StartCoroutine(treeFade(this.FindSiblingWithTag("OpenDoor"), 0, 3f));
            }
            // else if ((this.FindSiblingWithTag("OpenDoor") != null || this.FindSiblingWithTag("ClosedDoor") != null))
            //     {
            //         for (int i = 0; i < roomAbove.doorsBelow.Count; i++)
            //         {
            //             // this.closedDoorFadeCoroutine = StartCoroutine(treeFade(roomAbove.doorsBelow[i].FindSiblingWithTag("ClosedDoor"), 1f, 3f));                    
            //             // this.openDoorFadeCoroutine = StartCoroutine(treeFade(roomAbove.doorsBelow[i].FindSiblingWithTag("OpenDoor"), 0f, 3f)); 
            //             SetTreeAlpha(roomAbove.doorsBelow[i].FindSiblingWithTag("OpenDoor"), 0);
            //         }  
            //         // this.closedDoorFadeCoroutine = StartCoroutine(treeFade(this.FindSiblingWithTag("ClosedDoor"), 1, 3f));
            //         // this.openDoorFadeCoroutine = StartCoroutine(treeFade(this.FindSiblingWithTag("OpenDoor"), 0, 3f));
            //     }
            }
        } 
        // else {
        //     if (playerIsInDoorway) {
        //         for (int i = 0; i < closedDoorSpriteList.Count; i++)
        //         {
        //             SetTreeAlpha(closedDoorSpriteList[i], 0);
        //         }                    
                
        //         for (int i = 0; i < openDoorSpriteList.Count; i++)
        //         {
        //             SetTreeAlpha(openDoorSpriteList[i], initialOpenDoorAlpha[i]);
        //         }                    
        //     }
        //     else if (playerIsInRoomAbove) {
        //         for (int i = 0; i < closedDoorSpriteList.Count; i++)
        //         {
        //             SetTreeAlpha(closedDoorSpriteList[i], initialClosedDoorAlpha[i] - ((initialClosedDoorAlpha[i]/100)*85));
        //         }                           
                
        //         for (int i = 0; i < openDoorSpriteList.Count; i++)
        //         {
        //             SetTreeAlpha(openDoorSpriteList[i], 0);
        //         }               
        //     }
        //     else if (GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerOutside) {
        //         for (int i = 0; i < openDoorSpriteList.Count; i++)
        //         {
        //             SetTreeAlpha(openDoorSpriteList[i], 0);
        //         }       
        //     }
        //     else {
        //         for (int i = 0; i < closedDoorSpriteList.Count; i++)
        //         {
        //             SetTreeAlpha(closedDoorSpriteList[i], initialClosedDoorAlpha[i]);
        //         }         

        //         for (int i = 0; i < openDoorSpriteList.Count; i++)
        //         {
        //             SetTreeAlpha(openDoorSpriteList[i], 0);
        //         }     
        //     }
        // }


    

    public void SetPlayerIsInDoorway(bool playerIsInDoorway) 
    {
        this.playerIsInDoorway = playerIsInDoorway;
        SetDoorState(playerIsInDoorway, playerIsInRoomAbove = false, true);
    }

    public void SetPlayerIsInRoomAbove(bool playerIsInRoomAbove) 
    {
        this.playerIsInRoomAbove = playerIsInRoomAbove;
        //SetDoorState(playerIsInDoorway, playerIsInRoomAbove, true);
        SetDoorState(playerIsInDoorway = false, playerIsInRoomAbove, true);
    }
}



