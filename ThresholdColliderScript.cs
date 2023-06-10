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
    private bool onTrigger;

    PlayerMovement playerMovement;
    IsoSpriteSorting isoSpriteSorting; 
    private Vector3 originalSpriteSorterPos;

    [SerializeField] GameObject Player;
    public string motionDirection = "normal";
    public string previousMotionDirection = "normal";


    void Awake()
    { 
        Player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = Player.GetComponent<PlayerMovement>(); 
        isoSpriteSorting = Player.GetComponent<IsoSpriteSorting>();
        originalSpriteSorterPos = isoSpriteSorting.SorterPositionOffset;
        isoSpriteSorting.SorterPositionOffset2 = originalSpriteSorterPos;
        isoSpriteSorting.sortType = IsoSpriteSorting.SortType.Point;
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
        onTrigger = true;
        
        if (openDoor != null)
        {
            openDoor.color = new Color(openDoor.color.r, openDoor.color.g, openDoor.color.b, currentAlpha);
        }
        if (closedDoor != null)
        {
            closedDoor.color = new Color(closedDoor.color.r, closedDoor.color.g, closedDoor.color.b, 0);
        }

        if (isPlayerCrossingUp())
        {
            // if(isPlayerCrossingLeft())
            // {
            //     isoSpriteSorting.sortType = IsoSpriteSorting.SortType.Line;
            //     isoSpriteSorting.SorterPositionOffset = isoSpriteSorting.SorterPositionOffset + new Vector3(-1,1,0);
            //     isoSpriteSorting.SorterPositionOffset2 = isoSpriteSorting.SorterPositionOffset2 + new Vector3(1,0,0);
            // }
            // else
            // {
            //     isoSpriteSorting.sortType = IsoSpriteSorting.SortType.Line;
            //     isoSpriteSorting.SorterPositionOffset = isoSpriteSorting.SorterPositionOffset + new Vector3(1,1,0);
            //     isoSpriteSorting.SorterPositionOffset2 = isoSpriteSorting.SorterPositionOffset2 + new Vector3(-1,0,0);            
            // }        
        }
        else
        {
            if (roomBelow != null)
            {
                roomBelow.EnterRoom();
            }
            
            // if(isPlayerCrossingLeft())
            // {
            //     isoSpriteSorting.sortType = IsoSpriteSorting.SortType.Line;
            //     isoSpriteSorting.SorterPositionOffset = isoSpriteSorting.SorterPositionOffset + new Vector3(1,1,0);
            //     isoSpriteSorting.SorterPositionOffset2 = isoSpriteSorting.SorterPositionOffset2 + new Vector3(-1,0,0);             }
            // else
            // {
            //     isoSpriteSorting.sortType = IsoSpriteSorting.SortType.Line;
            //     isoSpriteSorting.SorterPositionOffset = isoSpriteSorting.SorterPositionOffset + new Vector3(-1,1,0);
            //     isoSpriteSorting.SorterPositionOffset2 = isoSpriteSorting.SorterPositionOffset2 + new Vector3(1,0,0);             
            // }
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

        playerMovement.motionDirection = previousMotionDirection;
        isoSpriteSorting.SorterPositionOffset = originalSpriteSorterPos;
        isoSpriteSorting.SorterPositionOffset2 = originalSpriteSorterPos;
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
