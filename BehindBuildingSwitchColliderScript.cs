using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehindBuildingSwitchColliderScript : MonoBehaviour
{
    public BuildingScript building;

    CharacterMovement playerMovement;
    [SerializeField] GameObject Player;
    private Coroutine fadeCoroutine;


    void Awake()
    { 
        Player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = Player.GetComponent<CharacterMovement>(); 
    }  
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(building != null)
            {
                if (playerMovement.fixedDirectionLeftDiagonal || playerMovement.fixedDirectionRightDiagonal)
                {
                    // do nuttin
                }
                else if(!playerMovement.playerIsOutside)
                {
                    for (int i = 0; i < building.outerBuildingSpriteList.Count; i++)
                    {
                        building.SetTreeAlpha(building.outerBuildingSpriteList[i], 0.35f);
                    }                   
                }
                else
                {
                    building.GoBehindBuilding();     
                }
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(building != null)
            {
                if ((playerMovement.fixedDirectionLeftDiagonal || playerMovement.fixedDirectionRightDiagonal))
                {
                    // do nuttin
                }
                else if(!playerMovement.playerIsOutside)
                {
                    for (int i = 0; i < building.gameObjectsToShowWhileOutsideSpriteList.Count; i++)
                    {
                        building.SetTreeAlpha(building.gameObjectsToShowWhileOutsideSpriteList[i], building.gameObjectsToShowWhileOutsideColorList[i].a);
                    }              
                    for (int i = 0; i < building.outerBuildingSpriteList.Count; i++)
                    {
                        building.SetTreeAlpha(building.outerBuildingSpriteList[i], building.outerBuildingInitialColorList[i].a);
                    }              
                }
                else
                {   
                    building.ExitBuilding(0.1f, 0.1f, true);
                }
            }
        }        
    }
}


