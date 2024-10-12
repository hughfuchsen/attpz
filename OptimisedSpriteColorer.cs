// using UnityEngine;
// using UnityEditor;

// public class OptimisedSpriteColorer : EditorWindow
// {
//     private Color colorAdjustment = Color.white;

//     [MenuItem("Window/Optimised Sprite Colorer")]
//     public static void ShowWindow()
//     {
//         EditorWindow.GetWindow(typeof(OptimisedSpriteColorer));
//     }

//     private void OnGUI()
//     {
//         GUILayout.Label("Sprite Color Randomizer", EditorStyles.boldLabel);

//         EditorGUILayout.Space(10);

//         colorAdjustment = EditorGUILayout.ColorField("Color Adjustment", colorAdjustment);

//         EditorGUILayout.Space(10);

//         if (GUILayout.Button("Randomize Selected Sprites - HSV"))
//         {
//             RandomizeSpriteColorsHSV();
//         }
//         // if (GUILayout.Button("Randomize Selected Sprites RGB"))
//         // {
//         //     RandomizeSpriteColorsRGB();
//         // }

//         if (GUILayout.Button("Apply Color Adjustment"))
//         {
//             ApplyColorAdjustment();
//         }
//     }

//     private void RandomizeSpriteColorsHSV()
//     {
//         Object[] selectedSprites = Selection.GetFiltered(typeof(GameObject), SelectionMode.Editable | SelectionMode.ExcludePrefab);

//         foreach (Object obj in selectedSprites)
//         {
//             GameObject gameObject = obj as GameObject;
//             SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

//             if (spriteRenderer != null)
//             {
//                 Undo.RecordObject(spriteRenderer, "Randomize Sprite Color");

//                 float s = 0.5f; // Saturation value (50%)
//                 float v = 1f; // Value (maximum brightness)
                
//                 // Generate random hue value
//                 float h = Random.value;

//                 // Convert HSV to RGB
//                 Color newColor = Color.HSVToRGB(h, s, v);

//                 // Set the alpha value to 1 (100%)
//                 newColor.a = 1f;

//                 spriteRenderer.color = newColor;

//                 EditorUtility.SetDirty(spriteRenderer);
//             }
//         }
//     }


//     // private void RandomizeSpriteColorsRBG()
//     // {
//     //     Object[] selectedSprites = Selection.GetFiltered(typeof(GameObject), SelectionMode.Editable | SelectionMode.ExcludePrefab);

//     //     foreach (Object obj in selectedSprites)
//     //     {
//     //         GameObject gameObject = obj as GameObject;
//     //         SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

//     //         if (spriteRenderer != null)
//     //         {
//     //             Undo.RecordObject(spriteRenderer, "Randomize Sprite Color");

//     //             float r = Random.value;
//     //             float g = Random.value;
//     //             float b = Random.value;
//     //             float a = spriteRenderer.color.a;

//     //             spriteRenderer.color = new Color(r, g, b, a);

//     //             EditorUtility.SetDirty(spriteRenderer);
//     //         }
//     //     }
//     // }

//     private void ApplyColorAdjustment()
//     {
//         Object[] selectedSprites = Selection.GetFiltered(typeof(GameObject), SelectionMode.Editable | SelectionMode.ExcludePrefab);

//         foreach (Object obj in selectedSprites)
//         {
//             GameObject gameObject = obj as GameObject;
//             SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

//             if (spriteRenderer != null)
//             {
//                 Undo.RecordObject(spriteRenderer, "Apply Color Adjustment");

//                 Color newColor = spriteRenderer.color + colorAdjustment;

//                 spriteRenderer.color = newColor;

//                 EditorUtility.SetDirty(spriteRenderer);
//             }
//         }
//     }
// }
