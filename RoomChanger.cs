using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomChanger : MonoBehaviour
{


    public List<Vector3> vectorThrees = new List<Vector3>();

    public List<GameObject> roomsBelow = new List<GameObject>();
    public List<GameObject> roomsSameOrAbove = new List<GameObject>();

    public List<SpriteRenderer> objectsToFadeOut = new List<SpriteRenderer>();
    public List<SpriteRenderer> objectsToFadeIn = new List<SpriteRenderer>();
    public float fadeSpeed = 1;
    public List<GameObject> objectsToActivate = new List<GameObject>();
    public List<GameObject> objectsToDeActivate = new List<GameObject>();
    private List<Vector3> roomsBelowLoweredPositions = new List<Vector3>();
    private List<Vector3> roomsSameOrAboveLoweredPositions = new List<Vector3>();



   
   private Vector3 startPosition;

   public Vector3 offset = new Vector3(0, -30, 0);





    public float speed=300;

    
    // Start is called before the first frame update
    void Start()
    {
       for (int i = 0; i < roomsBelow.Count; i++)
        {
            roomsBelowLoweredPositions.Insert(i, roomsBelow[i].transform.position + offset);
        };
      for (int i = 0; i < roomsSameOrAbove.Count; i++)
        {
            roomsSameOrAboveLoweredPositions.Insert(i, roomsSameOrAbove[i].transform.position + offset);
        };
      //  targetPosition1 = myObjects[0].transform.position + vectorThrees[0];
        //targetPosition2 = myObjects[0].transform.position + vectorThrees[0];

    }

    void Awake()
    {
        foreach(GameObject objectsToActivate in objectsToActivate)
        objectsToActivate.SetActive(false);
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
      for (int i = 0; i < roomsBelow.Count; i++)
        {
            roomsBelow[i].transform.position = Vector3.MoveTowards(roomsBelow[i].transform.position, roomsBelowLoweredPositions[i], speed * Time.deltaTime);
        };
      for (int i = 0; i < roomsSameOrAbove.Count; i++)
        {
            roomsSameOrAbove[i].transform.position = Vector3.MoveTowards(roomsSameOrAbove[i].transform.position, roomsSameOrAboveLoweredPositions[i] - offset, speed * Time.deltaTime);
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

}
