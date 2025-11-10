using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingThreshColliderScript : MonoBehaviour
{
    public BuildingScript building;
    // CharacterMovement cm;
    [SerializeField] GameObject Player;
    public bool backOfBuilding;
    public bool rooftopLadder;

    SoundtrackScript soundtrackScript;

    private Dictionary<GameObject, bool> aboveColliderByCharacter = new Dictionary<GameObject, bool>();



    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        // cm = Player.GetComponent<CharacterMovement>(); 
        soundtrackScript = GameObject.FindGameObjectWithTag("SoundtrackScript").GetComponent<SoundtrackScript>();

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("PlayerCollider"))
        {
            CharacterMovement cm = other.transform.parent.GetComponent<CharacterMovement>();
        
            bool isAbove = !IsCrossingUp(cm); // if crossing down, they’re above

            aboveColliderByCharacter[other.gameObject] = isAbove;
            cm.playerOnBuildingThresh = true;

            if(rooftopLadder)
            {
                // if(IsCrossingUp(CharacterMovement cm))
                // {
                    if (!cm.playerIsOutside)
                    {           
                        // Debug.Log("exiting");     
                        building.ExitBuilding(0.3f, 0.3f, false);
                        cm.playerIsOutside = true;  
                    }
                // }
            }
        }
        else if (other.CompareTag("NPCCollider"))
        {
            GameObject character = other.transform.parent.gameObject;

            CharacterMovement cm = other.transform.parent.GetComponent<CharacterMovement>();
        
            bool isAbove = !IsCrossingUp(cm); // if crossing down, they’re above

            aboveColliderByCharacter[other.gameObject] = isAbove;
            cm.playerOnBuildingThresh = true;

            if(rooftopLadder)
            {
                // if(IsCrossingUp(CharacterMovement cm))
                // {
                    if (!cm.playerIsOutside)
                    {           
                        // Debug.Log("exiting");     
                        // building.ExitBuilding(0.3f, 0.3f, false);
                        // building.npcListForBuilding.Remove(other.transform.parent.gameObject);
                        building.npcEnterExitBuilding(character, false);
                        cm.playerIsOutside = true;  
                    }
                // }
            }
        }

    }
    void OnTriggerExit2D(Collider2D other)
    {
        GameObject character = other.transform.parent.gameObject;

        CharacterMovement cm = other.transform.parent.GetComponent<CharacterMovement>(); // how do i control multiple of these?
        
        bool aboveCollider = aboveColliderByCharacter.ContainsKey(other.gameObject) && aboveColliderByCharacter[other.gameObject];

        if(other.CompareTag("PlayerCollider"))
        {
            cm.playerOnBuildingThresh = false;

            if(!rooftopLadder)
            { 
                if(IsCrossingUp(cm)) //if player crossing up
                {
                    if (backOfBuilding)
                    {
                        if (!cm.playerIsOutside)
                        {   
                            if(this.GetComponent<RoomThresholdColliderScript>().roomAbove == null)
                            {        
                                building.GoBehindBuilding(); // go outside behind the buildinng
                                soundtrackScript.FadeOutIn(soundtrackScript.track2, soundtrackScript.track1);
                                cm.playerIsOutside = true;
                            }
                        }
                        else
                        {
                            if(this.GetComponent<RoomThresholdColliderScript>().roomAbove != null)
                            {                          
                                building.EnterBuilding();
                                cm.playerIsOutside = false;
                            }
                        }
                    }
                    else
                    {
                            if(cm.playerIsOutside) 
                            {     
                                building.EnterBuilding();
                            }                
                            cm.playerIsOutside = false;            
                    }
                }
                else //if player crossing down
                {
                        if (backOfBuilding)   // entering the building from the back
                        {   
                            if(cm.playerIsOutside) 
                            {     
                                if(this.GetComponent<RoomThresholdColliderScript>().roomBelow != null)
                                {
                                    building.EnterBuilding();
                                    cm.playerIsOutside = false;  
                                }          
                            }
                            else
                            {
                                if(this.GetComponent<RoomThresholdColliderScript>().roomBelow == null)
                                {
                                    building.GoBehindBuilding(); // go outside behind the buildinng
                                    soundtrackScript.FadeOutIn(soundtrackScript.track2, soundtrackScript.track1);
                                    cm.playerIsOutside = true;  
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
                            cm.playerIsOutside = true;
                        }
                    
                }
            }
            else
            {
                    if (cm.playerIsOutside)
                    {
                        if(!IsCrossingUp(cm) && ((cm.motionDirection == "upLadder" || cm.motionDirection == "downLadder" || cm.motionDirection == "upDownLadder")))
                        {
                            building.EnterBuilding();
                            cm.playerIsOutside = false;  
                        }                
                    }
                    else if (!cm.playerIsOutside)
                    {
                        building.ExitBuilding(0.3f, 0.3f, false);
                        cm.playerIsOutside = true;  
                    }

            }
        }
        else if (other.CompareTag("NPCCollider")) //if it's an NPC
        {
            cm.playerOnBuildingThresh = false;

            if(!rooftopLadder)
            { 
                if(IsCrossingUp(cm)) //if player crossing up
                {
                    if (backOfBuilding)
                    {
                        if (!cm.playerIsOutside)
                        {   
                            if(this.GetComponent<RoomThresholdColliderScript>().roomAbove == null)
                            {        
                                // building.GoBehindBuilding(); // go outside behind the buildinng
                                // building.npcListForBuilding.Remove(other.transform.parent.gameObject);
                                building.npcEnterExitBuilding(character, false);
                                cm.playerIsOutside = true;
                            }
                        }
                        else
                        {
                            if(this.GetComponent<RoomThresholdColliderScript>().roomAbove != null)
                            {           
                                // building.npcListForBuilding.Add(other.transform.parent.gameObject);
                                building.npcEnterExitBuilding(character, true);
                                cm.playerIsOutside = false;
                            }
                        }
                    }
                    else
                    {
                        if(cm.playerIsOutside) 
                        {     
                            // building.EnterBuilding();
                            // building.npcListForBuilding.Add(other.transform.parent.gameObject);
                            building.npcEnterExitBuilding(character, true);
                        }                
                        cm.playerIsOutside = false;            
                    }
                }
                else //if player crossing down
                {
                        if (backOfBuilding)   // entering the building from the back
                        {   
                            if(cm.playerIsOutside) 
                            {     
                                if(this.GetComponent<RoomThresholdColliderScript>().roomBelow != null)
                                {
                                    // building.EnterBuilding();
                                    // building.npcListForBuilding.Add(other.transform.parent.gameObject);
                                    building.npcEnterExitBuilding(character, true);
                                    cm.playerIsOutside = false;  
                                }          
                            }
                            else
                            {
                                if(this.GetComponent<RoomThresholdColliderScript>().roomBelow == null)
                                {
                                    // building.GoBehindBuilding(); // go outside behind the buildinng
                                    // building.npcListForBuilding.Remove(other.transform.parent.gameObject);
                                    building.npcEnterExitBuilding(character, false);
                                    cm.playerIsOutside = true;  
                                }          
                            }
                        }
                        else // entering the building from the front
                        {
                            if (LayerMask.LayerToName(this.gameObject.layer) != "Default") // if player is not exiting building on the ground level
                            {
                                // building.ExitBuilding(0.3f, 0.3f, false);
                                // building.npcListForBuilding.Remove(other.transform.parent.gameObject);
                                building.npcEnterExitBuilding(character, false);
                            }
                            else      // if player is exiting building on the ground level
                            {
                                // building.ExitBuilding(0.3f, 0.3f, false); //
                                // building.npcListForBuilding.Remove(other.transform.parent.gameObject);
                                building.npcEnterExitBuilding(character, false);
                            }
                            cm.playerIsOutside = true;
                        }
                    
                }
            }
            else // if it is a rooftop Ladder
            {
                    if (cm.playerIsOutside)
                    {
                        if(!IsCrossingUp(cm) && ((cm.motionDirection == "upLadder" || cm.motionDirection == "downLadder" || cm.motionDirection == "upDownLadder")))
                        {
                            // building.EnterBuilding();
                            // building.npcListForBuilding.Add(other.transform.parent.gameObject);
                            building.npcEnterExitBuilding(character, true);
                            cm.playerIsOutside = false;  
                        }                
                    }
                    else if (!cm.playerIsOutside)
                    {
                        // building.ExitBuilding(0.3f, 0.3f, false);
                        // building.npcListForBuilding.Remove(other.transform.parent.gameObject);
                        building.npcEnterExitBuilding(character, false);
                        cm.playerIsOutside = true;  
                    }
            }           
        }
        aboveColliderByCharacter.Remove(other.gameObject);
    }
    private bool IsCrossingUp(CharacterMovement cm)
    {
        return cm.change.y > 0;
    }
}