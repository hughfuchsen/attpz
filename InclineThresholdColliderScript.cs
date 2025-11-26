using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InclineThresholdColliderScript : MonoBehaviour
{
    // CharacterMovement cm;
    CharacterAnimation myCharacterAnimation;
    IsoSpriteSorting isoSpriteSortingScript;
    [SerializeField] GameObject Player;

    [HideInInspector] public BuildingScript building = null;

    [SerializeField] LevelScript levelItLeaves;
    public string motionDirection;
    public Coroutine thresholdSortingSequenceCoro;
    public string lowerSortingLayerToAssign;
    public string higherSortingLayerToAssign;
    public string lowerColliderLayerName; // The name of the layer you want to switch to
    public string higherColliderLayerName; // The name of the layer you want to switch to
    public bool topOfIncline;
    public bool middleOfIncline;
    public bool itsALadder;
    public int ladderHeight = 0;
    public bool plyrCrsngLeft = false; 
    public bool straightMiddleLanding = false; 
    public enum InFrontOrBehindLadder {none, inFrontOfLadder, behindLadder}
    public InFrontOrBehindLadder inFrontOrBehindLadder;

    // Start is called before the first frame update
    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player"); // assign to player
        // playerCm = Player.GetComponent<CharacterMovement>();
        myCharacterAnimation = Player.GetComponent<CharacterAnimation>();
        isoSpriteSortingScript = Player.GetComponent<IsoSpriteSorting>();

        if(!itsALadder)
            CheckIfTheStairsHaveAStraightMiddleLanding();

        building = FindParentByBuildingScriptComponent();
    }

    void Start()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("BAM");
        if (other.CompareTag("PlayerCollider"))
        {
            CharacterMovement cm = other.transform.parent.GetComponent<CharacterMovement>();
            cm.currentInclineThreshold = this;

            BoxCollider2D bc = other.GetComponent<BoxCollider2D>();// this is the playa'z collider
            BoxCollider2D thisCollider = GetComponent<BoxCollider2D>(); // this collider belongs to the trigger

            cm.playerOnThresh = true;

            if (!itsALadder) // if it's not a ladder
            {
                cm.motionDirection = "normal";  

                if(plyrCrsngLeft == true)
                {
                    // fixed direction left
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
                else
                {
                    // fixed direction right
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

                // //alter the motion direction
                // if((IsCrossingUp(cm) && cm.motionDirection == "normal" && !topOfIncline) 
                //     || (!IsCrossingUp(cm) && cm.motionDirection == "normal" && topOfIncline))
                // {
                //     cm.motionDirection = motionDirection;  
                // }
                // else if((IsCrossingUp(cm) && cm.motionDirection != "normal" && topOfIncline) 
                //     || (!IsCrossingUp(cm) && cm.motionDirection != "normal" && !topOfIncline))
                // {
                //     cm.motionDirection = "normal";  
                // }

                // alter the collider layer and sprite sorting layer that is active with the player
                if(IsCrossingUp(cm) && topOfIncline)
                {
                    // gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
                    SetCollisionLayer(higherColliderLayerName);
                    SetTreeSortingLayer(other.transform.parent.gameObject, higherSortingLayerToAssign);
                }
                else if((IsCrossingUp(cm)||(IsCrossingLeft(cm)&&plyrCrsngLeft)||!(IsCrossingLeft(cm)&&plyrCrsngLeft)) 
                        && !topOfIncline) // bottom bottom of stairs
                {
                    // gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
                    if(!middleOfIncline)
                    {
                        // 2. Re-enable collision with the other NPC's collider
                        // Physics2D.IgnoreCollision(bc, thisCollider, false);

                        // 3. Re-enable collision with all sibling trigger objects
                        foreach (Transform sibling in transform.parent) // or transform if same level
                        {
                            if (sibling.CompareTag("Trigger"))
                            {
                               BoxCollider2D siblingCollider = sibling.GetComponent<BoxCollider2D>();
                                if (siblingCollider != null)
                                {
                                    Physics2D.IgnoreCollision(bc, siblingCollider, false);
                                }
                            }
                        }
                    }

                    SetCollisionLayer(higherColliderLayerName);
                    SetTreeSortingLayer(other.transform.parent.gameObject, higherSortingLayerToAssign);
                }                       
                else if((!IsCrossingUp(cm)||(IsCrossingLeft(cm)&&plyrCrsngLeft)||!(IsCrossingLeft(cm)&&plyrCrsngLeft)) 
                && topOfIncline) // top top of stairs
                {
                    // gameObject.layer = LayerMask.NameToLayer(lowerColliderLayerName);
                    if(!middleOfIncline)
                    {
                        // 2. Re-enable collision with the other NPC's collider
                        // Physics2D.IgnoreCollision(bc, thisCollider, false);

                        // 3. Re-enable collision with all sibling trigger objects
                        foreach (Transform sibling in transform.parent) // or transform if same level
                        {
                            if (sibling.CompareTag("Trigger"))
                            {
                               BoxCollider2D siblingCollider = sibling.GetComponent<BoxCollider2D>();
                                if (siblingCollider != null)
                                {
                                    Physics2D.IgnoreCollision(bc, siblingCollider, false);
                                }
                            }
                        }
                    }

                    SetCollisionLayer(lowerColliderLayerName);
                    SetTreeSortingLayer(other.transform.parent.gameObject, lowerSortingLayerToAssign);
                } 
                else if(!IsCrossingUp(cm) && !topOfIncline)
                {
                    // gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
                    SetCollisionLayer(higherColliderLayerName);
                    SetTreeSortingLayer(other.transform.parent.gameObject, higherSortingLayerToAssign);
                } 


                    // if (Player.GetComponent<SpriteRenderer>().sortingLayerName == "ThresholdSequence")                
                    // {
                    //     if (this.gameObject.layer == LayerMask.NameToLayer("Default"))
                    //     {
                    //         SetTreeSortingLayer(Player, "Level0");
                    //     }
                    //     else
                    //     {
                    //         SetTreeSortingLayer(Player, LayerMask.LayerToName(this.gameObject.layer));
                    //     }
                    // }
            }  
            else // if it is a ladder, baby
            {
                //alter the motion direction
                if(cm.motionDirection == "normal")
                {
                    if(inFrontOrBehindLadder == InFrontOrBehindLadder.inFrontOfLadder)
                    {
                        if(cm.facingLeft == true)
                        {
                            myCharacterAnimation.currentAnimationDirection = myCharacterAnimation.upLeftAnim;
                        }
                        else
                        {                           
                            myCharacterAnimation.currentAnimationDirection = myCharacterAnimation.upRightAnim;
                        }
                    }
                    else if(inFrontOrBehindLadder == InFrontOrBehindLadder.behindLadder)
                    {
                        if(cm.facingLeft == true)
                        {
                            myCharacterAnimation.currentAnimationDirection = myCharacterAnimation.leftDownAnim;
                        }
                        else
                        {                           
                            myCharacterAnimation.currentAnimationDirection = myCharacterAnimation.rightDownAnim;
                        }
                    }
                    else if(inFrontOrBehindLadder == InFrontOrBehindLadder.none) 
                    {
                        // do nutting;
                    }


                    // anchor player to ladder
                    if(!topOfIncline)
                    {
                        Player.transform.position = FindSiblingWithTag("Ladder").transform.position - isoSpriteSortingScript.SorterPositionOffset;
                    }
                    else
                    {
                        Player.transform.position = FindSiblingWithTag("Ladder").transform.position - isoSpriteSortingScript.SorterPositionOffset + new Vector3(0, ladderHeight, 0);
                    }

                    if(IsCrossingUp(cm))
                    {
                        if(!topOfIncline)
                        {
                            cm.motionDirection = "upLadder";
                        }
                        else
                        {
                            cm.motionDirection = "downLadder";
                        }
                        //add certain animation and anchoring here
                        gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
                        SetCollisionLayer(higherColliderLayerName);
                    }
                    else if(!IsCrossingUp(cm))
                    {
                        //add certain animation 
                        if(!topOfIncline)
                        {
                            cm.motionDirection = "upLadder";
                        }
                        else
                        {
                            cm.motionDirection = "downLadder";
                            
                        }
                        gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
                        SetCollisionLayer(higherColliderLayerName);
                    }
                    isoSpriteSortingScript.isMovable = false;
                }
                else if((IsCrossingUp(cm) && cm.motionDirection != "normal")) // on ladder and exiting at top of ladder
                {
                    cm.motionDirection = "normal";  
                    SetTreeSortingLayer(other.transform.parent.gameObject, higherSortingLayerToAssign);
                    isoSpriteSortingScript.isMovable = true;
                }
                // else if((!IsCrossingUp(cm) && cm.motionDirection != "normal")) // on ladder and exiting at bottom of ladder
                // {
                //     // cm.motionDirection = "normal"; 
                //     // gameObject.layer = LayerMask.NameToLayer(lowerColliderLayerName);
                //     // SetCollisionLayer(lowerColliderLayerName);
                //     // SetTreeSortingLayer(other.transform.parent.gameObject, lowerSortingLayerToAssign);
                //     // isoSpriteSortingScript.isMovable = true;
                // }

                // alter the collider layer and sprite sorting layer that is active with the player
                // if(!topOfIncline)
                // {
                //     gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
                //     SetCollisionLayer(higherColliderLayerName);
                //     SetTreeSortingLayer(other.transform.parent.gameObject, higherSortingLayerToAssign);
                // }                   
                // else
                // {
                //     gameObject.layer = LayerMask.NameToLayer(lowerColliderLayerName);
                //     SetCollisionLayer(lowerColliderLayerName);
                //     SetTreeSortingLayer(other.transform.parent.gameObject, lowerSortingLayerToAssign);
                // } 

            }

            
        }

    //     void OnTriggerEnter2D(Collider2D other)
    // { 
        if (other.CompareTag("NPCCollider"))
        {
            CharacterMovement cm = other.transform.parent.GetComponent<CharacterMovement>();
            cm.currentInclineThreshold = this;
            BoxCollider2D bc = other.GetComponent<BoxCollider2D>();// this is the npc's collider
            BoxCollider2D thisCollider = GetComponent<BoxCollider2D>(); // this collider belongs to the trigger

            // cm.playerOnThresh = true;

            if (!itsALadder) // if it's not a ladder
            {
                cm.motionDirection = "normal";  

                if(plyrCrsngLeft == true)
                {
                    // fixed direction left
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
                else
                {
                    // fixed direction right
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

                // //alter the motion direction
                // if((IsCrossingUp(cm) && cm.motionDirection == "normal" && !topOfIncline) 
                //     || (!IsCrossingUp(cm) && cm.motionDirection == "normal" && topOfIncline))
                // {
                //     cm.motionDirection = motionDirection;  
                // }
                // else if((IsCrossingUp(cm) && cm.motionDirection != "normal" && topOfIncline) 
                //     || (!IsCrossingUp(cm) && cm.motionDirection != "normal" && !topOfIncline))
                // {
                //     cm.motionDirection = "normal";  
                // }

                // alter the collider layer and sprite sorting layer that is active with the player
                if(IsCrossingUp(cm) && topOfIncline)
                {
                    // gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
                    // SetCollisionLayer(higherColliderLayerName);
                    SetTreeObjectLayer(other.transform.parent.gameObject, LayerMask.NameToLayer(higherColliderLayerName));
                    SetTreeSortingLayer(other.transform.parent.gameObject, higherSortingLayerToAssign);
                }
                else if((IsCrossingUp(cm)||(IsCrossingLeft(cm)&&plyrCrsngLeft)||!(IsCrossingLeft(cm)&&plyrCrsngLeft))
                     && !topOfIncline)
                {
                    if(!middleOfIncline)
                    {
                        // SetTreeObjectLayer(other.transform.parent.gameObject, LayerMask.NameToLayer(higherColliderLayerName));

                        // 2. Re-enable collision with the other NPC's collider
                        // Physics2D.IgnoreCollision(bc, thisCollider, false);

                        // 3. Re-enable collision with all sibling trigger objects
                        foreach (Transform sibling in transform.parent) // or transform if same level
                        {
                            if (sibling.CompareTag("Trigger"))
                            {
                               BoxCollider2D siblingCollider = sibling.GetComponent<BoxCollider2D>();
                                if (siblingCollider != null)
                                {
                                    Physics2D.IgnoreCollision(bc, siblingCollider, false);
                                    // // Debug.Log("shiverS!!");

                                }
                            }
                        }
                    }

                    // gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
                    // SetCollisionLayer(higherColliderLayerName);
                    SetTreeObjectLayer(other.transform.parent.gameObject, LayerMask.NameToLayer(higherColliderLayerName));
                    SetTreeSortingLayer(other.transform.parent.gameObject, higherSortingLayerToAssign);
                    // bc.excludealllayeroverridesexcepthigherColliderLayerNameandlowerColliderLayerName



                }                       
                else if((!IsCrossingUp(cm)||(IsCrossingLeft(cm)&&plyrCrsngLeft)||!(IsCrossingLeft(cm)&&plyrCrsngLeft))
                     && topOfIncline)
                {
                    if(!middleOfIncline)
                    {
                        // 2. Re-enable collision with the other NPC's collider
                        // Physics2D.IgnoreCollision(bc, thisCollider, false);

                        // 3. Re-enable collision with all sibling trigger objects
                        foreach (Transform sibling in transform.parent) // or transform if same level
                        {
                            if (sibling.CompareTag("Trigger"))
                            {
                               BoxCollider2D siblingCollider = sibling.GetComponent<BoxCollider2D>();
                                if (siblingCollider != null)
                                {
                                    Physics2D.IgnoreCollision(bc, siblingCollider, false);
                                     // Debug.Log("shiverS!!");

                                }
                            }
                        }
                    }
                    // gameObject.layer = LayerMask.NameToLayer(lowerColliderLayerName);
                    // SetCollisionLayer(lowerColliderLayerName);
                    SetTreeObjectLayer(other.transform.parent.gameObject, LayerMask.NameToLayer(lowerColliderLayerName));
                    SetTreeSortingLayer(other.transform.parent.gameObject, lowerSortingLayerToAssign);
                } 
                else if(!IsCrossingUp(cm) && !topOfIncline)
                {
                    // gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
                    // SetCollisionLayer(higherColliderLayerName);
                    SetTreeObjectLayer(other.transform.parent.gameObject, LayerMask.NameToLayer(higherColliderLayerName));
                    SetTreeSortingLayer(other.transform.parent.gameObject, higherSortingLayerToAssign);
                } 


                    // if (Player.GetComponent<SpriteRenderer>().sortingLayerName == "ThresholdSequence")                
                    // {
                    //     if (this.gameObject.layer == LayerMask.NameToLayer("Default"))
                    //     {
                    //         SetTreeSortingLayer(Player, "Level0");
                    //     }
                    //     else
                    //     {
                    //         SetTreeSortingLayer(Player, LayerMask.LayerToName(this.gameObject.layer));
                    //     }
                    // }
            }
            else // if it is a ladder, baby
            {
                //alter the motion direction
                if(cm.motionDirection == "normal")
                {
                    if(inFrontOrBehindLadder == InFrontOrBehindLadder.inFrontOfLadder)
                    {
                        if(cm.facingLeft == true)
                        {
                            myCharacterAnimation.currentAnimationDirection = myCharacterAnimation.upLeftAnim;
                        }
                        else
                        {                           
                            myCharacterAnimation.currentAnimationDirection = myCharacterAnimation.upRightAnim;
                        }
                    }
                    else if(inFrontOrBehindLadder == InFrontOrBehindLadder.behindLadder)
                    {
                        if(cm.facingLeft == true)
                        {
                            myCharacterAnimation.currentAnimationDirection = myCharacterAnimation.leftDownAnim;
                        }
                        else
                        {                           
                            myCharacterAnimation.currentAnimationDirection = myCharacterAnimation.rightDownAnim;
                        }
                    }
                    else if(inFrontOrBehindLadder == InFrontOrBehindLadder.none) 
                    {
                        // do nutting;
                    }


                    // anchor player to ladder
                    if(!topOfIncline)
                    {
                        Player.transform.position = FindSiblingWithTag("Ladder").transform.position - isoSpriteSortingScript.SorterPositionOffset;
                    }
                    else
                    {
                        Player.transform.position = FindSiblingWithTag("Ladder").transform.position - isoSpriteSortingScript.SorterPositionOffset + new Vector3(0, ladderHeight, 0);
                    }

                    if(IsCrossingUp(cm))
                    {
                        if(!topOfIncline)
                        {
                            cm.motionDirection = "upLadder";
                        }
                        else
                        {
                            cm.motionDirection = "downLadder";
                        }
                        //add certain animation and anchoring here
                        // gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
                        // SetCollisionLayer(higherColliderLayerName);
                        SetTreeObjectLayer(other.transform.parent.gameObject, LayerMask.NameToLayer(higherColliderLayerName));
                    }
                    else if(!IsCrossingUp(cm))
                    {
                        //add certain animation 
                        if(!topOfIncline)
                        {
                            cm.motionDirection = "upLadder";
                        }
                        else
                        {
                            cm.motionDirection = "downLadder";
                            
                        }
                        // gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
                        // SetCollisionLayer(higherColliderLayerName);
                        SetTreeObjectLayer(other.transform.parent.gameObject, LayerMask.NameToLayer(higherColliderLayerName));
                    }
                    isoSpriteSortingScript.isMovable = false;
                }
                else if((IsCrossingUp(cm) && cm.motionDirection != "normal")) // on ladder and exiting at top of ladder
                {
                    cm.motionDirection = "normal";  
                    SetTreeSortingLayer(other.transform.parent.gameObject, higherSortingLayerToAssign);
                    isoSpriteSortingScript.isMovable = true;
                }
                // else if((!IsCrossingUp(cm) && cm.motionDirection != "normal")) // on ladder and exiting at bottom of ladder
                // {
                //     // cm.motionDirection = "normal"; 
                //     // gameObject.layer = LayerMask.NameToLayer(lowerColliderLayerName);
                //     // SetCollisionLayer(lowerColliderLayerName);
                //     // SetTreeSortingLayer(other.transform.parent.gameObject, lowerSortingLayerToAssign);
                //     // isoSpriteSortingScript.isMovable = true;
                // }

                // alter the collider layer and sprite sorting layer that is active with the player
                // if(!topOfIncline)
                // {
                //     gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
                //     SetCollisionLayer(higherColliderLayerName);
                //     SetTreeSortingLayer(other.transform.parent.gameObject, higherSortingLayerToAssign);
                // }                   
                // else
                // {
                //     gameObject.layer = LayerMask.NameToLayer(lowerColliderLayerName);
                //     SetCollisionLayer(lowerColliderLayerName);
                //     SetTreeSortingLayer(other.transform.parent.gameObject, lowerSortingLayerToAssign);
                // } 

            }

            
        }
    }


 


    void OnTriggerExit2D(Collider2D other)
    {

            if(other.CompareTag("PlayerCollider"))
            {
                CharacterMovement cm = other.transform.parent.GetComponent<CharacterMovement>();
                cm.currentInclineThreshold = null;

                BoxCollider2D bc = other.GetComponent<BoxCollider2D>();// this is the playa's collider
                BoxCollider2D thisCollider = GetComponent<BoxCollider2D>(); // this collider belongs to the trigger

                cm.playerOnThresh = false;

                if (!itsALadder)
                {
                    if((IsCrossingUp(cm) && cm.motionDirection == "normal" && !topOfIncline) 
                        || (!IsCrossingUp(cm) && cm.motionDirection == "normal" && topOfIncline))
                    {
                        cm.motionDirection = motionDirection;  
                    }
                    else if((IsCrossingUp(cm) && cm.motionDirection != "normal" && topOfIncline) 
                        || (!IsCrossingUp(cm) && cm.motionDirection != "normal" && !topOfIncline))
                    {
                        cm.motionDirection = "normal";  
                    }

                    if(IsCrossingUp(cm) && topOfIncline) // leaving stairs!!!
                    {
                        // gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);

                        if(!middleOfIncline)
                        {
                            if(building != null)
                            {
                                foreach(InclineThresholdColliderScript inclineThreshScript in building.inclineThresholdColliderScripts)
                                {
                                    if(inclineThreshScript == this)
                                        continue;

                                    BoxCollider2D threshCol = inclineThreshScript.GetComponent<BoxCollider2D>();
                                    Physics2D.IgnoreCollision(bc, threshCol, true);
                                }
                                foreach(InclineThresholdColliderScript inclineThreshScript in cm.currentLevel.inclineEntrances)
                                {
                                    BoxCollider2D threshCol = inclineThreshScript.GetComponent<BoxCollider2D>();
                                    Physics2D.IgnoreCollision(bc, threshCol, false);
                                }


                            }

                            // // 3. Re-enable collision with all sibling trigger objects
                            // foreach (Transform sibling in transform.parent) // or transform if same level
                            // {
                            //     if (sibling.CompareTag("Trigger"))
                            //     {
                            //     BoxCollider2D siblingCollider = sibling.GetComponent<BoxCollider2D>();
                            //         if (siblingCollider != null)
                            //         {
                            //             Physics2D.IgnoreCollision(bc, siblingCollider, true);
                            //         }
                            //     }
                            // }

                            Physics2D.IgnoreCollision(bc, thisCollider, false);
                        }
                        else // if at a middle landing and the stairs are straight
                        {
                            if(straightMiddleLanding)
                            {
                                if(plyrCrsngLeft == true)
                                {
                                    // fixed direction left (on landing)
                                    cm.controlDirectionToPlayerDirection[Direction.Left] = Direction.UpLeft;
                                    cm.controlDirectionToPlayerDirection[Direction.UpLeft] = Direction.UpLeft;
                                    cm.controlDirectionToPlayerDirection[Direction.UpFacingLeft] = Direction.UpLeft;
                                    cm.controlDirectionToPlayerDirection[Direction.UpFacingRight] = Direction.UpLeft;
                                    cm.controlDirectionToPlayerDirection[Direction.UpRight] = Direction.UpRight;
                                    cm.controlDirectionToPlayerDirection[Direction.Right] = Direction.RightDown;
                                    cm.controlDirectionToPlayerDirection[Direction.RightDown] = Direction.RightDown;
                                    cm.controlDirectionToPlayerDirection[Direction.DownFacingRight] = Direction.RightDown;
                                    cm.controlDirectionToPlayerDirection[Direction.DownFacingLeft] = Direction.RightDown;
                                    cm.controlDirectionToPlayerDirection[Direction.DownLeft] = Direction.DownLeft;
                                }
                                else
                                {
                                    // fixed direction right (on landing)
                                    cm.controlDirectionToPlayerDirection[Direction.Left] = Direction.DownLeft;
                                    cm.controlDirectionToPlayerDirection[Direction.UpLeft] = Direction.UpLeft;
                                    cm.controlDirectionToPlayerDirection[Direction.UpFacingLeft] = Direction.UpRight;
                                    cm.controlDirectionToPlayerDirection[Direction.UpFacingRight] = Direction.UpRight;
                                    cm.controlDirectionToPlayerDirection[Direction.UpRight] = Direction.UpRight;
                                    cm.controlDirectionToPlayerDirection[Direction.Right] = Direction.UpRight;
                                    cm.controlDirectionToPlayerDirection[Direction.RightDown] = Direction.RightDown;
                                    cm.controlDirectionToPlayerDirection[Direction.DownFacingRight] = Direction.DownLeft;
                                    cm.controlDirectionToPlayerDirection[Direction.DownFacingLeft] = Direction.DownLeft;
                                    cm.controlDirectionToPlayerDirection[Direction.DownLeft] = Direction.DownLeft;
                                }
                            }
                        }

                        SetCollisionLayer(higherColliderLayerName);
                        SetTreeSortingLayer(other.transform.parent.gameObject, higherSortingLayerToAssign);
                    }            
                    else if(!IsCrossingUp(cm) && !topOfIncline)
                    {
                        // gameObject.layer = LayerMask.NameToLayer(lowerColliderLayerName);

                        if(!middleOfIncline)
                        {
                            if(building != null)
                            {
                                foreach(InclineThresholdColliderScript inclineThreshScript in building.inclineThresholdColliderScripts)
                                {
                                    if(inclineThreshScript == this)
                                        continue;
                                        
                                    BoxCollider2D threshCol = inclineThreshScript.GetComponent<BoxCollider2D>();
                                    Physics2D.IgnoreCollision(bc, threshCol, true);
                                }
                                foreach(InclineThresholdColliderScript inclineThreshScript in cm.currentLevel.inclineEntrances)
                                {
                                    BoxCollider2D threshCol = inclineThreshScript.GetComponent<BoxCollider2D>();
                                    Physics2D.IgnoreCollision(bc, threshCol, false);
                                }
                            }
                            // else
                            // {
                            //     // 3. Re-enable collision with all sibling trigger objects
                            //     foreach (Transform sibling in transform.parent) // or transform if same level
                            //     {
                            //         if (sibling.CompareTag("Trigger"))
                            //         {
                            //         BoxCollider2D siblingCollider = sibling.GetComponent<BoxCollider2D>();
                            //             if (siblingCollider != null)
                            //             {
                            //                 Physics2D.IgnoreCollision(bc, siblingCollider, true);
                            //             }
                            //         }
                            //     }
                            // }

                            Physics2D.IgnoreCollision(bc, thisCollider, false);
                        }
                        else // if at a middle landing and the stairs are straight
                        {
                            if(straightMiddleLanding)
                            {
                                if(plyrCrsngLeft == true)
                                {
                                    // fixed direction left (on landing)
                                    cm.controlDirectionToPlayerDirection[Direction.Left] = Direction.UpLeft;
                                    cm.controlDirectionToPlayerDirection[Direction.UpLeft] = Direction.UpLeft;
                                    cm.controlDirectionToPlayerDirection[Direction.UpFacingLeft] = Direction.UpLeft;
                                    cm.controlDirectionToPlayerDirection[Direction.UpFacingRight] = Direction.UpLeft;
                                    cm.controlDirectionToPlayerDirection[Direction.UpRight] = Direction.UpRight;
                                    cm.controlDirectionToPlayerDirection[Direction.Right] = Direction.RightDown;
                                    cm.controlDirectionToPlayerDirection[Direction.RightDown] = Direction.RightDown;
                                    cm.controlDirectionToPlayerDirection[Direction.DownFacingRight] = Direction.RightDown;
                                    cm.controlDirectionToPlayerDirection[Direction.DownFacingLeft] = Direction.RightDown;
                                    cm.controlDirectionToPlayerDirection[Direction.DownLeft] = Direction.DownLeft;
                                }
                                else
                                {
                                    // fixed direction right (on landing)
                                    cm.controlDirectionToPlayerDirection[Direction.Left] = Direction.DownLeft;
                                    cm.controlDirectionToPlayerDirection[Direction.UpLeft] = Direction.UpLeft;
                                    cm.controlDirectionToPlayerDirection[Direction.UpFacingLeft] = Direction.UpRight;
                                    cm.controlDirectionToPlayerDirection[Direction.UpFacingRight] = Direction.UpRight;
                                    cm.controlDirectionToPlayerDirection[Direction.UpRight] = Direction.UpRight;
                                    cm.controlDirectionToPlayerDirection[Direction.Right] = Direction.UpRight;
                                    cm.controlDirectionToPlayerDirection[Direction.RightDown] = Direction.RightDown;
                                    cm.controlDirectionToPlayerDirection[Direction.DownFacingRight] = Direction.DownLeft;
                                    cm.controlDirectionToPlayerDirection[Direction.DownFacingLeft] = Direction.DownLeft;
                                    cm.controlDirectionToPlayerDirection[Direction.DownLeft] = Direction.DownLeft;
                                }
                            }
                        }

                        SetCollisionLayer(lowerColliderLayerName);
                        SetTreeSortingLayer(other.transform.parent.gameObject, lowerSortingLayerToAssign);
                    }
                    else if(!IsCrossingUp(cm) && topOfIncline)
                    {
                        // gameObject.layer = LayerMask.NameToLayer(lowerColliderLayerName);
                        SetCollisionLayer(lowerColliderLayerName);
                        SetTreeSortingLayer(other.transform.parent.gameObject, lowerSortingLayerToAssign);
                    }
                    
                    // un-fix the player in \/ left/right diag way upon collider exit. 
                    if(!middleOfIncline || !straightMiddleLanding)
                    cm.ResetPlayerMovement();                 
                }
                else //if it is a ladder!
                {
                    //alter the motion direction
                    if(cm.motionDirection == "normal")
                    {
                        if(topOfIncline)
                        {
                            gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
                            SetCollisionLayer(higherColliderLayerName);
                        }
                        else
                        {
                            gameObject.layer = LayerMask.NameToLayer(lowerColliderLayerName);
                            SetCollisionLayer(lowerColliderLayerName);
                        }
                    }
                    else if (cm.motionDirection != "normal") // if it's going ladder movemento 
                    {
                        if(!topOfIncline)
                        {
                            if (!IsCrossingUp(cm))
                            {
                                cm.motionDirection = "normal";  
                                gameObject.layer = LayerMask.NameToLayer(lowerColliderLayerName);
                                SetCollisionLayer(lowerColliderLayerName);
                                isoSpriteSortingScript.isMovable = true;
                                SetTreeSortingLayer(other.transform.parent.gameObject, lowerSortingLayerToAssign);
                            }
                        }
                        else if(topOfIncline)
                        {
                            if (IsCrossingUp(cm))
                            {
                                cm.motionDirection = "normal";  
                                gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
                                SetCollisionLayer(higherColliderLayerName);
                                isoSpriteSortingScript.isMovable = true;
                            }
                            else
                            {
                                gameObject.layer = LayerMask.NameToLayer(lowerColliderLayerName);
                                SetCollisionLayer(lowerColliderLayerName);
                            }
                        }
                    }
                }
            }
            else if(other.CompareTag("NPCCollider"))
            {
                // Debug.Log("Exited");
                CharacterMovement cm = other.transform.parent.GetComponent<CharacterMovement>();
                cm.currentInclineThreshold = null;


                BoxCollider2D bc = other.GetComponent<BoxCollider2D>();// this is the npc's collider
                BoxCollider2D thisCollider = GetComponent<BoxCollider2D>(); // this collider belongs to the trigger


                // cm.playerOnThresh = false;

                if (!itsALadder)
                {
                    if((IsCrossingUp(cm) && cm.motionDirection == "normal" && !topOfIncline) 
                        || (!IsCrossingUp(cm) && cm.motionDirection == "normal" && topOfIncline))
                    {
                        // cm.motionDirection = motionDirection;  
                    }
                    else if((IsCrossingUp(cm) && cm.motionDirection != "normal" && topOfIncline) 
                        || (!IsCrossingUp(cm) && cm.motionDirection != "normal" && !topOfIncline))
                    {
                        cm.motionDirection = "normal";  
                    }

                    if(IsCrossingUp(cm) && topOfIncline)
                    {

                        SetTreeSortingLayer(other.transform.parent.gameObject, higherSortingLayerToAssign);
                        SetTreeObjectLayer(other.transform.parent.gameObject, LayerMask.NameToLayer(higherColliderLayerName));
                        
                        if(!middleOfIncline)
                        {
                            if(building != null)
                            {
                                foreach(InclineThresholdColliderScript inclineThreshScript in building.inclineThresholdColliderScripts)
                                {
                                    BoxCollider2D threshCol = inclineThreshScript.GetComponent<BoxCollider2D>();
                                    Physics2D.IgnoreCollision(bc, threshCol, true);
                                }
                                foreach(InclineThresholdColliderScript inclineThreshScript in cm.currentLevel.inclineEntrances)
                                {
                                    BoxCollider2D threshCol = inclineThreshScript.GetComponent<BoxCollider2D>();
                                    Physics2D.IgnoreCollision(bc, threshCol, false);
                                }
                            }
                            // // 3. Re-enable collision with all sibling trigger objects
                            // foreach (Transform sibling in transform.parent) // or transform if same level
                            // {
                            //     if (sibling.CompareTag("Trigger"))
                            //     {
                            //        BoxCollider2D siblingCollider = sibling.GetComponent<BoxCollider2D>();
                            //         if (siblingCollider != null)
                            //         {
                            //             Physics2D.IgnoreCollision(bc, siblingCollider, true);
                            //         }
                            //     }
                            // }
                            Physics2D.IgnoreCollision(bc, thisCollider, false);
                        }

                    }            
                    else if(!IsCrossingUp(cm) && !topOfIncline)
                    {
                        // gameObject.layer = LayerMask.NameToLayer(lowerColliderLayerName);
                        // SetCollisionLayer(lowerColliderLayerName);
                        SetTreeSortingLayer(other.transform.parent.gameObject, lowerSortingLayerToAssign);
                        SetTreeObjectLayer(other.transform.parent.gameObject, LayerMask.NameToLayer(lowerColliderLayerName));

                        if(!middleOfIncline)
                        {
                            if(building != null)
                            {
                                foreach(InclineThresholdColliderScript inclineThreshScript in building.inclineThresholdColliderScripts)
                                {
                                    BoxCollider2D threshCol = inclineThreshScript.GetComponent<BoxCollider2D>();
                                    Physics2D.IgnoreCollision(bc, threshCol, true);
                                }
                                foreach(InclineThresholdColliderScript inclineThreshScript in cm.currentLevel.inclineEntrances)
                                {
                                    BoxCollider2D threshCol = inclineThreshScript.GetComponent<BoxCollider2D>();
                                    Physics2D.IgnoreCollision(bc, threshCol, false);
                                }
                            }
                            // // 3. Re-enable collision with all sibling trigger objects
                            // foreach (Transform sibling in transform.parent) // or transform if same level
                            // {

                            //        BoxCollider2D siblingCollider = sibling.GetComponent<BoxCollider2D>();
                            //         if (siblingCollider != null && siblingCollider.isTrigger)
                            //         {
                            //             Physics2D.IgnoreCollision(bc, siblingCollider, true);
                            //         }
                                
                            // }
                            Physics2D.IgnoreCollision(bc, thisCollider, false);
                        }

                    }
                    else if(!IsCrossingUp(cm) && topOfIncline)
                    {
                        // gameObject.layer = LayerMask.NameToLayer(lowerColliderLayerName);
                        // SetCollisionLayer(lowerColliderLayerName);
                        SetTreeSortingLayer(other.transform.parent.gameObject, lowerSortingLayerToAssign);
                        SetTreeObjectLayer(other.transform.parent.gameObject, LayerMask.NameToLayer(lowerColliderLayerName));
                    }
                    
                    // un-fix the player in \/ left/right diag way upon collider exit. 
                    cm.ResetPlayerMovement();                 
                }
                else //if it is a ladder!
                {
                    //alter the motion direction
                    if(cm.motionDirection == "normal")
                    {
                        if(topOfIncline)
                        {
                            // gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
                            // SetCollisionLayer(higherColliderLayerName);
                            SetTreeObjectLayer(other.transform.parent.gameObject, LayerMask.NameToLayer(higherColliderLayerName));
                        }
                        else
                        {
                            // gameObject.layer = LayerMask.NameToLayer(lowerColliderLayerName);
                            // SetCollisionLayer(lowerColliderLayerName);
                            SetTreeObjectLayer(other.transform.parent.gameObject, LayerMask.NameToLayer(lowerColliderLayerName));
                        }
                    }
                    else if (cm.motionDirection != "normal") // if it's going ladder movemento 
                    {
                        if(!topOfIncline)
                        {
                            if (!IsCrossingUp(cm))
                            {
                                cm.motionDirection = "normal";  
                                // gameObject.layer = LayerMask.NameToLayer(lowerColliderLayerName);
                                // SetCollisionLayer(lowerColliderLayerName);
                                SetTreeObjectLayer(other.transform.parent.gameObject, LayerMask.NameToLayer(lowerColliderLayerName));
                                isoSpriteSortingScript.isMovable = true;
                                SetTreeSortingLayer(other.transform.parent.gameObject, lowerSortingLayerToAssign);
                            }
                        }
                        else if(topOfIncline)
                        {
                            if (IsCrossingUp(cm))
                            {
                                cm.motionDirection = "normal";  
                                // gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
                                // SetCollisionLayer(higherColliderLayerName);
                                SetTreeObjectLayer(other.transform.parent.gameObject, LayerMask.NameToLayer(higherColliderLayerName));
                                isoSpriteSortingScript.isMovable = true;
                            }
                            else
                            {
                                // gameObject.layer = LayerMask.NameToLayer(lowerColliderLayerName);
                                // SetCollisionLayer(lowerColliderLayerName);
                                SetTreeObjectLayer(other.transform.parent.gameObject, LayerMask.NameToLayer(lowerColliderLayerName));
                            }
                        }
                    }
                }
            }
    }

    // sortingLayerToAssign is just a variable name, it's given a value in the preceding code somewhere. 
    //In the SetTreeSortingLayer function the second parameter is given the name sortingLayerName, 
    //but this name is local to the function definition. It works because when the function is called, 
    //the sortingLayerName variable inside the function is given the value of sortingLayerToAssign. 

    static void SetTreeSortingLayer(GameObject gameObject, string sortingLayerName)
    {
        if(gameObject.GetComponent<SpriteRenderer>() != null) {
          gameObject.GetComponent<SpriteRenderer>().sortingLayerName = sortingLayerName;
        }
        foreach (Transform child in gameObject.transform)
        {
            InclineThresholdColliderScript.SetTreeSortingLayer(child.gameObject, sortingLayerName);
        }
    }
    static void SetTreeObjectLayer(GameObject gameObject, int layerIndex)
    {
        gameObject.layer = layerIndex;

        foreach (Transform child in gameObject.transform)
        {
            SetTreeObjectLayer(child.gameObject, layerIndex);
        }
    }

    private bool IsCrossingUp(CharacterMovement cm)
    {
        return cm.change.y > 0;
    }
    private bool IsCrossingLeft(CharacterMovement cm)
    {
        return cm.change.x < 0;
    }

    
    public void SetCollisionLayer(string targetLayerName)
    {
        int targetLayerIndex = LayerMask.NameToLayer(targetLayerName);

        // Disable collisions with all layers
        for (int i = 0; i < 32; i++)
        {
                // if(i == LayerMask.NameToLayer("InclineTrigger"))
                //     continue;
                if(i != LayerMask.NameToLayer("InclineTrigger"))
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), i, true);
                // Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("NPC"), i, true);
        }

        // Re-enable other with the target layer
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), targetLayerIndex, false);
        // Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("InclineTrigger"), false);
        // Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("NPC"), targetLayerIndex, false);
    }

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

    public void CheckIfTheStairsHaveAStraightMiddleLanding()
    {
        bool? firstValue = null;
        bool allMatch = true;

        foreach (Transform sibling in transform.parent)
        {
            if (sibling.CompareTag("Trigger"))
            {
                InclineThresholdColliderScript siblingScript =
                    sibling.GetComponent<InclineThresholdColliderScript>();

                if (siblingScript != null)
                {
                    bool value = siblingScript.plyrCrsngLeft;

                    if (firstValue == null)
                    {
                        firstValue = value;    // set reference on first trigger
                    }
                    else if (firstValue != value)
                    {
                        allMatch = false;      // mismatch found
                        break;
                    }
                }
            }
        }

        straightMiddleLanding = allMatch;
    }

    public BuildingScript FindParentByBuildingScriptComponent()
    {
        Transform current = transform;

        while (current != null)
        {
            if (current.GetComponent<BuildingScript>() != null)
            {
                return current.GetComponent<BuildingScript>();
            }
            current = current.parent; // Move up to the next parent
        }

        return null; // Return null if no matching parent is found
    }
    
}

