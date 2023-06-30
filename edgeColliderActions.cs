using System.Diagnostics;
// using System.Diagnostics;
using Random=UnityEngine.Random;
using Debug=UnityEngine.Debug;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DisplacedObject
{
    public GameObject gameObject;
    public Vector3 displacement;
}

// [Serializable]
// public class FadeObject
// {
//     public GameObject gameObject;
//     public Vector3 displacement;
// }




// public class sweetRosieJones : Editor
// {
//     public override void OnInspectorGUI()
//     {
//         base.OnInspectorGUI();

//         sweetRosieJones myScript = (sweetRosieJones)target;

//         // Create a dropdown menu for the selected option
//         myScript.selectedOption = (sweetRosieJones.Option)EditorGUILayout.Popup(
//             "Selected Option",
//             (int)myScript.selectedOption,
//             System.Enum.GetNames(typeof(sweetRosieJones.Option))
//         );
//     }
// }


public class edgeColliderActions : MonoBehaviour
{
   // PlayerMovement playerMovement;
   // [SerializeField] GameObject Player;

//  DISPLACE ON EXIT CROSSING UP
    public List<DisplacedObject> displaceOnExitCrossingUp = new List<DisplacedObject>();
    private List<Vector3> displaceOnExitCrossingUpInitialPosition = new List<Vector3>();  

//  REINSTATE ON EXIT CROSSING UP
    public List<DisplacedObject> reinstateOnExitCrossingUp = new List<DisplacedObject>();
    private List<Vector3> reinstateOnExitCrossingUpInitialPosition = new List<Vector3>();

//  DISPLACE ON EXIT CROSSING DOWN
    public List<DisplacedObject> displaceOnExitCrossingDown = new List<DisplacedObject>();
    private List<Vector3> displaceOnExitCrossingDownInitialPosition = new List<Vector3>(); 

//  REINSTATE ON EXIT CROSSING DOWN    
    public List<DisplacedObject> reinstateOnExitCrossingDown = new List<DisplacedObject>();
    private List<Vector3> reinstateOnExitCrossingDownInitialPosition = new List<Vector3>();  

//  DISPLACE ON ENTER CROSSING UP
    public List<DisplacedObject> displaceOnEnterCrossingUp = new List<DisplacedObject>();
    private List<Vector3> displaceOnEnterCrossingUpInitialPosition = new List<Vector3>();

// REINSTATE ON ENTER CROSSING UP
    public List<DisplacedObject> reinstateOnEnterCrossingUp = new List<DisplacedObject>(); 
    private List<Vector3> reinstateOnEnterCrossingUpInitialPosition = new List<Vector3>();

//  DISPLACE ON ENTER CROSSING Down
    public List<DisplacedObject> displaceOnEnterCrossingDown = new List<DisplacedObject>();
    private List<Vector3> displaceOnEnterCrossingDownInitialPosition = new List<Vector3>();

// REINSTATE ON ENTER CROSSING DOWN
    public List<DisplacedObject> reinstateOnEnterCrossingDown = new List<DisplacedObject>(); 
    private List<Vector3> reinstateOnEnterCrossingDownInitialPosition = new List<Vector3>();

//  DISPLACE ON EXIT
    public List<DisplacedObject> displaceOnExit = new List<DisplacedObject>();
    private List<Vector3> displaceOnExitInitialPosition = new List<Vector3>(); 

//  REINSTATE ON EXIT    
    public List<DisplacedObject> reinstateOnExit = new List<DisplacedObject>();
    private List<Vector3> reinstateOnExitInitialPosition = new List<Vector3>();

//  DISPLACE ON ENTER        
    public List<DisplacedObject> displaceOnEnter = new List<DisplacedObject>();
    private List<Vector3> displaceOnEnterInitialPosition = new List<Vector3>(); 

//  REINSTATE ON ENTER    
    public List<DisplacedObject> reinstateOnEnter = new List<DisplacedObject>();
    private List<Vector3> reinstateOnEnterInitialPosition = new List<Vector3>(); 


    public float displaceSpeed = 100;

