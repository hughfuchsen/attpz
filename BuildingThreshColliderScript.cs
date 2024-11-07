using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingThreshColliderScript : MonoBehaviour
{
    public BuildingScript building;
    CharacterMovement characterMovement;
    [SerializeField] GameObject Player;
    public bool backOfBuilding;
    public bool rooftopLadder;

    SoundtrackScript soundtrackScript;


    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        characterMovement = Player.GetComponent<CharacterMovement>(); 
        soundtrackScript = GameObject.FindGameObjectWithTag("SoundtrackScript").GetComponent<SoundtrackScript>();

    }

    void OnTriggerEnter2D()
    {
        characterMovement.playerOnThresh = true;

        if(rooftopLadder)
        {
            // if(isPlayerCrossingUp())
            // {
                if (!characterMovement.playerIsOutside)
                {           
                    // Debug.Log("exiting");     
                    building.ExitBuilding(0.3f, 0.3f, false);
                    characterMovement.playerIsOutside = true;  
                }
            // }
        }
    }
    void OnTriggerExit2D()
    {
        characterMovement.playerOnThresh = false;

        if(!rooftopLadder)
        { 
            if(isPlayerCrossingUp()) //if player crossing up
            {
                if (backOfBuilding)
                {
                    if (!characterMovement.playerIsInside())
                    {   
                        if(this.GetComponent<RoomThresholdColliderScript>().roomAbove == null)
                        {           
                            building.GoBehindBuilding(); // go outside behind the buildinng
                            soundtrackScript.FadeOutIn(soundtrackScript.track2, soundtrackScript.track1);
                            characterMovement.playerIsOutside = true;
                        }
                    }
                    else
                    {
                        if(this.GetComponent<RoomThresholdColliderScript>().roomAbove != null)
                        {                          
                            building.EnterBuilding();
                            characterMovement.playerIsOutside = false;
                        }
                    }
                }
                else
                {
                        if(characterMovement.playerIsOutside) 
                        {     
                            building.EnterBuilding();
                        }                
                        characterMovement.playerIsOutside = false;            
                }
            }
            else //if player crossing down
            {
                    if (backOfBuilding)   // entering the building from the back
                    {   
                        if(characterMovement.playerIsOutside) 
                        {     
                            if(this.GetComponent<RoomThresholdColliderScript>().roomBelow != null)
                            {
                                building.EnterBuilding();
                                characterMovement.playerIsOutside = false;  
                            }          
                        }
                        else
                        {
                            if(this.GetComponent<RoomThresholdColliderScript>().roomBelow == null)
                            {
                                building.GoBehindBuilding(); // go outside behind the buildinng
                                soundtrackScript.FadeOutIn(soundtrackScript.track2, soundtrackScript.track1);
                                characterMovement.playerIsOutside = true;  
                            }          
                        }
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
                        characterMovement.playerIsOutside = true;
                    }
                
            }
        }
        else
        {
            // if(!isPlayerCrossingUp() && !(isPlayerCrossingLeft() && isPlayerCrossingLeft()))
            // {
                if (characterMovement.playerIsOutside)
                {
                    if(!isPlayerCrossingUp() && characterMovement.change.x == 0)
                    {
                        building.EnterBuilding();
                        characterMovement.playerIsOutside = false;  
                    }                
                }
                else if (!characterMovement.playerIsOutside)
                {
                    building.ExitBuilding(0.3f, 0.3f, false);
                    characterMovement.playerIsOutside = true;  
                }
            // }

        }
    }
    
    private bool isPlayerCrossingUp()
    {
        return characterMovement.change.y > 0;
    }
    private bool isPlayerCrossingLeft()
    {
        return characterMovement.change.x < 0;
    }
}