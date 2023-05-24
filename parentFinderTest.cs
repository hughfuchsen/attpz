using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parentFinderTest : MonoBehaviour
{

    public string parentObjectName = "building10"; // Specify the name of the parent object

    // Start is called before the first frame update
    void Start()
    {
                // AccessParent(transform, parentObjectName);

    }

    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player"){
            AccessParent(transform, parentObjectName);
        }    
    }


    public void AccessParent(Transform currentTransform, string targetObjectName) {
        if (currentTransform.parent != null) {
        // Access the parent transform and check its name
        Transform parentTransform = currentTransform.parent;
        string parentName = parentTransform.name;
            if (parentName == targetObjectName)
            {
                // Parent object found, perform operations
                // Example: print the parent's name
                Debug.Log("Parent object found: " + parentName);
                // Do additional operations with the parent object if needed
                AccessChild(parentTransform, 3);
            }
            else
            {
                // Continue the traversal recursively with the parent transform
                AccessParent(parentTransform, targetObjectName);
            }
        } else {
            Debug.Log("Parent object not found!");
        }
    }
    public void AccessChild(Transform parent, int maxIterations)
    {
        // Access the child objects
        for (int i = 1; i <= maxIterations; i++)
        {
            string childObjectName = "room" + i;

            // Find the child object by name
            Transform childTransform = parent.Find(childObjectName);

            if (childTransform != null)
            {
                // Child object found, perform operations
                // Example: print the child's name
                Debug.Log("Child object found: " + childTransform.name);
                // Do additional operations with the child object if needed
            }
            else
            {
                // Child object not found
                Debug.Log("Child object not found: " + childObjectName);
            }

            // Recursively search the child's children
            if (childTransform != null)
            {
                AccessChild(childTransform, maxIterations);
            }
        }
    }

    // public void AccessChild(Transform parent, int maxIterations)
    // {
    //     // Access the childTransform objects
    //     for (int i = 1; i <= maxIterations; i++)
    //     {
    //         string childObjectName = "room" + i;

    //         // Find the childTransform object by name
    //         Transform childTransform = parent.child;
    //         string childName = childTransform.name;

    //         if (childName == childObjectName)
    //         {
    //             // Child object found, perform operations
    //             // Example: print the childTransform's name
    //             Debug.Log("Child object found: " + childTransform.name);
    //             // Do additional operations with the childTransform object if needed
    //         }
    //         else
    //         {
    //             // Child object not found
    //             Debug.Log("Child object not found: " + childObjectName);
    //         }

    //         // Recursively search the childTransform's children
    //         AccessChild(childTransform, maxIterations);
    //     }
    // }

}



// if(GameObject.Find("ObjectName")
