using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelThreshColliderScript : MonoBehaviour
{
    PlayerMovement playerMovement;
    [SerializeField] GameObject Player;

    // public string initialSortingLayerUponEntry;

    public List<LevelScript> levelsEntering = new List<LevelScript>();

    public List<LevelScript> levelsBelowToMove = new List<LevelScript>();

    public List<LevelScript> levelsEnteringToMove = new List<LevelScript>();

    public bool moveLevelsAbove;
    public bool moveLevelsBelow;
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
        else // player crossing down
        {
            aboveCollider = true;
        }

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
    private void OnTriggerExit2D()
    {
        playerMovement.fixedDirectionLeft = false;
        playerMovement.fixedDirectionRight = false;        
        
        if(isPlayerCrossingUp())
        {
            if(this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() != null && !aboveCollider && !playerMovement.isPlayerInside)
            {
                // insert only the level you are entering my dog!
                if(levelsEntering != null)
                {
                    for (int i = 0; i < levelsEntering.Count; i++)
                    {                       
                        levelsEntering[i].ExitBuilding();
                    }    
                }
                else if(levelsBelowToMove != null)
                {
                    for (int i = 0; i < levelsBelowToMove.Count; i++)
                    {                        
                        levelsBelowToMove[i].ExitBuilding();
                    }
                }
                else if(levelsEnteringToMove != null)
                {
                    for (int i = 0; i < levelsEnteringToMove.Count; i++)
                    {     
                        levelsEnteringToMove[i].ExitBuilding();
                    }
                }  
            }
            else if(this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() != null && !aboveCollider && playerMovement.isPlayerInside)
            {
                // insert only the level you are entering my dog!
                if(levelsEntering != null)
                {
                    for (int i = 0; i < levelsEntering.Count; i++)
                    {                      
                        levelsEntering[i].EnterLevel(true);
                    }
                }
                else if(levelsBelowToMove != null)
                {
                    for (int i = 0; i < levelsBelowToMove.Count; i++)
                    {                          
                        levelsBelowToMove[i].EnterLevel(true);
                    }
                }
                else if(levelsEnteringToMove != null)
                {
                    for (int i = 0; i < levelsEnteringToMove.Count; i++)
                    {   
                        levelsEnteringToMove[i].EnterLevel(true);
                    }

                    if(levelsBelowToMove != null)
                    {
                        for (int i = 0; i < levelsBelowToMove.Count; i++)
                        {                              
                            levelsBelowToMove[i].MoveDown(false, null);
                        }
                    }  
                }   
            }
            else if(moveLevelsAbove && !aboveCollider)
            {
                if(levelsEntering != null)
                {
                    for (int i = 0; i < levelsEntering.Count; i++)
                    {                      
                        levelsEntering[i].EnterLevel(false);
                    }
                }  
            }
            else if(!aboveCollider)
            {
                if(levelsEnteringToMove != null)
                {
                    for (int i = 0; i < levelsEnteringToMove.Count; i++)
                    {                       
                        levelsEnteringToMove[i].EnterLevel(false);
                    }
                }  
            }

            if(moveLevelsBelow)
            {
                if(levelsBelowToMove != null)
                {
                    for (int i = 0; i < levelsBelowToMove.Count; i++)
                    {                          
                        levelsBelowToMove[i].MoveDown(false, null);
                    }
                }  
            }  

            aboveCollider = true;   
        }
        else    // if player crossing down
        {
            if(this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() != null && aboveCollider && !playerMovement.isPlayerInside) 
            {
                // insert only the level you are going through my dog!
                if(levelsBelowToMove != null)
                {
                    for (int i = 0; i < levelsBelowToMove.Count; i++)
                    {      
                        levelsBelowToMove[i].ExitBuilding();
                    }
                }        
                if(levelsEntering != null)
                {
                    for (int i = 0; i < levelsEntering.Count; i++)
                    {  
                        levelsEntering[i].ExitBuilding();
                    }
                }   
                if(levelsEnteringToMove != null)
                {
                    for (int i = 0; i < levelsEnteringToMove.Count; i++)
                    {                       
                        levelsEnteringToMove[i].ExitBuilding();
                    }
                } 
            }
            else if(this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() != null && aboveCollider && playerMovement.isPlayerInside) 
            {
                Debug.Log("Correct");

                // insert only the level you are going through my dog!
                if(levelsEntering != null)
                {
                    for (int i = 0; i < levelsEntering.Count; i++)
                    {                      
                        levelsEntering[i].EnterLevel(true);
                    }
                }   
                if(levelsBelowToMove != null)
                {
                    for (int i = 0; i < levelsBelowToMove.Count; i++)
                    {                          
                        levelsBelowToMove[i].EnterLevel(true);
                    }
                }        
            }
            else if(moveLevelsAbove && aboveCollider)
            {
                if(levelsBelowToMove != null)
                {
                    for (int i = 0; i < levelsBelowToMove.Count; i++)
                    {                          
                        levelsBelowToMove[i].EnterLevel(false);
                    }
                }       
            }
            else if(aboveCollider)
            {
                if(levelsEnteringToMove != null)
                {
                    for (int i = 0; i < levelsEnteringToMove.Count; i++)
                    {                       
                        levelsEnteringToMove[i].EnterLevel(false);
                    }
                }  
            }

            if(moveLevelsBelow)
            {
                if(levelsBelowToMove != null)
                {
                    for (int i = 0; i < levelsBelowToMove.Count; i++)
                    {                          
                        levelsBelowToMove[i].MoveUp();
                    }
                } 
            }

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
