using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehindBuildingSwitchColliderScript : MonoBehaviour
{
    public BuildingScript building;

    private Coroutine fadeCoroutine;

    void OnTriggerEnter2D()
    {
        if(building != null)
        {
            building.GoBehindBuilding();
        }
    }
    void OnTriggerExit2D()
    {
        if(building != null)
        {
            building.ExitBuilding(0.3f, 0.3f);
        }        
    }
}
