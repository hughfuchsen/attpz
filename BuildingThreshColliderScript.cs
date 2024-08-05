using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingThreshColliderScript : MonoBehaviour
{
    public BuildingScript building;
    PlayerMovement playerMovement;
    [SerializeField] GameObject Player;
    public bool backOfBuilding;
    public bool rooftopLadder;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = Player.GetComponent<PlayerMovement>(); 
    }

    void OnTriggerEnter2D()
    {
        if(rooftopLadder)
        {
            // if(isPlayerCrossingUp())
            // {
                if (!playerMovement.playerIsOutside)
                {           
                    // Debug.Log("exiting");     
                    building.ExitBuilding(0.3f, 0.3f, false);
                    playerMovement.playerIsOutside = true;  
                }
            // }
        }
    }
    void OnTriggerExit2D()
    {
        if(!rooftopLadder)
        { 
            if(isPlayerCrossingUp()) //if player crossing up
            {
                if (backOfBuilding)
                {
                    if (playerMovement.playerIsInside())
                    {                
                        building.GoBehindBuilding(); // go outside behind the buildinng
                    }
                    playerMovement.playerIsOutside = true;
                }
                else
                {
                        if(playerMovement.playerIsOutside) 
                        {     
                            building.EnterBuilding();
                        }                
                        playerMovement.playerIsOutside = false;            
                }
            }
            else //if player crossing down
            {
                    if (backOfBuilding)   // entering the building from the back
                    {   
                        if(playerMovement.playerIsOutside) 
                        {     
                            building.EnterBuilding();
                        }
                        playerMovement.playerIsOutside = false;            
                    }
                    else // entering the building from the front
                    {
                        if (LayerMask.LayerToName(this.gameObject.layer) != "Default") // if player is not exiting building on the ground level
                        {
                            building.ExitBuilding(0.3f, 0.3f, false);
                        }
                        else      // if player is exiting building on the ground level
                        {
                            building.ExitBuilding(0.3f, 0.3f, false); //
                        }
                        playerMovement.playerIsOutside = true;
                    }
                
            }
        }
        else
        {
            // if(!isPlayerCrossingUp() && !(isPlayerCrossingLeft() && isPlayerCrossingLeft()))
            // {
                if (playerMovement.playerIsOutside)
                {
                    if(!isPlayerCrossingUp() && playerMovement.change.x == 0)
                    {
                        building.EnterBuilding();
                        playerMovement.playerIsOutside = false;  
                    }                
                }
                else if (!playerMovement.playerIsOutside)
                {
                    building.ExitBuilding(0.3f, 0.3f, false);
                    playerMovement.playerIsOutside = true;  
                }
            // }

        }
    }
    
    private bool isPlayerCrossingUp()
    {
        return playerMovement.change.y > 0;
    }
    private bool isPlayerCrossingLeft()
    {
        return playerMovement.change.x < 0;
    }
}