using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetZPosition : MonoBehaviour
{
    void Start()
    {
        // Get all objects in the scene
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        // Loop through all objects
        foreach (GameObject obj in allObjects)
        {
            // Set the Z position to 0 for all objects
            Vector3 newPosition = obj.transform.position;
            newPosition.z = 0f;
            obj.transform.position = newPosition;

            // Check if the object has the "Main Camera" tag
            if (obj.CompareTag("MainCamera"))
            {
                // Set the Z position to -1 for the object with the "Main Camera" tag
                newPosition.z = -1;
                obj.transform.position = newPosition;
            }
        }
    }
}

