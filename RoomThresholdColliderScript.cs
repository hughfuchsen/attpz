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
    public RoomScript roomAbove;
    public RoomScript roomBelow;
    LevelThreshColliderScript levelThreshColliderScript;
    CharacterMovement characterMovement;
    [SerializeField] GameObject Player;
    [SerializeField] BoxCollider2D playerCollider;
    public bool itsALadder;
    public bool ladderTop;
    private bool aboveCollider;
    private List<float> initialOpenDoorAlpha = new List<float>();
    private List<float> initialClosedDoorAlpha = new List<float>();
    private List<GameObject> openDoorSpriteList = new List<GameObject>();
    private List<GameObject> closedDoorSpriteList = new List<GameObject>();

    private Coroutine closedDoorFadeCoroutine;
    private Coroutine openDoorFadeCoroutine;
    public Coroutine thresholdSortingSequenceCoro;
    public Coroutine moveToCldrCenterCoro;

    private string initialSortingLayer;
    private bool playerIsInDoorway = false;
    private bool playerIsInRoomAbove = false;

    private string[] tagsToExludeEntExt = { "OpenDoor", "AlphaZeroEntExt" };

    private bool isMoving;
    

    public bool plyrCrsngLeft = false;


    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        characterMovement = Player.GetComponent<CharacterMovement>(); 
        playerCollider = Player.GetComponent<BoxCollider2D>();

        
        // get the sprites and add them to the -> corresponding lists 
        GetSprites(FindSiblingWithTag("OpenDoor"), openDoorSpriteList);
        GetSprites(FindSiblingWithTag("ClosedDoor"), closedDoorSpriteList);

        PopulateInitialColorLists();

        SetOpenDoorToZeroAlpha();

        SetClosedDoorToInitialAlpha(); 
        
        SetDoorState(false, false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
    
        if (collision.gameObject.tag == "Player")
        {    
            StopAllCoros();
            if (!itsALadder)
            {
                // // Get the bounds of the BoxCollider2D attached to the object that this script is attached to
                // Bounds triggerBounds = GetComponent<BoxCollider2D>().bounds;

                // // Find the point of contact (based on the collision between the player and the trigger object)
                // Vector2 contactPoint = collision.ClosestPoint(playerCollider.transform.position);

                // // Define regions within the trigger object's collider
                // Vector2 center = triggerBounds.center;
                // Vector2 max = triggerBounds.max;
                // Vector2 min = triggerBounds.min;

                // // Check if the contact point (from the player's collider) is within the top-right quarter of the other object's collider
                // if (contactPoint.x >= center.x)
                // {
                //     Debug.Log("founditRIGHT");
                //     // Execute your desired code here for top-right of the trigger object
                //     if(plyrCrsngLeft) 
                //     {
                //         // characterMovement.fixedDirectionLeftDiagonal = true; // fix the player in \ left diag way while inside the collider
                //         // characterMovement.fixedDirectionRightDiagonal = false; // fix the player in \ left diag way while inside the collider
                //     }
                //     else if (!plyrCrsngLeft)
                //     {
                //         // characterMovement.fixedDirectionRightDiagonal = true; // fix the player in / right diag way while inside the collider
                //         // characterMovement.fixedDirectionLeftDiagonal = false; // fix the player in / right diag way while inside the collider
                //     }
                // }
                // // Check if the contact point is within the top-left quarter of the other object's collider
                // else if (contactPoint.x <= center.x)
                // {
                //     Debug.Log("founditLEFT");
                //     // Execute your desired code here for top-left of the trigger object
                    
                // }
                // // Check if the contact point is within the bottom-left quarter of the other object's collider
                // else if (contactPoint.x <= center.x && contactPoint.y <= center.y)
                // {
                //     // Debug.Log("founditbottomLEFT");
                //     // Execute your desired code here for bottom-left of the trigger object
                // }
                // // Check if the contact point is within the bottom-right quarter of the other object's collider
                // else if (contactPoint.x >= center.x && contactPoint.y <= center.y)
                // {
                //     // Debug.Log("founditbottomRIGHT");
                //     // Execute your desired code here for bottom-right of the trigger object
                // }


                // Calculate the world space center of the collider
                // Vector3 worldCenter = transform.TransformPoint(GetComponent<Collider2D>().bounds.center);

                // moveToCldrCenterCoro = StartCoroutine(MovePlayerToCenter(collision.transform, worldCenter));

                characterMovement.motionDirection = "normal";


    //   if (angle > 292.5f && angle <= 337.5f)    { change = new Vector3(-1f,0.5f,0f); currentAnimationDirection = upLeftAnim;}
    //   if (angle > 22.5f && angle <= 67.5f)      { change = new Vector3(-1f,0.5f,0f); currentAnimationDirection = upLeftAnim;}
    //   if (angle > 337.5f && angle <= 360f)      { change = new Vector3(-1f,0.5f,0f); currentAnimationDirection = upLeftAnim;}
    //   if (angle >= 0f && angle <= 22.5f)         { change = new Vector3(-1f,0.5f,0f); currentAnimationDirection = upLeftAnim;}
    //   if (angle > 67.5f && angle <= 112.5f)     { change = new Vector3(-1f,0.5f,0f); currentAnimationDirection = upLeftAnim;}

    //   // if (angle > 247.5f && angle <= 292.5f)    { change = new Vector3(-1f,0.5f,0f); currentAnimationDirection = upLeftAnim;}
     
    //   if (angle > 202.5f && angle <= 247.5f)    { change = new Vector3(1f,-0.5f,0f); currentAnimationDirection = rightDownAnim;}
    //   if (angle > 112.5f && angle <= 157.5f)    { change = new Vector3(1f,-0.5f,0f); currentAnimationDirection = rightDownAnim;}
    //   if (angle > 157.5f && angle <= 202.5f)    { change = new Vector3(1f,-0.5f,0f); currentAnimationDirection = rightDownAnim;}
    //   if (angle > 247.5f && angle <= 292.5f)    { change = new Vector3(1f,-0.5f,0f); currentAnimationDirection = rightDownAnim;}

                if(plyrCrsngLeft) 
                {
                    characterMovement.fixedDirectionLeftDiagonal = true; // fix the player in \ left diag way while inside the collider
                    characterMovement.fixedDirectionRightDiagonal = false; // fix the player in \ left diag way while inside the collider

                    characterMovement.controlDirectionToPlayerDirection[Direction.Left] = Direction.UpLeft;
                    characterMovement.controlDirectionToPlayerDirection[Direction.UpLeft] = Direction.UpLeft;
                    characterMovement.controlDirectionToPlayerDirection[Direction.UpFacingLeft] = Direction.UpLeft;
                    characterMovement.controlDirectionToPlayerDirection[Direction.UpFacingRight] = Direction.UpLeft;
                    characterMovement.controlDirectionToPlayerDirection[Direction.UpRight] = Direction.UpLeft;
                    characterMovement.controlDirectionToPlayerDirection[Direction.Right] = Direction.RightDown;
                    characterMovement.controlDirectionToPlayerDirection[Direction.RightDown] = Direction.RightDown;
                    characterMovement.controlDirectionToPlayerDirection[Direction.DownFacingRight] = Direction.RightDown;
                    characterMovement.controlDirectionToPlayerDirection[Direction.DownFacingLeft] = Direction.RightDown;
                    characterMovement.controlDirectionToPlayerDirection[Direction.DownLeft] = Direction.RightDown;


                }
                else if (!plyrCrsngLeft)
                {
                    characterMovement.fixedDirectionRightDiagonal = true; // fix the player in / right diag way while inside the collider
                    characterMovement.fixedDirectionLeftDiagonal = false; // fix the player in / right diag way while inside the collider

                    characterMovement.controlDirectionToPlayerDirection[Direction.Left] = Direction.DownLeft;
                    characterMovement.controlDirectionToPlayerDirection[Direction.UpLeft] = Direction.UpRight;
                    characterMovement.controlDirectionToPlayerDirection[Direction.UpFacingLeft] = Direction.UpRight;
                    characterMovement.controlDirectionToPlayerDirection[Direction.UpFacingRight] = Direction.UpRight;
                    characterMovement.controlDirectionToPlayerDirection[Direction.UpRight] = Direction.UpRight;
                    characterMovement.controlDirectionToPlayerDirection[Direction.Right] = Direction.UpRight;
                    characterMovement.controlDirectionToPlayerDirection[Direction.RightDown] = Direction.DownLeft;
                    characterMovement.controlDirectionToPlayerDirection[Direction.DownFacingRight] = Direction.DownLeft;
                    characterMovement.controlDirectionToPlayerDirection[Direction.DownFacingLeft] = Direction.DownLeft;
                    characterMovement.controlDirectionToPlayerDirection[Direction.DownLeft] = Direction.DownLeft;
                }

                if (isPlayerCrossingUp())
                {
                    aboveCollider = false;
                }
                else
                {
                    aboveCollider = true;
                }




                SetClosedDoorToZeroAlpha();
                SetOpenDoorToInitialAlpha();
                SetPlayerIsInDoorway(true);
            }
            else // if it is a ladder
            {
                if(ladderTop)
                {
                    if (roomBelow != null || roomAbove != null)
                    {
                        roomBelow.ResetRoomPositions(); 
                    }
                }  
                else
                {
                    if (roomBelow != null || roomAbove != null)
                    {
                        // roomAbove.EnterRoom(true, 0.3f); 
                        roomBelow.EnterRoom(true, 0.3f); 
                    }
                }
            }
        }
        else
        { // for npc random movement
            if(collision.GetComponent<CharacterMovement>() != null && collision == collision.GetComponent<CharacterMovement>().boxCollider)
            {
                collision.GetComponent<CharacterMovement>().ReverseDirection();
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {    
            StopAllCoros();
            if(!itsALadder)
            {
                characterMovement.fixedDirectionLeftDiagonal = false;
                characterMovement.fixedDirectionRightDiagonal = false; // un-fix the player in \/ left/right diag way upon collider exit. 
                characterMovement.ResetPlayerMovement(); 

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
                            SetClosedDoorToInitialAlpha();
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
                            roomAbove.ExitRoomAndSetDoorwayInstances();
                        }   
                        else
                        if (!characterMovement.playerIsOutside && roomBelow != null) // if the player IS inside bulding and ABOVE collider, going down stairs and entering room downstairs
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

                        if (!characterMovement.playerIsOutside && aboveCollider) // if player is inside 
                        {
                            initialSortingLayer = Player.transform.Find("head").GetComponent<SpriteRenderer>().sortingLayerName;

                            thresholdSortingSequenceCoro = StartCoroutine(ThresholdLayerSortingSequence(0.3f, Player, "ThresholdSequence"));
                        }
                    }
                                
                    aboveCollider = false;

                }

                SetPlayerIsInDoorway(false);
            }
            else // if it is a ladder
            {
                if(isPlayerCrossingUp())
                {
                    if(characterMovement.playerIsOutside)
                    {
                        if (roomBelow != null || roomAbove != null)
                        {
                            roomBelow.ResetRoomPositions(); 
                        }
                    }   
                }  
                else
                {
                    if(!characterMovement.playerIsOutside)
                    {
                        if (roomBelow != null || roomAbove != null)
                        {
                            // roomAbove.EnterRoom(true, 0.3f); 
                            roomBelow.EnterRoom(true, 0.3f); 
                        }
                    }
                    else
                    {
                        if (roomBelow != null || roomAbove != null)
                        {
                            roomBelow.ResetRoomPositions(); 
                        }
                    }
                }
            }
        }
    }
    private bool isPlayerCrossingUp()
    {
        return GameObject.FindWithTag("Player").GetComponent<CharacterMovement>().change.y > 0;
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
        if (moveToCldrCenterCoro != null)
        {
            StopCoroutine(moveToCldrCenterCoro);
        }
    }

    IEnumerator treeFade(
    GameObject obj,
    float fadeTo,
    float elapsedTime,
    float waitTime = 0f) 
    {
        float fadeFrom = obj.GetComponent<SpriteRenderer>().color.a;
        float timer = 0.0f;

        if(waitTime != 0f)
        {
            yield return new WaitForSeconds(waitTime);
        }

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
        initialSortingLayer = Player.transform.Find("head").GetComponent<SpriteRenderer>().sortingLayerName; 
        // initialSortingLayer = LayerMask.LayerToName(this.gameObject.layer);

        SetTreeSortingLayer(gameObject, newSortingLayer);

        yield return new WaitForSeconds(waitTime);

        if (Player.transform.Find("head").GetComponent<SpriteRenderer>().sortingLayerName == newSortingLayer)
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
                    for (int j = 0; j < roomAbove.doorsBelow[i].closedDoorSpriteList.Count; j++)
                    {
                        this.closedDoorFadeCoroutine = StartCoroutine(treeFade(roomAbove.doorsBelow[i].closedDoorSpriteList[j], 0.3f, 0.3f));
                    }  
                    for (int k = 0; k < roomAbove.doorsBelow[i].closedDoorSpriteList.Count; k++)
                    {
                        SetTreeAlpha(roomAbove.doorsBelow[i].openDoorSpriteList[k], 0);
                    }
                  
                }

            }  
        }
        else if (characterMovement.playerIsOutside) {
            if(this.FindSiblingWithTag("OpenDoor") != null)
            {
                for (int i = 0; i < this.openDoorSpriteList.Count; i++)
                {
                    this.openDoorFadeCoroutine = StartCoroutine(treeFade(this.openDoorSpriteList[i], 0f, 0.3f));
                }  
            }
            if (roomAbove != null && roomBelow == null)
            {
                for (int i = 0; i < roomAbove.doorsBelow.Count; i++)
                {
                    for (int j = 0; j < roomAbove.doorsBelow[i].closedDoorSpriteList.Count; j++)
                    {
                        if(roomAbove.doorsBelow[i].ThisThresholdIsAnEntranceToTheBuildingOrIsStairs())
                        {
                            this.closedDoorFadeCoroutine = StartCoroutine(treeFade(roomAbove.doorsBelow[i].closedDoorSpriteList[j], 
                                                                            roomAbove.doorsBelow[i].initialClosedDoorAlpha[j], 0.3f, 0.3f)); 
                            // SetTreeAlpha(roomAbove.doorsBelow[i].closedDoorSpriteList[j], roomAbove.doorsBelow[i].initialClosedDoorAlpha[j]);
                        }
                    } 
                }   
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

    private IEnumerator MovePlayerToCenter(Transform player, Vector3 centerPoint)
    {
        isMoving = true;
        // Continue moving until the player reaches close enough to the center
        while (Vector2.Distance(player.position, centerPoint) > 0.1f)
        {
            // Move the player towards the center smoothly over time
            player.position = Vector2.MoveTowards(player.position, centerPoint, 6 * Time.deltaTime);
            yield return null; // Wait until the next frame before continuing the loop
        }

        isMoving = false;
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



