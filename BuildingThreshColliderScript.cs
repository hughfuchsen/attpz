using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingThreshColliderScript : MonoBehaviour
{
    public BuildingScript building;
    PlayerMovement playerMovement;
    [SerializeField] GameObject Player;
    private bool aboveCollider;


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = Player.GetComponent<PlayerMovement>(); 
    }
    void OnTriggerEnter2D()
    {
        if (isPlayerCrossingUp())
        {
            aboveCollider = false;
        }    
        else 
        {
            aboveCollider = true; 
        }
    }
    void OnTriggerExit2D()
    {
        if(isPlayerCrossingUp())
        {
            if (playerMovement.isPlayerInside && !aboveCollider)
                {
                    building.GoBehindBuilding();
                    
                    playerMovement.isPlayerInside = false;
                }
                else if (!aboveCollider)
                {
                    building.EnterBuilding();

                    playerMovement.isPlayerInside = true;
                }
            aboveCollider = true;   
        }
        else
        {
                if (playerMovement.isPlayerInside && aboveCollider)
                {                    
                    building.ExitBuilding();

                    playerMovement.isPlayerInside = false;          
                }
                else if (aboveCollider)
                {
                    building.EnterBuilding();

                    playerMovement.isPlayerInside = true;
                }
            
            aboveCollider = false;    
        }
    }
    private bool isPlayerCrossingUp()
    {
        return playerMovement.change.y > 0;
    }
}
