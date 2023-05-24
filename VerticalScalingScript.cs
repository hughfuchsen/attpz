using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalScalingScript : MonoBehaviour
{

    public List<GameObject> slideDown = new List<GameObject>();
    public List<GameObject> slideUp = new List<GameObject>();

    public List<GameObject> instantDown = new List<GameObject>();
    public List<GameObject> instantUp = new List<GameObject>();

    public List<SpriteRenderer> objectsToFadeOut = new List<SpriteRenderer>();
    public List<SpriteRenderer> objectsToFadeIn = new List<SpriteRenderer>();
    public float fadeSpeed = 1;
    public List<GameObject> objectsToActivate = new List<GameObject>();
    public List<GameObject> objectsToDeActivate = new List<GameObject>();
    private List<Vector3> objectsBelowLoweredPositions = new List<Vector3>();
    private List<Vector3> objectsSameOrAboveLoweredPositions = new List<Vector3>();
    private List<Vector3> objectsInstantlyBelowLoweredPositions = new List<Vector3>();
    private List<Vector3> objectsInstantlySameOrAboveLoweredPositions = new List<Vector3>();

   public Vector3 slideOffset = new Vector3(0, -30, 0);
   public Vector3 instantOffset = new Vector3(0, -30, 0);

    public float slideSpeed=300;

    
    // Start is called before the first frame update
    void Start()
    {
       for (int i = 0; i < slideDown.Count; i++)
        {
            objectsBelowLoweredPositions.Insert(i, slideDown[i].transform.position + slideOffset);
        };
      for (int i = 0; i < slideUp.Count; i++)
        {
            objectsSameOrAboveLoweredPositions.Insert(i, slideUp[i].transform.position + slideOffset);
        };
        for (int i = 0; i < instantDown.Count; i++)
        {
            objectsInstantlyBelowLoweredPositions.Insert(i, instantDown[i].transform.position + instantOffset);
        };
        for (int i = 0; i < instantUp.Count; i++)
        {
            objectsInstantlySameOrAboveLoweredPositions.Insert(i, instantUp[i].transform.position + instantOffset);
        };
        //targetPosition1 = myObjects[0].transform.position + vectorThrees[0];
        //targetPosition2 = myObjects[0].transform.position + vectorThrees[0];

    }

    void Awake()
    {
        //foreach(GameObject objectsToActivate in objectsToActivate)
        //objectsToActivate.SetActive(false);
    }
    

    // Update is called once per frame
    void OnTriggerStay2D(Collider2D collision)    
    {
            if(collision.gameObject.tag =="Player")
            {
                Move();
            }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
      if(collision.gameObject.tag =="Player")
            {
              instantMove();
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
            
            }

    }
   
    void Move()
    {
      for (int i = 0; i < slideDown.Count; i++)
        {
            slideDown[i].transform.position = Vector3.MoveTowards(slideDown[i].transform.position, objectsBelowLoweredPositions[i], slideSpeed * Time.deltaTime);
        };
      for (int i = 0; i < slideUp.Count; i++)
        {
            slideUp[i].transform.position = Vector3.MoveTowards(slideUp[i].transform.position, objectsSameOrAboveLoweredPositions[i] - slideOffset, slideSpeed * Time.deltaTime);
        };
        for (int i = 0; i < objectsToFadeOut.Count; i++)
        {
            Color objectColor = objectsToFadeOut[i].color;
            float fadeAmount = objectsToFadeOut[i].color.a - (fadeSpeed * Time.deltaTime);

            if(fadeAmount<0.35f)
            {
              fadeAmount = 0.35f;
            }
            
            objectColor = new Color(objectColor.r,objectColor.g,objectColor.b,fadeAmount);
            objectsToFadeOut[i].color = objectColor;

        };
        for (int i = 0; i < objectsToFadeIn.Count; i++)
        {
            Color objectColor = objectsToFadeIn[i].color;
            float fadeAmount = objectsToFadeIn[i].color.a + (fadeSpeed * Time.deltaTime);
          
            if(fadeAmount>1)
            {
              fadeAmount = 1;
            }

            objectColor = new Color(objectColor.r,objectColor.g,objectColor.b,fadeAmount);
            objectsToFadeIn[i].color = objectColor;
        }; 
    }

        void instantMove()
    {
      for (int i = 0; i < instantDown.Count; i++)
        {
            instantDown[i].transform.position = objectsInstantlyBelowLoweredPositions[i];
        };
      for (int i = 0; i < instantUp.Count; i++)
        {
            instantUp[i].transform.position = objectsInstantlySameOrAboveLoweredPositions[i] - instantOffset;
        };
     
    }

}
