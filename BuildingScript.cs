using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{
    public GameObject innerBuilding;
    public GameObject outerBuilding;
    private List<GameObject> innerBuildingSpriteList = new List<GameObject>();
    private List<GameObject> outerBuildingSpriteList = new List<GameObject>();
    private List<Color> innerBuildingInitialColorList = new List<Color>();
    private List<Color> outerBuildingInitialColorList = new List<Color>();
    private List<IsoSpriteSorting> innerSpriteSortingScriptObj = new List<IsoSpriteSorting>();
    private List<IsoSpriteSorting> outerSpriteSortingScriptObj = new List<IsoSpriteSorting>();

    private Coroutine innerBuildingFadeCoroutine;
    private Coroutine outerBuildingFadeCoroutine;
    private string[] tagsToExludeEntExt = { "OpenDoor", "AlphaZeroEntExt" };


    



    // Start is called before the first frame update
    void Start()
    {
        innerBuilding.SetActive(true);
        outerBuilding.SetActive(true);
        GetSpritesAndAddToLists(innerBuilding, innerBuildingSpriteList, innerBuildingInitialColorList);
        GetSpritesAndAddToLists(outerBuilding, outerBuildingSpriteList, outerBuildingInitialColorList);
        GetIsoSpriteSortComponentsAndAddToLists(innerBuilding, innerSpriteSortingScriptObj);
        GetIsoSpriteSortComponentsAndAddToLists(outerBuilding, outerSpriteSortingScriptObj);
        TagChildrenOfTaggedParents("OpenDoor");
        TagChildrenOfTaggedParents("ClosedDoor");
        TagChildrenOfTaggedParents("AlphaZeroEntExt");
        ExitBuilding();
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
        // innerBuildingFadeCoroutine = StartCoroutine(FadeThenSetDontSort(innerSpriteSortingScriptObj, false, false, 0f, false, innerBuildingSpriteList, innerBuildingInitialColorList, null, tagsToExludeEntExt));
        // outerBuildingFadeCoroutine = StartCoroutine(FadeThenSetDontSort(outerSpriteSortingScriptObj, true, false, 0f, false, outerBuildingSpriteList, null, 0f));
        innerBuildingFadeCoroutine = StartCoroutine(Fade(false, null, false, innerBuildingSpriteList, innerBuildingInitialColorList, null, tagsToExludeEntExt));
        outerBuildingFadeCoroutine = StartCoroutine(Fade(false, null, false, outerBuildingSpriteList, null, 0f));
        // SetDontSort(innerSpriteSortingScriptObj, false);
        // SetDontSort(outerSpriteSortingScriptObj, true);
    }
    public void ExitBuilding()
    {
        if(this.innerBuildingFadeCoroutine != null)
        {
            StopCoroutine(this.innerBuildingFadeCoroutine);
        }
        if(this.outerBuildingFadeCoroutine != null)
        {
            StopCoroutine(this.outerBuildingFadeCoroutine);
        }
        // innerBuildingFadeCoroutine = StartCoroutine(FadeThenSetDontSort(innerSpriteSortingScriptObj, true, true, 1f, false, innerBuildingSpriteList, null, 0f));
        // outerBuildingFadeCoroutine = StartCoroutine(FadeThenSetDontSort(outerSpriteSortingScriptObj, false, true, 1f, false, outerBuildingSpriteList, outerBuildingInitialColorList, null, tagsToExludeEntExt));
        innerBuildingFadeCoroutine = StartCoroutine(Fade(true, 1f, false, innerBuildingSpriteList, null, 0f));
        outerBuildingFadeCoroutine = StartCoroutine(Fade(true, 1f, false, outerBuildingSpriteList, outerBuildingInitialColorList, null, tagsToExludeEntExt));
        // SetDontSort(innerSpriteSortingScriptObj, true);
        // SetDontSort(outerSpriteSortingScriptObj, false);
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
        innerBuildingFadeCoroutine = StartCoroutine(Fade(true, 1f, true, innerBuildingSpriteList, null, 0f));
        outerBuildingFadeCoroutine = StartCoroutine(Fade(true, 1f, false, outerBuildingSpriteList, null, 0.35f, tagsToExludeEntExt));
        
    }




    // coroutine for fading things in a sequence
    public IEnumerator Fade(bool shouldWait, float? waitTime, bool behindBuilding, List<GameObject> spriteList, List<Color> colorList, float? alpha, string[] tagsToExclude = null)
    {
        if (shouldWait && waitTime.HasValue) // Check if waitTime has a value
        {
            yield return new WaitForSeconds(waitTime.Value);
        }

        for (float t = 0.0f; t < 1; t += Time.deltaTime) 
        {        
            for (int i = 0; i < spriteList.Count; i++)
            {
                SpriteRenderer sr = spriteList[i].GetComponent<SpriteRenderer>();
                Transform tr = spriteList[i].transform;
                // Transform parentTransform = tr.parent;

                if (sr != null && (tagsToExclude == null || !Array.Exists(tagsToExclude, element => element == spriteList[i].tag)))        
                {
                    if(colorList == null)
                    {
                        if(behindBuilding && sr.CompareTag("ClosedDoor") && tr.parent.GetComponentInChildren<BuildingThreshColliderScript>() != null)
                        {
                            float nextAlpha = Mathf.Lerp(sr.color.a, 0.35f, t * 7f);
                            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, nextAlpha);
                        }
                        else
                        {
                            if (alpha.HasValue)
                            {
                                float nextAlpha = Mathf.Lerp(sr.color.a, alpha.Value, t * 7f);
                                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, nextAlpha);
                            }
                        }
                    }
                    else
                    {
                        float nextAlpha = Mathf.Lerp(sr.color.a, colorList[i].a, t * 7f);
                        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, nextAlpha);
                        // sr.color = colorList[i];
                    }    
                }  
            }   
            yield return null;    
        }

    }


    // private IEnumerator FadeThenSetDontSort
    //                                         ( 
    //                                         List<IsoSpriteSorting> issList, 
    //                                         bool setDontSort, bool shouldWait, 
    //                                         float? waitTime, 
    //                                         bool behindBuilding, 
    //                                         List<GameObject> spriteList, 
    //                                         List<Color> colorList, 
    //                                         float? alpha, 
    //                                         string[] tagsToExclude = null
    //                                         )
    // {
    //     Fade(shouldWait, waitTime.Value, behindBuilding, spriteList, colorList, alpha, tagsToExclude);
    //     SetDontSort(issList, setDontSort);
    //     yield return null;
    // }







    private void GetSpritesAndAddToLists(GameObject obj, List<GameObject> spriteList, List<Color> colorList)
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
                stack.Push(child.gameObject);
            }
        }
    }
    private void GetIsoSpriteSortComponentsAndAddToLists(GameObject obj, List<IsoSpriteSorting> objList)
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
                stack.Push(child.gameObject);
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

}
