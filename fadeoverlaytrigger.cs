using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadeoverlaytrigger : MonoBehaviour

{
  public GameObject obj;
  public float fadeSpeed;
    private bool fadeOut,fadeIn;



  public void Update()
   {

     if(fadeOut)
     {
       Color objectColor = obj.GetComponent<SpriteRenderer>().color;
       float fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

       objectColor = new Color(objectColor.r,objectColor.g,objectColor.b,fadeAmount);
       obj.GetComponent<SpriteRenderer>().color = objectColor;

       if(objectColor.a == 0)
       {
         fadeOut = false;
       }
     }

     if(fadeIn)
     {      
       Color objectColor = obj.GetComponent<SpriteRenderer>().color;
       float fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

       objectColor = new Color(objectColor.r,objectColor.g,objectColor.b,fadeAmount);
       obj.GetComponent<SpriteRenderer>().color = objectColor;

       if(objectColor.a >= 1)
       {
         fadeIn = false;
       }
     }
   }

  public void FadeOutObject()
   {
    fadeOut = true;
   }
  public void FadeInObject()
   {
     fadeIn = true;
   }
    // Update is called once per frame



    void OnTriggerEnter2D(Collider2D collision)
  {
    if(collision.gameObject.tag =="Player")
    {
      FadeOutObject();
    }

  }
  void OnTriggerExit2D(Collider2D collision)
  
  {
    if(collision.gameObject.tag =="Player")
   { 
    FadeInObject();
   }
  }
 }