    private bool displaceExit, reinstateExit, 
                 displaceEnter, reinstateEnter;



    
    public List<GameObject> fadeOutOnExitCrossingDown = new List<GameObject>();
    public List<GameObject> fadeInOnExitCrossingDown = new List<GameObject>();
    public List<GameObject> fadeOutOnExitCrossingUp = new List<GameObject>();
    public List<GameObject> fadeInOnExitCrossingUp = new List<GameObject>();
    public List<GameObject> fadeOutOnEnterCrossingDown = new List<GameObject>();
    public List<GameObject> fadeInOnEnterCrossingDown = new List<GameObject>();
    public List<GameObject> fadeOutOnEnterCrossingUp = new List<GameObject>();
    public List<GameObject> fadeInOnEnterCrossingUp = new List<GameObject>();    
    public List<GameObject> spritesToFadeOut = new List<GameObject>();
    public List<GameObject> spritesToFadeIn = new List<GameObject>();
    public List<GameObject> transpFdOutOnExtCrsngDown = new List<GameObject>(); 
    public List<GameObject> transpFdOutOnExtCrsngUp = new List<GameObject>();
    public List<GameObject> transpFdOutOnEntCrsngDown = new List<GameObject>();
    public List<GameObject> transpFdOutOnEntCrsngUp = new List<GameObject>(); 
  
 
    public List<GameObject> spritesToDisappearCrossingDown = new List<GameObject>();
    public List<GameObject> spritesToAppearCrossingDown = new List<GameObject>();
    public List<GameObject> spritesToDisappearCrossingUp = new List<GameObject>();
    public List<GameObject> spritesToAppearCrossingUp = new List<GameObject>();
    public List<SpriteRenderer> spritesToDisappear = new List<SpriteRenderer>();
    public List<SpriteRenderer> spritesToAppear = new List<SpriteRenderer>();

    // public List<SpriteRenderer> closedDoor = new List<SpriteRenderer>();
    // public List<SpriteRenderer> openedDoor = new List<SpriteRenderer>();

    private bool onCollider, belowCollider, aboveCollider;

    PlayerMovement playerMovement;
    [SerializeField] GameObject Player;
    public string sortingLayerToAssign = "Default";

    public float fadeSpeed = 10f; // Time it takes to fade out the sprites

    public float waitBetween = 0.01f; // Time it takes between treFadeSequence(fade obj's)


    public GameObject multiFdObjInside;
    public GameObject multiFdObjOutside;
    // private GameObject everything = GameObject.FindWithTag("Everything");
 

    [SerializeField] bool itsAnEntrnceOrExt;
    [SerializeField] bool itsStairs;
    public GameObject building;

    private Coroutine fadeCoroutine; // Coroutine handle for stopping previous fade actions

    void Awake()
    { 
       Player = GameObject.FindGameObjectWithTag("Player");
       playerMovement = Player.GetComponent<PlayerMovement>();  
    }    

    private bool isPlayerCrossingUp()
    {
        return GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().change.y > 0;
    }

    void Start()
    {   
      if (multiFdObjInside != null && multiFdObjOutside != null) {
        if (GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside) {
          fadeCoroutine = StartCoroutine(treeFadeSequence(waitBetween, multiFdObjInside, 1f, 0f, multiFdObjOutside, 0f, 1f, fadeSpeed));  
        } else if (!GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside) {
          fadeCoroutine = StartCoroutine(treeFadeSequence(waitBetween, multiFdObjOutside, 0f, 1f, multiFdObjInside, 1f, 0f, fadeSpeed));
        }
      }



        //DISPLACEMENT/REINSTATEMENT START

      for (int i = 0; i < displaceOnExitCrossingUp.Count; i++)
        {
          displaceOnExitCrossingUpInitialPosition.Add(displaceOnExitCrossingUp[i].gameObject.transform.position);
        };

      for (int i = 0; i < reinstateOnExitCrossingUp.Count; i++){
          reinstateOnExitCrossingUpInitialPosition.Add(reinstateOnExitCrossingUp[i].gameObject.transform.position);
        };

      for (int i = 0; i < displaceOnExitCrossingDown.Count; i++)
        {
          displaceOnExitCrossingDownInitialPosition.Add(displaceOnExitCrossingDown[i].gameObject.transform.position);
        };  

      for (int i = 0; i < reinstateOnExitCrossingDown.Count; i++)
        {
          reinstateOnExitCrossingDownInitialPosition.Add(reinstateOnExitCrossingDown[i].gameObject.transform.position);
        };                      
        
      for (int i = 0; i < displaceOnEnterCrossingUp.Count; i++)
        {
          displaceOnEnterCrossingUpInitialPosition.Add(displaceOnEnterCrossingUp[i].gameObject.transform.position);
        };

      for (int i = 0; i < reinstateOnEnterCrossingUp.Count; i++)
        {
          reinstateOnEnterCrossingUpInitialPosition.Add(reinstateOnEnterCrossingUp[i].gameObject.transform.position);
        };

      for (int i = 0; i < displaceOnEnterCrossingDown.Count; i++)
        {
          displaceOnEnterCrossingDownInitialPosition.Add(displaceOnEnterCrossingDown[i].gameObject.transform.position);
        };

      for (int i = 0; i < reinstateOnEnterCrossingDown.Count; i++)
        {
          reinstateOnEnterCrossingDownInitialPosition.Add(reinstateOnEnterCrossingDown[i].gameObject.transform.position);
        };

      for (int i = 0; i < displaceOnExit.Count; i++)
        {
          displaceOnExitInitialPosition.Add(displaceOnExit[i].gameObject.transform.position);
        };

      for (int i = 0; i < reinstateOnExit.Count; i++)
        {
          reinstateOnExitInitialPosition.Add(reinstateOnExit[i].gameObject.transform.position);
        };

      for (int i = 0; i < displaceOnEnter.Count; i++)
        {
          displaceOnEnterInitialPosition.Add(displaceOnEnter[i].gameObject.transform.position);
        };

      for (int i = 0; i < reinstateOnEnter.Count; i++)
        {
          reinstateOnEnterInitialPosition.Add(reinstateOnEnter[i].gameObject.transform.position);
        };
    }

