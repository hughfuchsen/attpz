using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThresholdColliderScript : MonoBehaviour
{
    public RoomScript roomAbove;
    public RoomScript roomBelow;
    public SpriteRenderer openDoor;
    public SpriteRenderer closedDoor;

    public float currentAlpha = 1;

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
            closedDoor.color = new Color(closedDoor.color.r, closedDoor.color.g, closedDoor.color.b, currentAlpha);
        }

    }

    void OnTriggerEnter2D()
    {
        playerMovement.motionDirection = motionDirection;
                
        if (openDoor != null)
        {
            openDoor.color = new Color(openDoor.color.r, openDoor.color.g, openDoor.color.b, currentAlpha);
        }
        if (closedDoor != null)
        {
            closedDoor.color = new Color(closedDoor.color.r, closedDoor.color.g, closedDoor.color.b, 0);
        }

        if (!isPlayerCrossingUp())
        {
            if (roomBelow != null)
            {
                roomBelow.EnterRoom();
            }
        }
    }

    void OnTriggerExit2D()
    {
        playerMovement.motionDirection = previousMotionDirection;

        if (openDoor != null)
        {
            openDoor.color = new Color(openDoor.color.r, openDoor.color.g, openDoor.color.b, 0);
        }
        if (closedDoor != null)
        {
            closedDoor.color = new Color(closedDoor.color.r, closedDoor.color.g, closedDoor.color.b, currentAlpha);
        }

        if (isPlayerCrossingUp())
        {
            if (roomBelow != null)
            {
                roomBelow.ExitRoom();
            }
            if (roomAbove != null)
            {
                roomAbove.EnterRoom();
            }
        }
        else
        {
            if (roomAbove != null)
            {
                roomAbove.ExitRoom();
            }
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

    private Color SetAlpha(SpriteRenderer sprite, float alpha)
    {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
        return sprite.color;
    }
}
