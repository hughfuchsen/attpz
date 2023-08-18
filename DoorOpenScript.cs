using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenScript : MonoBehaviour
{
    private float openDoorAlpha;
    private float closedDoorAlpha;
        void Awake()
    {
        Color initialColorOpen = this.FindSiblingWithTag("OpenDoor").GetComponent<SpriteRenderer>().color;
        initialColorOpen.a = openDoorAlpha;
        Color initialColorClosed = this.FindSiblingWithTag("ClosedDoor").GetComponent<SpriteRenderer>().color;
        initialColorClosed.a = closedDoorAlpha;
    }
    void Start()
    {
        SetTreeAlpha(this.FindSiblingWithTag("OpenDoor"), 0);
        SetTreeAlpha(this.FindSiblingWithTag("ClosedDoor"), closedDoorAlpha);
    }

    // NOTE: The parent of the trigger must be the door parent
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SetTreeAlpha(this.FindSiblingWithTag("OpenDoor"), openDoorAlpha);
            SetTreeAlpha(this.FindSiblingWithTag("ClosedDoor"), 0);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SetTreeAlpha(this.FindSiblingWithTag("OpenDoor"), 0);
            SetTreeAlpha(this.FindSiblingWithTag("ClosedDoor"), closedDoorAlpha);
        }
    }
}
