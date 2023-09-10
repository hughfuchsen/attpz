// Have been working on things to have zero alpha when walking inside the building
//             things such as open cupboards, doors, fridges etc.
// Issue as of now - 2/7/23: as I enter a building from below, the 'closed' door sprite I go through does no set back to 0.15f
// It does this because I made it so all objects with 0.15f alpha would go to 0 upon entry... except I need this particular door to stay at 0.15f.  



using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThresholdColliderScript : MonoBehaviour
{
    public BuildingScript building;
    public RoomScript roomAbove;
    public RoomScript roomBelow;
    LevelColliderScript levelColliderScript;
    PlayerMovement playerMovement;
    [SerializeField] GameObject Player;
    public string motionDirection = "normal";
    public bool itsAnEntrnceOrExt;
    public GameObject multiFdObjInside;
    public GameObject multiFdObjOutside;

    // public List<GameObject> alphaZeroEntExt = new List<GameObject>();
    private float waitTime = 0.01f; // Time it takes between treFadeSequence(fade obj's)
    private float buildingFadeSpeed = 7f; // Time it takes to fade out the sprites
    private bool aboveCollider;
    private Coroutine fadeCoroutine;

    private List<float> openDoorAlpha = new List<float>();
    private List<float> closedDoorAlpha = new List<float>();
    private List<GameObject> openDoorSpriteList = new List<GameObject>();
    private List<GameObject> closedDoorSpriteList = new List<GameObject>();

    private Coroutine closedDoorFadeCoroutine;
    private Coroutine openDoorFadeCoroutine;
    private Coroutine thresholdSortingSequenceCoro;
    private string initialSortingLayer;

    private bool playerIsInDoorway = false;
    private bool playerIsInRoomAbove = false;

    private string[] tagsToExludeEntExt = { "OpenDoor", "AlphaZeroEntExt" };
    


    void Awake()
    { 
        Player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = Player.GetComponent<PlayerMovement>(); 
    }  

    void Start()
    {
        if (multiFdObjInside != null)
        {
            // Call the function to access and tag child game objects with the specified tag
            TagChildrenOfTaggedParents(multiFdObjInside.transform, "AlphaZeroEntExt");
            TagChildrenOfTaggedParents(multiFdObjInside.transform, "OpenDoor");
        }

        // get the sprites and add them to the corresponding lists
        GetSprites(FindSiblingWithTag("OpenDoor"), openDoorSpriteList);
        GetSprites(FindSiblingWithTag("ClosedDoor"), closedDoorSpriteList);

        for (int i = 0; i < openDoorSpriteList.Count; i++)
        {
            Color initialColorOpen = openDoorSpriteList[i].GetComponent<SpriteRenderer>().color;
            openDoorAlpha.Add(initialColorOpen.a);
        }

        for (int i = 0; i < closedDoorSpriteList.Count; i++)
        {
            Color initialColorClosed = closedDoorSpriteList[i].GetComponent<SpriteRenderer>().color;
            closedDoorAlpha.Add(initialColorClosed.a);
        }

        for (int i = 0; i < openDoorSpriteList.Count; i++)
        {
            SetTreeAlpha(openDoorSpriteList[i], 0);
        }        
        for (int i = 0; i < closedDoorSpriteList.Count; i++)
        {
            SetTreeAlpha(closedDoorSpriteList[i], closedDoorAlpha[i]);
        }    


        SetDoorState(false, false);
    }

    void OnTriggerEnter2D()
    {
        initialSortingLayer = Player.GetComponent<SpriteRenderer>().sortingLayerName;

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        if (thresholdSortingSequenceCoro != null)
        {
            StopCoroutine(thresholdSortingSequenceCoro);
            SetSortingLayer(Player, initialSortingLayer);
        }
        
        playerMovement.motionDirection = motionDirection;

        if((isPlayerCrossingUp() && isPlayerCrossingLeft()) 
            || !isPlayerCrossingUp() && !isPlayerCrossingLeft())
        {
            playerMovement.fixedDirectionLeft = true;
        }
        else
        {
            playerMovement.fixedDirectionRight = true;
        }


        if (isPlayerCrossingUp())
        {
            aboveCollider = false;
        }    
        else 
        {
            aboveCollider = true; 
        }

        SetPlayerIsInDoorway(true);
    }

    void OnTriggerExit2D()
    {
        // playerMovement.motionDirection = previousMotionDirection;

            playerMovement.fixedDirectionLeft = false;
            playerMovement.fixedDirectionRight = false;

       // Bug avoidance: in case the player is inside and isPlayerInside is false etc. (need to optimise)
        if (!itsAnEntrnceOrExt && !GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside
            && multiFdObjInside != null && multiFdObjOutside != null)
        {
            SetTreeAlpha(multiFdObjInside, 1, tagsToExludeEntExt);
            SetTreeAlpha(multiFdObjOutside, 0);
            multiFdObjOutside.SetActive(false);
            GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside = true;
        }


        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        if (thresholdSortingSequenceCoro != null)
        {
            StopCoroutine(thresholdSortingSequenceCoro);
            SetSortingLayer(Player, initialSortingLayer);
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
                    if (roomBelow != null || roomAbove == null)
                    {
                        roomBelow.ExitBuilding();
                    }  
                }
                else if (!aboveCollider)
                {
                    fadeCoroutine = StartCoroutine(treeFadeSequence(waitTime, multiFdObjOutside, 1f, 0f, multiFdObjInside, 0f, 1f, buildingFadeSpeed, tagsToExludeEntExt));
                    
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

                if (aboveCollider && GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside)
                {
                    thresholdSortingSequenceCoro = StartCoroutine(ThresholdLayerSortingSequence(0.5f, Player, "ThresholdSequence"));
                }
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
                        roomBelow.ExitBuilding();
                    }                
                }
                else if (aboveCollider)
                {
                    fadeCoroutine = StartCoroutine(treeFadeSequence(waitTime, multiFdObjOutside, 0.35f, 0f, multiFdObjInside, 0f, 1f, buildingFadeSpeed, tagsToExludeEntExt));

                    multiFdObjOutside.SetActive(false);

                    GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside = true;
                }
            }
            
            aboveCollider = false;

        }

        SetPlayerIsInDoorway(false);
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
        float buildingFadeSpeed,
        string[] tagsToExclude = null) 
        {
            for (float t = 0.0f; t < 1; t += Time.deltaTime) 
            {
                float currentAlpha = Mathf.Lerp(fadeFromFirst, fadeToFirst, t * buildingFadeSpeed);
                SetTreeAlpha(objFirst, currentAlpha, tagsToExclude);
                // yield return null;
            }

            yield return new WaitForSeconds(waitTime);

            for (float t = 0.0f; t < 1; t += Time.deltaTime) 
            {
                float currentAlpha = Mathf.Lerp(fadeFromSecond, fadeToSecond, t * buildingFadeSpeed);
                SetTreeAlpha(objSecond, currentAlpha, tagsToExclude);
                yield return null; 
            }
        }

    // private void SetTreeAlpha(GameObject root, float alpha, string[] tagsToExclude = null)
    // {
    //     Stack<GameObject> stack = new Stack<GameObject>();
    //     stack.Push(root);

    //     while (stack.Count > 0)
    //     {
    //         GameObject currentNode = stack.Pop();
    //         SpriteRenderer sr = currentNode.GetComponent<SpriteRenderer>();

    //         if (sr != null && (tagsToExclude == null || !Array.Exists(tagsToExclude, element => element == root.tag)))
    //         {
    //             sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
    //         }

    //         foreach (Transform child in currentNode.transform)
    //         {
    //             stack.Push(child.gameObject);
    //         }
    //     }
    // }

    void SetTreeAlpha(GameObject treeNode, float alpha, string[] tagsToExclude = null) 
    {
        if (treeNode == null) {
            return; // TODO: remove this
        }
        SpriteRenderer sr = treeNode.GetComponent<SpriteRenderer>();
        if (sr != null && (tagsToExclude == null || !Array.Exists(tagsToExclude, element => element == treeNode.tag)))
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
        }
        foreach (Transform child in treeNode.transform) 
        {
            SetTreeAlpha(child.gameObject, alpha, tagsToExclude);
        }
    }  

    IEnumerator treeFade(
      GameObject obj,
      float fadeTo,
      float fadeSpeed) 
    {
        float fadeFrom = obj.GetComponent<SpriteRenderer>().color.a;

        for (float t = 0.0f; t < 1; t += Time.deltaTime) 
        {
            float currentAlpha = Mathf.Lerp(fadeFrom, fadeTo, t * fadeSpeed);
            SetTreeAlpha(obj, currentAlpha);
            yield return null;
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
        if(gameObject.GetComponent<SpriteRenderer>() != null) 
        {
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
    string newSortingLayer) 
    {
        initialSortingLayer = FindSiblingWithTag("ClosedDoor").GetComponent<SpriteRenderer>().sortingLayerName;

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

    // NOTE: The parent of the threshold collider must be the door parent
    public GameObject FindSiblingWithTag(string tag) 
    {
        foreach (Transform child in this.gameObject.transform.parent)
        {
            if (child.CompareTag(tag))
            {
                return child.gameObject;
            }
        }
        return null;
    }

    private void ClosedDoorExternalVisibility()
    {
        if (multiFdObjInside != null)
        {
            Stack<Transform> stack = new Stack<Transform>();
            stack.Push(multiFdObjInside.transform);

            while (stack.Count > 0)
            {
                Transform current = stack.Pop();

                // Tag the children of the current parent game object with the specified tag
                foreach (Transform child in current)
                {
                    //  if (child.gameObject.CompareTag("ClosedDoor") || child.GetComponent<ThresholdColliderScript>() != null)
                    if (child.GetComponent<ThresholdColliderScript>() != null && child.GetComponent<ThresholdColliderScript>().itsAnEntrnceOrExt)
                    {
                        SetTreeAlpha(child.GetComponent<ThresholdColliderScript>().FindSiblingWithTag("ClosedDoor"), 1);
                    }
                }

                // Push the children of the current parent game object to the stack
                for (int i = 0; i < current.childCount; i++)
                {
                    stack.Push(current.GetChild(i));
                }
            } 
        }       
    }

    private void GetSprites(GameObject root, List<GameObject> spriteList)
    {
        if(root != null)
        {
            Stack<GameObject> stack = new Stack<GameObject>();
            stack.Push(root);

            while (stack.Count > 0)
            {
                GameObject currentNode = stack.Pop();
                SpriteRenderer sr = currentNode.GetComponent<SpriteRenderer>();

                if (sr != null)
                {
                    spriteList.Add(currentNode);
                }

                foreach (Transform child in currentNode.transform)
                {
                    stack.Push(child.gameObject);
                }
            }
        }
    }

    public void SetDoorState(bool playerIsInDoorway, bool playerIsInRoomAbove, bool fade=false) 
    {
        this.playerIsInDoorway = playerIsInDoorway;
        this.playerIsInRoomAbove = playerIsInRoomAbove;

        if (fade) {
            if(this.closedDoorFadeCoroutine != null)
            {
                StopCoroutine(this.closedDoorFadeCoroutine);
            }

            if(this.openDoorFadeCoroutine != null)
            {
                StopCoroutine(this.openDoorFadeCoroutine);
            }

            if (playerIsInDoorway) {
                this.closedDoorFadeCoroutine = StartCoroutine(treeFade(this.FindSiblingWithTag("ClosedDoor"), 0, 3f));
                this.openDoorFadeCoroutine = StartCoroutine(treeFade(this.FindSiblingWithTag("OpenDoor"), 1, 3f));
            }
            else if (playerIsInRoomAbove) {
                this.closedDoorFadeCoroutine = StartCoroutine(treeFade(this.FindSiblingWithTag("ClosedDoor"), 0.15f, 3f));
                this.openDoorFadeCoroutine = StartCoroutine(treeFade(this.FindSiblingWithTag("OpenDoor"), 0, 3f));
            }
            else if (!GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside) {
                ClosedDoorExternalVisibility();            
                this.openDoorFadeCoroutine = StartCoroutine(treeFade(this.FindSiblingWithTag("OpenDoor"), 0, 3f));
            }
            else {
                this.closedDoorFadeCoroutine = StartCoroutine(treeFade(this.FindSiblingWithTag("ClosedDoor"), 1, 3f));
                this.openDoorFadeCoroutine = StartCoroutine(treeFade(this.FindSiblingWithTag("OpenDoor"), 0, 3f));
            }
        } else {
            if (playerIsInDoorway) {
                for (int i = 0; i < closedDoorSpriteList.Count; i++)
                {
                    SetTreeAlpha(closedDoorSpriteList[i], 0);
                }                    
                
                for (int i = 0; i < openDoorSpriteList.Count; i++)
                {
                    SetTreeAlpha(openDoorSpriteList[i], openDoorAlpha[i]);
                }                    
            }
            else if (playerIsInRoomAbove) {
                for (int i = 0; i < closedDoorSpriteList.Count; i++)
                {
                    SetTreeAlpha(closedDoorSpriteList[i], closedDoorAlpha[i] - ((closedDoorAlpha[i]/100)*85));
                }                           
                
                for (int i = 0; i < openDoorSpriteList.Count; i++)
                {
                    SetTreeAlpha(openDoorSpriteList[i], 0);
                }               
            }
            else if (!GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside) {
                ClosedDoorExternalVisibility(); 

                for (int i = 0; i < openDoorSpriteList.Count; i++)
                {
                    SetTreeAlpha(openDoorSpriteList[i], 0);
                }       
            }
            else {
                for (int i = 0; i < closedDoorSpriteList.Count; i++)
                {
                    SetTreeAlpha(closedDoorSpriteList[i], closedDoorAlpha[i]);
                }         

                for (int i = 0; i < openDoorSpriteList.Count; i++)
                {
                    SetTreeAlpha(openDoorSpriteList[i], 0);
                }     
            }
        }


    }

    public void SetPlayerIsInDoorway(bool playerIsInDoorway) 
    {
        this.playerIsInDoorway = playerIsInDoorway;
        SetDoorState(playerIsInDoorway, playerIsInRoomAbove);
    }

    public void SetPlayerIsInRoomAbove(bool playerIsInRoomAbove) 
    {
        this.playerIsInRoomAbove = playerIsInRoomAbove;
        //SetDoorState(playerIsInDoorway, playerIsInRoomAbove, true);
        SetDoorState(playerIsInDoorway, playerIsInRoomAbove, false);
    }
}







// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class ThresholdColliderScript : MonoBehaviour
// {
//     public RoomScript roomAbove;
//     public RoomScript roomBelow;
//     LevelColliderScript levelColliderScript;
//     PlayerMovement playerMovement;
//     [SerializeField] GameObject Player;
//     public string motionDirection = "normal";
//     [SerializeField] bool itsAnEntrnceOrExt;
//     public GameObject multiFdObjInside;
//     public GameObject multiFdObjOutside;

//     // public List<GameObject> alphaZeroEntExt = new List<GameObject>();
//     private float waitTime = 0.01f; // Time it takes between treFadeSequence(fade obj's)
//     private float buildingFadeSpeed = 7f; // Time it takes to fade out the sprites
//     private bool aboveCollider;
//     private Coroutine fadeCoroutine;

//     private Coroutine closedDoorFadeCoroutine;
//     private Coroutine openDoorFadeCoroutine;
//     private Coroutine thresholdSortingSequenceCoro;
//     private string initialSortingLayer;

//     private bool playerIsInDoorway = false;
//     private bool playerIsInRoomAbove = false;

//     private string[] tagsToExludeEntExt = { "OpenDoor", "AlphaZeroEntExt" };
    


//     void Awake()
//     { 
//         Player = GameObject.FindGameObjectWithTag("Player");
//         playerMovement = Player.GetComponent<PlayerMovement>(); 
//     }  

//     void Start()
//     {
//         if (multiFdObjInside != null)
//         {
//             // Call the function to access and tag child game objects with the specified tag
//             TagChildrenOfTaggedParents(multiFdObjInside.transform, "AlphaZeroEntExt");
//         }

//         SetDoorState(false, false);
//     }

//     void OnTriggerEnter2D()
//     {
//         initialSortingLayer = Player.GetComponent<SpriteRenderer>().sortingLayerName;

//         if (fadeCoroutine != null)
//         {
//             StopCoroutine(fadeCoroutine);
//         }

//         if (thresholdSortingSequenceCoro != null)
//         {
//             StopCoroutine(thresholdSortingSequenceCoro);
//             SetSortingLayer(Player, initialSortingLayer);
//         }
        
//         playerMovement.motionDirection = motionDirection;

//         if((isPlayerCrossingUp() && isPlayerCrossingLeft()) 
//             || !isPlayerCrossingUp() && !isPlayerCrossingLeft())
//         {
//             playerMovement.fixedDirectionLeft = true;
//         }
//         else
//         {
//             playerMovement.fixedDirectionRight = true;
//         }


//         if (isPlayerCrossingUp())
//         {
//             aboveCollider = false;
//         }    
//         else 
//         {
//             aboveCollider = true; 
//         }

//         SetPlayerIsInDoorway(true);
//     }

//     void OnTriggerExit2D()
//     {
//         // playerMovement.motionDirection = previousMotionDirection;

//             playerMovement.fixedDirectionLeft = false;
//             playerMovement.fixedDirectionRight = false;

//        // Bug avoidance: in case the player is inside and isPlayerInside is false etc. (need to optimise)
//         if (!itsAnEntrnceOrExt && !GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside
//             && multiFdObjInside != null && multiFdObjOutside != null)
//         {
//             SetTreeAlpha(multiFdObjInside, 1, tagsToExludeEntExt);
//             SetTreeAlpha(multiFdObjOutside, 0);
//             multiFdObjOutside.SetActive(false);
//             GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside = true;
//         }


//         if (fadeCoroutine != null)
//         {
//             StopCoroutine(fadeCoroutine);
//         }

//         if (thresholdSortingSequenceCoro != null)
//         {
//             StopCoroutine(thresholdSortingSequenceCoro);
//             SetSortingLayer(Player, initialSortingLayer);
//         }

//         // if (openDoor != null)
//         // {
//         //     openDoor.color = new Color(openDoor.color.r, openDoor.color.g, openDoor.color.b, 0);
//         // }
//         // if (closedDoor != null)
//         // {
//         //     closedDoor.color = new Color(closedDoor.color.r, closedDoor.color.g, closedDoor.color.b, currentDoorAlpha);
//         // }

//             //ON EXIT CROSSING UP
//         if (isPlayerCrossingUp())
//         {
//             if (roomBelow != null)
//             {
//                 roomBelow.ExitRoom();
//             }
//             if (roomAbove != null)
//             {
//                 roomAbove.EnterRoom();
//             }

//             if (itsAnEntrnceOrExt)
//             {
//                 if (GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside && !aboveCollider)
//                 {
//                     multiFdObjOutside.SetActive(true);

//                     fadeCoroutine = StartCoroutine(treeFadeSequence(waitTime, multiFdObjInside, 1f, 0f, multiFdObjOutside, 0f, 0.35f, buildingFadeSpeed));
                   
//                     GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside = false;

//                     //need to optimise
//                     if (roomBelow != null || roomAbove == null)
//                     {
//                         roomBelow.ExitBuilding();
//                     }  
//                 }
//                 else if (!aboveCollider)
//                 {
//                     fadeCoroutine = StartCoroutine(treeFadeSequence(waitTime, multiFdObjOutside, 1f, 0f, multiFdObjInside, 0f, 1f, buildingFadeSpeed, tagsToExludeEntExt));
                    
//                     multiFdObjOutside.SetActive(false);

//                     GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside = true;
//                 }
//             }
            
//             aboveCollider = true;
//         }
//             //ON EXIT CROSSING DOWN
//         else
//         {
//             if (roomAbove != null)
//             {
//                 roomAbove.ExitRoom();
//             }

//             if (roomBelow != null)
//             {
//                 roomBelow.EnterRoom();

//                 if (aboveCollider && GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside)
//                 {
//                     thresholdSortingSequenceCoro = StartCoroutine(ThresholdLayerSortingSequence((float)roomBelow.wallHeight/100, Player, "ThresholdSequence"));
//                 }
//             } 
            
//             if (itsAnEntrnceOrExt)
//             {
//                 if (GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside && aboveCollider)
//                 {
//                     multiFdObjOutside.SetActive(true);
                    
//                     fadeCoroutine = StartCoroutine(treeFadeSequence(waitTime, multiFdObjInside, 1f, 0f, multiFdObjOutside, 0f, 1f, buildingFadeSpeed));

//                     GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside = false;
                    
//                     //need to optimise
//                     if (roomBelow != null)
//                     {
//                         roomBelow.ExitBuilding();
//                     }                
//                 }
//                 else if (aboveCollider)
//                 {
//                     fadeCoroutine = StartCoroutine(treeFadeSequence(waitTime, multiFdObjOutside, 0.35f, 0f, multiFdObjInside, 0f, 1f, buildingFadeSpeed, tagsToExludeEntExt));

//                     multiFdObjOutside.SetActive(false);

//                     GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside = true;
//                 }
//             }
            
//             aboveCollider = false;

//         }

//         SetPlayerIsInDoorway(false);
//     }
//     private bool isPlayerCrossingUp()
//     {
//         return GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().change.y > 0;
//     }
//     private bool isPlayerCrossingLeft()
//     {
//         return GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().change.x < 0;
//     }

//     IEnumerator treeFadeSequence(
//         float waitTime,
//         GameObject objFirst,
//         float fadeFromFirst, 
//         float fadeToFirst,
//         GameObject objSecond,
//         float fadeFromSecond,
//         float fadeToSecond,
//         float buildingFadeSpeed,
//         string[] tagsToExclude = null) 
//         {
//             for (float t = 0.0f; t < 1; t += Time.deltaTime) 
//             {
//                 float currentAlpha = Mathf.Lerp(fadeFromFirst, fadeToFirst, t * buildingFadeSpeed);
//                 SetTreeAlpha(objFirst, currentAlpha, tagsToExclude);
//                 // yield return null;
//             }

//             yield return new WaitForSeconds(waitTime);

//             for (float t = 0.0f; t < 1; t += Time.deltaTime) 
//             {
//                 float currentAlpha = Mathf.Lerp(fadeFromSecond, fadeToSecond, t * buildingFadeSpeed);
//                 SetTreeAlpha(objSecond, currentAlpha, tagsToExclude);
//                 yield return null; 
//             }
//         }

//     void SetTreeAlpha(GameObject treeNode, float alpha, string[] tagsToExclude = null) 
//     {
//         if (treeNode == null) {
//             return; // TODO: remove this
//         }
//         SpriteRenderer sr = treeNode.GetComponent<SpriteRenderer>();
//         if (sr != null && (tagsToExclude == null || !Array.Exists(tagsToExclude, element => element == treeNode.tag)))
//         {
//             sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
//         }
//         foreach (Transform child in treeNode.transform) 
//         {
//             SetTreeAlpha(child.gameObject, alpha, tagsToExclude);
//         }
//     }  

//     IEnumerator treeFade(
//       GameObject obj,
//       float fadeTo,
//       float fadeSpeed) 
//     {
//         float fadeFrom = obj.GetComponent<SpriteRenderer>().color.a;

//         for (float t = 0.0f; t < 1; t += Time.deltaTime) 
//         {
//             float currentAlpha = Mathf.Lerp(fadeFrom, fadeTo, t * fadeSpeed);
//             SetTreeAlpha(obj, currentAlpha);
//             yield return null;
//         }
//     }
//     private void TagChildrenOfTaggedParents(Transform parent, string tag)
//     {
//         Stack<Transform> stack = new Stack<Transform>();
//         stack.Push(parent);

//         while (stack.Count > 0)
//         {
//             Transform current = stack.Pop();

//             // Check if the current parent game object has the specified tag
//             if (current.CompareTag(tag))
//             {
//                 // Tag the children of the current parent game object with the specified tag
//                 foreach (Transform child in current)
//                 {
//                     if (child.gameObject.CompareTag("Untagged") || child.GetComponent<SpriteRenderer>() != null)
//                     {
//                         child.gameObject.tag = tag;
//                     }
//                 }
//             }

//             // Push the children of the current parent game object to the stack
//             for (int i = 0; i < current.childCount; i++)
//             {
//                 stack.Push(current.GetChild(i));
//             }
//         }
//     }
    
//     static void SetSortingLayer(GameObject gameObject, string sortingLayerName)
//     {
//         if(gameObject.GetComponent<SpriteRenderer>() != null) 
//         {
//             gameObject.GetComponent<SpriteRenderer>().sortingLayerName = sortingLayerName;
//         }
//         foreach (Transform child in gameObject.transform)
//         {
//             ThresholdColliderScript.SetSortingLayer(child.gameObject, sortingLayerName);
//         }
//     }  

//     IEnumerator ThresholdLayerSortingSequence(
//     float waitTime,
//     GameObject gameObject,
//     string newSortingLayer) 
//     {
//         initialSortingLayer = FindSiblingWithTag("ClosedDoor").GetComponent<SpriteRenderer>().sortingLayerName;

//         SetSortingLayer(gameObject, newSortingLayer);

//         yield return new WaitForSeconds(waitTime);

//         SetSortingLayer(gameObject, initialSortingLayer);

//         yield return null;
//     }
// // a test
//     private GameObject FindParentWithTag(GameObject gameObject, string name)
//         {
//             Transform parent = gameObject.transform.parent;

//             while (parent != null)
//             {
//                 if (parent.CompareTag(name))
//                 {
//                     return parent.gameObject;
//                 }

//                 parent = parent.parent;
//             }

//             return null;
//         }

//     // NOTE: The parent of the threshold collider must be the door parent
//     private GameObject FindSiblingWithTag(string tag) 
//     {
//         foreach (Transform child in this.gameObject.transform.parent)
//         {
//             if (child.CompareTag(tag))
//             {
//                 return child.gameObject;
//             }
//         }
//         return null;
//     }

//     private void ClosedDoorExternalVisibility()
//     {
//         if (multiFdObjInside != null)
//         {
//             Stack<Transform> stack = new Stack<Transform>();
//             stack.Push(multiFdObjInside.transform);

//             while (stack.Count > 0)
//             {
//                 Transform current = stack.Pop();

//                 // Tag the children of the current parent game object with the specified tag
//                 foreach (Transform child in current)
//                 {
//                     //  if (child.gameObject.CompareTag("ClosedDoor") || child.GetComponent<ThresholdColliderScript>() != null)
//                     if (child.GetComponent<ThresholdColliderScript>() != null && child.GetComponent<ThresholdColliderScript>().itsAnEntrnceOrExt)
//                     {
//                         SetTreeAlpha(child.GetComponent<ThresholdColliderScript>().FindSiblingWithTag("ClosedDoor"), 1);
//                     }
//                 }

//                 // Push the children of the current parent game object to the stack
//                 for (int i = 0; i < current.childCount; i++)
//                 {
//                     stack.Push(current.GetChild(i));
//                 }
//             } 
//         }       
//     }

//     public void SetDoorState(bool playerIsInDoorway, bool playerIsInRoomAbove, bool fade=false) 
//     {
//         this.playerIsInDoorway = playerIsInDoorway;
//         this.playerIsInRoomAbove = playerIsInRoomAbove;

//         if (fade) {
//             if(this.closedDoorFadeCoroutine != null)
//             {
//                 StopCoroutine(this.closedDoorFadeCoroutine);
//             }

//             if(this.openDoorFadeCoroutine != null)
//             {
//                 StopCoroutine(this.openDoorFadeCoroutine);
//             }

//             if (playerIsInDoorway) {
//                 this.closedDoorFadeCoroutine = StartCoroutine(treeFade(this.FindSiblingWithTag("ClosedDoor"), 0, 3f));
//                 this.openDoorFadeCoroutine = StartCoroutine(treeFade(this.FindSiblingWithTag("OpenDoor"), 1, 3f));
//             }
//             else if (playerIsInRoomAbove) {
//                 this.closedDoorFadeCoroutine = StartCoroutine(treeFade(this.FindSiblingWithTag("ClosedDoor"), 0.15f, 3f));
//                 this.openDoorFadeCoroutine = StartCoroutine(treeFade(this.FindSiblingWithTag("OpenDoor"), 0, 3f));
//             }
//             else if (!GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside) {
//                 ClosedDoorExternalVisibility();            
//                 this.openDoorFadeCoroutine = StartCoroutine(treeFade(this.FindSiblingWithTag("OpenDoor"), 0, 3f));
//             }
//             else {
//                 this.closedDoorFadeCoroutine = StartCoroutine(treeFade(this.FindSiblingWithTag("ClosedDoor"), 1, 3f));
//                 this.openDoorFadeCoroutine = StartCoroutine(treeFade(this.FindSiblingWithTag("OpenDoor"), 0, 3f));
//             }
//         } else {
//             if (playerIsInDoorway) {
//                 SetTreeAlpha(this.FindSiblingWithTag("ClosedDoor"), 0);
//                 SetTreeAlpha(this.FindSiblingWithTag("OpenDoor"), 1);
//             }
//             else if (playerIsInRoomAbove) {
//                 SetTreeAlpha(this.FindSiblingWithTag("ClosedDoor"), 0.15f);
//                 SetTreeAlpha(this.FindSiblingWithTag("OpenDoor"), 0);
//             }
//             else if (!GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().isPlayerInside) {
//                 ClosedDoorExternalVisibility();            
//                 SetTreeAlpha(this.FindSiblingWithTag("OpenDoor"), 0);
//             }
//             else {
//                 SetTreeAlpha(this.FindSiblingWithTag("ClosedDoor"), 1);
//                 SetTreeAlpha(this.FindSiblingWithTag("OpenDoor"), 0);
//             }
//         }


//     }

//     public void SetPlayerIsInDoorway(bool playerIsInDoorway) 
//     {
//         this.playerIsInDoorway = playerIsInDoorway;
//         SetDoorState(playerIsInDoorway, playerIsInRoomAbove);
//     }

//     public void SetPlayerIsInRoomAbove(bool playerIsInRoomAbove) 
//     {
//         this.playerIsInRoomAbove = playerIsInRoomAbove;
//         //SetDoorState(playerIsInDoorway, playerIsInRoomAbove, true);
//         SetDoorState(playerIsInDoorway, playerIsInRoomAbove, false);
//     }
// }




