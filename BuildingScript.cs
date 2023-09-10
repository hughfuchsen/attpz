using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{
    private List<GameObject> spriteList = new List<GameObject>();
    private List<Color> initialColorList = new List<Color>();
    private List<float> initialAlphaList = new List<float>();






    // Start is called before the first frame update
    void Start()
    {
        GetSprites(this.gameObject, spriteList, initialColorList, initialAlphaList);
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
        if (sr != null)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
        }
        foreach (Transform child in treeNode.transform) 
        {
            SetTreeAlpha(child.gameObject, alpha, tagsToExclude);
        }
    } 

    private void GetSprites(GameObject root, List<GameObject> spriteList, List<Color> colorList, List<float> floatList)
    {
        Stack<GameObject> stack = new Stack<GameObject>();
        stack.Push(root);

        while (stack.Count > 0)
        {
            GameObject currentNode = stack.Pop();
            SpriteRenderer sr = currentNode.GetComponent<SpriteRenderer>();
            Color col = sr.color;
            float alph = col.a;

            if (sr != null)
            {
                spriteList.Add(currentNode);
                colorList.Add(col);
                floatList.Add(alph);
            }

            foreach (Transform child in currentNode.transform)
            {
                stack.Push(child.gameObject);
            }
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

    bool CompareLayer(GameObject gameObject, string layerName)
    {
        int layerMask = LayerMask.GetMask(layerName);
        return (gameObject.layer & layerMask) != 0;
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

}
