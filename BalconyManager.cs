using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalconyManager : MonoBehaviour
{

    public List<GameObject> balconyList = new List<GameObject>();
    public List<Color> balconyInitialColorList = new List<Color>();

    

    
    // Start is called before the first frame update
    void Start()
    {
        GetSpritesAndAddToList();
    }
    private void GetSpritesAndAddToList()
    {
        // Get all game objects with the "Balcony" tag
        GameObject[] balconyObjects = GameObject.FindGameObjectsWithTag("Balcony");

        // Stack for traversing the hierarchy
        Stack<GameObject> stack = new Stack<GameObject>();

        // Push all the balcony objects to the stack
        foreach (GameObject balcony in balconyObjects)
        {
            stack.Push(balcony);
        }

        while (stack.Count > 0)
        {
            GameObject currentNode = stack.Pop();
            SpriteRenderer sr = currentNode.GetComponent<SpriteRenderer>();

            if (sr != null)
            {
                Color col = sr.color;
                balconyList.Add(currentNode);
                balconyInitialColorList.Add(col);
            }

            foreach (Transform child in currentNode.transform)
            {
                    stack.Push(child.gameObject);
            }
        }
    }

//  void CollectBalconyChildren()
//     {
//         // Get all game objects with the "Balcony" tag
//         GameObject[] balconyObjects = GameObject.FindGameObjectsWithTag("Balcony");

//         // Stack for traversing the hierarchy
//         Stack<Transform> stack = new Stack<Transform>();

//         // Push all the balcony objects to the stack
//         foreach (GameObject balcony in balconyObjects)
//         {
//             stack.Push(balcony.transform);
//         }

//         // Traverse the hierarchy using the stack
//         while (stack.Count > 0)
//         {
//             Transform current = stack.Pop();

//             // Check if the current game object has a SpriteRenderer
//             SpriteRenderer spriteRenderer = current.GetComponent<SpriteRenderer>();
//             if (spriteRenderer != null)
//             {
//                 // Add the game object to the list
//                 childrenWithSpriteRenderers.Add(current.gameObject);
//             }

//             // Push all children of the current object onto the stack
//             foreach (Transform child in current)
//             {
//                 stack.Push(child);
//             }
//         }

//         // Debug the result
//         Debug.Log("Total children with SpriteRenderer: " + childrenWithSpriteRenderers.Count);
//     }
}


