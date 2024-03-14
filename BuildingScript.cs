using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{
    public GameObject innerBuilding;
    public GameObject outerBuilding;
    public List<GameObject> gameObjectsToShowOutside = new List<GameObject>();
    PlayerMovement playerMovement;
    [SerializeField] GameObject Player;
    private List<GameObject> gameObjectsToShowOutsideSpriteList = new List<GameObject>();
    private List<Color> gameObjectsToShowOutsideColorList = new List<Color>();
    private List<GameObject> innerBuildingSpriteList = new List<GameObject>();
    private List<GameObject> outerBuildingSpriteList = new List<GameObject>();
    private List<Color> innerBuildingInitialColorList = new List<Color>();
    private List<Color> outerBuildingInitialColorList = new List<Color>();
    private List<IsoSpriteSorting> innerSpriteSortingScriptObj = new List<IsoSpriteSorting>();
    private List<IsoSpriteSorting> outerSpriteSortingScriptObj = new List<IsoSpriteSorting>();

    private Coroutine innerBuildingFadeCoroutine;
    private Coroutine outerBuildingFadeCoroutine;
    private Coroutine backdropFadeCoroutine;
    private string[] tagsToExludeEntExt = { "OpenDoor", "AlphaZeroEntExt" };

    void Awake()
    { 
        Player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = Player.GetComponent<PlayerMovement>(); 
    }  

    // Start is called before the first frame update
    void Start()
    {
        innerBuilding.SetActive(true);
        outerBuilding.SetActive(true);
        GetSpritesAndAddToLists(innerBuilding, innerBuildingSpriteList, gameObjectsToShowOutside, innerBuildingInitialColorList);
        GetSpritesAndAddToLists(outerBuilding, outerBuildingSpriteList, new List<GameObject>(), outerBuildingInitialColorList);
        foreach(GameObject obj in gameObjectsToShowOutside) {
            GetSpritesAndAddToLists(obj, gameObjectsToShowOutsideSpriteList, new List<GameObject>(), gameObjectsToShowOutsideColorList);
        }

        GetIsoSpriteSortComponentsAndAddToLists(innerBuilding, innerSpriteSortingScriptObj, gameObjectsToShowOutside);
        GetIsoSpriteSortComponentsAndAddToLists(outerBuilding, outerSpriteSortingScriptObj, new List<GameObject>());
        TagChildrenOfTaggedParents("OpenDoor");
        TagChildrenOfTaggedParents("ClosedDoor");
        TagChildrenOfTaggedParents("AlphaZeroEntExt");
        this.ExitBuilding(0f, 0f);
    }

    public void EnterBuilding()
    {
        if(this.innerBuildingFadeCoroutine != null)
        {
            StopCoroutine(this.innerBuildingFadeCoroutine);
        }
        if(this.outerBuildingFadeCoroutine != null)
        {
            StopCoroutine(this.outerBuildingFadeCoroutine);
        }
        if(this.backdropFadeCoroutine != null)
        {
            StopCoroutine(this.backdropFadeCoroutine);
        }

        
        // for (int i = 0; i < innerBuildingSpriteList.Count; i++)
        // {                       
        //     if (innerBuildingSpriteList[i].GetComponent<SpriteRenderer>().sortingLayerName == "Default")
        //     {
        //         SetSortingLayer(innerBuildingSpriteList[i], "Level0");
        //         SetSortingLayer(Player, "Level0");
        //     }
        // }
        backdropFadeCoroutine = StartCoroutine(FadeObject(false, 0f, GameObject.FindGameObjectWithTag("InnerBuildingBackdrop"), 1f));

        innerBuildingFadeCoroutine = StartCoroutine(FadeThenSetDontSort(false, innerSpriteSortingScriptObj, false, false, 0f, false, innerBuildingSpriteList, innerBuildingInitialColorList, null, tagsToExludeEntExt));
        outerBuildingFadeCoroutine = StartCoroutine(FadeThenSetDontSort(true, outerSpriteSortingScriptObj, true, false, 0f, false, outerBuildingSpriteList, null, 0f));
    }
    public void ExitBuilding(float waitTimeInside, float waitTimeOutside)
    {
        if(this.innerBuildingFadeCoroutine != null)
        {
            StopCoroutine(this.innerBuildingFadeCoroutine);
        }
        if(this.outerBuildingFadeCoroutine != null)
        {
            StopCoroutine(this.outerBuildingFadeCoroutine);
        }
        if(this.backdropFadeCoroutine != null)
        {
            StopCoroutine(this.backdropFadeCoroutine);
        }

        // for (int i = 0; i < innerBuildingSpriteList.Count; i++)
        // {                       
        //     if (innerBuildingSpriteList[i].GetComponent<SpriteRenderer>().sortingLayerName == "Level0")
        //     {
        //         SetSortingLayer(innerBuildingSpriteList[i], "Default");
        //         SetSortingLayer(Player, "Default");
        //     }
        // }
        backdropFadeCoroutine = StartCoroutine(FadeObject(false, 0f, GameObject.FindGameObjectWithTag("InnerBuildingBackdrop"), 0f));

        innerBuildingFadeCoroutine = StartCoroutine(FadeThenSetDontSort(true, innerSpriteSortingScriptObj, true, true, waitTimeInside, false, innerBuildingSpriteList, null, 0f));
        outerBuildingFadeCoroutine = StartCoroutine(FadeThenSetDontSort(false, outerSpriteSortingScriptObj, false, true, waitTimeOutside, false, outerBuildingSpriteList, outerBuildingInitialColorList, null, tagsToExludeEntExt));
    }
    public void GoBehindBuilding()
    {
        if(this.innerBuildingFadeCoroutine != null)
        {
            StopCoroutine(this.innerBuildingFadeCoroutine);
        }
        if(this.outerBuildingFadeCoroutine != null)
        {
            StopCoroutine(this.outerBuildingFadeCoroutine);
        }
        // FadeThenSetDontSort
        //                                     ( 
        //                                     bool fadeFirst,
        //                                     List<IsoSpriteSorting> issList, 
        //                                     bool setDontSort, bool shouldWait, 
        //                                     float? waitTime, 
        //                                     bool behindBuilding, 
        //                                     List<GameObject> spriteList, 
        //                                     List<Color> colorList, 
        //                                     float? alpha, 
        //                                     string[] tagsToExclude = null
        //                                     )

        backdropFadeCoroutine = StartCoroutine(FadeObject(true, 0.3f, GameObject.FindGameObjectWithTag("InnerBuildingBackdrop"), 0f));

        innerBuildingFadeCoroutine = StartCoroutine(FadeThenSetDontSort(true, innerSpriteSortingScriptObj, true, true, 1f, true, innerBuildingSpriteList, null, 0f));
        outerBuildingFadeCoroutine = StartCoroutine(FadeThenSetDontSort(false, outerSpriteSortingScriptObj, false, true, 1f, true, outerBuildingSpriteList, null, 0.35f, tagsToExludeEntExt));

        // innerBuildingFadeCoroutine = StartCoroutine(Fade(true, 0.3f, true, innerBuildingSpriteList, null, 0f));
        // outerBuildingFadeCoroutine = StartCoroutine(Fade(true, 0.3f, true, outerBuildingSpriteList, null, 0.35f, tagsToExludeEntExt));
    }




    // coroutine for fading things in a sequence
    public IEnumerator Fade(bool shouldWait, float? waitTime, bool behindBuilding, List<GameObject> spriteList, List<Color> colorList, float? alpha, string[] tagsToExclude = null)
    {
        if (shouldWait && waitTime.HasValue) // if Exiting the building
        {
            yield return new WaitForSeconds(waitTime.Value);

            for (int i = 0; i < innerBuildingSpriteList.Count; i++)
            {                       
                if (innerBuildingSpriteList[i].GetComponent<SpriteRenderer>().sortingLayerName == "Level0")
                {
                    SetSortingLayer(innerBuildingSpriteList[i], "Default");
                }
            }
            for (int i = 0; i < gameObjectsToShowOutsideSpriteList.Count; i++)
            {            
                    if (gameObjectsToShowOutsideSpriteList[i].GetComponent<SpriteRenderer>().sortingLayerName == "Level0")
                    {
                        SetSortingLayer(gameObjectsToShowOutsideSpriteList[i], "Default");
                    }
            }
            if (Player.GetComponent<SpriteRenderer>().sortingLayerName == "Level0") // if the player exits building from the ground flow
            {
                SetSortingLayer(Player, "Default");
            }
        }
        else // if entering the building
        {
            for (int i = 0; i < innerBuildingSpriteList.Count; i++)
            {                       
                if (innerBuildingSpriteList[i].GetComponent<SpriteRenderer>().sortingLayerName == "Default")
                {
                    SetSortingLayer(innerBuildingSpriteList[i], "Level0");
                }
            }
            for (int i = 0; i < gameObjectsToShowOutsideSpriteList.Count; i++)
            {                      
                    if (gameObjectsToShowOutsideSpriteList[i].GetComponent<SpriteRenderer>().sortingLayerName == "Default")
                    {
                        SetSortingLayer(gameObjectsToShowOutsideSpriteList[i], "Level0");
                    }
            }
            if (Player.GetComponent<SpriteRenderer>().sortingLayerName == "Default") // if the player enters building at the ground flow
            {
                SetSortingLayer(Player, "Level0");
            }
        }

        for (float t = 0.0f; t < 1; t += Time.deltaTime) 
        {        
            for (int i = 0; i < spriteList.Count; i++)
            {
                SpriteRenderer sr = spriteList[i].GetComponent<SpriteRenderer>();
                Transform tr = spriteList[i].transform;

                if (sr != null && (tagsToExclude == null || !Array.Exists(tagsToExclude, element => element == spriteList[i].tag)))        
                {
                    if(colorList == null)
                    {
                        if (alpha.HasValue)
                        {
                            float nextAlpha = Mathf.Lerp(sr.color.a, alpha.Value, t * 1f);
                            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, nextAlpha);
                        }
                    }
                    else
                    {
                        float nextAlpha = Mathf.Lerp(sr.color.a, colorList[i].a, t * 1f);
                        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, nextAlpha);
                        // sr.color = colorList[i];
                    }    
                }  
            }   
            for (int i = 0; i < gameObjectsToShowOutsideSpriteList.Count; i++)
            {
                SpriteRenderer sr = gameObjectsToShowOutsideSpriteList[i].GetComponent<SpriteRenderer>();

                if (behindBuilding && !gameObjectsToShowOutsideSpriteList[i].CompareTag("OpenDoor"))
                {
                    float nextAlpha = Mathf.Lerp(sr.color.a, gameObjectsToShowOutsideColorList[i].a, t * 1f);
                    sr.color = gameObjectsToShowOutsideColorList[i];
                }     
            }
            yield return null;    
        }

    }

    public IEnumerator FadeObject(bool shouldWait, float? waitTime, GameObject sprite, float alpha)
    {
        if (shouldWait && waitTime.HasValue)
        {
            yield return new WaitForSeconds(waitTime.Value);
        }

        SpriteRenderer sr = sprite.GetComponent<SpriteRenderer>();

        // Calculate duration based on the alpha difference
        float alphaDiff = Mathf.Abs(sr.color.a - alpha);
        float duration = alphaDiff / 1f; // Assuming alpha ranges from 0 to 1
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            float nextAlpha = Mathf.Lerp(sr.color.a, alpha, t);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, nextAlpha);

            yield return null;
        }

        // Ensure the final alpha value is set
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
    }


    private IEnumerator FadeThenSetDontSort
                                            ( 
                                            bool fadeFirst,
                                            List<IsoSpriteSorting> issList, 
                                            bool setDontSort, bool shouldWait, 
                                            float? waitTime, 
                                            bool behindBuilding, 
                                            List<GameObject> spriteList, 
                                            List<Color> colorList, 
                                            float? alpha, 
                                            string[] tagsToExclude = null
                                            )
    {
        if(fadeFirst)
        {
            yield return Fade(shouldWait, waitTime.Value, behindBuilding, spriteList, colorList, alpha, tagsToExclude);
            SetDontSort(issList, setDontSort);
        }
        else
        {
            SetDontSort(issList, setDontSort);
            yield return Fade(shouldWait, waitTime.Value, behindBuilding, spriteList, colorList, alpha, tagsToExclude);
        }
    }







    private void GetSpritesAndAddToLists(GameObject obj, List<GameObject> spriteList, List<GameObject> excludeList, List<Color> colorList)
    {
        Stack<GameObject> stack = new Stack<GameObject>();
        stack.Push(obj);

        while (stack.Count > 0)
        {
            GameObject currentNode = stack.Pop();
            SpriteRenderer sr = currentNode.GetComponent<SpriteRenderer>();

            if (sr != null)
            {
                Color col = sr.color;
                spriteList.Add(currentNode);
                colorList.Add(col);
            }

            foreach (Transform child in currentNode.transform)
            {
                if (!excludeList.Contains(child.gameObject)) {
                    stack.Push(child.gameObject);
                }
            }
        }
    }
    private void GetIsoSpriteSortComponentsAndAddToLists(GameObject obj, List<IsoSpriteSorting> objList, List<GameObject> excludeList)
    {
        Stack<GameObject> stack = new Stack<GameObject>();
        stack.Push(obj);

        while (stack.Count > 0)
        {
            GameObject currentNode = stack.Pop();
            IsoSpriteSorting iss = currentNode.GetComponent<IsoSpriteSorting>();

            if (iss != null)
            {
                objList.Add(iss);
            }

            foreach (Transform child in currentNode.transform)
            {
                if(!excludeList.Contains(child.gameObject)) {
                    stack.Push(child.gameObject);
                }
            }
        }
    }

    private void SetDontSort(List<IsoSpriteSorting> objList, bool dontSort)
    {
        for (int i = 0; i < objList.Count; i++)
        {
            objList[i].dontSort = dontSort;
        }
    }




    private void ClosedDoorExternalVisibility()
    {
        Stack<Transform> stack = new Stack<Transform>();
        stack.Push(this.gameObject.transform);

        while (stack.Count > 0)
        {
            Transform current = stack.Pop();

            // Tag the children of the current parent game object with the specified tag
            foreach (Transform child in current)
            {
                //  if (child.gameObject.CompareTag("ClosedDoor") || child.GetComponent<ThresholdColliderScript>() != null)
                
                // if (child.GetComponent<ThresholdColliderScript>() != null &&
                //     child.GetComponent<ThresholdColliderScript>().itsAnEntrnceOrExt &&
                //     CompareLayer(child, "Default"))
                // {
                //     //need to resolve this to access the list with the for loop, in order to make certain alphas correct
                //     SetTreeAlpha(child.FindSiblingWithTag("ClosedDoor"), 1);
                // }

            }

            // Push the children of the current parent game object to the stack
            for (int i = 0; i < current.childCount; i++)
            {
                stack.Push(current.GetChild(i));
            }
        } 
   
    }

    private GameObject FindSiblingWithTag(string tag) 
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

    private void TagChildrenOfTaggedParents(string tag)
    {
        // for (int i = 0; i < spriteList.Count; i++)
        // {
            Transform tr = this.gameObject.transform;

            Stack<Transform> stack = new Stack<Transform>();
            stack.Push(tr);



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
                for (int j = 0; j < current.childCount; j++)
                {
                    stack.Push(current.GetChild(j));
                }
            }              
              
        // }   
    }

    static void SetSortingLayer(GameObject gameObject, string sortingLayerName)
    {
        if(gameObject.GetComponent<SpriteRenderer>() != null) 
        {
          gameObject.GetComponent<SpriteRenderer>().sortingLayerName = sortingLayerName;
        }
        foreach (Transform child in gameObject.transform)
        {
            BuildingScript.SetSortingLayer(child.gameObject, sortingLayerName);
        }    
    }


}
