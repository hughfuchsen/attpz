using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThresholdColliderScript : MonoBehaviour
{
    public RoomScript roomAbove;
    
    public RoomScript roomBelow;

    public SpriteRenderer openDoor;
    
    public SpriteRenderer closedDoor;


    void OnTriggerEnter2D() {
        if(!isPlayerCrossingUp()) {
            roomBelow.EnterRoom();
        }
        openDoor.color = new Color(openDoor.color.r, openDoor.color.g, openDoor.color.b, 1);
        closedDoor.color = new Color(closedDoor.color.r, closedDoor.color.g, closedDoor.color.b, 0);
    }

    void OnTriggerExit2D() {
        if(isPlayerCrossingUp()) {
            roomAbove.EnterRoom();
            roomBelow.ExitRoom();            
        } else {
            roomAbove.ExitRoom();            
        }
        openDoor.color = new Color(openDoor.color.r, openDoor.color.g, openDoor.color.b, 0);
        closedDoor.color = new Color(closedDoor.color.r, closedDoor.color.g, closedDoor.color.b, 0.35f);
    }

    private bool isPlayerCrossingUp()
    {
        return GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().change.y > 0;
    }
}
