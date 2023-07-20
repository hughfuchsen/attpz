// Have been working on things to have zero alpha when walking inside the building
            // things such as open cupboards, doors, fridges etc.
// Issue as of now - 2/7/23: as I enter a building from below, the 'closed' door sprite I go through does no set back to 0.15f
// It does this because I made it so all objects with 0.15f alpha would go to 0 upon entry... except I need this particular door to stay at 0.15f.  


using System;
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

    // public List<GameObject> alphaZeroEntExt = new List<GameObject>();
    private float waitTime = 0.01f; // Time it takes between treFadeSequence(fade obj's)
    private float buildingFadeSpeed = 7f; // Time it takes to fade out the sprites
    private bool aboveCollider;
    private Coroutine fadeCoroutine;
    private Coroutine thresholdSortingSequenceCoro;




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
                
        if (multiFdObjInside != null)
        {
            // Call the function to access and tag child game objects with the specified tag
            TagChildrenOfTaggedParents(multiFdObjInside.transform, "AlphaZeroEntExt");
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

        GameObject doorParent = FindParentWithTag(this.gameObject, "Door");

        if (doorParent != null)
        {
            AdjustChildrenAlpha(doorParent, 1, 0);
        }

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        if (thresholdSortingSequenceCoro != null)
        {
            StopCoroutine(thresholdSortingSequenceCoro);
            SetSortingLayer(Player, "Default");
        }

        playerMovement.motionDirection = motionDirection;
                
        // if (openDoor != null)
        // {
        //     openDoor.color = new Color(openDoor.color.r, openDoor.color.g, openDoor.color.b, currentDoorAlpha);
        // }
        // if (closedDoor != null)
        // {
        //     closedDoor.color = new Color(closedDoor.color.r, closedDoor.color.g, closedDoor.color.b, 0);
        // }

        if (isPlayerCrossingUp())
        {
            aboveCollider = false;
        }    
        else 
        {
            aboveCollider = true; 
        }

        
    }

    void OnTriggerExit2D()
    {

        GameObject doorParent = FindParentWithTag(this.gameObject, "Door");

        if (doorParent != null)
        {
            AdjustChildrenAlpha(doorParent, 0, 1);
        }

        playerMovement.motionDirection = previousMotionDirection;

        //Bug avoidance: in case the player is inside and isPlayerInside is false etc. (need to optimise)
        // if (!itsAnEntrnceOrExt & GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside)
        // {
        //     fadeCoroutine = StartCoroutine(treeFadeSequence(waitTime, multiFdObjOutside, 1f, 1f, multiFdObjInside, 1f, 1f, buildingFadeSpeed));
        // }


        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        if (thresholdSortingSequenceCoro != null)
        {
            StopCoroutine(thresholdSortingSequenceCoro);
            SetSortingLayer(Player, "Default");
        }

        // if (openDoor != null)
        // {
        //     openDoor.color = new Color(openDoor.color.r, openDoor.color.g, openDoor.color.b, 0);
        // }
        // if (closedDoor != null)
        // {
        //     closedDoor.color = new Color(closedDoor.color.r, closedDoor.color.g, closedDoor.color.b, currentDoorAlpha);
        // }

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
                    multiFdObjOutside.SetActive(true);

                    fadeCoroutine = StartCoroutine(treeFadeSequence(waitTime, multiFdObjInside, 1f, 0f, multiFdObjOutside, 0f, 0.35f, buildingFadeSpeed));
                   
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
                    
                    multiFdObjOutside.SetActive(false);

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

            if (roomBelow != null)
            {
                roomBelow.EnterRoom();
            } 

            // not optimal. try adjusting the iso sorting point instead??
            if (aboveCollider)
            {
                thresholdSortingSequenceCoro = StartCoroutine(ThresholdLayerSortingSequence((float)roomBelow.wallHeight/100, Player, "Default", "ThresholdSequence"));
            }
            
            if (itsAnEntrnceOrExt)
            {
                if (GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside && aboveCollider)
                {
                    multiFdObjOutside.SetActive(true);
                    
                    fadeCoroutine = StartCoroutine(treeFadeSequence(waitTime, multiFdObjInside, 1f, 0f, multiFdObjOutside, 0f, 1f, buildingFadeSpeed));

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

                    multiFdObjOutside.SetActive(false);

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
        if ((sr != null && !treeNode.CompareTag("AlphaZeroEntExt")) || (sr != null && sr.color.a == 0.15f))
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
        }
        foreach (Transform child in treeNode.transform) 
        {
            setTreeAlpha(child.gameObject, alpha);
        }
    }  

    private void TagChildrenOfTaggedParents(Transform parent, string tag)
    {
        Stack<Transform> stack = new Stack<Transform>();
        stack.Push(parent);

        while (stack.Count > 0)
        {
            Transform current = stack.Pop();

            // Check if the current parent game object has the specified tag
            if (current.CompareTag(tag))
            {
                // Tag the children of the current parent game object with the specified tag
                foreach (Transform child in current)
                {
                    if (child.gameObject.CompareTag("Untagged") || child.GetComponent<SpriteRenderer>() != null)
                    {
                        child.gameObject.tag = tag;
                    }
                }
            }

            // Push the children of the current parent game object to the stack
            for (int i = 0; i < current.childCount; i++)
            {
                stack.Push(current.GetChild(i));
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
            ThresholdColliderScript.SetSortingLayer(child.gameObject, sortingLayerName);
        }
    }  

    IEnumerator ThresholdLayerSortingSequence(
    float waitTime,
    GameObject gameObject,
    string initialSortingLayer,
    string newSortingLayer) 
    {
        SetSortingLayer(gameObject, newSortingLayer);

        yield return new WaitForSeconds(waitTime);

        SetSortingLayer(gameObject, initialSortingLayer);

        yield return null;
    }
// a test
    private GameObject FindParentWithTag(GameObject gameObject, string name)
        {
            Transform parent = gameObject.transform.parent;

            while (parent != null)
            {
                if (parent.CompareTag(name))
                {
                    return parent.gameObject;
                }

                parent = parent.parent;
            }

            return null;
        }

    private void AdjustChildrenAlpha(GameObject parentObject, float openAlpha, float closedAlpha)
    {
        foreach (Transform child in parentObject.transform)
        {
            if (child.CompareTag("OpenDoor") || child.CompareTag("AlphaZeroEntExt"))
            {
                setTreeAlpha(child.gameObject, openAlpha); 
            }
            else if (child.CompareTag("ClosedDoor"))
            {
                setTreeAlpha(child.gameObject, closedAlpha); 
            }
        }
    }
}