    void spriteSwitch()
    {
      for (int i = 0; i < spritesToDisappear.Count; i++)
        {
          Color objectColorIn = spritesToDisappear[i].color;
          float fadeOutAmount = spritesToDisappear[i].color.a;
        
      
          fadeOutAmount = 0;
          

          objectColorIn = new Color(objectColorIn.r,objectColorIn.g,objectColorIn.b,fadeOutAmount);
          spritesToDisappear[i].color = objectColorIn;        
        };

      for (int i = 0; i < spritesToAppear.Count; i++)
        {
          Color objectColorIn = spritesToAppear[i].color;
          float fadeInAmount = spritesToAppear[i].color.a;
        
      
          fadeInAmount = 1;
          

          objectColorIn = new Color(objectColorIn.r,objectColorIn.g,objectColorIn.b,fadeInAmount);
          spritesToAppear[i].color = objectColorIn;        
          }; 
    }

    void spriteSwitchReverse()
    {
        
      for (int i = 0; i < spritesToDisappear.Count; i++)
        {
          Color objectColorIn = spritesToDisappear[i].color;
          float fadeInAmount = spritesToDisappear[i].color.a;
        
      
            fadeInAmount = 1;
          

          objectColorIn = new Color(objectColorIn.r,objectColorIn.g,objectColorIn.b,fadeInAmount);
          spritesToDisappear[i].color = objectColorIn;        
        };

      for (int i = 0; i < spritesToAppear.Count; i++)
        {
          Color objectColorIn = spritesToAppear[i].color;
          float fadeOutAmount = spritesToAppear[i].color.a;
        
      
            fadeOutAmount = 0;
          

          objectColorIn = new Color(objectColorIn.r,objectColorIn.g,objectColorIn.b,fadeOutAmount);
          spritesToAppear[i].color = objectColorIn;
        }; 
    }
    // void doorSwitch()
    // {
    //     for (int i = 0; i < closedDoor.Count; i++)
    //     {
    //         Color objectColorOut = closedDoor[i].color;
    //         float fadeOutAmount = closedDoor[i].color.a;

           
    //           fadeOutAmount = 0f;
            
            
    //         objectColorOut = new Color(objectColorOut.r,objectColorOut.g,objectColorOut.b,fadeOutAmount);
    //         closedDoor[i].color = objectColorOut;

    //     };
    //       for (int i = 0; i < openedDoor.Count; i++)
    //     {
    //         Color objectColorIn = openedDoor[i].color;
    //         float fadeInAmount = openedDoor[i].color.a;
          
          
    //           fadeInAmount = 1;
            

    //         objectColorIn = new Color(objectColorIn.r,objectColorIn.g,objectColorIn.b,fadeInAmount);
    //         openedDoor[i].color = objectColorIn;
    //     }; 
       
    // }

    // void doorSwitchReverse(float value1, float value2)
    // {
        
    //     for (int i = 0; i < closedDoor.Count; i++)
    //     {

