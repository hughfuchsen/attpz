using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerAppear : MonoBehaviour
{

   
    public List<GameObject> objectsToActivate = new List<GameObject>();

    public List<GameObject> objectsToDeActivate = new List<GameObject>();


    // Start is called before the first frame update
    void Awake()
    {

        foreach(GameObject objectsToActivate in objectsToActivate)
        objectsToActivate.SetActive(false);
       

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag =="Player")
        {
        foreach(GameObject objectsToActivate in objectsToActivate)
        objectsToActivate.SetActive(true);

        foreach(GameObject objectsToDeActivate in objectsToDeActivate)
        objectsToDeActivate.SetActive(false);
   
   
        }
    }
    
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag =="Player")
        {
        
        foreach(GameObject objectsToDeActivate in objectsToDeActivate)
        objectsToDeActivate.SetActive(true);
        
        foreach(GameObject objectsToActivate in objectsToActivate)
        objectsToActivate.SetActive(false);
        }
    }

  }

