using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelColliderScript : MonoBehaviour
{
    public LevelScript levelAbove;
    public LevelScript levelBelow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D()
    {
        levelAbove.EnterLevel();
    }
    private void OnTriggerExit2D()
    {
        levelAbove.ExitLevel();
    }
}