    //         Color objectColorOut = closedDoor[i].color;
    //         float fadeOutAmount = closedDoor[i].color.a;

    //           fadeOutAmount = value1;

            
    //         objectColorOut = new Color(objectColorOut.r,objectColorOut.g,objectColorOut.b,fadeOutAmount);
    //         closedDoor[i].color = objectColorOut;
    //     };

    //      for (int i = 0; i < openedDoor.Count; i++)
    //     {
    //         Color objectColorIn = openedDoor[i].color;
    //         float fadeInAmount = openedDoor[i].color.a;
          
        
    //           fadeInAmount = value2;
            

    //         objectColorIn = new Color(objectColorIn.r,objectColorIn.g,objectColorIn.b,fadeInAmount);
    //         openedDoor[i].color = objectColorIn;
    //     }; 
    // }
    // void doorSwitcher(List<SpriteRenderer> obj, float value1)
    // {
    //     for (int i = 0; i < obj.Count; i++)
    //     {
    //         Color objectColor = obj[i].color;
    //         float fadeOutAmount = obj[i].color.a;

    //         fadeOutAmount = value1;

            
    //         objectColor = new Color(objectColor.r,objectColor.g,objectColor.b,fadeOutAmount);
    //         obj[i].color = objectColor;
    //     };
    // }
    // void doorSwitchReverse(float value1, float value2)
    // {
        
    //     for (int i = 0; i < closedDoor.Count; i++)
    //     {

    //         Color objectColorOut = closedDoor[i].color;
    //         float fadeOutAmount = closedDoor[i].color.a;

    //           fadeOutAmount = value1;

            
    //         objectColorOut = new Color(objectColorOut.r,objectColorOut.g,objectColorOut.b,fadeOutAmount);
    //         closedDoor[i].color = objectColorOut;
    //     };

    //      for (int i = 0; i < openedDoor.Count; i++)
    //     {
    //         Color objectColorIn = openedDoor[i].color;
    //         float fadeInAmount = openedDoor[i].color.a;
          
        
    //           fadeInAmount = value2;
            

    //         objectColorIn = new Color(objectColorIn.r,objectColorIn.g,objectColorIn.b,fadeInAmount);
    //         openedDoor[i].color = objectColorIn;
    //     }; 
    // }

    // IEnumerator closeTheDoor()
    // {
    //     // int wait_time = Random.Range (2, 5);
    //     yield return new WaitForSeconds(Random.Range(1.0f, 2.9f));
    //     if(onCollider){
    //     fadeCoroutine = StartCoroutine(closeTheDoor());
    //     } else if (!aboveCollider) {
    //     doorSwitcher(openedDoor, 0f);
    //     doorSwitcher(closedDoor, 1f);
    //     } else if (aboveCollider) {
    //     doorSwitcher(openedDoor, 0f); 
    //     doorSwitcher(closedDoor, 0.35f); 
    //     }
    // }


// fadeOutCrossingUp, fadeInCrossingUp, fadeOutCrossingDown, fadeInCrossingDown;


