// using UnityEngine;
// using UnityEditor;
// using UnityEditor.ShortcutManagement;

// public static class ColorPickerShortcut
// {
//     [Shortcut("Tools/Open SpriteRenderer Color Picker", KeyCode.C, ShortcutModifiers.Shift | ShortcutModifiers.Alt)]
//     public static void OpenColorPicker()
//     {
//         if (Selection.activeGameObject != null)
//         {
//             SpriteRenderer spriteRenderer = Selection.activeGameObject.GetComponent<SpriteRenderer>();
//             if (spriteRenderer != null)
//             {
//                 OpenColorPickerForSpriteRenderer(spriteRenderer);
//             }
//             else
//             {
//                 Debug.LogWarning("Selected GameObject does not have a SpriteRenderer component.");
//             }
//         }
//         else
//         {
//             Debug.LogWarning("No GameObject selected.");
//         }
//     }

//     [MenuItem("Tools/Open SpriteRenderer Color Picker %#c")]
//     public static void OpenColorPickerMenu()
//     {
//         OpenColorPicker();
//     }

//     private static void OpenColorPickerForSpriteRenderer(SpriteRenderer spriteRenderer)
//     {
//         EditorGUIUtility.ShowObjectPicker<Color>(null, false, "", 0);
//         EditorApplication.update += () => WaitForColorPicker(spriteRenderer);
//     }

//     private static void WaitForColorPicker(SpriteRenderer spriteRenderer)
//     {
//         if (Event.current != null && Event.current.commandName == "ObjectSelectorClosed")
//         {
//             spriteRenderer.color = (Color)EditorGUIUtility.GetObjectPickerObject();
//             EditorApplication.update -= () => WaitForColorPicker(spriteRenderer);
//         }
//     }
// }
