using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{
    public List<BuildingScript> allOtherBuildings = new List<BuildingScript>();

    [HideInInspector] public CameraMovement cameraMovement;

    public GameObject innerBuilding;
    public GameObject outerBuilding;
    [SerializeField] public List<GameObject> gameObjectsToShowWhileOutside = new List<GameObject>();
    BalconyManager balconyManager;
    CharacterMovement myCharacterMovement;
    [SerializeField] GameObject Player;
    [HideInInspector] public List<GameObject> gameObjectsToShowWhileOutsideSpriteList = new List<GameObject>();
    // private List<GameObject> gameObjectsToHideWhileInsideSpriteList = new List<GameObject>();
    [HideInInspector] public List<Color> gameObjectsToShowWhileOutsideColorList = new List<Color>();
    // private List<Color> gameObjectsToHideWhileInsideColorList = new List<Color>();
    private List<GameObject> innerBuildingSpriteList = new List<GameObject>();
    public List<GameObject> outerBuildingSpriteList = new List<GameObject>();
    private List<Color> innerBuildingInitialColorList = new List<Color>();
    public List<Color> outerBuildingInitialColorList = new List<Color>();
    [HideInInspector] public List<IsoSpriteSorting> innerSpriteSortingScriptObj = new List<IsoSpriteSorting>();
    [HideInInspector] public List<IsoSpriteSorting> outerSpriteSortingScriptObj = new List<IsoSpriteSorting>();

    private Coroutine innerBuildingFadeCoroutine;
    private Coroutine outerBuildingFadeCoroutine;
    private Coroutine gameObjectsToHideWhileInsideFadeCoroutine;
    private Coroutine backdropFadeCoroutine;
    private Coroutine setDontSortForAllOuterBuildingsCoro;
    private string[] tagsToExludeEntExt = { "OpenDoor", "AlphaZeroEntExt" };

    public List<RoomScript> roomScripts; 
    public List<LevelScript> levelScripts; 

    public List<GameObject> npcListForBuilding = new List<GameObject>();
    [HideInInspector] public List<GameObject> npcSpriteListForBuilding = new List<GameObject>();
    [HideInInspector] public List<Color> npcColorListForBuilding = new List<Color>();


    SoundtrackScript soundtrackScript;

    public void Awake()
    { 
        Player = GameObject.FindGameObjectWithTag("Player");
        myCharacterMovement = Player.GetComponent<CharacterMovement>(); 
        balconyManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<BalconyManager>(); 

        soundtrackScript = GameObject.FindGameObjectWithTag("SoundtrackScript").GetComponent<SoundtrackScript>();

        foreach(GameObject obj in gameObjectsToShowWhileOutside) {
            GetSpritesAndAddToLists(obj, gameObjectsToShowWhileOutsideSpriteList, new List<GameObject>(), gameObjectsToShowWhileOutsideColorList);
        }

        GetSpritesAndAddToLists(innerBuilding, innerBuildingSpriteList, gameObjectsToShowWhileOutside, innerBuildingInitialColorList);
        GetSpritesAndAddToLists(outerBuilding, outerBuildingSpriteList, new List<GameObject>(), outerBuildingInitialColorList);


     
        GetIsoSpriteSortComponentsAndAddToLists(innerBuilding, innerSpriteSortingScriptObj, gameObjectsToShowWhileOutside);
        GetIsoSpriteSortComponentsAndAddToLists(outerBuilding, outerSpriteSortingScriptObj, new List<GameObject>());

        FindChildrenByRoomScriptComponent();
        FindChildrenByLevelScriptComponent();
        
        TagChildrenOfTaggedParents("OpenDoor");
        TagChildrenOfTaggedParents("ClosedDoor");
        TagChildrenOfTaggedParents("AlphaZeroEntExt");

        // Find all objects in the scene with the BuildingScript component
        BuildingScript[] allBuildings = FindObjectsOfType<BuildingScript>();
        
        // Add all other BuildingScripts to the list, excluding this one
        foreach (BuildingScript building in allBuildings)
        {
            if (building != this)
            {
                allOtherBuildings.Add(building);
            }
        }

    }  

    // Start is called before the first frame update
    void Start() 
    {
        StartCoroutine(LateStartCoroForNonPlayerCharacters());


        innerBuilding.SetActive(true);
        outerBuilding.SetActive(true);
        // GetSpritesAndAddToLists(GameObject obj, List<GameObject> spriteList, List<GameObject> excludeList, List<Color> colorList)
       
        cameraMovement = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>();

        this.ExitBuilding(0f, 0f, false);

    }

    public void EnterBuilding()
    {
        cameraMovement.currentBuilding = this;

        soundtrackScript.FadeOutIn(soundtrackScript.track1, soundtrackScript.track2);

        // foreach(GameObject npc in npcListForBuilding)
        // {
        //     npc.GetComponent<CharacterCustomization>().ResetAppearance();
        //     // npc.GetComponent<CharacterAnimation>().SetAlphaToZeroForAllSprites();
        //     // npc.GetComponent<CharacterAnimation>().SetTreeSortingLayer(npc.gameObject, "Level0");
        // }


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
        if(this.setDontSortForAllOuterBuildingsCoro != null)
        {
            StopCoroutine(this.setDontSortForAllOuterBuildingsCoro);
        }

        gameObjectsToHideWhileInsideFadeCoroutine = StartCoroutine(SetAlphaOfOuterItemsThatRNotDefaultSpriteLayer(false));
        
        backdropFadeCoroutine = StartCoroutine(FadeInnerBuildingBackdrop(false, 0f, GameObject.FindGameObjectWithTag("InnerBuildingBackdrop"), 1f));

                // FadeThenSetDontSort parameters
        //                                     ( 
            //                                  bool exitingBuilding,
          //                                    bool exitingBehindBuilding,
        //                                     bool fadeFirst,
        //                                     List<IsoSpriteSorting> issList, 
        //                                     bool setDontSort, bool shouldWait, 
        //                                     float? waitTime, 
        //                                     bool behindBuilding, 
        //                                     List<GameObject> spriteList, 
        //                                     List<Color> colorList, 
        //                                     float? alpha, 
        //                                     string[] tagsToExclude = null

        innerBuildingFadeCoroutine = StartCoroutine(FadeThenSetDontSort(false, false, false, innerSpriteSortingScriptObj, false, false, 0f, false, innerBuildingSpriteList, innerBuildingInitialColorList, null, tagsToExludeEntExt));
        outerBuildingFadeCoroutine = StartCoroutine(FadeThenSetDontSort(false, false, true, outerSpriteSortingScriptObj, true, false, 0f, false, outerBuildingSpriteList, null, 0f));
        setDontSortForAllOuterBuildingsCoro = StartCoroutine(SetDontSortForAllOtherOuterBuildings(true, true));

    }
    public void ExitBuilding(float waitTimeInside, float waitTimeOutside, bool exitingFromBehindAlreadyOutside)
    {
        if(cameraMovement.currentBuilding != null)
            cameraMovement.currentBuilding = null;
        if(cameraMovement.currentBuilding != null)
            cameraMovement.currentLevel = null;

        if(soundtrackScript != null)
        {
            soundtrackScript.FadeOutIn(soundtrackScript.track2, soundtrackScript.track1);
        }

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

        if(this.setDontSortForAllOuterBuildingsCoro != null)
        {
            StopCoroutine(this.setDontSortForAllOuterBuildingsCoro);
        }

         // FadeThenSetDontSort params
        //                                     ( 
            //                                  bool exitingBuilding,
          //                                    bool exitingBehindBuilding,
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
        gameObjectsToHideWhileInsideFadeCoroutine = StartCoroutine(SetAlphaOfOuterItemsThatRNotDefaultSpriteLayer(true));

        backdropFadeCoroutine = StartCoroutine(FadeInnerBuildingBackdrop(true, 0.3f, GameObject.FindGameObjectWithTag("InnerBuildingBackdrop"), 0f));

        innerBuildingFadeCoroutine = StartCoroutine(FadeThenSetDontSort(true, false, true, innerSpriteSortingScriptObj, true, true, waitTimeInside, false, innerBuildingSpriteList, null, 0f));
        outerBuildingFadeCoroutine = StartCoroutine(FadeThenSetDontSort(true, false, false, outerSpriteSortingScriptObj, false, true, waitTimeOutside, false, outerBuildingSpriteList, outerBuildingInitialColorList, null, tagsToExludeEntExt));
        setDontSortForAllOuterBuildingsCoro = StartCoroutine(SetDontSortForAllOtherOuterBuildings(false, false));

        // cameraMovement.currentRoom = null;
        // cameraMovement.currentLevel = null;
        // cameraMovement.currentBuilding = null;
        

    }
    public void GoBehindBuilding()
    {
        cameraMovement.currentBuilding = null;
        cameraMovement.currentLevel = null;

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
        if(this.setDontSortForAllOuterBuildingsCoro != null)
        {
            StopCoroutine(this.setDontSortForAllOuterBuildingsCoro);
        }
        // FadeThenSetDontSort params
                //                                     ( 
                    //                                  bool exitingBuilding,
                //                                    bool exitingBehindBuilding,
                //                                     bool fadeFirst,
                //                                     List<IsoSpriteSorting> issList, 
                //                                     bool setDontSort, bool shouldWait, 
                //                                     float? waitTime, 
                //                                     bool behindBuilding, 
                //                                     List<GameObject> spriteList, 
                //                                     List<Color> colorList, 
                //                                     float? alpha, 
                //                                     string[] tagsToExclude = null
                //                                     )        gameObjectsToHideWhileInsideFadeCoroutine = StartCoroutine(SetAlphaOfOuterItemsThatRNotDefaultSpriteLayer(true, buildingsWithBalconies));

        backdropFadeCoroutine = StartCoroutine(FadeInnerBuildingBackdrop(true, 0.3f, GameObject.FindGameObjectWithTag("InnerBuildingBackdrop"), 0f));

        innerBuildingFadeCoroutine = StartCoroutine(FadeThenSetDontSort(false, true, true, innerSpriteSortingScriptObj, true, true, 0.3f, true, innerBuildingSpriteList, null, 0f));
        outerBuildingFadeCoroutine = StartCoroutine(FadeThenSetDontSort(false, true, false, outerSpriteSortingScriptObj, false, true, 0.3f, true, outerBuildingSpriteList, null, 0.35f, tagsToExludeEntExt));
        setDontSortForAllOuterBuildingsCoro = StartCoroutine(SetDontSortForAllOtherOuterBuildings(false, false));
        // innerBuildingFadeCoroutine = StartCoroutine(BuildingSequence(true, 0.3f, true, innerBuildingSpriteList, null, 0f));
        // outerBuildingFadeCoroutine = StartCoroutine(BuildingSequence(true, 0.3f, true, outerBuildingSpriteList, null, 0.35f, tagsToExludeEntExt));
        
        // cameraMovement.currentRoom = null;
        // cameraMovement.currentLevel = null;
        // cameraMovement.currentBuilding = null;
    }


    public IEnumerator BuildingSequence(bool exitingBuilding, bool exitingBehindBuilding, bool shouldWait, float? waitTime, bool behindBuilding, List<GameObject> spriteList, List<Color> colorList, float? alpha, string[] tagsToExclude = null)
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
            // for (int i = 0; i < allOtherBuildings.Count; i++)
            // {     
            //     // allOtherBuildings[i].SetDontSort(allOtherBuildings[i].innerSpriteSortingScriptObj, false);
            //     allOtherBuildings[i].SetDontSort(allOtherBuildings[i].outerSpriteSortingScriptObj, false);
            // }

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
            if (Player.transform.Find("head").GetComponent<SpriteRenderer>().sortingLayerName == "Level0") // if the player exits building from the ground floor
            {
                SetTreeSortingLayer(Player, "Default");
            }
            for (int i = 0; i < gameObjectsToShowWhileOutsideSpriteList.Count; i++)
            {
                if (gameObjectsToShowWhileOutsideSpriteList[i].CompareTag("OpenDoor") 
                || gameObjectsToShowWhileOutsideSpriteList[i].CompareTag("AlphaZeroEntExt"))        
                {
                    SetTreeAlpha(gameObjectsToShowWhileOutsideSpriteList[i], 0);
                }
            }  

            for (int i = 0; i < npcSpriteListForBuilding.Count; i++)
            {  
                SetTreeAlpha(npcSpriteListForBuilding[i], 0f);
                if(npcSpriteListForBuilding[i].GetComponent<SpriteRenderer>().sortingLayerName == "Level0")
                {
                    SetTreeSortingLayer(npcSpriteListForBuilding[i], "Default");
                } 
            } 
            for (int i = 0; i < npcListForBuilding.Count; i++)
            {
                npcListForBuilding[i].GetComponent<IsoSpriteSorting>().dontSort = true;
                // if (npcListForBuilding[i].GetComponent<CharacterMovement>().npcRandomMovementCoro != null)
                // {
                //     npcListForBuilding[i].GetComponent<CharacterMovement>().StopCoroutine(npcListForBuilding[i].GetComponent<CharacterMovement>().npcRandomMovementCoro);
                //     npcListForBuilding[i].GetComponent<CharacterMovement>().change = Vector3.zero;
                //     npcListForBuilding[i].GetComponent<CharacterMovement>().npcRandomMovementCoro = null;
                // }
            }

            // cameraMovement.currentRoom = null;
            // cameraMovement.currentLevel = null;
            // cameraMovement.currentBuilding = null;

            // Your code after the wait time elapses
        }
        else if (exitingBehindBuilding)
        {
            // for (int i = 0; i < allOtherBuildings.Count; i++)
            // {     
            //     // allOtherBuildings[i].SetDontSort(allOtherBuildings[i].innerSpriteSortingScriptObj, false);
            //     allOtherBuildings[i].SetDontSort(allOtherBuildings[i].outerSpriteSortingScriptObj, false);            
            // }

            for (int i = 0; i < npcSpriteListForBuilding.Count; i++)
            {  
                SetTreeAlpha(npcSpriteListForBuilding[i], 0f); 
                if(npcSpriteListForBuilding[i].GetComponent<SpriteRenderer>().sortingLayerName == "Level0")
                {
                    SetTreeSortingLayer(npcSpriteListForBuilding[i], "Default");
                }
            }  
            for (int i = 0; i < npcListForBuilding.Count; i++)
            {
                npcListForBuilding[i].GetComponent<IsoSpriteSorting>().dontSort = true;
                // if (npcListForBuilding[i].GetComponent<CharacterMovement>().npcRandomMovementCoro != null)
                // {
                //     npcListForBuilding[i].GetComponent<CharacterMovement>().StopCoroutine(npcListForBuilding[i].GetComponent<CharacterMovement>().npcRandomMovementCoro);
                //     npcListForBuilding[i].GetComponent<CharacterMovement>().change = Vector3.zero;
                //     npcListForBuilding[i].GetComponent<CharacterMovement>().npcRandomMovementCoro = null;
                // }
            } 


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
            if (Player.transform.Find("head").GetComponent<SpriteRenderer>().sortingLayerName == "Level0") // if the player exits building from the ground flow
            {
                SetTreeSortingLayer(Player, "Default");
            }
            for (int i = 0; i < gameObjectsToShowWhileOutsideSpriteList.Count; i++)
            {
                if (!gameObjectsToShowWhileOutsideSpriteList[i].CompareTag("ClosedDoor"))        
                {
                    SetTreeAlpha(gameObjectsToShowWhileOutsideSpriteList[i], 0f);
                }
            }    

            // cameraMovement.currentRoom = null;
            // cameraMovement.currentLevel = null;
            // cameraMovement.currentBuilding = null;    
            // Your code after the wait time elapses
        }



        else// if entering the building
        {
            // for (int i = 0; i < allOtherBuildings.Count; i++)
            // {     
            //     // allOtherBuildings[i].SetDontSort(allOtherBuildings[i].innerSpriteSortingScriptObj, true);
            //     allOtherBuildings[i].SetDontSort(allOtherBuildings[i].outerSpriteSortingScriptObj, true);            
            // }
            // cameraMovement.currentBuilding = this;    


            for (int i = 0; i < npcSpriteListForBuilding.Count; i++)
            {
                SetTreeAlpha(npcSpriteListForBuilding[i], npcColorListForBuilding[i].a);
                if(npcSpriteListForBuilding[i].GetComponent<SpriteRenderer>().sortingLayerName == "Default")
                {
                    SetTreeSortingLayer(npcSpriteListForBuilding[i], "Level0");
                }
            }
            for (int i = 0; i < npcListForBuilding.Count; i++)
            {
                npcListForBuilding[i].GetComponent<IsoSpriteSorting>().dontSort = false;
                // if (npcListForBuilding[i].GetComponent<CharacterMovement>().npcRandomMovementCoro == null)
                // {
                //     npcListForBuilding[i].GetComponent<CharacterMovement>().npcRandomMovementCoro = StartCoroutine(npcListForBuilding[i].GetComponent<CharacterMovement>().MoveCharacterRandomly());
                // }
            }


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
            if (Player.transform.Find("head").GetComponent<SpriteRenderer>().sortingLayerName == "Default") // if the player enters building at the ground flow
            {
                SetTreeSortingLayer(Player, "Level0");
            }
            for (int i = 0; i < gameObjectsToShowWhileOutsideSpriteList.Count; i++)
            {
                if (gameObjectsToShowWhileOutsideSpriteList[i].CompareTag("OpenDoor") 
                || gameObjectsToShowWhileOutsideSpriteList[i].CompareTag("AlphaZeroEntExt"))        
                {
                    SetTreeAlpha(gameObjectsToShowWhileOutsideSpriteList[i], 0);
                }
                else
                {
                    SetTreeAlpha(gameObjectsToShowWhileOutsideSpriteList[i], gameObjectsToShowWhileOutsideColorList[i].a);
                }
            } 

 
        }

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
                else if (myCharacterMovement.playerIsOutside) 
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

            if (behindBuilding && gameObjectsToShowWhileOutsideSpriteList[i].CompareTag("ClosedDoor"))
            {
                // Set alpha value from the color list instantly without lerping
                sr.color = gameObjectsToShowWhileOutsideColorList[i];
            }     
        }
        yield return null;
    }

    public IEnumerator SetAlphaOfOuterItemsThatRNotDefaultSpriteLayer(bool exitingBuilding)
    // this function is for things such as balconies that show up over the backdrop. This function set's their alpha to 0 and back again upon building-exit
    {
        if(exitingBuilding)
        {
            yield return new WaitForSeconds(1f);

            for (int i = 0; i < balconyManager.balconyList.Count; i++)
            { 

                    SpriteRenderer spriteRenderer = balconyManager.balconyList[i].GetComponent<SpriteRenderer>();
                    spriteRenderer.color = balconyManager.balconyInitialColorList[i];
            }


        }
        else // if entering set the alphas to 0
        {
            // yield return new WaitForSeconds(0.3f);

            for (int i = 0; i < balconyManager.balconyList.Count; i++)
            { 
                SpriteRenderer spriteRenderer = balconyManager.balconyList[i].GetComponent<SpriteRenderer>();
                Color color = spriteRenderer.color;  // Get the current color
                color.a = 0f;                        // Modify the alpha channel
                spriteRenderer.color = color;        // Assign the modified color back

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

    private IEnumerator FadeThenSetDontSort
                                            ( 
                                            bool exitingBuilding,  
                                            bool exitingBehindBuilding,
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
            yield return BuildingSequence(exitingBuilding, exitingBehindBuilding, shouldWait, waitTime.Value, behindBuilding, spriteList, colorList, alpha, tagsToExclude);
            SetDontSort(issList, setDontSort);
                            
            yield return new WaitForSeconds(0.3f);

            if(!exitingBuilding && !exitingBehindBuilding)
            {
                for (int i = 0; i < allOtherBuildings.Count; i++)
                {     
                    // allOtherBuildings[i].SetDontSort(allOtherBuildings[i].innerSpriteSortingScriptObj, false);
                    allOtherBuildings[i].SetDontSort(allOtherBuildings[i].outerSpriteSortingScriptObj, true);
                }
            }
            else
            {
                for (int i = 0; i < allOtherBuildings.Count; i++)
                {     
                    // allOtherBuildings[i].SetDontSort(allOtherBuildings[i].innerSpriteSortingScriptObj, false);
                    allOtherBuildings[i].SetDontSort(allOtherBuildings[i].outerSpriteSortingScriptObj, false);
                }
            }

        }
        else
        {
            // if(!exitingBuilding && !exitingBehindBuilding)
            // {
            //     for (int i = 0; i < allOtherBuildings.Count; i++)
            //     {     
            //         // allOtherBuildings[i].SetDontSort(allOtherBuildings[i].innerSpriteSortingScriptObj, false);
            //         allOtherBuildings[i].SetDontSort(allOtherBuildings[i].outerSpriteSortingScriptObj, true);
            //     }
            // }
            // else
            // {
            //     for (int i = 0; i < allOtherBuildings.Count; i++)
            //     {     
            //         // allOtherBuildings[i].SetDontSort(allOtherBuildings[i].innerSpriteSortingScriptObj, false);
            //         allOtherBuildings[i].SetDontSort(allOtherBuildings[i].outerSpriteSortingScriptObj, false);
            //     }
            // }

            SetDontSort(issList, setDontSort);
            yield return BuildingSequence(exitingBuilding, exitingBehindBuilding, shouldWait, waitTime.Value, behindBuilding, spriteList, colorList, alpha, tagsToExclude);
        }
        
        // for (int i = 0; i < npcListForBuilding.Count; i++)
        // {
        //     SetTreeAlpha(npcSpriteListForBuilding[i], npcColorListForBuilding[i].a);
        // //     npc.GetComponent<CharacterAnimation>().SetAlphaToZeroForAllSprites();
        //     // SetTreeSortingLayer(npc.gameObject, "Level0");
        // }
        // for (int i = 0; i < npcListForBuilding.Count; i++)
        // {
        //     SetTreeAlpha(npcSpriteListForBuilding[i], npcColorListForBuilding[i].a);
        //     // npcListForBuilding[i].GetComponent<CharacterCustomization>().ResetAppearance();
        // }
    }
    private IEnumerator SetDontSortForAllOtherOuterBuildings(bool wait, bool setDontSort)
    {
        if(wait)
        {                            
            yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < allOtherBuildings.Count; i++)
            {     
                // allOtherBuildings[i].SetDontSort(allOtherBuildings[i].innerSpriteSortingScriptObj, false);
                allOtherBuildings[i].SetDontSort(allOtherBuildings[i].outerSpriteSortingScriptObj, setDontSort);
            }
        }
        else
        {
            for (int i = 0; i < allOtherBuildings.Count; i++)
            {     
                // allOtherBuildings[i].SetDontSort(allOtherBuildings[i].innerSpriteSortingScriptObj, false);
                allOtherBuildings[i].SetDontSort(allOtherBuildings[i].outerSpriteSortingScriptObj, setDontSort);
            }
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
                if (!excludeList.Contains(child.gameObject)){
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

    public void SetDontSort(List<IsoSpriteSorting> objList, bool dontSort)
    {
        for (int i = 0; i < objList.Count; i++)
        {
            objList[i].dontSort = dontSort;
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

    IEnumerator LateStartCoroForNonPlayerCharacters()
    {
        // Wait until the end of the current frame
        yield return new WaitForEndOfFrame();
        // yield return new WaitForSeconds(5);

        // FindAllChildrenWithTagAndAddToList(this.gameObject, "NPC");
        
        foreach(GameObject obj in npcListForBuilding) {
            GetSpritesAndAddToLists(obj, npcSpriteListForBuilding, new List<GameObject>(), npcColorListForBuilding);
        }


        for (int i = 0; i < npcSpriteListForBuilding.Count; i++)
        {  
            SetTreeAlpha(npcSpriteListForBuilding[i], 0f);
        } 

        for (int i = 0; i < npcListForBuilding.Count; i++)
        {  
            SetZToZero(npcListForBuilding[i]);
        } 
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
    // Recursive function to find a child with the specified tag
    void FindAllChildrenWithTagAndAddToList(GameObject parent, string tag)
    {
        Stack<Transform> stack = new Stack<Transform>();
        stack.Push(parent.transform);

        while (stack.Count > 0)
        {
            Transform current = stack.Pop();
            
            // Check if the current object has the desired tag
            if (current.CompareTag(tag))
            {
                npcListForBuilding.Add(current.gameObject);
            }
            
            // Add all children of the current object to the stack
            foreach (Transform child in current)
            {
                stack.Push(child);
            }
        }
    }

    public void FindChildrenByRoomScriptComponent()
    {
        Stack<Transform> stack = new Stack<Transform>();
        stack.Push(transform);

        while (stack.Count > 0)
        {
            Transform current = stack.Pop();
            RoomScript roomScript = current.GetComponent<RoomScript>();
            
            if (roomScript != null)
            {
                roomScripts.Add(roomScript);
            }

            // Push each child of the current transform onto the stack
            foreach (Transform child in current)
            {
                stack.Push(child);
            }
        }
    }
    public void FindChildrenByLevelScriptComponent()
    {
        Stack<Transform> stack = new Stack<Transform>();
        stack.Push(transform);

        while (stack.Count > 0)
        {
            Transform current = stack.Pop();
            LevelScript levelScript = current.GetComponent<LevelScript>();
            
            if (levelScript != null)
            {
                levelScripts.Add(levelScript);
            }

            // Push each child of the current transform onto the stack
            foreach (Transform child in current)
            {
                stack.Push(child);
            }
        }
    }

    public void SetZToZero(GameObject obj)
    {
        // Set the object's z position to zero
        Vector3 newPosition = obj.transform.position;
        newPosition.z = 0;
        obj.transform.position = newPosition;

        // Recursively set z position for all children
        foreach (Transform child in obj.transform)
        {
            SetZToZero(child.gameObject);
        }
    }
}