    void OnTriggerExit2D(Collider2D collision)
  {
    if(collision.gameObject.tag =="Player")
    {
      if (fadeCoroutine != null) {
        StopCoroutine(fadeCoroutine);
      }

      displaceExit = true;
      reinstateExit = true;
      displaceEnter = false;
      reinstateEnter = false;

      onCollider = false;

        if (isPlayerCrossingUp())
        {

          for (int i = 0; i < displaceOnExitCrossingUp.Count; i++){
            Vector3 targetPosition = displaceOnExitCrossingUpInitialPosition[i] + displaceOnExitCrossingUp[i].displacement;
            // TODO keep coroutines in a list so they can all be stopped
            StartCoroutine(displace(displaceOnExitCrossingUp[i].gameObject, targetPosition));
          }
          for (int i = 0; i < reinstateOnExitCrossingUp.Count; i++){
            Vector3 targetPosition = reinstateOnExitCrossingUpInitialPosition[i];
            StartCoroutine(displace(reinstateOnExitCrossingUp[i].gameObject, targetPosition));
          }

          for (int i = 0; i < fadeOutOnExitCrossingUp.Count; i++){
            fadeCoroutine = StartCoroutine(treeFade(fadeOutOnExitCrossingUp[i], 1f, 0f, fadeSpeed));      
          }
          for (int i = 0; i < fadeInOnExitCrossingUp.Count; i++){
            fadeCoroutine = StartCoroutine(treeFade(fadeInOnExitCrossingUp[i], 0f, 1f, fadeSpeed));      
          } 
          for (int i = 0; i < transpFdOutOnExtCrsngUp.Count; i++){
            fadeCoroutine = StartCoroutine(treeFade(transpFdOutOnExtCrsngUp[i], 1f, 0.35f, fadeSpeed));      
          }

          //ON EXIT CROSSING UP
          if(itsAnEntrnceOrExt) {
            if (fadeCoroutine != null) {
              StopCoroutine(fadeCoroutine);
            }
            if (GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside && !aboveCollider) {

              // multiFdObjInside.tag = "excludeChild";

              fadeCoroutine = StartCoroutine(treeFadeSequence(waitBetween, multiFdObjInside, 1f, 0f, multiFdObjOutside, 0f, 0.35f, fadeSpeed));

              multiFdObjInside.tag = "Untagged";

              GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside = false;



            } else if (!aboveCollider) {

              multiFdObjInside.tag = "excludeChild";

              fadeCoroutine = StartCoroutine(treeFadeSequence(waitBetween, multiFdObjOutside, 1f, 0f, multiFdObjInside, 0f, 1f, fadeSpeed));

              GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside = true;
            } else if (aboveCollider) {
              multiFdObjInside.tag = "Untagged";
            }


          }

          aboveCollider = true;
          Debug.Log(aboveCollider);

          if(itsStairs) {
            playerMovement.motionDirection = "inclineRightAway";  
          }

        }
        // onTriggerExit crossing down
        else
        {
          for (int i = 0; i < displaceOnExitCrossingDown.Count; i++){
            Vector3 targetPosition = displaceOnExitCrossingDownInitialPosition[i] + displaceOnExitCrossingDown[i].displacement;
            // TODO keep coroutines in a list so they can all be stopped
            StartCoroutine(displace(displaceOnExitCrossingDown[i].gameObject, targetPosition));
          }
          for (int i = 0; i < reinstateOnExitCrossingDown.Count; i++){
            Vector3 targetPosition = reinstateOnExitCrossingDownInitialPosition[i];
            StartCoroutine(displace(reinstateOnExitCrossingDown[i].gameObject, targetPosition));
          }

          for (int i = 0; i < fadeOutOnExitCrossingDown.Count; i++){
            fadeCoroutine = StartCoroutine(treeFade(fadeOutOnExitCrossingDown[i], 1f, 0f, fadeSpeed));      
          } 
          for (int i = 0; i < fadeInOnExitCrossingDown.Count; i++){
            fadeCoroutine = StartCoroutine(treeFade(fadeInOnExitCrossingDown[i], 0f, 1f, fadeSpeed));      
          }
          for (int i = 0; i < transpFdOutOnExtCrsngDown.Count; i++){
            fadeCoroutine = StartCoroutine(treeFade(transpFdOutOnExtCrsngDown[i], 1f, 0.35f, fadeSpeed));
          }




          // CROSSING DOWN
          if(itsAnEntrnceOrExt) {
            if (fadeCoroutine != null) {
              StopCoroutine(fadeCoroutine);
            }
            if (GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside && aboveCollider) {

              // multiFdObjInside.tag = "excludeChild";

              fadeCoroutine = StartCoroutine(treeFadeSequence(waitBetween, multiFdObjInside, 1f, 0f, multiFdObjOutside, 0f, 1f, fadeSpeed));

              multiFdObjInside.tag = "Untagged";

              GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside = false;

              

            } else if(aboveCollider) {

              multiFdObjInside.tag = "excludeChild";


              fadeCoroutine = StartCoroutine(treeFadeSequence(waitBetween, multiFdObjOutside, 0.35f, 0f, multiFdObjInside, 0f, 1f, fadeSpeed));

              GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside = true;  
              

            } else if(!aboveCollider) {
              multiFdObjInside.tag = "Untagged";
            }

          }

      


          aboveCollider = false;


                      
            
            // foreach(GameObject displace in displaceOnExitCrossingDown)
            // {
            //     GameObjectRaiseLower gameObjectraiseLowerInstance = displace.GetComponent<GameObjectRaiseLower>();
            //     if (gameObjectraiseLowerInstance != null)
            //     {
            //         gameObjectraiseLowerInstance.lower();
            //     }
                
            // }
            // foreach(GameObject reinstate in reinstateOnExitCrossingDown)
            // {
            //     GameObjectRaiseLower gameObjectraiseLowerInstance = reinstate.GetComponent<GameObjectRaiseLower>();
            //     if (gameObjectraiseLowerInstance != null)
            //     {
            //         gameObjectraiseLowerInstance.raise();
            //     }
                
            // } 
        }
      
      // StartCoroutine(closeTheDoor());

    }
  }
    void OnTriggerStay2D(Collider2D collision)
  {
    if(collision.gameObject.tag =="Player")
    {
      // doorSwitch();
    }
  }
    void OnTriggerEnter2D(Collider2D collision)
  {
    if(collision.gameObject.tag =="Player"){

      if(itsAnEntrnceOrExt) 
      {
        if (multiFdObjInside != null) 
        {
          StartCoroutine(treeFade(multiFdObjInside, 0f, 1f, fadeSpeed)); 
        }      
      }

      if (fadeCoroutine != null) {
        StopCoroutine(fadeCoroutine);
      }

      displaceEnter = true;
      reinstateEnter = true;
      displaceExit = false;
      reinstateExit = false;

      for (int i = 0; i < spritesToFadeOut.Count; i++){
        fadeCoroutine = StartCoroutine(treeFade(spritesToFadeOut[i], 1f, 0f, fadeSpeed));      
      } 
      for (int i = 0; i < spritesToFadeIn.Count; i++){
        fadeCoroutine = StartCoroutine(treeFade(spritesToFadeIn[i], 0f, 1f, fadeSpeed));      
      }

      // doorSwitcher(openedDoor, 1f);
      // doorSwitcher(closedDoor, 0f);

      // StartCoroutine(multiActivate(multiDeActivate));

      edgeColliderActions.SetSortingLayer(collision.gameObject, sortingLayerToAssign);

      if (isPlayerCrossingUp())
      {
        for (int i = 0; i < displaceOnEnterCrossingUp.Count; i++){
          Vector3 targetPosition = displaceOnEnterCrossingUpInitialPosition[i] + displaceOnEnterCrossingUp[i].displacement;
          // TODO keep coroutines in a list so they can all be stopped
          StartCoroutine(displace(displaceOnEnterCrossingUp[i].gameObject, targetPosition));
        }
        for (int i = 0; i < reinstateOnEnterCrossingUp.Count; i++){
          Vector3 targetPosition = reinstateOnEnterCrossingUpInitialPosition[i];
          StartCoroutine(displace(reinstateOnEnterCrossingUp[i].gameObject, targetPosition));
        }
        
        for (int i = 0; i < fadeOutOnEnterCrossingUp.Count; i++){
          fadeCoroutine = StartCoroutine(treeFade(fadeOutOnEnterCrossingUp[i], 1f, 0f, fadeSpeed));      
        } 
        for (int i = 0; i < fadeInOnEnterCrossingUp.Count; i++){
          fadeCoroutine = StartCoroutine(treeFade(fadeInOnEnterCrossingUp[i], 0f, 1f, fadeSpeed));      
        }
        for (int i = 0; i < transpFdOutOnEntCrsngUp.Count; i++){
          fadeCoroutine = StartCoroutine(treeFade(transpFdOutOnEntCrsngUp[i], 1f, 0.35f, fadeSpeed));      
        }


        // if(itsAnEntrnceOrExt) {
   
        //   if (GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside && !aboveCollider) {

        //     multiFdObjInside.tag = "excludeChild";
            

        //   } else if(!aboveCollider) {
        //     multiFdObjInside.tag = "excludeChild";
        //   }

        // }

        aboveCollider = false;

      }
      // onTriggerEnter crossing down
      else
      {
        for (int i = 0; i < displaceOnEnterCrossingDown.Count; i++){
          Vector3 targetPosition = displaceOnEnterCrossingDownInitialPosition[i] + displaceOnEnterCrossingDown[i].displacement;
          // TODO keep coroutines in a list so they can all be stopped
          StartCoroutine(displace(displaceOnEnterCrossingDown[i].gameObject, targetPosition));
        }
        for (int i = 0; i < reinstateOnEnterCrossingDown.Count; i++){
          Vector3 targetPosition = reinstateOnEnterCrossingDownInitialPosition[i];
          StartCoroutine(displace(reinstateOnEnterCrossingDown[i].gameObject, targetPosition));
        }

        for (int i = 0; i < fadeOutOnEnterCrossingDown.Count; i++){
          fadeCoroutine = StartCoroutine(treeFade(fadeOutOnEnterCrossingDown[i], 1f, 0f, fadeSpeed));
        } 
        for (int i = 0; i < fadeInOnEnterCrossingDown.Count; i++){
          fadeCoroutine = StartCoroutine(treeFade(fadeInOnEnterCrossingDown[i], 0f, 1f, fadeSpeed));
        }   
        for (int i = 0; i < transpFdOutOnEntCrsngDown.Count; i++){
          fadeCoroutine = StartCoroutine(treeFade(transpFdOutOnEntCrsngDown[i], 1f, 0.35f, fadeSpeed));
        }





        // if(itsAnEntrnceOrExt) {
   
        //   if (GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside && aboveCollider) {

        //     multiFdObjInside.tag = "excludeChild";
            

        //   } else if(aboveCollider) {
        //     multiFdObjInside.tag = "excludeChild";
        //   }

        // }

        aboveCollider = true;   
      
      }
    }
  }

