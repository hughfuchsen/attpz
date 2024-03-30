using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSwitchColliderScript : MonoBehaviour
{
    public BuildingScript bulding;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnTriggerEnter2D()
    {
        bulding.EnterBuilding();
    }
    void OnTriggerExit2D()
    {
        bulding.ExitBuilding(1f, 1f, false);
    }
}




