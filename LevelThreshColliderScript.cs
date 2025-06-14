using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelThreshColliderScript : MonoBehaviour
{
    CharacterMovement playerMovement;
    [SerializeField] GameObject Player;

    // public string initialSortingLayerUponEntry;

    public bool itsALadder;

    public List<LevelScript> levelAboveOrEntering = new List<LevelScript>();

    public List<LevelScript> levelBelowOrEntering = new List<LevelScript>();

    private bool aboveCollider;

    public bool plyrCrsngLeft = false;

    public Coroutine thresholdSortingSequenceCoro;

    private string initialSortingLayer;


    
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = Player.GetComponent<CharacterMovement>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("PlayerCollider"))
        {
            if(isPlayerCrossingUp())
            {
                aboveCollider = false;
            }
            else // player crossing down
            {
                aboveCollider = true;
            }

            if(plyrCrsngLeft) // is player going this \ (left diag) way or
            // is player going this / (right diag) way :-)
            {
                // playerMovement.fixedDirectionLeftDiagonal = true; // fix the player in \ left diag way while inside the collider
                playerMovement.controlDirectionToPlayerDirection[Direction.Left] = Direction.UpLeft;
                playerMovement.controlDirectionToPlayerDirection[Direction.UpLeft] = Direction.UpLeft;
                playerMovement.controlDirectionToPlayerDirection[Direction.UpFacingLeft] = Direction.UpLeft;
                playerMovement.controlDirectionToPlayerDirection[Direction.UpFacingRight] = Direction.UpLeft;
                playerMovement.controlDirectionToPlayerDirection[Direction.UpRight] = Direction.UpLeft;
                playerMovement.controlDirectionToPlayerDirection[Direction.Right] = Direction.RightDown;
                playerMovement.controlDirectionToPlayerDirection[Direction.RightDown] = Direction.RightDown;
                playerMovement.controlDirectionToPlayerDirection[Direction.DownFacingRight] = Direction.RightDown;
                playerMovement.controlDirectionToPlayerDirection[Direction.DownFacingLeft] = Direction.RightDown;
                playerMovement.controlDirectionToPlayerDirection[Direction.DownLeft] = Direction.RightDown;
            }
            else
            {
                // playerMovement.fixedDirectionRightDiagonal = true; // fix the player in / right diag way while inside the collider
            playerMovement.controlDirectionToPlayerDirection[Direction.Left] = Direction.DownLeft;
            playerMovement.controlDirectionToPlayerDirection[Direction.UpLeft] = Direction.UpRight;
            playerMovement.controlDirectionToPlayerDirection[Direction.UpFacingLeft] = Direction.UpRight;
            playerMovement.controlDirectionToPlayerDirection[Direction.UpFacingRight] = Direction.UpRight;
            playerMovement.controlDirectionToPlayerDirection[Direction.UpRight] = Direction.UpRight;
            playerMovement.controlDirectionToPlayerDirection[Direction.Right] = Direction.UpRight;
            playerMovement.controlDirectionToPlayerDirection[Direction.RightDown] = Direction.DownLeft;
            playerMovement.controlDirectionToPlayerDirection[Direction.DownFacingRight] = Direction.DownLeft;
            playerMovement.controlDirectionToPlayerDirection[Direction.DownFacingLeft] = Direction.DownLeft;
            playerMovement.controlDirectionToPlayerDirection[Direction.DownLeft] = Direction.DownLeft;
            }
        }
        // fixing the player's movement inside the collider ensures the correct conditions are met upon collider exit

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("PlayerCollider"))
        {
            // un-fix the player in \/ left/right diag way upon collider exit. 
            playerMovement.ResetPlayerMovement();     
            
            if(isPlayerCrossingUp()) //crossing up bro
            {
                if(this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() != null && playerMovement.playerIsOutside)
                {
                    if(levelAboveOrEntering != null)
                    {
                        for (int i = 0; i < levelAboveOrEntering.Count; i++)
                        {                       
                            levelAboveOrEntering[i].ResetLevels();
                        }    
                    }
                    else if(levelBelowOrEntering != null)
                    {
                        for (int i = 0; i < levelBelowOrEntering.Count; i++)
                        {                        
                            levelBelowOrEntering[i].ResetLevels();
                        }
                    }

                }
                else if(this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() != null && !playerMovement.playerIsOutside)
                {
                    if(levelAboveOrEntering != null)
                    {
                        for (int i = 0; i < levelAboveOrEntering.Count; i++)
                        {                      
                            levelAboveOrEntering[i].EnterLevel(true, true);
                        }
                    }
                    else if(levelBelowOrEntering != null)
                    {
                        for (int i = 0; i < levelBelowOrEntering.Count; i++)
                        {                          
                            levelBelowOrEntering[i].EnterLevel(true, true);
                        }
                    }
                }
                else if(levelAboveOrEntering != null)
                {
                    for (int i = 0; i < levelAboveOrEntering.Count; i++)
                    {                      
                        levelAboveOrEntering[i].EnterLevel(false, true);
                    }
                }  
                // }

                // if(moveLevelsBelow)
                // {
                //     if(levelBelowOrEntering != null)
                //     {
                //         for (int i = 0; i < levelBelowOrEntering.Count; i++)
                //         {                          
                //             levelBelowOrEntering[i].MoveDown(false, null);
                //         }
                //     }  
                // }  

                aboveCollider = true;   
            }
            else    // if player crossing down
            {
                if(this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() != null && aboveCollider && playerMovement.playerIsOutside) 
                {
                    // insert only the level you are going through my dog!
                    if(levelBelowOrEntering != null)
                    {
                        for (int i = 0; i < levelBelowOrEntering.Count; i++)
                        {      
                            levelBelowOrEntering[i].ResetLevels();
                        }
                    }        
                    if(levelAboveOrEntering != null)
                    {
                        for (int i = 0; i < levelAboveOrEntering.Count; i++)
                        {  
                            levelAboveOrEntering[i].ResetLevels();
                        }
                    }   
                }
                else if(this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() != null && aboveCollider && !playerMovement.playerIsOutside) 
                {
                    if(levelAboveOrEntering != null)
                    {
                        for (int i = 0; i < levelAboveOrEntering.Count; i++)
                        {                      
                            levelAboveOrEntering[i].EnterLevel(true, false);
                        }
                    }   
                    else if(levelBelowOrEntering != null)
                    {
                        for (int i = 0; i < levelBelowOrEntering.Count; i++)
                        {                          
                            levelBelowOrEntering[i].EnterLevel(true, false);
                        }
                    }        
                }
                // else if(moveLevelsAbove && aboveCollider)
                // {
                else if(levelBelowOrEntering != null)
                {
                    for (int i = 0; i < levelBelowOrEntering.Count; i++)
                    {                          
                        levelBelowOrEntering[i].EnterLevel(false, false);
                    }
                }       
                // }

                // if(moveLevelsBelow)
                // {
                //     if(levelBelowOrEntering != null)
                //     {
                //         for (int i = 0; i < levelBelowOrEntering.Count; i++)
                //         {                          
                //             levelBelowOrEntering[i].MoveUp();
                //         }
                //     } 
                // }

                aboveCollider = false;
            }
            if(this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() == null)
            {
                thresholdSortingSequenceCoro = StartCoroutine(ThresholdLayerSortingSequence(0.3f, Player, "ThresholdSequence"));
            }

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


    static void SetTreeSortingLayer(GameObject gameObject, string sortingLayerName)
    {
        if(gameObject.GetComponent<SpriteRenderer>() != null) 
        {
            gameObject.GetComponent<SpriteRenderer>().sortingLayerName = sortingLayerName;
        }
        foreach (Transform child in gameObject.transform)
        {
            LevelThreshColliderScript.SetTreeSortingLayer(child.gameObject, sortingLayerName);
        }
    }  

    private bool isPlayerCrossingUp()
    {
        return GameObject.FindWithTag("Player").GetComponent<CharacterMovement>().change.y > 0;
    }
    private bool isPlayerCrossingLeft()
    {
        return GameObject.FindWithTag("Player").GetComponent<CharacterMovement>().change.x < 0;
    }
}
