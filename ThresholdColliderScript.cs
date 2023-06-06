using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThresholdColliderScript : MonoBehaviour
{
    public RoomScript roomAbove;
    public RoomScript roomBelow;
    public SpriteRenderer openDoor;
    public SpriteRenderer closedDoor;
    private bool onTrigger;

    PlayerMovement playerMovement;
    [SerializeField] GameObject Player;
    public string motionDirection = "normal";
    public string previousMotionDirection = "normal";


    void Awake()
    { 
       Player = GameObject.FindGameObjectWithTag("Player");
       playerMovement = Player.GetComponent<PlayerMovement>();     
    }  

    void Start()
    {
        if (openDoor != null)
        {
            openDoor.color = new Color(openDoor.color.r, openDoor.color.g, openDoor.color.b, 0);
        }
        if (closedDoor != null)
        {
            closedDoor.color = new Color(closedDoor.color.r, closedDoor.color.g, closedDoor.color.b, 1);
        }
    }

    void OnTriggerEnter2D()
    {
        onTrigger = true;
        
        if (openDoor != null)
        {
            openDoor.color = new Color(openDoor.color.r, openDoor.color.g, openDoor.color.b, 1);
        }
        if (closedDoor != null)
        {
            closedDoor.color = new Color(closedDoor.color.r, closedDoor.color.g, closedDoor.color.b, 0);
        }

        if (!isPlayerCrossingUp())
        {
            roomBelow.EnterRoom();
        }

        playerMovement.motionDirection = motionDirection;
    }

    void OnTriggerExit2D()
    {
        onTrigger = false;

        if (openDoor != null)
        {
            openDoor.color = new Color(openDoor.color.r, openDoor.color.g, openDoor.color.b, 0);
        }
        if (closedDoor != null)
        {
            closedDoor.color = new Color(closedDoor.color.r, closedDoor.color.g, closedDoor.color.b, 0.35f);
        }

        if (isPlayerCrossingUp())
        {
            roomAbove.EnterRoom();
            roomBelow.ExitRoom();
        }
        else
        {
            roomAbove.ExitRoom();
        }

        playerMovement.motionDirection = previousMotionDirection;
    }

    private bool isPlayerCrossingUp()
    {
        return GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().change.y > 0;
    }
}
