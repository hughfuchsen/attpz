using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelThreshColliderScript : MonoBehaviour
{
    PlayerMovement playerMovement;
    [SerializeField] GameObject Player;

    // public string initialSortingLayerUponEntry;

    public List<LevelScript> levelAboveOrEntering = new List<LevelScript>();

    public List<LevelScript> levelBelowOrEntering = new List<LevelScript>();

    private bool aboveCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = Player.GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter2D()
    {
        if(isPlayerCrossingUp())
        {
            aboveCollider = false;
        }
        else // player crossing down
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
        // fixing the player's movement inside the collider ensures the correct conditions are met upon collider exit

    }
    private void OnTriggerExit2D()
    {
        playerMovement.fixedDirectionLeft = false;
        playerMovement.fixedDirectionRight = false;  // un-fix the player in \/ left/right diag way upon collider exit.      
        
        if(isPlayerCrossingUp()) //crossing up bro
        {
            if(this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() != null && !aboveCollider && playerMovement.isPlayerOutside)
            {
                if(levelAboveOrEntering != null)
                {
                    for (int i = 0; i < levelAboveOrEntering.Count; i++)
                    {                       
                        levelAboveOrEntering[i].ExitBuilding();
                    }    
                }
                else if(levelBelowOrEntering != null)
                {
                    for (int i = 0; i < levelBelowOrEntering.Count; i++)
                    {                        
                        levelBelowOrEntering[i].ExitBuilding();
                    }
                }

            }
            else if(this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() != null && !aboveCollider && !playerMovement.isPlayerOutside)
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
            if(this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() != null && aboveCollider && playerMovement.isPlayerOutside) 
            {
                // insert only the level you are going through my dog!
                if(levelBelowOrEntering != null)
                {
                    for (int i = 0; i < levelBelowOrEntering.Count; i++)
                    {      
                        levelBelowOrEntering[i].ExitBuilding();
                    }
                }        
                if(levelAboveOrEntering != null)
                {
                    for (int i = 0; i < levelAboveOrEntering.Count; i++)
                    {  
                        levelAboveOrEntering[i].ExitBuilding();
                    }
                }   
            }
            else if(this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() != null && aboveCollider && !playerMovement.isPlayerOutside) 
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
