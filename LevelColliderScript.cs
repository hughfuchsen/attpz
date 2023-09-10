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
        else
        {
            aboveCollider = true;
        }
    }
    private void OnTriggerExit2D()
    {

        if(isPlayerCrossingUp() && levelAboveMove && !aboveCollider)
        {
            levelAbove.EnterLevel();
            aboveCollider = true;
        }            
        else if(!isPlayerCrossingUp() && levelAboveMove && aboveCollider)
        {
            levelAbove.ExitLevel();
            aboveCollider = false;
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
