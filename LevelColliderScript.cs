using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelColliderScript : MonoBehaviour
{
    PlayerMovement playerMovement;
    [SerializeField] GameObject Player;
    // public string initialSortingLayerUponEntry;
    public LevelScript levelAbove;
    public LevelScript levelBelow;
    public bool levelAboveMove;
    public bool levelThreshold;
    
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = Player.GetComponent<PlayerMovement>();
        // initialSortingLayerUponEntry = Player.GetComponent<SpriteRenderer>().sortingLayerName; 
    }

    private void OnTriggerEnter2D()
    {
        // levelAbove.EnterLevel();

        // if(isPlayerCrossingUp() && levelAboveMove)
        // {
        // // levelAbove.EnterLevel();
        // }
        // else if(isPlayerCrossingUp() && !levelAboveMove)
        // {

        // }                       
        // else if(!isPlayerCrossingUp() && levelAboveMove)
        // {
        //     gameObject.layer = LayerMask.NameToLayer(lowerColliderLayerName);
        //     SetCollisionLayer(lowerColliderLayerName);
        //     SetSortingLayer(collision.gameObject, lowerSortingLayerToAssign);
        // } 
        // else if(!isPlayerCrossingUp() && !levelAboveMove)
        // {
        //     gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
        //     SetCollisionLayer(higherColliderLayerName);
        //     SetSortingLayer(collision.gameObject, higherSortingLayerToAssign);
        // }     
        }
    private void OnTriggerExit2D()
    {
        if(isPlayerCrossingUp() && levelAboveMove)
        {
            levelAbove.EnterLevel();
        }            
        else if(!isPlayerCrossingUp() && levelAboveMove)
        {
            levelAbove.ExitLevel();
        }   

        if(isPlayerCrossingUp() && levelThreshold)
        {
            levelBelow.MoveDown();
        }            
        else if(!isPlayerCrossingUp() && levelThreshold)
        {
            levelBelow.MoveUp();
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
