using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelColliderScript : MonoBehaviour
{
    PlayerMovement playerMovement;
    [SerializeField] GameObject Player;
    // public string initialSortingLayerUponEntry;
    public LevelScript levelAbove;
    public LevelScript levelEntering;
    public LevelScript levelBelow;
    public bool levelAboveMove;
    public bool levelThreshold;
    private bool aboveCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = Player.GetComponent<PlayerMovement>();
        // initialSortingLayerUponEntry = Player.GetComponent<SpriteRenderer>().sortingLayerName; 
    }

    private void OnTriggerEnter2D()
    {
        if(isPlayerCrossingUp())
        {
            aboveCollider = false;
        }
        else
        {
            aboveCollider = true;
        }
    }
    private void OnTriggerExit2D()
    {
        if(isPlayerCrossingUp())
        {
            if(this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() != null && !aboveCollider && !playerMovement.isPlayerInside)
            {
                // insert only the level you are entering my dog!
                if(levelAbove != null)
                {
                    levelAbove.ExitBuilding();
                }
                else if(levelBelow != null)
                {
                    levelBelow.ExitBuilding();
                }
                else if(levelEntering != null)
                {
                    levelEntering.ExitBuilding();
                }  
            }
            else if(this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() != null && !aboveCollider && playerMovement.isPlayerInside)
            {
                Debug.Log("player baby");
                // insert only the level you are entering my dog!
                if(levelAbove != null)
                {
                    levelAbove.EnterLevel(true);
                }
                else if(levelBelow != null)
                {
                    levelBelow.EnterLevel(true);
                }
                else if(levelEntering != null)
                {
                    levelEntering.EnterLevel(true);
                    // if(levelBelow != null)
                    // {
                    //     levelBelow.MoveDown(false, null);
                    // }  
                }   
            }
            else if(levelAboveMove && !aboveCollider)
            {
                if(levelAbove != null)
                {
                    levelAbove.EnterLevel(false);
                }  
            }
            else if(!aboveCollider)
            {
                if(levelEntering != null)
                {
                    levelEntering.EnterLevel(false);
                }  
            }

            if(levelThreshold)
            {
                if(levelBelow != null)
                {
                    levelBelow.MoveDown(false, null);
                }  
            }  

            aboveCollider = true;   
        }
        else    // if player crossing down
        {
            if(this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() != null && aboveCollider && !playerMovement.isPlayerInside) 
            {
                // insert only the level you are going through my dog!
                if(levelBelow != null)
                {
                    levelBelow.ExitBuilding();
                }        
                else if(levelAbove != null)
                {
                    levelAbove.ExitBuilding();
                }   
                else if(levelEntering != null)
                {
                    levelEntering.ExitBuilding();
                } 
            }
            else if(this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() != null && aboveCollider && playerMovement.isPlayerInside) 
            {
                // insert only the level you are going through my dog!
                if(levelBelow != null)
                {
                    levelBelow.EnterLevel(true);
                }        
                else if(levelAbove != null)
                {
                    levelAbove.EnterLevel(true);
                }   
                else if(levelEntering != null)
                {
                    levelEntering.EnterLevel(true);
                } 
            }
            else if(levelAboveMove && aboveCollider)
            {
                if(levelBelow != null)
                {
                    levelBelow.EnterLevel(false);
                }       
            }
            else if(aboveCollider)
            {
                if(levelEntering != null)
                {
                    levelEntering.EnterLevel(false);
                }  
            }

            if(levelThreshold)
            {
                if(levelBelow != null)
                {
                    levelBelow.MoveUp();
                } 
            }

            aboveCollider = false;
        }




        // if(!enteringLevelFromAbove) // entering level from below
        // {
        //     if(isPlayerCrossingUp() && levelEntering != null && levelAboveMove && !aboveCollider)
        //     {
        //         levelEntering.EnterLevel();
        //         aboveCollider = true;
        //     }    
        //     else if(isPlayerCrossingUp() && levelAboveMove && !aboveCollider)
        //     {
        //         if(levelAbove != null)
        //         {
        //             levelAbove.EnterLevel();
        //         }    
        //         enteringLevelFromAbove = true;
        //         aboveCollider = true;
        //     } 
        //     else if(!isPlayerCrossingUp() && this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() != null && aboveCollider) 
        //     // if player crossing down
        //     {
        //         if(levelBelow != null)
        //         {
        //             levelBelow.ExitBuilding();
        //         }        
        //         else if(levelAbove != null)
        //         {
        //             levelAbove.ExitBuilding();
        //         }   

        //         aboveCollider = false;   
        //     }
        //     else if(!isPlayerCrossingUp() && levelAboveMove && aboveCollider)
        //     {
        //         if(levelBelow != null)
        //         {
        //             levelBelow.EnterLevel();
        //         }       
        //         aboveCollider = false;
        //     }   


        //     if(isPlayerCrossingUp() && levelThreshold)
        //     {
        //         if(levelBelow != null)
        //         {
        //             levelBelow.MoveDown();
        //         }  
        //     }            
       //     else if(!isPlayerCrossingUp() && levelThreshold)
        //     {
        //         if(levelBelow != null)
        //         {
        //             levelBelow.MoveUp();
        //         }   
        //     }
        // }
        // else // entering the LEvel (not collider) from above collider
        // {
        //     // if(!isPlayerCrossingUp() && levelAboveMove && !aboveCollider)
        //     // {
        //     //     if(levelBelow != null)
        //     //     {
        //     //         levelBelow.EnterLevel();
        //     //     }    
        //     //     aboveCollider = true;
        //     // }
        //     if(!isPlayerCrossingUp() && levelEntering != null && levelAboveMove && aboveCollider)
        //     {
        //         levelEntering.EnterLevel();
        //         enteringLevelFromAbove = false;
        //         aboveCollider = false;
        //     }            
        //     // else if(!isPlayerCrossingUp() && levelAboveMove && aboveCollider)
        //     // {
        //     //     if(levelBelow != null)
        //     //     {
        //     //         levelBelow.EnterLevel();
        //     //     }
        //     //     enteringLevelFromAbove = false;
        //     //     aboveCollider = false;
        //     // } 
        //     else if(isPlayerCrossingUp() && this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() != null)
        //     {
        //         if(levelAbove != null)
        //         {
        //             levelAbove.ExitBuilding();
        //         }
        //         else if(levelBelow != null)
        //         {
        //             levelBelow.ExitBuilding();
        //         }

        //         aboveCollider = true;   
        //     }


        //     if(isPlayerCrossingUp() && levelThreshold)
        //     {
        //         levelBelow.MoveUp();
        //     }            
        //     else if(!isPlayerCrossingUp() && levelThreshold)
        //     {
        //         levelBelow.MoveDown();
        //     }          
        // }

    }

    private bool isPlayerCrossingUp()
    {
        return GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().change.y > 0;
    }
    private bool isPlayerCrossingLeft()
    {
        return GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().change.x < 0;
    }
}
