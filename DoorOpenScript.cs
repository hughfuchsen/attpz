using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenScript : MonoBehaviour
{
    private List<float> openDoorAlpha = new List<float>();
    private List<float> closedDoorAlpha = new List<float>();
    private List<GameObject> openDoorSpriteList = new List<GameObject>();
    private List<GameObject> closedDoorSpriteList = new List<GameObject>();

    void Start()
    {
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

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i < openDoorSpriteList.Count; i++)
            {
                SetTreeAlpha(openDoorSpriteList[i], openDoorAlpha[i]);
            }        
            for (int i = 0; i < closedDoorSpriteList.Count; i++)
            {
                SetTreeAlpha(closedDoorSpriteList[i], 0);
            }            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i < openDoorSpriteList.Count; i++)
            {
                SetTreeAlpha(openDoorSpriteList[i], 0);
            }        
            for (int i = 0; i < closedDoorSpriteList.Count; i++)
            {
                SetTreeAlpha(closedDoorSpriteList[i], closedDoorAlpha[i]);
            }        
        }
    }



    // // NOTE: The parent of the trigger must be the door parent
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
    private void SetTreeAlpha(GameObject root, float alpha)
    {
        Stack<GameObject> stack = new Stack<GameObject>();
        stack.Push(root);

        while (stack.Count > 0)
        {
            GameObject currentNode = stack.Pop();
            SpriteRenderer sr = currentNode.GetComponent<SpriteRenderer>();

            if (sr != null)
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
            }

            foreach (Transform child in currentNode.transform)
            {
                stack.Push(child.gameObject);
            }
        }
    }
    private void GetSprites(GameObject root, List<GameObject> spriteList)
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

