using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class PickAColorShortcut : MonoBehaviour
{
    [MenuItem("Tools/Select color &x")]
    static void SelectAColor()
    {
        GameObject selectedObject = Selection.activeGameObject;

        if (selectedObject != null)
        {
            SpriteRenderer spriteRenderer = selectedObject.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                Color pickedColor = UnityEditor.EditorGUILayout.ColorField("Pick a color from the screen", spriteRenderer.color);
                spriteRenderer.color = pickedColor;  
            }
            else
            {
                Debug.Log("Selected GameObject does not have a SpriteRenderer component.");
            }
        }
        else
        {
            Debug.Log("No GameObject selected.");
        }
    }
}