   static void SetSortingLayer(GameObject gameObject, string sortingLayerName)
    {
        if(gameObject.GetComponent<SpriteRenderer>() != null) {
          gameObject.GetComponent<SpriteRenderer>().sortingLayerName = sortingLayerName;
        }
        foreach (Transform child in gameObject.transform)
        {
            edgeColliderActions.SetSortingLayer(child.gameObject, sortingLayerName);
        }
    }  


// static void multiFadeTestOne(GameObject gameObject)
//     {
//         if(thingAndItsChildrenToAlter.GetComponent<SpriteRenderer>() != null) {
//           Color objectColor = thingAndItsChildrenToAlter.GetComponent<SpriteRenderer>.color;
//           float fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

//           objectColor = new Color(objectColor.r,objectColor.g,objectColor.b,fadeAmount);
//           thingAndItsChildrenToAlter.GetComponent<SpriteRenderer>.color = objectColor;

//           if(objectColor.a <= 0)
//             {
//             objectColor.a = 0;
//             fadeOut = false;
//             }
//             else {
//             fadeOut = true;
//             }        
//         }
//         foreach (Transform child in gameObject.transform)
//         {
//             edgeColliderActions.multiFadeTestOne(gameObject);
//         }
//     }  


    // IEnumerator multiFadeOutside(GameObject obj, float fadeFrom, float fadeTo) {
    //     // Fade out the sprite renderer on this object
    //     SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
    //     if (sr != null) {
    //         for (float t = 0.0f; t < fadeSpeed; t += Time.deltaTime) {
    //             sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.Lerp(fadeFrom, fadeTo, t / fadeSpeed));
    //             yield return null;
    //         }          
    //         sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, fadeTo);
    //         yield return null;
    //     }
        
