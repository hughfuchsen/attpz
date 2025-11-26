using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelThreshColliderScript : MonoBehaviour
{
    [SerializeField] GameObject Player;

    // public string initialSortingLayerUponEntry;

    public bool itsALadder;

    public LevelScript levelAboveOrEntering;

    public LevelScript levelBelowOrEntering;

    public bool plyrCrsngLeft = false;

    public Coroutine thresholdSortingSequenceCoro;

    private string initialSortingLayer;


    
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // private bool aboveCollider;

    private Dictionary<GameObject, bool> aboveColliderByCharacter = new Dictionary<GameObject, bool>();

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("PlayerCollider") || other.CompareTag("NPCCollider"))
        {
            CharacterMovement cm = other.transform.parent.GetComponent<CharacterMovement>();
            cm.currentLevelThreshhold = this;

            bool isAbove = !IsCrossingUp(cm); // if crossing down, they’re above

            aboveColliderByCharacter[other.gameObject] = isAbove;


            if(plyrCrsngLeft) // is player going this \ (left diag) way or
            // is player going this / (right diag) way :-)
            {
                // cm.fixedDirectionLeftDiagonal = true; // fix the player in \ left diag way while inside the collider
                cm.controlDirectionToPlayerDirection[Direction.Left] = Direction.UpLeft;
                cm.controlDirectionToPlayerDirection[Direction.UpLeft] = Direction.UpLeft;
                cm.controlDirectionToPlayerDirection[Direction.UpFacingLeft] = Direction.UpLeft;
                cm.controlDirectionToPlayerDirection[Direction.UpFacingRight] = Direction.UpLeft;
                cm.controlDirectionToPlayerDirection[Direction.UpRight] = Direction.UpLeft;
                cm.controlDirectionToPlayerDirection[Direction.Right] = Direction.RightDown;
                cm.controlDirectionToPlayerDirection[Direction.RightDown] = Direction.RightDown;
                cm.controlDirectionToPlayerDirection[Direction.DownFacingRight] = Direction.RightDown;
                cm.controlDirectionToPlayerDirection[Direction.DownFacingLeft] = Direction.RightDown;
                cm.controlDirectionToPlayerDirection[Direction.DownLeft] = Direction.RightDown;
            }
            else
            {
                // cm.fixedDirectionRightDiagonal = true; // fix the player in / right diag way while inside the collider
                cm.controlDirectionToPlayerDirection[Direction.Left] = Direction.DownLeft;
                cm.controlDirectionToPlayerDirection[Direction.UpLeft] = Direction.UpRight;
                cm.controlDirectionToPlayerDirection[Direction.UpFacingLeft] = Direction.UpRight;
                cm.controlDirectionToPlayerDirection[Direction.UpFacingRight] = Direction.UpRight;
                cm.controlDirectionToPlayerDirection[Direction.UpRight] = Direction.UpRight;
                cm.controlDirectionToPlayerDirection[Direction.Right] = Direction.UpRight;
                cm.controlDirectionToPlayerDirection[Direction.RightDown] = Direction.DownLeft;
                cm.controlDirectionToPlayerDirection[Direction.DownFacingRight] = Direction.DownLeft;
                cm.controlDirectionToPlayerDirection[Direction.DownFacingLeft] = Direction.DownLeft;
                cm.controlDirectionToPlayerDirection[Direction.DownLeft] = Direction.DownLeft;
            }
        }
        // fixing the player's movement inside the collider ensures the correct conditions are met upon collider exit

    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("PlayerCollider"))
        {
            CharacterMovement cm = other.transform.parent.GetComponent<CharacterMovement>();
            cm.currentLevelThreshhold = null;

            bool aboveCollider = aboveColliderByCharacter.ContainsKey(other.gameObject) && aboveColliderByCharacter[other.gameObject];

            // un-fix the player in \/ left/right diag way upon collider exit. 
            cm.ResetPlayerMovement();     
            
            if(IsCrossingUp(cm)) //crossing up bro
            {
                if(this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() != null && cm.playerIsOutside)
                {
                    if(levelAboveOrEntering != null)
                    {
                        levelAboveOrEntering.ResetLevels();
                    }
                    else if(levelBelowOrEntering != null)
                    {                            
                        levelBelowOrEntering.ResetLevels();
                    }

                }
                else if(this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() != null && !cm.playerIsOutside)
                {
                    if(levelAboveOrEntering != null)
                    {
                        // for (int i = 0; i < levelAboveOrEntering.Count; i++)
                        // {                      
                            levelAboveOrEntering.EnterLevel(true, true);
                        // }
                    }
                    else if(levelBelowOrEntering != null)
                    {
                        // for (int i = 0; i < levelBelowOrEntering.Count; i++)
                        // {                          
                            levelBelowOrEntering.EnterLevel(true, true);
                        // }
                    }
                }
                else if(levelAboveOrEntering != null)
                {
                    // for (int i = 0; i < levelAboveOrEntering.Count; i++)
                    // {                      
                        levelAboveOrEntering.EnterLevel(false, true);
                    // }
                }  
                // }

            }
            else    // if player crossing down
            {
                if(this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() != null && aboveCollider && cm.playerIsOutside) 
                {
                    // insert only the level you are going through my dog!
                    if(levelBelowOrEntering != null)
                    {
                        // for (int i = 0; i < levelBelowOrEntering.Count; i++)
                        // {      
                            levelBelowOrEntering.ResetLevels();
                        // }
                    }        
                    if(levelAboveOrEntering != null)
                    {
                        // for (int i = 0; i < levelAboveOrEntering.Count; i++)
                        // {  
                            levelAboveOrEntering.ResetLevels();
                        // }
                    }   
                }
                else if(this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() != null && aboveCollider && !cm.playerIsOutside) 
                {
                    if(levelAboveOrEntering != null)
                    {
                        // for (int i = 0; i < levelAboveOrEntering.Count; i++)
                        // {                      
                            levelAboveOrEntering.EnterLevel(true, false);
                        // }
                    }   
                    else if(levelBelowOrEntering != null)
                    {
                        // for (int i = 0; i < levelBelowOrEntering.Count; i++)
                        // {                          
                            levelBelowOrEntering.EnterLevel(true, false);
                        // }
                    }        
                }
                // else if(moveLevelsAbove && aboveCollider)
                // {
                else if(levelBelowOrEntering != null)
                {
                    // for (int i = 0; i < levelBelowOrEntering.Count; i++)
                    // {                          
                        levelBelowOrEntering.EnterLevel(false, false);
                    // }
                }       
                // }


            }
            if(this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() == null)
            {
                thresholdSortingSequenceCoro = StartCoroutine(ThresholdLayerSortingSequence(0.3f, Player, "ThresholdSequence"));
            }

        }
        else if(other.CompareTag("NPCCollider"))
        {
            GameObject character = other.transform.parent.gameObject;

            CharacterMovement cm = other.transform.parent.GetComponent<CharacterMovement>();
            cm.currentLevelThreshhold = null;

            bool aboveCollider = aboveColliderByCharacter.ContainsKey(other.gameObject) && aboveColliderByCharacter[other.gameObject];
            
            // un-fix the player in \/ left/right diag way upon collider exit. 
            cm.ResetPlayerMovement(); 
            
            if(IsCrossingUp(cm)) //crossing up bro
            {
                if(this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() != null && cm.playerIsOutside)
                {
                    if(levelAboveOrEntering != null)
                    {
                        // levelAboveOrEntering.NpcEnterLevel(character);
                        // for (int i = 0; i < levelAboveOrEntering.Count; i++)
                        // {                       
                            // levelAboveOrEntering[i].ResetLevels();
                        // }    
                    }
                    else if(levelBelowOrEntering != null)
                    {
                        // levelBelowOrEntering.NpcEnterLevel(character);
                        // for (int i = 0; i < levelBelowOrEntering.Count; i++)
                        // {                        
                            // levelBelowOrEntering[i].ResetLevels();
                        // }
                    }

                }
                else if(this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() != null && !cm.playerIsOutside)
                {
                    if(levelAboveOrEntering != null)
                    {                     
                        // levelAboveOrEntering.NpcEnterLevel(character);
                    }
                    else if(levelBelowOrEntering != null)
                    {
                        // levelBelowOrEntering.NpcEnterLevel(character);
                    }
                }
                else if(levelAboveOrEntering != null)
                {
                    // levelAboveOrEntering.NpcEnterLevel(character);
                }  

            }
            else    // if player crossing down
            {
                if(this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() != null && aboveCollider && cm.playerIsOutside) 
                {
                    // insert only the level you are going through my dog!
                    if(levelBelowOrEntering != null)
                    {
                        // levelBelowOrEntering.NpcEnterLevel(character);

                        // for (int i = 0; i < levelBelowOrEntering.Count; i++)
                        // {      
                        //     levelBelowOrEntering[i].ResetLevels();
                        // }
                    }        
                    if(levelAboveOrEntering != null)
                    {
                        // levelAboveOrEntering.NpcEnterLevel(character);

                        // for (int i = 0; i < levelAboveOrEntering.Count; i++)
                        // {  
                        //     levelAboveOrEntering[i].ResetLevels();
                        // }
                    }   
                }
                else if(this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() != null && aboveCollider && !cm.playerIsOutside) 
                {
                    if(levelAboveOrEntering != null)
                    {
                        // levelAboveOrEntering.NpcEnterLevel(character);
                    }   
                    else if(levelBelowOrEntering != null)
                    {
                        // levelBelowOrEntering.NpcEnterLevel(character);
                    }        
                }
                else if(levelBelowOrEntering != null)
                {
                    // levelBelowOrEntering.NpcEnterLevel(character);
                }       

            }
            // if(this.transform.parent.GetComponentInChildren<BuildingThreshColliderScript>() == null)
            // {
            //     thresholdSortingSequenceCoro = StartCoroutine(ThresholdLayerSortingSequence(0.3f, Player, "ThresholdSequence"));
            // }
           
        }
        aboveColliderByCharacter.Remove(other.gameObject); // bool logic
    }

    IEnumerator ThresholdLayerSortingSequence(
    float waitTime,
    GameObject gameObject,
    string newSortingLayer) 
    {  
        initialSortingLayer = Player.transform.Find("head").GetComponent<SpriteRenderer>().sortingLayerName; 
        // initialSortingLayer = LayerMask.LayerToName(this.gameObject.layer);

        SetTreeSortingLayer(gameObject, newSortingLayer);

        yield return new WaitForSeconds(waitTime);

        if (Player.transform.Find("head").GetComponent<SpriteRenderer>().sortingLayerName == newSortingLayer)
        {
            SetTreeSortingLayer(gameObject, initialSortingLayer);
        }

        yield return null;
    }


    static void SetTreeSortingLayer(GameObject gameObject, string sortingLayerName)
    {
        if(gameObject.GetComponent<SpriteRenderer>() != null) 
        {
            gameObject.GetComponent<SpriteRenderer>().sortingLayerName = sortingLayerName;
        }
        foreach (Transform child in gameObject.transform)
        {
            LevelThreshColliderScript.SetTreeSortingLayer(child.gameObject, sortingLayerName);
        }
    }  

    private bool IsCrossingUp(CharacterMovement cm)
    {
        return cm.change.y > 0;
    }
    private bool IsCrossingLeft(CharacterMovement cm)
    {
        return cm.change.x < 0;
    }
}
