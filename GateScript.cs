using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour
{
    private List<float> initialOpenGateAlpha = new List<float>();
    private List<float> initialClosedGateAlpha = new List<float>();
    private List<GameObject> openGateSpriteList = new List<GameObject>();
    private List<GameObject> closedGateSpriteList = new List<GameObject>();



    // Start is called before the first frame update
    void Awake()
    {
        // get the sprites and add them to the -> corresponding lists 
        GetSprites(FindSiblingWithTag("OpenDoor"), openGateSpriteList);
        GetSprites(FindSiblingWithTag("ClosedDoor"), closedGateSpriteList);

        PopulateInitialColorLists();

        // set open gate alpha to zero
        for (int i = 0; i < openGateSpriteList.Count; i++)
        {    
            SetAlpha(openGateSpriteList[i], 0f);
        }    
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerCollider")
        {
            for (int i = 0; i < openGateSpriteList.Count; i++)
            {    
                SetAlpha(openGateSpriteList[i], initialOpenGateAlpha[i]);
            }
           
            for (int i = 0; i < openGateSpriteList.Count; i++)
            {    
                SetAlpha(closedGateSpriteList[i], 0f);
            }

        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerCollider")
        {
            for (int i = 0; i < openGateSpriteList.Count; i++)
            {    
                SetAlpha(openGateSpriteList[i], 0f);
            }
           
            for (int i = 0; i < openGateSpriteList.Count; i++)
            {    
                SetAlpha(closedGateSpriteList[i], initialClosedGateAlpha[i]);
            }

        }
    }











    public GameObject FindSiblingWithTag(string tag, Transform transform = null)
    {
        if (transform == null)
        {
            foreach (Transform child in this.gameObject.transform.parent)
            {
                if (child.CompareTag(tag))
                {
                    return child.gameObject;
                }
                else
                {
                    FindSiblingWithTag(tag, child);
                }
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
                if (child.CompareTag(tag))
                {
                    return child.gameObject; //this returns the first sibling that has the corresponding tag
                                             // then, if this group of siblings doesn's contain the tag, the next bit checks their descendants 
                                             //and so on
                }
                else
                {
                    Transform descendant = child.gameObject.transform;
                    if (descendant != null)
                    {
                        foreach (Transform child2 in descendant)
                        {
                            FindSiblingWithTag(tag, child2);
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        return null;
    }

    private void GetSprites(GameObject root, List<GameObject> spriteList)
    {
        if (root != null)
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

    void PopulateInitialColorLists()
    {
        //populate the initialOpenGateAlpha list
        for (int i = 0; i < openGateSpriteList.Count; i++)
        {
            Color initialColorOpen = openGateSpriteList[i].GetComponent<SpriteRenderer>().color;
            initialOpenGateAlpha.Add(initialColorOpen.a);
        }

        //populate the initialClosedGateAlpha list
        for (int i = 0; i < closedGateSpriteList.Count; i++)
        {
            Color initialColorClosed = closedGateSpriteList[i].GetComponent<SpriteRenderer>().color;
            initialClosedGateAlpha.Add(initialColorClosed.a);
        }
    }


    void SetGateToZeroAlpha(List<GameObject> gateList)
    {
        for (int i = 0; i < gateList.Count; i++)
        {
            SetAlpha(gateList[i], 0);
        }   
    }
    void SetGateToInitialAlpha()
    {
        for (int i = 0; i < openGateSpriteList.Count; i++)
        {
            SetAlpha(openGateSpriteList[i], 0);
        }   
    }

    void SetAlpha(GameObject gameObject, float alpha) 
    {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
        }
    }      
    

}
