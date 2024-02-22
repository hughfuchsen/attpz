using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingThreshColliderScript : MonoBehaviour
{
    public BuildingScript building;
    PlayerMovement playerMovement;
    [SerializeField] GameObject Player;
    public bool backOfBuilding;


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = Player.GetComponent<PlayerMovement>(); 
    }


    void OnTriggerExit2D()
    {
        if(isPlayerCrossingUp())
        {
            if (backOfBuilding)
                {
                    building.GoBehindBuilding();
                    playerMovement.isPlayerInside = false;
                }
                else
                {
                    building.EnterBuilding();
                    playerMovement.isPlayerInside = true;
                }
        }
        else //if player crossing down
        {
                if (backOfBuilding)
                {         
                    building.EnterBuilding();
                    playerMovement.isPlayerInside = true;
                }
                else
                {
                    building.ExitBuilding();
                    playerMovement.isPlayerInside = false;
                }
            
        }
    }
    
    private bool isPlayerCrossingUp()
    {
        return playerMovement.change.y > 0;
    }
}
