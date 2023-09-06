using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSwitchScript : MonoBehaviour
{
    public RoomScript roomAbove;
    public RoomScript roomBelow;

    void OnTriggerEnter2D()
    {
        roomAbove.EnterRoom();
        roomBelow.ExitRoom();
    }
    void OnTriggerExit2D()
    {
        roomAbove.ExitRoom();
        roomBelow.EnterRoom();
    }
}