    //     // Fade out the sprite renderers on all child objects
    //     foreach (Transform child in obj.transform) {
    //         StartCoroutine(multiFadeOutside(child.gameObject, fadeFrom, fadeTo));
    //     }
    // }
    IEnumerator multiFade(GameObject obj, float fadeFrom, float fadeTo) {

        foreach (Transform child in obj.transform) {
            StartCoroutine(multiFade(child.gameObject, fadeFrom, fadeTo));
        }

        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null) {
            for (float t = 0.0f; t < fadeSpeed; t += Time.deltaTime) {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.Lerp(fadeFrom, fadeTo, t / fadeSpeed));
                yield return null;
            }          
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, fadeTo);
            yield return null;
        }
    }
    //     IEnumerator multiActivate(GameObject obj) {

    //     BoxCollider2D bc = obj.GetComponent<BoxCollider2D>();
    //     BoxCollider2D bc2 = obj.GetComponent<BoxCollider2D>();
    //     if (bc != null) {
    //          obj.SetActive(false);
    //          yield return null;
    //     }
    //     // if (bc2 != null) {
    //     //      obj2.SetActive(true);
    //     //      yield return null;
    //     // }
        
    //     // Fade out the sprite renderers on all child objects
    //     foreach (Transform child in obj.transform) {
    //         StartCoroutine(multiActivate(child.gameObject));
    //     }
    // }

    // IEnumerator timedSpriteFadeFromInside(float waitFinal, GameObject objInside, float fadeFromInside, 
    //                   float fadeToInside, GameObject objOutside, float fadeFromOutside, float fadeToOutside)
    // {
    //     StartCoroutine(multiFade(objInside, fadeFromInside, fadeToInside));
    //     yield return new WaitForSeconds(waitFinal);
    //     StartCoroutine(multiFade(objOutside, fadeFromOutside, fadeToOutside));
    // }    
    // IEnumerator timedSpriteFadeFromOutside(float waitFinal, GameObject objInside, float fadeFromInside, 
    //                   float fadeToInside, GameObject objOutside, float fadeFromOutside, float fadeToOutside)
    // {
    //     StartCoroutine(multiFade(objOutside, fadeFromOutside, fadeToOutside));
    //     yield return new WaitForSeconds(waitFinal);
    //     StartCoroutine(multiFade(objInside, fadeFromInside, fadeToInside));
    // }
    IEnumerator timedSpriteFadeSequence(
      float waitBetween,
      GameObject objFirst,
      float fadeFromFirst, 
      float fadeToFirst,
      GameObject objSecond,
      float fadeFromSecond,
      float fadeToSecond) {
        StartCoroutine(multiFade(objFirst, fadeFromFirst, fadeToFirst));
        yield return new WaitForSeconds(waitBetween);
        StartCoroutine(multiFade(objSecond, fadeFromSecond, fadeToSecond));
    }    

    void setTreeAlpha(GameObject treeNode, float alpha) {
        SpriteRenderer sr = treeNode.GetComponent<SpriteRenderer>();
        if (sr != null) {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
        }
        foreach (Transform child in treeNode.transform) {
            setTreeAlpha(child.gameObject, alpha);
        }
    }

    //  && GameObject.FindWithTag("Untagged") == null
    IEnumerator treeFadeSequence(
      float waitBetween,
      GameObject objFirst,
      float fadeFromFirst, 
      float fadeToFirst,
      GameObject objSecond,
      float fadeFromSecond,
      float fadeToSecond,
      float fadeSpeed) {

        for (float t = 0.0f; t < 1; t += Time.deltaTime) {
            float currentAlpha = Mathf.Lerp(fadeFromFirst, fadeToFirst, t * fadeSpeed);
            setTreeAlpha(objFirst, currentAlpha);
            // yield return null;
        }

        yield return new WaitForSeconds(waitBetween);

        for (float t = 0.0f; t < 1; t += Time.deltaTime) {
            float currentAlpha = Mathf.Lerp(fadeFromSecond, fadeToSecond, t * fadeSpeed);
            setTreeAlpha(objSecond, currentAlpha);
            yield return null;
        }
    }

    // StartCoroutine(treeFade(exterior, 0, 1))
    IEnumerator treeFade(
      GameObject obj,
      float fadeFrom, 
      float fadeTo,
      float fadeSpeed) {
        for (float t = 0.0f; t < 1; t += Time.deltaTime) {
            float currentAlpha = Mathf.Lerp(fadeFrom, fadeTo, t * fadeSpeed);
            setTreeAlpha(obj, currentAlpha);
            yield return null;
        }
    }

    IEnumerator displace(GameObject obj, Vector3 objTargetPosition) {
      for (float t = 0.0f; t < 1; t += Time.deltaTime) {
        obj.transform.position = Vector3.MoveTowards(obj.transform.position, objTargetPosition, displaceSpeed * Time.deltaTime);
        yield return null;
      }
    //  reinstateOnExit[i].gameObject.transform.position = 
    //  Vector3.MoveTowards(
    //   reinstateOnExit[i].gameObject.transform.position,
    //   reinstateOnExitInitialPosition[i] + reinstateOnExit[i].displacement,
    //   displaceSpeed * Time.deltaTime);
    }

  // public void AccessParent(Transform currentTransform, string targetObjectName) {
  //   if (currentTransform.parent != null) {
  //     // Access the parent transform and check its name
  //     Transform parentTransform = currentTransform.parent;
  //     string parentName = parentTransform.name;
  //       if (parentName == targetObjectName)
  //         {
  //             // Parent object found, perform operations
  //             // Example: print the parent's name
  //             Debug.Log("Parent object found: " + parentName);
  //             // Do additional operations with the parent object if needed
  //         }
  //         else
  //         {
  //             // Continue the traversal recursively with the parent transform
  //             FindParentByName(parentTransform, targetObjectName);
  //         }
  //   } else {
  //       Debug.Log("Parent object not found!");
  //   }
  // }

}