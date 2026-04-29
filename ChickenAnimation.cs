using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenAnimation : MonoBehaviour
{
    // [HideInInspector]
    public Sprite[] feetSprites;
    [HideInInspector] public float animationSpeed = 0.09f; // Time between frames

    public List<GameObject> characterSpriteList = new List<GameObject>();


    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
