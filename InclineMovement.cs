using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InclineMovement : MonoBehaviour
{
    PlayerMovement playerMovement;
    [SerializeField] GameObject Player;
    public string motionDirection;
    public string lowerSortingLayerToAssign;
    public string higherSortingLayerToAssign;
    public string lowerColliderLayerName; // The name of the layer you want to switch to
    public string higherColliderLayerName; // The name of the layer you want to switch to
    // public LayerMask lowerColliderLayerName; // The name of the layer you want to switch to
    // public LayerMask higherColliderLayerName; // The name of the layer you want to switch to
    public bool topOfStairCase;

    public bool middleOfStairCase;

    // Start is called before the first frame update

    void Start()
    {
        // SetCollisionLayer("Default");
        // for (int i = 0; i < 32; i++)
        // {
        //     for (int j = 0; j < 32; j++)
        //     {
        //         Physics2D.IgnoreLayerCollision(i, j, true);
        //     }
        // }
        // Debug.Log(LayerMask.NameToLayer("Default"));
        // Debug.Log(LayerMask.NameToLayer("Player"));
        // Debug.Log(LayerMask.NameToLayer("Level1"));
        // Debug.Log(LayerMask.NameToLayer("Stairs1"));
    }
    void Awake()
    {
        playerMovement = Player.GetComponent<PlayerMovement>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(!middleOfStairCase)
            {
                if((isPlayerCrossingUp() && isPlayerCrossingLeft()) 
                    || !isPlayerCrossingUp() && !isPlayerCrossingLeft())
                {
                    playerMovement.fixedDirectionLeft = true;
                }
                else
                {
                    playerMovement.fixedDirectionRight = true;
                }
            }

            if((isPlayerCrossingUp() && playerMovement.motionDirection == "normal" && !topOfStairCase) 
                || (!isPlayerCrossingUp() && playerMovement.motionDirection == "normal" && topOfStairCase))
            {
                playerMovement.motionDirection = motionDirection;  
            }
            else if((isPlayerCrossingUp() && playerMovement.motionDirection != "normal" && topOfStairCase) 
                || (!isPlayerCrossingUp() && playerMovement.motionDirection != "normal" && !topOfStairCase))
            {
                playerMovement.motionDirection = "normal";  
            }

            if(isPlayerCrossingUp() && topOfStairCase)
            {
                gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
                SetCollisionLayer(higherColliderLayerName);
                SetSortingLayer(collision.gameObject, higherSortingLayerToAssign);
            }
            else if(isPlayerCrossingUp() && !topOfStairCase)
            {
                gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
                SetCollisionLayer(higherColliderLayerName);
                SetSortingLayer(collision.gameObject, higherSortingLayerToAssign);
            }                       
            else if(!isPlayerCrossingUp() && topOfStairCase)
            {
                gameObject.layer = LayerMask.NameToLayer(lowerColliderLayerName);
                SetCollisionLayer(lowerColliderLayerName);
                SetSortingLayer(collision.gameObject, lowerSortingLayerToAssign);
            } 
            else if(!isPlayerCrossingUp() && !topOfStairCase)
            {
                gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
                SetCollisionLayer(higherColliderLayerName);
                SetSortingLayer(collision.gameObject, higherSortingLayerToAssign);
            } 
        }
    }



        void OnTriggerExit2D(Collider2D collision)
    {
            if(collision.gameObject.tag =="Player")
            {
                playerMovement.fixedDirectionLeft = false;
                playerMovement.fixedDirectionRight = false;


                if((isPlayerCrossingUp() && playerMovement.motionDirection == "normal" && !topOfStairCase) 
                    || (!isPlayerCrossingUp() && playerMovement.motionDirection == "normal" && topOfStairCase))
                {
                  playerMovement.motionDirection = motionDirection;  
                }
                else if((isPlayerCrossingUp() && playerMovement.motionDirection != "normal" && topOfStairCase) 
                    || (!isPlayerCrossingUp() && playerMovement.motionDirection != "normal" && !topOfStairCase))
                {
                  playerMovement.motionDirection = "normal";  
                }

                if(isPlayerCrossingUp() && topOfStairCase)
                {
                    gameObject.layer = LayerMask.NameToLayer(higherColliderLayerName);
                    SetCollisionLayer(higherColliderLayerName);
                    SetSortingLayer(collision.gameObject, higherSortingLayerToAssign);
                }            
                else if(!isPlayerCrossingUp() && !topOfStairCase)
                {
                    gameObject.layer = LayerMask.NameToLayer(lowerColliderLayerName);
                    SetCollisionLayer(lowerColliderLayerName);
                    SetSortingLayer(collision.gameObject, lowerSortingLayerToAssign);
                }
                else if(!isPlayerCrossingUp() && topOfStairCase)
                {
                    gameObject.layer = LayerMask.NameToLayer(lowerColliderLayerName);
                    SetCollisionLayer(lowerColliderLayerName);
                    SetSortingLayer(collision.gameObject, lowerSortingLayerToAssign);
                }
            }
    }

    // sortingLayerToAssign is just a variable name, it's given a value in the preceding code somewhere. 
    //In the SetSortingLayer function the second parameter is given the name sortingLayerName, 
    //but this name is local to the function definition. It works because when the function is called, 
    //the sortingLayerName variable inside the function is given the value of sortingLayerToAssign. 

    static void SetSortingLayer(GameObject gameObject, string sortingLayerName)
    {
        if(gameObject.GetComponent<SpriteRenderer>() != null) {
          gameObject.GetComponent<SpriteRenderer>().sortingLayerName = sortingLayerName;
        }
        foreach (Transform child in gameObject.transform)
        {
            InclineMovement.SetSortingLayer(child.gameObject, sortingLayerName);
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
    // public void SetCollisionLayer(string targetLayerName)
    // {
    //     GameObject.FindWithTag("Player").layer = LayerMask.NameToLayer(targetLayerName);
    // }
    
    public void SetCollisionLayer(string targetLayerName)
    {
        int targetLayerIndex = LayerMask.NameToLayer(targetLayerName);

        // Disable collisions with all layers
        for (int i = 0; i < 32; i++)
        {
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), i, true);
        }

        // Re-enable collision with the target layer
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), targetLayerIndex, false);
    }
    
}

