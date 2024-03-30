using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{
    public GameObject innerBuilding;
    public GameObject outerBuilding;
    [SerializeField] public List<GameObject> gameObjectsToShowWhileOutside = new List<GameObject>();
    [SerializeField] public List<BuildingScript> buildingsWithBalconies = new List<BuildingScript>();
    PlayerMovement playerMovement;
    [SerializeField] GameObject Player;
    private List<GameObject> gameObjectsToShowWhileOutsideSpriteList = new List<GameObject>();
    // private List<GameObject> gameObjectsToHideWhileInsideSpriteList = new List<GameObject>();
    private List<Color> gameObjectsToShowWhileOutsideColorList = new List<Color>();
    // private List<Color> gameObjectsToHideWhileInsideColorList = new List<Color>();
    private List<GameObject> innerBuildingSpriteList = new List<GameObject>();
    public List<GameObject> outerBuildingSpriteList = new List<GameObject>();
    private List<Color> innerBuildingInitialColorList = new List<Color>();
    public List<Color> outerBuildingInitialColorList = new List<Color>();
    private List<IsoSpriteSorting> innerSpriteSortingScriptObj = new List<IsoSpriteSorting>();
    private List<IsoSpriteSorting> outerSpriteSortingScriptObj = new List<IsoSpriteSorting>();

    private Coroutine innerBuildingFadeCoroutine;
    private Coroutine outerBuildingFadeCoroutine;
    private Coroutine gameObjectsToHideWhileInsideFadeCoroutine;
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
        // GetSpritesAndAddToLists(GameObject obj, List<GameObject> spriteList, List<GameObject> excludeList, List<Color> colorList)
        GetSpritesAndAddToLists(innerBuilding, innerBuildingSpriteList, gameObjectsToShowWhileOutside, innerBuildingInitialColorList);
        GetSpritesAndAddToLists(outerBuilding, outerBuildingSpriteList, new List<GameObject>(), outerBuildingInitialColorList);
        foreach(GameObject obj in gameObjectsToShowWhileOutside) {
            GetSpritesAndAddToLists(obj, gameObjectsToShowWhileOutsideSpriteList, new List<GameObject>(), gameObjectsToShowWhileOutsideColorList);
        }
        GetIsoSpriteSortComponentsAndAddToLists(innerBuilding, innerSpriteSortingScriptObj, gameObjectsToShowWhileOutside);
        GetIsoSpriteSortComponentsAndAddToLists(outerBuilding, outerSpriteSortingScriptObj, new List<GameObject>());
        TagChildrenOfTaggedParents("OpenDoor");
        TagChildrenOfTaggedParents("ClosedDoor");
        TagChildrenOfTaggedParents("AlphaZeroEntExt");
        this.ExitBuilding(0f, 0f, false);
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
        if(this.gameObjectsToHideWhileInsideFadeCoroutine != null)
        {
            StopCoroutine(this.gameObjectsToHideWhileInsideFadeCoroutine);
        }

        gameObjectsToHideWhileInsideFadeCoroutine = StartCoroutine(SetAlphaOfOuterItemsThatRNotDefaultSpriteLayer(false, buildingsWithBalconies));
        
        backdropFadeCoroutine = StartCoroutine(FadeInnerBuildingBackdrop(false, 0f, GameObject.FindGameObjectWithTag("InnerBuildingBackdrop"), 1f));

                // FadeThenSetDontSort
        //                                     ( 
            //                                  bool exitingBuilding,
        //                                     bool fadeFirst,
        //                                     List<IsoSpriteSorting> issList, 
        //                                     bool setDontSort, bool shouldWait, 
        //                                     float? waitTime, 
        //                                     bool behindBuilding, 
        //                                     List<GameObject> spriteList, 
        //                                     List<Color> colorList, 
        //                                     float? alpha, 
        //                                     string[] tagsToExclude = null

        innerBuildingFadeCoroutine = StartCoroutine(FadeThenSetDontSort(false, false, innerSpriteSortingScriptObj, false, false, 0f, false, innerBuildingSpriteList, innerBuildingInitialColorList, null, tagsToExludeEntExt));
        outerBuildingFadeCoroutine = StartCoroutine(FadeThenSetDontSort(false, true, outerSpriteSortingScriptObj, true, false, 0f, false, outerBuildingSpriteList, null, 0f));
    }
    public void ExitBuilding(float waitTimeInside, float waitTimeOutside, bool exitingFromBehindAlreadyOutside)
    {
        if(this.innerBuildingFadeCoroutine != null)
        {
            StopCoroutine(this.innerBuildingFadeCoroutine);
        }
        if(this.outerBuildingFadeCoroutine != null)
        {
            StopCoroutine(this.outerBuildingFadeCoroutine);
        }
        if (exitingFromBehindAlreadyOutside)
        {
            // do nutting
        }
        else if(this.backdropFadeCoroutine != null)
        {
            StopCoroutine(this.backdropFadeCoroutine);
        }
        if(this.gameObjectsToHideWhileInsideFadeCoroutine != null)
        {
            StopCoroutine(this.gameObjectsToHideWhileInsideFadeCoroutine);
        }

         // FadeThenSetDontSort
        //                                     ( 
            //                                  bool exitingBuilding,
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
        gameObjectsToHideWhileInsideFadeCoroutine = StartCoroutine(SetAlphaOfOuterItemsThatRNotDefaultSpriteLayer(true, buildingsWithBalconies));

        backdropFadeCoroutine = StartCoroutine(FadeInnerBuildingBackdrop(true, 0.3f, GameObject.FindGameObjectWithTag("InnerBuildingBackdrop"), 0f));

        innerBuildingFadeCoroutine = StartCoroutine(FadeThenSetDontSort(true, true, innerSpriteSortingScriptObj, true, true, waitTimeInside, false, innerBuildingSpriteList, null, 0f));
        outerBuildingFadeCoroutine = StartCoroutine(FadeThenSetDontSort(true, false, outerSpriteSortingScriptObj, false, true, waitTimeOutside, false, outerBuildingSpriteList, outerBuildingInitialColorList, null, tagsToExludeEntExt));

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
        if(this.gameObjectsToHideWhileInsideFadeCoroutine != null)
        {
            StopCoroutine(this.gameObjectsToHideWhileInsideFadeCoroutine);
        }
        // FadeThenSetDontSort
        //                                     (
                                                //bool exitingBuilding 
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

        gameObjectsToHideWhileInsideFadeCoroutine = StartCoroutine(SetAlphaOfOuterItemsThatRNotDefaultSpriteLayer(true, buildingsWithBalconies));

        backdropFadeCoroutine = StartCoroutine(FadeInnerBuildingBackdrop(true, 0.3f, GameObject.FindGameObjectWithTag("InnerBuildingBackdrop"), 0f));

        innerBuildingFadeCoroutine = StartCoroutine(FadeThenSetDontSort(true, true, innerSpriteSortingScriptObj, true, true, 0.3f, true, innerBuildingSpriteList, null, 0f));
        outerBuildingFadeCoroutine = StartCoroutine(FadeThenSetDontSort(true, false, outerSpriteSortingScriptObj, false, true, 0.3f, true, outerBuildingSpriteList, null, 0.35f, tagsToExludeEntExt));

        // innerBuildingFadeCoroutine = StartCoroutine(BuildingSequence(true, 0.3f, true, innerBuildingSpriteList, null, 0f));
        // outerBuildingFadeCoroutine = StartCoroutine(BuildingSequence(true, 0.3f, true, outerBuildingSpriteList, null, 0.35f, tagsToExludeEntExt));
    }




    // coroutine for fading things in a sequence
    // public IEnumerator BuildingSequence(bool shouldWait, float? waitTime, bool behindBuilding, List<GameObject> spriteList, List<Color> colorList, float? alpha, string[] tagsToExclude = null)
    // {
    //     if (shouldWait && waitTime.HasValue) // if Exiting the building
    //     {
    //         yield return new WaitForSeconds(waitTime.Value);

    //         for (int i = 0; i < innerBuildingSpriteList.Count; i++)
    //         {                       
    //             if (innerBuildingSpriteList[i].GetComponent<SpriteRenderer>().sortingLayerName == "Level0")
    //             {
    //                 SetTreeSortingLayer(innerBuildingSpriteList[i], "Default");
    //             }
    //         }
    //         for (int i = 0; i < gameObjectsToShowWhileOutsideSpriteList.Count; i++)
    //         {            
    //                 if (gameObjectsToShowWhileOutsideSpriteList[i].GetComponent<SpriteRenderer>().sortingLayerName == "Level0")
    //                 {
    //                     SetTreeSortingLayer(gameObjectsToShowWhileOutsideSpriteList[i], "Default");
    //                 }
    //         }
    //         if (Player.GetComponent<SpriteRenderer>().sortingLayerName == "Level0") // if the player exits building from the ground flow
    //         {
    //             SetTreeSortingLayer(Player, "Default");
    //         }
    //     }
    //     else // if entering the building
    //     {
    //         for (int i = 0; i < innerBuildingSpriteList.Count; i++)
    //         {                       
    //             if (innerBuildingSpriteList[i].GetComponent<SpriteRenderer>().sortingLayerName == "Default")
    //             {
    //                 SetTreeSortingLayer(innerBuildingSpriteList[i], "Level0");
    //             }
    //         }
    //         for (int i = 0; i < gameObjectsToShowWhileOutsideSpriteList.Count; i++)
    //         {                      
    //                 if (gameObjectsToShowWhileOutsideSpriteList[i].GetComponent<SpriteRenderer>().sortingLayerName == "Default")
    //                 {
    //                     SetTreeSortingLayer(gameObjectsToShowWhileOutsideSpriteList[i], "Level0");
    //                 }
    //         }
    //         if (Player.GetComponent<SpriteRenderer>().sortingLayerName == "Default") // if the player enters building at the ground flow
    //         {
    //             SetTreeSortingLayer(Player, "Level0");
    //         }
    //     }

    //     for (float t = 0.0f; t < 1; t += Time.deltaTime) 
    //     {        
    //         for (int i = 0; i < spriteList.Count; i++)
    //         {
    //             SpriteRenderer sr = spriteList[i].GetComponent<SpriteRenderer>();
    //             Transform tr = spriteList[i].transform;

    //             if (sr != null && (tagsToExclude == null || !Array.Exists(tagsToExclude, element => element == spriteList[i].tag)))        
    //             {
    //                 if(colorList == null)
    //                 {
    //                     if (alpha.HasValue)
    //                     {
    //                         float nextAlpha = Mathf.Lerp(sr.color.a, alpha.Value, t * 1f);
    //                         sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, nextAlpha);
    //                     }
    //                 }
    //                 else
    //                 {
    //                     float nextAlpha = Mathf.Lerp(sr.color.a, colorList[i].a, t * 1f);
    //                     sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, nextAlpha);
    //                     // sr.color = colorList[i];
    //                 }    
    //             }  
    //         }   
    //         for (int i = 0; i < gameObjectsToShowWhileOutsideSpriteList.Count; i++)
    //         {
    //             SpriteRenderer sr = gameObjectsToShowWhileOutsideSpriteList[i].GetComponent<SpriteRenderer>();

    //             if (behindBuilding && !gameObjectsToShowWhileOutsideSpriteList[i].CompareTag("OpenDoor"))
    //             {
    //                 float nextAlpha = Mathf.Lerp(sr.color.a, gameObjectsToShowWhileOutsideColorList[i].a, t * 1f);
    //                 sr.color = gameObjectsToShowWhileOutsideColorList[i];
    //             }     
    //         }
    //         yield return null;    
    //     }

    // }
    // coroutine for fading things in a sequence
    public IEnumerator BuildingSequence(bool exitingBuilding, bool shouldWait, float? waitTime, bool behindBuilding, List<GameObject> spriteList, List<Color> colorList, float? alpha, string[] tagsToExclude = null)
    {
        float timer = 0.0f;
        if (shouldWait && waitTime.HasValue) // if Exiting the building
        {
            while (timer < waitTime.Value)
            {
                timer += Time.deltaTime;
                yield return null;
            }
        }
        if (exitingBuilding)
        {
            for (int i = 0; i < innerBuildingSpriteList.Count; i++)
            {     
                SetTreeSortingLayer(innerBuildingSpriteList[i], "Default");
            }
            for (int i = 0; i < gameObjectsToShowWhileOutsideSpriteList.Count; i++)
            {            
                    if (gameObjectsToShowWhileOutsideSpriteList[i].GetComponent<SpriteRenderer>().sortingLayerName == "Level0")
                    {
                        SetTreeSortingLayer(gameObjectsToShowWhileOutsideSpriteList[i], "Default");
                    }
            }
            if (Player.GetComponent<SpriteRenderer>().sortingLayerName == "Level0") // if the player exits building from the ground flow
            {
                SetTreeSortingLayer(Player, "Default");
            }
            // Your code after the wait time elapses
        }
        else// if entering the building
        {
            for (int i = 0; i < innerBuildingSpriteList.Count; i++)
            {                                       
                if (LayerMask.LayerToName(innerBuildingSpriteList[i].layer) == "Default")
                {
                    innerBuildingSpriteList[i].GetComponent<SpriteRenderer>().sortingLayerName = "Level0";
                }
                else
                {
                    innerBuildingSpriteList[i].GetComponent<SpriteRenderer>().sortingLayerName = LayerMask.LayerToName(innerBuildingSpriteList[i].layer);
                }
            }
            for (int i = 0; i < gameObjectsToShowWhileOutsideSpriteList.Count; i++)
            {                      
                    if (gameObjectsToShowWhileOutsideSpriteList[i].GetComponent<SpriteRenderer>().sortingLayerName == "Default")
                    {
                        SetTreeSortingLayer(gameObjectsToShowWhileOutsideSpriteList[i], "Level0");
                    }
            }
            if (Player.GetComponent<SpriteRenderer>().sortingLayerName == "Default") // if the player enters building at the ground flow
            {
                SetTreeSortingLayer(Player, "Level0");
            }
        }
        // fade part
        // for (float t = 0.0f; t < 1; t += Time.deltaTime*3f) 
        // {        
        //     for (int i = 0; i < spriteList.Count; i++)
        //     {
        //         SpriteRenderer sr = spriteList[i].GetComponent<SpriteRenderer>();
        //         Transform tr = spriteList[i].transform;

        //         if (sr != null && (tagsToExclude == null || !Array.Exists(tagsToExclude, element => element == spriteList[i].tag)))        
        //         {
        //             if(colorList == null)
        //             {
        //                 if (alpha.HasValue)
        //                 {
        //                     float nextAlpha = Mathf.Lerp(sr.color.a, alpha.Value, t * 1f);
        //                     sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, nextAlpha);
        //                 }
        //             }
        //             else
        //             {
        //                 float nextAlpha = Mathf.Lerp(sr.color.a, colorList[i].a, t * 1f);
        //                 sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, nextAlpha);
        //                 // sr.color = colorList[i];
        //             }    
        //         }  
        //     }   
        //     for (int i = 0; i < gameObjectsToShowWhileOutsideSpriteList.Count; i++)
        //     {
        //         SpriteRenderer sr = gameObjectsToShowWhileOutsideSpriteList[i].GetComponent<SpriteRenderer>();

        //         if (behindBuilding && !gameObjectsToShowWhileOutsideSpriteList[i].CompareTag("OpenDoor"))
        //         {
        //             float nextAlpha = Mathf.Lerp(sr.color.a, gameObjectsToShowWhileOutsideColorList[i].a, t * 1f);
        //             sr.color = gameObjectsToShowWhileOutsideColorList[i];
        //         }     
        //     }        
        // }
        

        //instant alpha
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
                        // Set alpha value instantly without lerping
                        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha.Value);
                    }
                }
                else if (playerMovement.playerIsOutside) 
                {
                    // // Set alpha value from the color list instantly without lerping
                    // sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, colorList[i].a);
                    // // Alternatively, you can directly assign the color from the list
                    sr.color = colorList[i];
                } 
              

            }  
        }   

        for (int i = 0; i < gameObjectsToShowWhileOutsideSpriteList.Count; i++)
        {
            SpriteRenderer sr = gameObjectsToShowWhileOutsideSpriteList[i].GetComponent<SpriteRenderer>();

            if (behindBuilding && !gameObjectsToShowWhileOutsideSpriteList[i].CompareTag("OpenDoor"))
            {
                // Set alpha value from the color list instantly without lerping
                sr.color = gameObjectsToShowWhileOutsideColorList[i];
            }     
        }

        yield return null;
    }

    public IEnumerator SetAlphaOfOuterItemsThatRNotDefaultSpriteLayer(bool exitingBuilding, List<BuildingScript> buildingList)
    // this function is for things such as balconies that show up over the backdrop. This function set's their alpha to 0 and back again upon building exit
    {
        if(exitingBuilding)
        {
            yield return new WaitForSeconds(0.3f);

            for (int i = 0; i < buildingsWithBalconies.Count; i++)
            { 
                // gameObjectsToHideWhileInsideFadeCoroutine = StartCoroutine(treeFade());

                for (int j = 0; j < buildingsWithBalconies[i].outerBuildingSpriteList.Count; j++)
                {
                    // Get the SpriteRenderer component
                    SpriteRenderer spriteRenderer = buildingsWithBalconies[i].outerBuildingSpriteList[j].GetComponent<SpriteRenderer>();
                    
                    // Check if the sorting layer name is not "Default"
                    if (spriteRenderer.sortingLayerName != "Default")
                    {
                        // Get the initial color from the outerBuildingInitialColorList
                        Color initialColor = buildingsWithBalconies[i].outerBuildingInitialColorList[j];

                        // Set the alpha value from the initial color
                        Color newColor = spriteRenderer.color;
                        newColor.a = initialColor.a;
                        
                        // Assign the modified color back to the sprite's color
                        spriteRenderer.color = newColor;
                    }
                }
                for (int j = 0; j < buildingsWithBalconies[i].gameObjectsToShowWhileOutsideSpriteList.Count; j++)
                {
                    // Get the SpriteRenderer component
                    SpriteRenderer spriteRenderer = buildingsWithBalconies[i].gameObjectsToShowWhileOutsideSpriteList[j].GetComponent<SpriteRenderer>();

                    // Create a new color with modified alpha value
                    Color newColor = spriteRenderer.color;
                    newColor.a = buildingsWithBalconies[i].gameObjectsToShowWhileOutsideColorList[j].a;
                    
                    // Assign the modified color back to the sprite's color
                    spriteRenderer.color = newColor;
                }

            }
        }
        else // if entering set the alphas to 0
        {
            yield return new WaitForSeconds(0.1f);

            for (int i = 0; i < buildingsWithBalconies.Count; i++)
            { 
                // gameObjectsToHideWhileInsideFadeCoroutine = StartCoroutine(treeFade());

                for (int j = 0; j < buildingsWithBalconies[i].outerBuildingSpriteList.Count; j++)
                {
                    // Get the SpriteRenderer component
                    SpriteRenderer spriteRenderer = buildingsWithBalconies[i].outerBuildingSpriteList[j].GetComponent<SpriteRenderer>();
                    
                    // Check if the sorting layer name is not "Default"
                    if (spriteRenderer.sortingLayerName != "Default")
                    {
                        // Get the initial color from the outerBuildingInitialColorList
                        Color initialColor = buildingsWithBalconies[i].outerBuildingInitialColorList[j];

                        // Set the alpha value to 0
                        Color newColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0);
                        
                        // Assign the modified color back to the sprite's color
                        spriteRenderer.color = newColor;
                    }
                }
                for (int j = 0; j < buildingsWithBalconies[i].gameObjectsToShowWhileOutsideSpriteList.Count; j++)
                {
                    // Get the SpriteRenderer component
                    SpriteRenderer spriteRenderer = buildingsWithBalconies[i].gameObjectsToShowWhileOutsideSpriteList[j].GetComponent<SpriteRenderer>();

                    // Create a new color with modified alpha value
                    Color newColor = spriteRenderer.color;
                    newColor.a = 0;
                    
                    // Assign the modified color back to the sprite's color
                        spriteRenderer.color = newColor;
                }

            }
        }
        yield return null;
    }

    public IEnumerator FadeInnerBuildingBackdrop(bool shouldWait, float? waitTime, GameObject sprite, float alpha)
    {
        if (shouldWait && waitTime.HasValue)
        {
            yield return new WaitForSeconds(waitTime.Value);
        }

        SpriteRenderer sr = sprite.GetComponent<SpriteRenderer>();

        // Calculate duration based on the alpha difference
        float alphaDiff = Mathf.Abs(sr.color.a - alpha);
        float duration = alphaDiff; // Assuming alpha ranges from 0 to 1
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

    // public IEnumerator treeFade(
    //   GameObject obj,
    //   float fadeTo,
    //   float fadeSpeed) 
    // {
    //     float fadeFrom = obj.GetComponent<SpriteRenderer>().color.a;

    //     for (float t = 0.0f; t < 1f; t += Time.deltaTime) 
    //     {
    //         float currentAlpha = Mathf.Lerp(fadeFrom, fadeTo, t * fadeSpeed);
    //         SetTreeAlpha(obj, currentAlpha);
    //         yield return null;
    //     }
    // }


    private IEnumerator FadeThenSetDontSort
                                            ( 
                                            bool exitingBuilding,    
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
            yield return BuildingSequence(exitingBuilding, shouldWait, waitTime.Value, behindBuilding, spriteList, colorList, alpha, tagsToExclude);
            SetDontSort(issList, setDontSort);
        }
        else
        {
            SetDontSort(issList, setDontSort);
            yield return BuildingSequence(exitingBuilding, shouldWait, waitTime.Value, behindBuilding, spriteList, colorList, alpha, tagsToExclude);
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

    static void SetTreeSortingLayer(GameObject gameObject, string sortingLayerName)
    {
        if(gameObject.GetComponent<SpriteRenderer>() != null) 
        {
          gameObject.GetComponent<SpriteRenderer>().sortingLayerName = sortingLayerName;
        }
        foreach (Transform child in gameObject.transform)
        {
            BuildingScript.SetTreeSortingLayer(child.gameObject, sortingLayerName);
        }    
    }

    public void SetTreeAlpha(GameObject treeNode, float alpha)
    {
        if (treeNode == null)
        {
            return; // TODO: remove this
        }
        SpriteRenderer sr = treeNode.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
        }
        foreach (Transform child in treeNode.transform)
        {
            SetTreeAlpha(child.gameObject, alpha);
        }
    }


}