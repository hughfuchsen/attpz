using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InclineMovement : MonoBehaviour
{
    PlayerMovement playerMovement;
    [SerializeField] GameObject Player;
    public string motionDirection;
    public string previousMotionDirection;
    public string sortingLayerToAssign;
    
    // Start is called before the first frame update
    void Awake()
    {
        playerMovement = Player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

        void OnTriggerEnter2D(Collider2D collision)
    {
      if(collision.gameObject.tag =="Player")
            {
                previousMotionDirection = playerMovement.motionDirection;
                playerMovement.motionDirection = motionDirection;
                InclineMovement.SetSortingLayer(collision.gameObject, sortingLayerToAssign);
            }
    }



        void OnTriggerExit2D(Collider2D collision)
    {
            if(collision.gameObject.tag =="Player")
            {
                playerMovement.motionDirection = previousMotionDirection;
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
}
