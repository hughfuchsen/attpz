using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InclineMovement : MonoBehaviour
{
    PlayerMovement playerMovement;
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

    // Start is called before the first frame update
    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = Player.GetComponent<PlayerMovement>();
        isoSpriteSortingScript = Player.GetComponent<IsoSpriteSorting>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!itsALadder) // if it's not a ladder
            {
                if(!middleOfStairCase) // turn off fixed direction
                {
                    if((isPlayerCrossingUp() && isPlayerCrossingLeft()) 
                        || !isPlayerCrossingUp() && !isPlayerCrossingLeft())
                    {
                        playerMovement.fixedDirectionLeft = true;
                    }
                    else
                    {
                        playerMovement.fixedDirectionRight = true;
                    }
                }

                //alter the motion direction
                if((isPlayerCrossingUp() && playerMovement.motionDirection == "normal" && !topOfStairCase) 
                    || (!isPlayerCrossingUp() && playerMovement.motionDirection == "normal" && topOfStairCase))
                {
                    playerMovement.motionDirection = motionDirection;  
                }
                else if((isPlayerCrossingUp() && playerMovement.motionDirection != "normal" && topOfStairCase) 
                    || (!isPlayerCrossingUp() && playerMovement.motionDirection != "normal" && !topOfStairCase))
                {
                    playerMovement.motionDirection = "normal";  
                }

                // alter the collider layer and sprite sorting layer that is active with the player
                if(isPlayerCrossingUp() && topOfStairCase)
                {
                    gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
                    SetCollisionLayer(higherColliderLayerName);
                    SetTreeSortingLayer(collision.gameObject, higherSortingLayerToAssign);
                }
                else if(isPlayerCrossingUp() && !topOfStairCase)
                {
                    gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
                    SetCollisionLayer(higherColliderLayerName);
                    SetTreeSortingLayer(collision.gameObject, higherSortingLayerToAssign);
                }                       
                else if(!isPlayerCrossingUp() && topOfStairCase)
                {
                    gameObject.layer = LayerMask.NameToLayer(lowerColliderLayerName);
                    SetCollisionLayer(lowerColliderLayerName);
                    SetTreeSortingLayer(collision.gameObject, lowerSortingLayerToAssign);
                } 
                else if(!isPlayerCrossingUp() && !topOfStairCase)
                {
                    gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
                    SetCollisionLayer(higherColliderLayerName);
                    SetTreeSortingLayer(collision.gameObject, higherSortingLayerToAssign);
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
            else // if it is a ladder baby
            {
                //alter the motion direction
                if(playerMovement.motionDirection == "normal")
                {
                    // anchor player to ladder
                    if(!topOfStairCase)
                    {
                        Player.transform.position = FindSiblingWithTag("Ladder").transform.position - isoSpriteSortingScript.SorterPositionOffset;
                    }
                    else
                    {
                        Player.transform.position = FindSiblingWithTag("Ladder").transform.position - isoSpriteSortingScript.SorterPositionOffset + new Vector3(0, ladderHeight, 0);
                    }

                    if((isPlayerCrossingUp() && (isPlayerCrossingLeft()||!isPlayerCrossingLeft())))
                    {
                        //manage animation
                        if(isPlayerCrossingLeft())
                        {
                            playerMovement.ladderAnimDirectionIndex = playerMovement.upLeftAnim;
                        }
                        else
                        {
                            playerMovement.ladderAnimDirectionIndex = playerMovement.upRightAnim;
                        }

                        
                        if(!topOfStairCase)
                        {
                            playerMovement.motionDirection = "upLadder";
                        }
                        else
                        {
                            playerMovement.motionDirection = "downLadder";
                        }
                        //add certain animation and anchoring here
                        gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
                        SetCollisionLayer(higherColliderLayerName);
                    }
                    else if((!isPlayerCrossingUp() && (isPlayerCrossingLeft()||!isPlayerCrossingLeft())))
                    {
                        //add certain animation 
                        if(!topOfStairCase)
                        {
                            playerMovement.motionDirection = "upLadder";
                            
                            //manage animation
                            if(isPlayerCrossingLeft())
                            {
                                playerMovement.ladderAnimDirectionIndex = playerMovement.leftAnim;
                            }
                            else
                            {
                                playerMovement.ladderAnimDirectionIndex = playerMovement.rightAnim;
                            }
                        }
                        else
                        {
                            playerMovement.motionDirection = "downLadder";
                            
                            //manage animation
                            if(isPlayerCrossingLeft())
                            {
                                playerMovement.ladderAnimDirectionIndex = playerMovement.upRightAnim;
                            }
                            else
                            {
                                playerMovement.ladderAnimDirectionIndex = playerMovement.upLeftAnim;
                            }
                        }
                        gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
                        SetCollisionLayer(higherColliderLayerName);
                    }
                    isoSpriteSortingScript.isMovable = false;
                }
                else if((isPlayerCrossingUp() && playerMovement.motionDirection != "normal"))
                {
                    playerMovement.motionDirection = "normal";  
                    SetTreeSortingLayer(collision.gameObject, higherSortingLayerToAssign);
                    isoSpriteSortingScript.isMovable = true;
                }
                else if((!isPlayerCrossingUp() && playerMovement.motionDirection != "normal"))
                {
                    playerMovement.motionDirection = "normal"; 
                    gameObject.layer = LayerMask.NameToLayer(lowerColliderLayerName);
                    SetCollisionLayer(lowerColliderLayerName);
                    SetTreeSortingLayer(collision.gameObject, lowerSortingLayerToAssign);
                    isoSpriteSortingScript.isMovable = true;
                }

                // alter the collider layer and sprite sorting layer that is active with the player
                // if(!topOfStairCase)
                // {
                //     gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
                //     SetCollisionLayer(higherColliderLayerName);
                //     SetTreeSortingLayer(collision.gameObject, higherSortingLayerToAssign);
                // }                   
                // else
                // {
                //     gameObject.layer = LayerMask.NameToLayer(lowerColliderLayerName);
                //     SetCollisionLayer(lowerColliderLayerName);
                //     SetTreeSortingLayer(collision.gameObject, lowerSortingLayerToAssign);
                // } 

            }

            
        }
    }



    void OnTriggerExit2D(Collider2D collision)
    {
            if(collision.gameObject.tag == "Player")
            {
                if (!itsALadder)
                {
                    playerMovement.fixedDirectionLeft = false;
                    playerMovement.fixedDirectionRight = false;


                    if((isPlayerCrossingUp() && playerMovement.motionDirection == "normal" && !topOfStairCase) 
                        || (!isPlayerCrossingUp() && playerMovement.motionDirection == "normal" && topOfStairCase))
                    {
                        playerMovement.motionDirection = motionDirection;  
                    }
                    else if((isPlayerCrossingUp() && playerMovement.motionDirection != "normal" && topOfStairCase) 
                        || (!isPlayerCrossingUp() && playerMovement.motionDirection != "normal" && !topOfStairCase))
                    {
                        playerMovement.motionDirection = "normal";  
                    }

                    if(isPlayerCrossingUp() && topOfStairCase)
                    {
                        gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
                        SetCollisionLayer(higherColliderLayerName);
                        SetTreeSortingLayer(collision.gameObject, higherSortingLayerToAssign);
                    }            
                    else if(!isPlayerCrossingUp() && !topOfStairCase)
                    {
                        gameObject.layer = LayerMask.NameToLayer(lowerColliderLayerName);
                        SetCollisionLayer(lowerColliderLayerName);
                        SetTreeSortingLayer(collision.gameObject, lowerSortingLayerToAssign);
                    }
                    else if(!isPlayerCrossingUp() && topOfStairCase)
                    {
                        gameObject.layer = LayerMask.NameToLayer(lowerColliderLayerName);
                        SetCollisionLayer(lowerColliderLayerName);
                        SetTreeSortingLayer(collision.gameObject, lowerSortingLayerToAssign);
                    }
                }
                else //if it is a ladder!
                {
                    //alter the motion direction
                    if(playerMovement.motionDirection == "normal")
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
                    else if (playerMovement.motionDirection != "normal")
                    {
                        if(!topOfStairCase)
                        {
                            if (!isPlayerCrossingUp())
                            {
                                playerMovement.motionDirection = "normal";  
                                gameObject.layer = LayerMask.NameToLayer(lowerColliderLayerName);
                                SetCollisionLayer(lowerColliderLayerName);
                            }
                        }
                        else if(topOfStairCase)
                        {
                            if (isPlayerCrossingUp())
                            {
                                playerMovement.motionDirection = "normal";  
                                gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
                                SetCollisionLayer(higherColliderLayerName);
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

    private bool isPlayerCrossingUp()
    {
        return GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().change.y > 0;
    }
    private bool isPlayerCrossingLeft()
    {
        return GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().change.x < 0;
    }
    // public void SetCollisionLayer(string targetLayerName)
    // {
    //     GameObject.FindWithTag("Player").layer = LayerMask.NameToLayer(targetLayerName);
    // }
    
    public void SetCollisionLayer(string targetLayerName)
    {
        int targetLayerIndex = LayerMask.NameToLayer(targetLayerName);

        // Disable collisions with all layers
        for (int i = 0; i < 32; i++)
        {
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), i, true);
        }

        // Re-enable collision with the target layer
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

