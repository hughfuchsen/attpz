using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onTriggerEnter : MonoBehaviour


{
    public List<SpriteRenderer> fadeItems = new List<SpriteRenderer>();

    public float fadeSpeed;
    private bool fadeOut = false;



    // Start is called before the first frame update

    
    void Start()
    {
       
    }

    // Update is called once per frame
     public void Update()
   {
     if(fadeOut)
     {
        for (int i = 0; i < fadeItems.Count; i++)
        {
            Color objectColor = fadeItems[i].color;
            float fadeAmount = fadeItems[i].color.a - (fadeSpeed * Time.deltaTime);

            if(fadeAmount<0)
            {
              fadeAmount = 0;
            }
            
            objectColor = new Color(objectColor.r,objectColor.g,objectColor.b,fadeAmount);
            fadeItems[i].color = objectColor;

        };
       
       
     }

     if(!fadeOut)
     {
       for (int i = 0; i < fadeItems.Count; i++)
        {
            Color objectColor = fadeItems[i].color;
            float fadeAmount = fadeItems[i].color.a + (fadeSpeed * Time.deltaTime);
          
            if(fadeAmount>1)
            {
              fadeAmount = 1;
            }

            objectColor = new Color(objectColor.r,objectColor.g,objectColor.b,fadeAmount);
            fadeItems[i].color = objectColor;
        }; 

       
     }

   
   }


    void OnTriggerStay2D(Collider2D collision)
  {
    if(collision.gameObject.tag =="Player")
    {
    fadeOut = true;
    }
  }
      void OnTriggerExit2D(Collider2D collision)
  {
    if(collision.gameObject.tag =="Player")
    {
     fadeOut = false;
    }
}
}

