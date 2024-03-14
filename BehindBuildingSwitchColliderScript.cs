using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehindBuildingSwitchColliderScript : MonoBehaviour
{
    public BuildingScript building;

    PlayerMovement playerMovement;
    [SerializeField] GameObject Player;
    private Coroutine fadeCoroutine;


    void Awake()
    { 
        Player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = Player.GetComponent<PlayerMovement>(); 
    }  
    void OnTriggerEnter2D()
    {
        if(building != null)
        {
            if (playerMovement.fixedDirectionLeft || playerMovement.fixedDirectionRight)
            {
                // do nuttin
            }
            else
            {
                building.GoBehindBuilding();
            }
        }
    }
    void OnTriggerExit2D()
    {
        if(building != null)
        {
            if (playerMovement.fixedDirectionLeft || playerMovement.fixedDirectionRight)
            {
                // do nuttin
            }
            else
            {
                building.ExitBuilding(0.3f, 0.3f);
            }
        }        
    }
}
