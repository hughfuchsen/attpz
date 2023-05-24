using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenScript : MonoBehaviour
{


    public GameObject door;
    public GameObject doorTrigger;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onTriggerEnter(Collider2D collision)
    
    {
       if(collision.gameObject.tag =="Player")
        {
        door.GetComponent<Transform>().position = new Vector3(-1,0,0);
        }
    }
}
