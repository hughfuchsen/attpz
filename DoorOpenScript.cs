using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenScript : MonoBehaviour
{
    private List<float> openDoorAlpha = new List<float>();
    private List<float> closedDoorAlpha = new List<float>();
    private List<GameObject> openDoorSprites = new List<GameObject>();
    private List<GameObject> closedDoorSprites = new List<GameObject>();

    void Start()
    {
        GetSprites(FindSiblingWithTag("OpenDoor"), openDoorSprites);
        GetSprites(FindSiblingWithTag("ClosedDoor"), closedDoorSprites);

        for (int i = 0; i < openDoorSprites.Count; i++)
        {
            Color initialColorOpen = openDoorSprites[i].GetComponent<SpriteRenderer>().color;
            openDoorAlpha.Add(initialColorOpen.a);
        }

        for (int i = 0; i < closedDoorSprites.Count; i++)
        {
            Color initialColorClosed = closedDoorSprites[i].GetComponent<SpriteRenderer>().color;
            closedDoorAlpha.Add(initialColorClosed.a);
        }

        for (int i = 0; i < openDoorSprites.Count; i++)
        {
            SetTreeAlpha(openDoorSprites[i], 0);
        }        
        for (int i = 0; i < closedDoorSprites.Count; i++)
        {
            SetTreeAlpha(closedDoorSprites[i], closedDoorAlpha[i]);
        }    

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i < openDoorSprites.Count; i++)
            {
                SetTreeAlpha(openDoorSprites[i], openDoorAlpha[i]);
            }        
            for (int i = 0; i < closedDoorSprites.Count; i++)
            {
                SetTreeAlpha(closedDoorSprites[i], 0);
            }            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i < openDoorSprites.Count; i++)
            {
                SetTreeAlpha(openDoorSprites[i], 0);
            }        
            for (int i = 0; i < closedDoorSprites.Count; i++)
            {
                SetTreeAlpha(closedDoorSprites[i], closedDoorAlpha[i]);
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

