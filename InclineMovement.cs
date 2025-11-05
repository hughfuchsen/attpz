using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InclineMovement : MonoBehaviour
{
    // CharacterMovement cm;
    CharacterAnimation myCharacterAnimation;
    IsoSpriteSorting isoSpriteSortingScript;
    [SerializeField] GameObject Player;
    public string motionDirection;
    public Coroutine thresholdSortingSequenceCoro;
    public string lowerSortingLayerToAssign;
    public string higherSortingLayerToAssign;
    public string lowerColliderLayerName; // The name of the layer you want to switch to
    public string higherColliderLayerName; // The name of the layer you want to switch to
    public bool topOfStairCase;
    public bool middleOfStairCase;
    public bool itsALadder;
    public int ladderHeight = 0;
    public bool plyrCrsngLeft = false; 
    public enum InFrontOrBehindLadder {none, inFrontOfLadder, behindLadder}
    public InFrontOrBehindLadder inFrontOrBehindLadder;

    // Start is called before the first frame update
    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player"); // assign to player
        // cm = Player.GetComponent<CharacterMovement>();
        myCharacterAnimation = Player.GetComponent<CharacterAnimation>();
        isoSpriteSortingScript = Player.GetComponent<IsoSpriteSorting>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        CharacterMovement cm = other.transform.parent.GetComponent<CharacterMovement>();

        if (other.CompareTag("PlayerCollider"))
        {
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
                // if((IsCrossingUp(cm) && cm.motionDirection == "normal" && !topOfStairCase) 
                //     || (!IsCrossingUp(cm) && cm.motionDirection == "normal" && topOfStairCase))
                // {
                //     cm.motionDirection = motionDirection;  
                // }
                // else if((IsCrossingUp(cm) && cm.motionDirection != "normal" && topOfStairCase) 
                //     || (!IsCrossingUp(cm) && cm.motionDirection != "normal" && !topOfStairCase))
                // {
                //     cm.motionDirection = "normal";  
                // }

                // alter the collider layer and sprite sorting layer that is active with the player
                if(IsCrossingUp(cm) && topOfStairCase)
                {
                    gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
                    SetCollisionLayer(higherColliderLayerName);
                    SetTreeSortingLayer(other.transform.parent.gameObject, higherSortingLayerToAssign);
                }
                else if(IsCrossingUp(cm) && !topOfStairCase)
                {
                    gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
                    SetCollisionLayer(higherColliderLayerName);
                    SetTreeSortingLayer(other.transform.parent.gameObject, higherSortingLayerToAssign);
                }                       
                else if(!IsCrossingUp(cm) && topOfStairCase)
                {
                    gameObject.layer = LayerMask.NameToLayer(lowerColliderLayerName);
                    SetCollisionLayer(lowerColliderLayerName);
                    SetTreeSortingLayer(other.transform.parent.gameObject, lowerSortingLayerToAssign);
                } 
                else if(!IsCrossingUp(cm) && !topOfStairCase)
                {
                    gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
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
                    if(!topOfStairCase)
                    {
                        Player.transform.position = FindSiblingWithTag("Ladder").transform.position - isoSpriteSortingScript.SorterPositionOffset;
                    }
                    else
                    {
                        Player.transform.position = FindSiblingWithTag("Ladder").transform.position - isoSpriteSortingScript.SorterPositionOffset + new Vector3(0, ladderHeight, 0);
                    }

                    if(IsCrossingUp(cm))
                    {
                        if(!topOfStairCase)
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
                        if(!topOfStairCase)
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
                // if(!topOfStairCase)
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
        if (other.CompareTag("NPCCollider"))
        {
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
                // if((IsCrossingUp(cm) && cm.motionDirection == "normal" && !topOfStairCase) 
                //     || (!IsCrossingUp(cm) && cm.motionDirection == "normal" && topOfStairCase))
                // {
                //     cm.motionDirection = motionDirection;  
                // }
                // else if((IsCrossingUp(cm) && cm.motionDirection != "normal" && topOfStairCase) 
                //     || (!IsCrossingUp(cm) && cm.motionDirection != "normal" && !topOfStairCase))
                // {
                //     cm.motionDirection = "normal";  
                // }

                // alter the collider layer and sprite sorting layer that is active with the player
                if(IsCrossingUp(cm) && topOfStairCase)
                {
                    gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
                    SetCollisionLayer(higherColliderLayerName);
                    SetTreeSortingLayer(other.transform.parent.gameObject, higherSortingLayerToAssign);
                }
                else if(IsCrossingUp(cm) && !topOfStairCase)
                {
                    gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
                    SetCollisionLayer(higherColliderLayerName);
                    SetTreeSortingLayer(other.transform.parent.gameObject, higherSortingLayerToAssign);
                }                       
                else if(!IsCrossingUp(cm) && topOfStairCase)
                {
                    gameObject.layer = LayerMask.NameToLayer(lowerColliderLayerName);
                    SetCollisionLayer(lowerColliderLayerName);
                    SetTreeSortingLayer(other.transform.parent.gameObject, lowerSortingLayerToAssign);
                } 
                else if(!IsCrossingUp(cm) && !topOfStairCase)
                {
                    gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
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
                    if(!topOfStairCase)
                    {
                        Player.transform.position = FindSiblingWithTag("Ladder").transform.position - isoSpriteSortingScript.SorterPositionOffset;
                    }
                    else
                    {
                        Player.transform.position = FindSiblingWithTag("Ladder").transform.position - isoSpriteSortingScript.SorterPositionOffset + new Vector3(0, ladderHeight, 0);
                    }

                    if(IsCrossingUp(cm))
                    {
                        if(!topOfStairCase)
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
                        if(!topOfStairCase)
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
                // if(!topOfStairCase)
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


    // void OnTriggerStay2D(Collider2D other)
    // {
    //     if(itsALadder) // if it is a ladder baby
    //     {
    //         if (Input.GetKeyDown(KeyCode.Space)) // Y button on Xbox controller
    //         {
    //             //alter the motion direction
    //             if(cm.motionDirection == "normal")
    //             {
    //                 // anchor player to ladder
    //                 if(!topOfStairCase)
    //                 {
    //                     Player.transform.position = FindSiblingWithTag("Ladder").transform.position - isoSpriteSortingScript.SorterPositionOffset;
    //                 }
    //                 else
    //                 {
    //                     Player.transform.position = FindSiblingWithTag("Ladder").transform.position - isoSpriteSortingScript.SorterPositionOffset + new Vector3(0, ladderHeight, 0);
    //                 }

    //                 if((IsCrossingUp(cm) && (isPlayerCrossingLeft()||!isPlayerCrossingLeft())))
    //                 {
    //                     //manage animation
    //                     if(isPlayerCrossingLeft())
    //                     {
    //                         myCharacterAnimation.ladderAnimDirectionIndex = myCharacterAnimation.upLeftAnim;
    //                     }
    //                     else
    //                     {
    //                         myCharacterAnimation.ladderAnimDirectionIndex = myCharacterAnimation.upRightAnim;
    //                     }

                        
    //                     if(!topOfStairCase)
    //                     {
    //                         cm.motionDirection = "upLadder";
    //                     }
    //                     else
    //                     {
    //                         cm.motionDirection = "downLadder";
    //                     }
    //                     //add certain animation and anchoring here
    //                     gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
    //                     SetCollisionLayer(higherColliderLayerName);
    //                 }
    //                 else if((!IsCrossingUp(cm) && (isPlayerCrossingLeft()||!isPlayerCrossingLeft())))
    //                 {
    //                     //add certain animation 
    //                     if(!topOfStairCase)
    //                     {
    //                         cm.motionDirection = "upLadder";
                            
    //                         //manage animation
    //                         if(isPlayerCrossingLeft())
    //                         {
    //                             myCharacterAnimation.ladderAnimDirectionIndex = myCharacterAnimation.leftAnim;
    //                         }
    //                         else
    //                         {
    //                             myCharacterAnimation.ladderAnimDirectionIndex = myCharacterAnimation.rightAnim;
    //                         }
    //                     }
    //                     else
    //                     {
    //                         cm.motionDirection = "downLadder";
                            
    //                         //manage animation
    //                         if(isPlayerCrossingLeft())
    //                         {
    //                             myCharacterAnimation.ladderAnimDirectionIndex = myCharacterAnimation.upRightAnim;
    //                         }
    //                         else
    //                         {
    //                             myCharacterAnimation.ladderAnimDirectionIndex = myCharacterAnimation.upLeftAnim;
    //                         }
    //                     }
    //                     gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
    //                     SetCollisionLayer(higherColliderLayerName);
    //                 }
    //                 isoSpriteSortingScript.isMovable = false;
    //             }
    //             else if((IsCrossingUp(cm) && cm.motionDirection != "normal"))
    //             {
    //                 cm.motionDirection = "normal";  
    //                 SetTreeSortingLayer(other.transform.parent.gameObject, higherSortingLayerToAssign);
    //                 isoSpriteSortingScript.isMovable = true;
    //             }
    //             else if((!IsCrossingUp(cm) && cm.motionDirection != "normal"))
    //             {
    //                 cm.motionDirection = "normal"; 
    //                 gameObject.layer = LayerMask.NameToLayer(lowerColliderLayerName);
    //                 SetCollisionLayer(lowerColliderLayerName);
    //                 SetTreeSortingLayer(other.transform.parent.gameObject, lowerSortingLayerToAssign);
    //                 isoSpriteSortingScript.isMovable = true;
    //             }

    //             // alter the collider layer and sprite sorting layer that is active with the player
    //             // if(!topOfStairCase)
    //             // {
    //             //     gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
    //             //     SetCollisionLayer(higherColliderLayerName);
    //             //     SetTreeSortingLayer(other.transform.parent.gameObject, higherSortingLayerToAssign);
    //             // }                   
    //             // else
    //             // {
    //             //     gameObject.layer = LayerMask.NameToLayer(lowerColliderLayerName);
    //             //     SetCollisionLayer(lowerColliderLayerName);
    //             //     SetTreeSortingLayer(other.transform.parent.gameObject, lowerSortingLayerToAssign);
    //             // } 
    //             }
    //         }
    //     }


    void OnTriggerExit2D(Collider2D other)
    {

            if(other.CompareTag("PlayerCollider"))
            {
                CharacterMovement cm = other.transform.parent.GetComponent<CharacterMovement>();

                cm.playerOnThresh = false;

                if (!itsALadder)
                {
                    if((IsCrossingUp(cm) && cm.motionDirection == "normal" && !topOfStairCase) 
                        || (!IsCrossingUp(cm) && cm.motionDirection == "normal" && topOfStairCase))
                    {
                        cm.motionDirection = motionDirection;  
                    }
                    else if((IsCrossingUp(cm) && cm.motionDirection != "normal" && topOfStairCase) 
                        || (!IsCrossingUp(cm) && cm.motionDirection != "normal" && !topOfStairCase))
                    {
                        cm.motionDirection = "normal";  
                    }

                    if(IsCrossingUp(cm) && topOfStairCase)
                    {
                        gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
                        SetCollisionLayer(higherColliderLayerName);
                        SetTreeSortingLayer(other.transform.parent.gameObject, higherSortingLayerToAssign);
                    }            
                    else if(!IsCrossingUp(cm) && !topOfStairCase)
                    {
                        gameObject.layer = LayerMask.NameToLayer(lowerColliderLayerName);
                        SetCollisionLayer(lowerColliderLayerName);
                        SetTreeSortingLayer(other.transform.parent.gameObject, lowerSortingLayerToAssign);
                    }
                    else if(!IsCrossingUp(cm) && topOfStairCase)
                    {
                        gameObject.layer = LayerMask.NameToLayer(lowerColliderLayerName);
                        SetCollisionLayer(lowerColliderLayerName);
                        SetTreeSortingLayer(other.transform.parent.gameObject, lowerSortingLayerToAssign);
                    }
                    
                    // un-fix the player in \/ left/right diag way upon collider exit. 
                    cm.ResetPlayerMovement();                 
                }
                else //if it is a ladder!
                {
                    //alter the motion direction
                    if(cm.motionDirection == "normal")
                    {
                        if(topOfStairCase)
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
                        if(!topOfStairCase)
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
                        else if(topOfStairCase)
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
                CharacterMovement cm = other.transform.parent.GetComponent<CharacterMovement>();

                cm.playerOnThresh = false;

                if (!itsALadder)
                {
                    if((IsCrossingUp(cm) && cm.motionDirection == "normal" && !topOfStairCase) 
                        || (!IsCrossingUp(cm) && cm.motionDirection == "normal" && topOfStairCase))
                    {
                        cm.motionDirection = motionDirection;  
                    }
                    else if((IsCrossingUp(cm) && cm.motionDirection != "normal" && topOfStairCase) 
                        || (!IsCrossingUp(cm) && cm.motionDirection != "normal" && !topOfStairCase))
                    {
                        cm.motionDirection = "normal";  
                    }

                    if(IsCrossingUp(cm) && topOfStairCase)
                    {
                        gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
                        SetCollisionLayer(higherColliderLayerName);
                        SetTreeSortingLayer(other.transform.parent.gameObject, higherSortingLayerToAssign);
                    }            
                    else if(!IsCrossingUp(cm) && !topOfStairCase)
                    {
                        gameObject.layer = LayerMask.NameToLayer(lowerColliderLayerName);
                        SetCollisionLayer(lowerColliderLayerName);
                        SetTreeSortingLayer(other.transform.parent.gameObject, lowerSortingLayerToAssign);
                    }
                    else if(!IsCrossingUp(cm) && topOfStairCase)
                    {
                        gameObject.layer = LayerMask.NameToLayer(lowerColliderLayerName);
                        SetCollisionLayer(lowerColliderLayerName);
                        SetTreeSortingLayer(other.transform.parent.gameObject, lowerSortingLayerToAssign);
                    }
                    
                    // un-fix the player in \/ left/right diag way upon collider exit. 
                    cm.ResetPlayerMovement();                 
                }
                else //if it is a ladder!
                {
                    //alter the motion direction
                    if(cm.motionDirection == "normal")
                    {
                        if(topOfStairCase)
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
                        if(!topOfStairCase)
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
                        else if(topOfStairCase)
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
            InclineMovement.SetTreeSortingLayer(child.gameObject, sortingLayerName);
        }
    }

    private bool IsCrossingUp(CharacterMovement cm)
    {
        return cm.change.y > 0;
    }

    
    public void SetCollisionLayer(string targetLayerName)
    {
        int targetLayerIndex = LayerMask.NameToLayer(targetLayerName);

        // Disable collisions with all layers
        for (int i = 0; i < 32; i++)
        {
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), i, true);
        }

        // Re-enable other with the target layer
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), targetLayerIndex, false);
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
    
}

