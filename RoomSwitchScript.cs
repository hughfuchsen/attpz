using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSwitchScript : MonoBehaviour
{
    public RoomScript roomAbove;
    public RoomScript roomBelow;

    void OnTriggerEnter2D()
    {
        roomAbove.EnterRoom(false, 0f);
        roomBelow.ExitRoom();
    }
    void OnTriggerExit2D()
    {
        roomAbove.ExitRoom();
        roomBelow.EnterRoom(false, 0f);
    }
}
