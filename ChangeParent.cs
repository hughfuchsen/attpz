using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeParent : MonoBehaviour
{
    // Reference to the new parent object
    public Transform newParent;

    // Call this method to change the parent of the GameObject
    public void ChangeParentObject()
    {
        // Ensure there is a new parent assigned
        if (newParent != null)
        {
            // Change the parent of the GameObject to the new parent
            transform.SetParent(newParent);
        }
        else
        {
            Debug.LogError("New parent is not assigned.");
        }
    }
}

