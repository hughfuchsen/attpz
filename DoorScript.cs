using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public GameObject doorObj;
    public SpriteRenderer doorSprite;

    public Coroutine doorCoro;
    // Start is called before the first frame update
    void Start()
    {
        doorSprite = doorObj.GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {

    }

    IEnumerator SlideDoor(bool open)
    {
        // animate the 
        yield return null;
    }
}
