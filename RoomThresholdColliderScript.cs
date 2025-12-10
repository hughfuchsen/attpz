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
    CharacterMovement myCM;
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

    private Dictionary<GameObject, bool> aboveColliderByCharacter = new Dictionary<GameObject, bool>();



    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        // cm = Player.GetComponent<CharacterMovement>(); 
        playerCollider = Player.GetComponentInChildren<BoxCollider2D>();

        
        // get the sprites and add them to the -> corresponding lists 
        GetSprites(FindSiblingWithTag("OpenDoor"), openDoorSpriteList);
        GetSprites(FindSiblingWithTag("ClosedDoor"), closedDoorSpriteList);

        PopulateInitialColorLists();

        CloseDoor(); 

        myCM = Player.GetComponent<CharacterMovement>();
        
        SetDoorState(false, false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerCollider"))
        {   
            CharacterMovement cm = other.transform.parent.GetComponent<CharacterMovement>();
            bool isAbove = !IsCrossingUp(cm); // if crossing down, they’re above
            aboveColliderByCharacter[other.gameObject] = isAbove;

            cm.currentRoomThreshold = this;

            cm.playerOnThresh = true;

            StopAllCoros();
            if (!itsALadder)
            {
 
                cm.motionDirection = "normal";

                if(plyrCrsngLeft) 
                {
                    cm.fixedDirectionLeftDiagonal = true; // fix the player in \ left diag way while inside the collider
                    cm.fixedDirectionRightDiagonal = false; // fix the player in \ left diag way while inside the collider

                    cm.controlDirectionToPlayerDirection[Direction.Left] = Direction.UpLeft;
                    cm.controlDirectionToPlayerDirection[Direction.UpLeft] = Direction.UpLeft;
                    cm.controlDirectionToPlayerDirection[Direction.UpFacingLeft] = Direction.UpLeft;
                    cm.controlDirectionToPlayerDirection[Direction.UpFacingRight] = Direction.UpLeft;
                    cm.controlDirectionToPlayerDirection[Direction.UpRight] = Direction.UpLeft;
                    cm.controlDirectionToPlayerDirection[Direction.Right] = Direction.RightDown;
                    cm.controlDirectionToPlayerDirection[Direction.RightDown] = Direction.RightDown;
                    cm.controlDirectionToPlayerDirection[Direction.DownFacingRight] = Direction.RightDown;
                    cm.controlDirectionToPlayerDirection[Direction.DownFacingLeft] = Direction.RightDown;
                    cm.controlDirectionToPlayerDirection[Direction.DownLeft] = Direction.RightDown;


                }
                else if (!plyrCrsngLeft)
                {
                    cm.fixedDirectionRightDiagonal = true; // fix the player in / right diag way while inside the collider
                    cm.fixedDirectionLeftDiagonal = false; // fix the player in / right diag way while inside the collider

                    cm.controlDirectionToPlayerDirection[Direction.Left] = Direction.DownLeft;
                    cm.controlDirectionToPlayerDirection[Direction.UpLeft] = Direction.UpRight;
                    cm.controlDirectionToPlayerDirection[Direction.UpFacingLeft] = Direction.UpRight;
                    cm.controlDirectionToPlayerDirection[Direction.UpFacingRight] = Direction.UpRight;
                    cm.controlDirectionToPlayerDirection[Direction.UpRight] = Direction.UpRight;
                    cm.controlDirectionToPlayerDirection[Direction.Right] = Direction.UpRight;
                    cm.controlDirectionToPlayerDirection[Direction.RightDown] = Direction.DownLeft;
                    cm.controlDirectionToPlayerDirection[Direction.DownFacingRight] = Direction.DownLeft;
                    cm.controlDirectionToPlayerDirection[Direction.DownFacingLeft] = Direction.DownLeft;
                    cm.controlDirectionToPlayerDirection[Direction.DownLeft] = Direction.DownLeft;
                }



                OpenDoor();
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
        else if (other.CompareTag("NPCCollider")) //if it's an NPC
        {  
            CharacterMovement cm = other.transform.parent.GetComponent<CharacterMovement>();
            bool isAbove = !IsCrossingUp(cm); // if crossing down, they’re above
            aboveColliderByCharacter[other.gameObject] = isAbove;

            cm.currentRoomThreshold = this;

            cm.playerOnThresh = true;

            // StopAllCoros();
            if (!itsALadder)
            {
                if(plyrCrsngLeft) 
                {
                    cm.fixedDirectionLeftDiagonal = true; // fix the player in \ left diag way while inside the collider
                    cm.fixedDirectionRightDiagonal = false; // fix the player in \ left diag way while inside the collider

                    cm.controlDirectionToPlayerDirection[Direction.Left] = Direction.UpLeft;
                    cm.controlDirectionToPlayerDirection[Direction.UpLeft] = Direction.UpLeft;
                    cm.controlDirectionToPlayerDirection[Direction.UpFacingLeft] = Direction.UpLeft;
                    cm.controlDirectionToPlayerDirection[Direction.UpFacingRight] = Direction.UpLeft;
                    cm.controlDirectionToPlayerDirection[Direction.UpRight] = Direction.UpLeft;
                    cm.controlDirectionToPlayerDirection[Direction.Right] = Direction.RightDown;
                    cm.controlDirectionToPlayerDirection[Direction.RightDown] = Direction.RightDown;
                    cm.controlDirectionToPlayerDirection[Direction.DownFacingRight] = Direction.RightDown;
                    cm.controlDirectionToPlayerDirection[Direction.DownFacingLeft] = Direction.RightDown;
                    cm.controlDirectionToPlayerDirection[Direction.DownLeft] = Direction.RightDown;


                }
                else if (!plyrCrsngLeft)
                {
                    cm.fixedDirectionRightDiagonal = true; // fix the player in / right diag way while inside the collider
                    cm.fixedDirectionLeftDiagonal = false; // fix the player in / right diag way while inside the collider

                    cm.controlDirectionToPlayerDirection[Direction.Left] = Direction.DownLeft;
                    cm.controlDirectionToPlayerDirection[Direction.UpLeft] = Direction.UpRight;
                    cm.controlDirectionToPlayerDirection[Direction.UpFacingLeft] = Direction.UpRight;
                    cm.controlDirectionToPlayerDirection[Direction.UpFacingRight] = Direction.UpRight;
                    cm.controlDirectionToPlayerDirection[Direction.UpRight] = Direction.UpRight;
                    cm.controlDirectionToPlayerDirection[Direction.Right] = Direction.UpRight;
                    cm.controlDirectionToPlayerDirection[Direction.RightDown] = Direction.DownLeft;
                    cm.controlDirectionToPlayerDirection[Direction.DownFacingRight] = Direction.DownLeft;
                    cm.controlDirectionToPlayerDirection[Direction.DownFacingLeft] = Direction.DownLeft;
                    cm.controlDirectionToPlayerDirection[Direction.DownLeft] = Direction.DownLeft;
                }

                if(ThisThresholdIsAnEntranceToTheBuildingOrLevel() 
                    || (cm.currentLevel == myCM.currentLevel && cm.currentBuilding == myCM.currentBuilding))
                {
                    if(myCM.currentRoomThreshold != this)
                        OpenDoor();
                }
            }
            else // if it is a ladder
            {
                if(ladderTop)
                {
                    if (roomBelow != null || roomAbove != null)
                    {
                      
                    }
                }  
                else
                {
                    if (roomBelow != null || roomAbove != null)
                    {
                     
                    }
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerCollider")
        {    
            CharacterMovement cm = other.transform.parent.GetComponent<CharacterMovement>();

            cm.currentRoomThreshold = null;

            bool aboveCollider = aboveColliderByCharacter.ContainsKey(other.gameObject) && aboveColliderByCharacter[other.gameObject];
            
            cm.playerOnThresh = false;

            StopAllCoros();
            if(!itsALadder)
            {
                cm.fixedDirectionLeftDiagonal = false;
                cm.fixedDirectionRightDiagonal = false; // un-fix the player in \/ left/right diag way upon collider exit. 
                cm.ResetPlayerMovement(); 

                    //ON EXIT CROSSING UP
                if (IsCrossingUp(cm))
                {
                    if (ThisThresholdIsAnEntranceToTheBuildingOrLevel())
                    {
                        if((roomBelow != null && roomAbove != null) && roomBelow.level != roomAbove.level) // traversing levels
                        {
                            roomBelow.ResetRoomPositions(); 
                            roomAbove.EnterRoom(true, 0.3f);
                        }
                        else if (roomBelow != null && roomAbove == null) // if player is exiting the back of the building
                        { 
                            roomBelow.ResetRoomPositions(); 
                            roomBelow.ExitRoomAndSetDoorwayInstances(); 
                            CloseDoor();
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

                        if (roomBelow != null) // this first in order to correctly assign the current room
                        {
                            roomBelow.ExitRoomAndSetDoorwayInstances();
                        }
                        if (roomBelow != null && roomAbove == null) // if there is no room above the thresh i.e. if 
                                                                    // the player is leaving the level and the rooms need to go in their original pos.
                        {
                            roomBelow.ResetRoomPositions(); 
                        }
                        else if (roomAbove != null) 
                        {
                            roomAbove.EnterRoom(false, 0f);
                        }
                    }
                }  
                else //ON EXIT CROSSING DOWN
                {
                    if (roomAbove != null)
                    {
                        roomAbove.ExitRoomAndSetDoorwayInstances();
                        CloseDoor();
                    }

                    if (ThisThresholdIsAnEntranceToTheBuildingOrLevel())
                    {
                        if((roomBelow != null && roomAbove != null) && roomBelow.level != roomAbove.level) // traversing levels
                        {
                            roomAbove.ResetRoomPositions(); 
                            roomBelow.EnterRoom(true, 0.3f);
                        }
                        else if (roomBelow == null && roomAbove != null) // if the player IS inside bulding and ABOVE collider prior to collider exit
                        {
                            roomAbove.ResetRoomPositions();
                            roomAbove.ExitRoomAndSetDoorwayInstances();
                        }   
                        else
                        if (!cm.playerIsOutside && roomBelow != null) // if the player IS inside bulding and ABOVE collider, going down stairs and entering room downstairs
                        {
                            roomBelow.EnterRoom(false, 0f);
                            
                            //CLOSE DOOR
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

                        if (!cm.playerIsOutside && aboveCollider) // if player is inside 
                        {
                            initialSortingLayer = Player.transform.Find("head").GetComponent<SpriteRenderer>().sortingLayerName;

                            thresholdSortingSequenceCoro = StartCoroutine(ThresholdLayerSortingSequence(0.3f, Player, "ThresholdSequence"));
                        }
                    }
                }

                SetPlayerIsInDoorway(false);
            }
            else // if it is a ladder
            {
                if(IsCrossingUp(cm))
                {
                    if(cm.playerIsOutside)
                    {
                        if (roomBelow != null || roomAbove != null)
                        {
                            roomBelow.ResetRoomPositions(); 
                        }
                    }   
                }  
                else
                {
                    if(!cm.playerIsOutside)
                    {
                        if (roomBelow != null || roomAbove != null)
                        {
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
        else if (other.gameObject.tag == "NPCCollider")
        {    
            GameObject character = other.transform.parent.gameObject;

            CharacterMovement cm = other.transform.parent.GetComponent<CharacterMovement>();

            cm.currentRoomThreshold = null;

            bool aboveCollider = aboveColliderByCharacter.ContainsKey(other.gameObject) && aboveColliderByCharacter[other.gameObject];
            
            cm.playerOnThresh = false;

            // StopAllCoros();
            if(!itsALadder)
            {
                cm.fixedDirectionLeftDiagonal = false;
                cm.fixedDirectionRightDiagonal = false; // un-fix the player in \/ left/right diag way upon collider exit. 
                cm.ResetPlayerMovement(); 

                    //ON EXIT CROSSING UP
                if (IsCrossingUp(cm))
                {
                    if (ThisThresholdIsAnEntranceToTheBuildingOrLevel())
                    {
                        if (roomBelow != null && roomAbove == null) // if player is exiting the back of the building
                        { 
                            if(myCM.currentRoomThreshold != this)
                                CloseDoor();
                        }  
                        else if (roomAbove != null) // if player is entering the building from the fro
                        {

                            if(myCM.currentRoomThreshold != this)
                                CloseDoor();

                            // if(roomAbove.building != null)
                            roomAbove.NpcEnterRoom(character);

                            // {roomAbove.building.NpcEnterExitBuilding(character, true);}   

                            // if(roomAbove.level != null)
                            //         roomAbove.NpcEnterRoom(character);

                            // {roomAbove.level.NpcEnterLevel(character);}

    
                        }
                    }
                    else // if this threshold is just the entrance to a room and not a building entrance
                    {    
                        if(cm.currentBuilding == myCM.currentBuilding)
                        {    
                            if(myCM.currentRoomThreshold != this)       
                                CloseDoor();
                        } 

                        if (roomBelow != null) // this first in order to correctly assign the current room
                        {

                        }
                        if (roomBelow != null && roomAbove == null) // if there is no room above the thresh i.e. if 
                                                                    // the player is leaving the level and the rooms need to go in their original pos.
                        {
                          
                        }
                        else if (roomAbove != null) 
                        {
                            roomAbove.NpcEnterRoom(character);
                        }


                    }
                }  
                else //ON EXIT CROSSING DOWN
                {
                    if (roomAbove != null)
                    {
                        // roomAbove.ExitRoomAndSetDoorwayInstances();
                        // roomAbove.npcListForRoom.Remove(other.transform.parent.gameObject);
                        if(cm.currentBuilding == myCM.currentBuilding)
                        {
                            if(myCM.currentRoomThreshold != this)
                                CloseDoor();
                        }
                    }

                    if (ThisThresholdIsAnEntranceToTheBuildingOrLevel())
                    {
                        if (roomBelow == null && roomAbove != null) // if the player IS inside bulding and ABOVE collider prior to collider exit
                        {
                            // if(cm.previouseBuilding == myCM.currentBuilding)
                            // {
                                if(myCM.currentRoomThreshold != this)
                                    CloseDoor();
                            // }

                            // if(roomAbove.building != null)
                            // {roomAbove.building.NpcEnterExitBuilding(character, false);}   

                            // if(roomAbove.level != null)
                            // {roomAbove.level.NpcEnterLevel(character);}
                            roomAbove.NpcExitRoom(character);
                            // cm.currentBuilding = null;
                        }   
                        else
                        if (!cm.playerIsOutside && roomBelow != null) // if the npc IS inside bulding and ABOVE collider, going down stairs and entering room downstairs
                        {
                            if(roomBelow.building != null)
                            // {roomBelow.building.NpcEnterExitBuilding(character, true);}   

                            if(roomBelow.level != null)
                            // {roomBelow.level.NpcEnterLevel(character);}

                            roomBelow.NpcEnterRoom(character);

                            //CLOSE DOOR
                                SetClosedDoorToInitialAlpha();
                                for (int i = 0; i < openDoorSpriteList.Count; i++)
                                { 
                                    SetTreeAlpha(this.FindSiblingWithTag("OpenDoor"), 0);
                                }  
                        }    
                    }
                    else if (roomBelow != null) // entering a room going down while inside building
                    {
                        roomBelow.NpcEnterRoom(character);

                        if (!cm.playerIsOutside && aboveCollider) // if player is inside 
                        {

                        }
                    }

                }
            }
            else // if it is a ladder
            {
                if(IsCrossingUp(cm))
                {
                    if(cm.playerIsOutside)
                    {
                        if (roomBelow != null || roomAbove != null)
                        {

                        }
                    }   
                }  
                else
                {
                    if(!cm.playerIsOutside)
                    {
                        if (roomBelow != null || roomAbove != null)
                        {
                            roomBelow.NpcEnterRoom(character);
                        }
                    }
                    else
                    {
                        if (roomBelow != null || roomAbove != null)
                        {

                        }
                    }
                }
            }
        }

        aboveColliderByCharacter.Remove(other.gameObject);
    }
    private bool IsCrossingUp(CharacterMovement cm)
    {
        return cm.change.y > 0;
    }

    void OpenDoor()
    {
        SetClosedDoorToZeroAlpha();
        SetOpenDoorToInitialAlpha();
    }
    void CloseDoor()
    {
        SetClosedDoorToInitialAlpha();
        SetOpenDoorToZeroAlpha();
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

    bool ThisThresholdIsAnEntranceToTheBuildingOrLevel()
    {
            if (this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() != null
            ||  this.transform.parent.GetComponentInChildren<LevelThreshColliderScript>() != null
            || this.transform.parent.GetComponentInChildren<InclineThresholdColliderScript>() != null)
            {
                return true;
            } 
            return false;
    }
    bool ThisThresholdHasLevelScript()
    {
            if (this.transform.parent.GetComponentInChildren<LevelThreshColliderScript>() != null)
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
                    if(myCM.currentRoomThreshold != roomAbove.doorsBelow[i])
                    {
                        for (int j = 0; j < roomAbove.doorsBelow[i].closedDoorSpriteList.Count; j++)
                        {
                            this.closedDoorFadeCoroutine = StartCoroutine(treeFade(roomAbove.doorsBelow[i].closedDoorSpriteList[j], 0.4f, 0.3f));
                        }  
                        for (int k = 0; k < roomAbove.doorsBelow[i].closedDoorSpriteList.Count; k++)
                        {
                            SetTreeAlpha(roomAbove.doorsBelow[i].openDoorSpriteList[k], 0);
                        }
                    }
                  
                }

            }  
        }
        else if (myCM.playerIsOutside) {
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
                    if(myCM.currentRoomThreshold != roomAbove.doorsBelow[i])
                    {
                        for (int j = 0; j < roomAbove.doorsBelow[i].closedDoorSpriteList.Count; j++)
                        {
                            if(roomAbove.doorsBelow[i].ThisThresholdIsAnEntranceToTheBuildingOrLevel())
                            {
                                this.closedDoorFadeCoroutine = StartCoroutine(treeFade(roomAbove.doorsBelow[i].closedDoorSpriteList[j], 
                                                                                roomAbove.doorsBelow[i].initialClosedDoorAlpha[j], 0.4f, 0.3f)); 
                                // SetTreeAlpha(roomAbove.doorsBelow[i].closedDoorSpriteList[j], roomAbove.doorsBelow[i].initialClosedDoorAlpha[j]);
                            }
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
                    if(myCM.currentRoomThreshold != roomAbove.doorsBelow[i])
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


