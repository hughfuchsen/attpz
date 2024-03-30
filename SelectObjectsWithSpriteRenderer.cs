using UnityEngine;
using UnityEditor;

public class SelectObjectsWithSpriteRenderer : MonoBehaviour
{
    [MenuItem("Tools/Select Objects With SpriteRenderers")]
    static void SelectObjects()
    {
        // Find all objects with SpriteRenderer components in the scene
        SpriteRenderer[] spriteRenderers = FindObjectsOfType<SpriteRenderer>();

        // Create an array to hold the GameObjects
        GameObject[] selectedObjects = new GameObject[spriteRenderers.Length];

        // Fill the array with the GameObjects
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            selectedObjects[i] = spriteRenderers[i].gameObject;
        }

        // Set the selection in the Unity Editor
        Selection.objects = selectedObjects;
    }
}
