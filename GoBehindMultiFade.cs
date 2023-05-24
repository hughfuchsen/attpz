using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBehindMultiFade : MonoBehaviour
{
    public GameObject object1;
    public float fadeSpeed = 1;
    
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

        void OnTriggerEnter2D(Collider2D collision)
    {
   
    }
        void OnTriggerStay2D(Collider2D collision)
    {
      //if(collision.gameObject.tag =="Player")
      //{        
      FadeOutSelectedSprites(collision.gameObject);
      //}  
    }




        void OnTriggerExit2D(Collider2D collision)
    {
            if(collision.gameObject.tag =="Player")
            {

            }

    }

    void FadeOutSelectedSprites(GameObject gameObject)
    {
        if(object1.GetComponent<SpriteRenderer>() != null) {
            Color objectColor = object1.GetComponent<SpriteRenderer>().color;
            float fadeAmount = object1.GetComponent<SpriteRenderer>().color.a - (fadeSpeed * Time.deltaTime);

              if(fadeAmount<0.35f)
            {
             fadeAmount = 0.35f;
            }
            
             objectColor = new Color(objectColor.r,objectColor.g,objectColor.b,fadeAmount);
            }
       

        foreach (Transform child in object1.transform)
        {
            child.gameObject.SetActive(false);        
        }
    }

    }
    //Create an array of SpriteRenderers
