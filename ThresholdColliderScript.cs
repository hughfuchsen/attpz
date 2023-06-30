using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThresholdColliderScript : MonoBehaviour
{
    public RoomScript roomAbove;
    public RoomScript roomBelow;
    public SpriteRenderer openDoor;
    public SpriteRenderer closedDoor;
    public float currentDoorAlpha = 1;
    PlayerMovement playerMovement;
    [SerializeField] GameObject Player;
    public string motionDirection = "normal";
    public string previousMotionDirection = "normal";
    [SerializeField] bool itsAnEntrnceOrExt;
    public GameObject multiFdObjInside;
    public GameObject multiFdObjOutside;
    private float waitTime = 0.01f; // Time it takes between treFadeSequence(fade obj's)
    private float buildingFadeSpeed = 7f; // Time it takes to fade out the sprites
    private bool aboveCollider;
    private Coroutine fadeCoroutine;
    private Coroutine fadeCoroutine2;




    void Awake()
    { 
        Player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = Player.GetComponent<PlayerMovement>(); 
    }  

    void Start()
    {
        if (openDoor != null)
        {
            openDoor.color = new Color(openDoor.color.r, openDoor.color.g, openDoor.color.b, 0);
        }
        if (closedDoor != null)
        {
            closedDoor.color = new Color(closedDoor.color.r, closedDoor.color.g, closedDoor.color.b, currentDoorAlpha);
        }
        // TODO 
        // if (GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside)
        // {
        //     fadeCoroutine = StartCoroutine(treeFadeSequence(waitTime, multiFdObjInside, 1f, 1f, multiFdObjOutside, 0f, 0f, buildingFadeSpeed));
        // }
        // else
        // {
        //     fadeCoroutine = StartCoroutine(treeFadeSequence(waitTime, multiFdObjOutside, 1f, 1f, multiFdObjInside, 0f, 0f, buildingFadeSpeed));
        // }
    }

    void OnTriggerEnter2D()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        playerMovement.motionDirection = motionDirection;
                
        if (openDoor != null)
        {
            openDoor.color = new Color(openDoor.color.r, openDoor.color.g, openDoor.color.b, currentDoorAlpha);
        }
        if (closedDoor != null)
        {
            closedDoor.color = new Color(closedDoor.color.r, closedDoor.color.g, closedDoor.color.b, 0);
        }

        if (isPlayerCrossingUp())
        {
            aboveCollider = false;
        }    
        else 
        {
            aboveCollider = true;

            if (roomBelow != null)
            {
                roomBelow.EnterRoom();
            }  
        }

        
    }

    void OnTriggerExit2D()
    {
        playerMovement.motionDirection = previousMotionDirection;

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        if (fadeCoroutine2 != null)
        {
            StopCoroutine(fadeCoroutine2);
        }

        if (openDoor != null)
        {
            openDoor.color = new Color(openDoor.color.r, openDoor.color.g, openDoor.color.b, 0);
        }
        if (closedDoor != null)
        {
            closedDoor.color = new Color(closedDoor.color.r, closedDoor.color.g, closedDoor.color.b, currentDoorAlpha);
        }
            //ON EXIT CROSSING UP
        if (isPlayerCrossingUp())
        {
            if (roomBelow != null)
            {
                roomBelow.ExitRoom();
            }
            if (roomAbove != null)
            {
                roomAbove.EnterRoom();
            }

            if (itsAnEntrnceOrExt)
            {
                if (GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside && !aboveCollider)
                {
                    // if the sprites are flickering, use treeFadeSequence to fade out the interior of the house instead of treeFade. 
                    // I would prefer to keep the interoir visible at all times

                    fadeCoroutine = StartCoroutine(treeFadeSequence(waitTime, multiFdObjInside, 1f, 0f, multiFdObjOutside, 0f, 0.35f, buildingFadeSpeed));
                    // fadeCoroutine2 = StartCoroutine(treeFade(multiFdObjOutside, 0f, 0.35f, buildingFadeSpeed));
                    GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside = false;

                    //need to optimise
                    if (roomBelow != null)
                    {
                        roomBelow.ExitRoom();
                    }                
                }
                else if (!aboveCollider)
                {
                    fadeCoroutine = StartCoroutine(treeFadeSequence(waitTime, multiFdObjOutside, 1f, 0f, multiFdObjInside, 0f, 1f, buildingFadeSpeed));
                    // fadeCoroutine2 = StartCoroutine(treeFade(multiFdObjOutside, 1f, 0f, buildingFadeSpeed));
                    GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside = true;
                }
            }

            aboveCollider = true;

        }
            //ON EXIT CROSSING DOWN
        else
        {
            if (roomAbove != null)
            {
                roomAbove.ExitRoom();
            }
            
            if (itsAnEntrnceOrExt)
            {
                if (GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside && aboveCollider)
                {
                    fadeCoroutine = StartCoroutine(treeFadeSequence(waitTime, multiFdObjInside, 1f, 0f, multiFdObjOutside, 0f, 1f, buildingFadeSpeed));
                    // fadeCoroutine2 = StartCoroutine(treeFade(multiFdObjOutside, 0f, 1f, buildingFadeSpeed));
                    GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside = false;
                    
                    //need to optimise
                    if (roomBelow != null)
                    {
                        roomBelow.ExitRoom();
                    }                
                }
                else if (aboveCollider)
                {
                    fadeCoroutine = StartCoroutine(treeFadeSequence(waitTime, multiFdObjOutside, 0.35f, 0f, multiFdObjInside, 0f, 1f, buildingFadeSpeed));
                    // fadeCoroutine2 = StartCoroutine(treeFade(multiFdObjOutside, 0.35f, 0f, buildingFadeSpeed));
                    GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside = true;
                }
            }
            
            aboveCollider = false;

        }
    }
    private bool isPlayerCrossingUp()
    {
        return GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().change.y > 0;
    }
    private bool isPlayerCrossingLeft()
    {
        return GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().change.x < 0;
    }

    private Color SetAlpha(SpriteRenderer sprite, float alpha)
    {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
        return sprite.color;
    }

    IEnumerator treeFadeSequence(
        float waitTime,
        GameObject objFirst,
        float fadeFromFirst, 
        float fadeToFirst,
        GameObject objSecond,
        float fadeFromSecond,
        float fadeToSecond,
        float buildingFadeSpeed) 
        {
            for (float t = 0.0f; t < 1; t += Time.deltaTime) 
            {
                float currentAlpha = Mathf.Lerp(fadeFromFirst, fadeToFirst, t * buildingFadeSpeed);
                setTreeAlpha(objFirst, currentAlpha);
                // yield return null;
            }

            yield return new WaitForSeconds(waitTime);

            for (float t = 0.0f; t < 1; t += Time.deltaTime) 
            {
                float currentAlpha = Mathf.Lerp(fadeFromSecond, fadeToSecond, t * buildingFadeSpeed);
                setTreeAlpha(objSecond, currentAlpha);
                yield return null;
            }
        }


    IEnumerator treeFade(
      GameObject obj,
      float fadeFrom, 
      float fadeTo,
      float buildingFadeSpeed) 
      {
        for (float t = 0.0f; t < 1; t += Time.deltaTime) 
        {
            float currentAlpha = Mathf.Lerp(fadeFrom, fadeTo, t * buildingFadeSpeed);
            setTreeAlpha(obj, currentAlpha);
            yield return null;
        }
    }

        void setTreeAlpha(GameObject treeNode, float alpha) 
        {
            SpriteRenderer sr = treeNode.GetComponent<SpriteRenderer>();
            if (sr != null) 
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
            }
            foreach (Transform child in treeNode.transform) 
            {
                setTreeAlpha(child.gameObject, alpha);
            }
        
            if (!GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside)
            {
                multiFdObjOutside.SetActive(true);
            }
            else
            {
                multiFdObjOutside.SetActive(false);
            }
        }    
    }
