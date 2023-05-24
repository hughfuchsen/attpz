using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantFadeTrigger : MonoBehaviour
{
    public List<SpriteRenderer> objectsToFadeOut = new List<SpriteRenderer>();
    public List<SpriteRenderer> objectsToFadeIn = new List<SpriteRenderer>();
    public float fadeSpeed = 20;

    // Start is called before the first frame update
    void Start()
    {
      for (int i = 0; i < objectsToFadeIn.Count; i++)
        {
            Color objectColorIn = objectsToFadeIn[i].color;
            float fadeInAmount = objectsToFadeIn[i].color.a;

           
              fadeInAmount = 0f;
            
            
            objectColorIn = new Color(objectColorIn.r,objectColorIn.g,objectColorIn.b,fadeInAmount);
            objectsToFadeIn[i].color = objectColorIn;

        };   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void OnTriggerEnter2D(Collider2D collision)    
    {
            if(collision.gameObject.tag =="Player")
            {
                instantFade();
            }
    }
    void OnTriggerExit2D(Collider2D collision)    
    {
            if(collision.gameObject.tag =="Player")
            {
                instantFadeReverse();
            }
    }

    void instantFade()
    {
        for (int i = 0; i < objectsToFadeOut.Count; i++)
        {
            Color objectColorOut = objectsToFadeOut[i].color;
            float fadeOutAmount = objectsToFadeOut[i].color.a;

           
              fadeOutAmount = 0f;
            
            
            objectColorOut = new Color(objectColorOut.r,objectColorOut.g,objectColorOut.b,fadeOutAmount);
            objectsToFadeOut[i].color = objectColorOut;

        };
          for (int i = 0; i < objectsToFadeIn.Count; i++)
        {
            Color objectColorIn = objectsToFadeIn[i].color;
            float fadeInAmount = objectsToFadeIn[i].color.a;
          
          
              fadeInAmount = 1;
            

            objectColorIn = new Color(objectColorIn.r,objectColorIn.g,objectColorIn.b,fadeInAmount);
            objectsToFadeIn[i].color = objectColorIn;
        }; 
       
    }

    void instantFadeReverse()
    {
        for (int i = 0; i < objectsToFadeOut.Count; i++)
        {
            Color objectColorOut = objectsToFadeOut[i].color;
            float fadeOutAmount = objectsToFadeOut[i].color.a;

           
              fadeOutAmount = 1f;
        
            
            objectColorOut = new Color(objectColorOut.r,objectColorOut.g,objectColorOut.b,fadeOutAmount);
            objectsToFadeOut[i].color = objectColorOut;

        };
         for (int i = 0; i < objectsToFadeIn.Count; i++)
        {
            Color objectColorIn = objectsToFadeIn[i].color;
            float fadeInAmount = objectsToFadeIn[i].color.a;
          
        
              fadeInAmount = 0;
            

            objectColorIn = new Color(objectColorIn.r,objectColorIn.g,objectColorIn.b,fadeInAmount);
            objectsToFadeIn[i].color = objectColorIn;
        }; 
    }
}
