// #if UNITY_EDITOR
// using UnityEditor;
// #endif
// using UnityEngine;

// public class CollapseGameObjects : MonoBehaviour
// {
// #if UNITY_EDITOR
//     [MenuItem("Tools/Collapse All Nested GameObjects %#g")] // Shortcut: Ctrl + G (Cmd + G on Mac)
//     static void CollapseAllNestedGameObjectsFromMenu()
//     {
//         CollapseAllNestedGameObjects();
//     }
// #endif

//     [ContextMenu("Collapse All Nested GameObjects")]
//     static void CollapseAllNestedGameObjects()
//     {
//         GameObject[] rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

//         foreach (GameObject rootObject in rootObjects)
//         {
//             CollapseNestedGameObjects(rootObject);
//         }
//     }

//     static void CollapseNestedGameObjects(GameObject gameObject)
//     {
//         foreach (Transform child in gameObject.transform)
//         {
//             CollapseNestedGameObjects(child.gameObject);
//         }

// #if UNITY_EDITOR
//         EditorApplication.ExecuteMenuItem("GameObject/Collapse All");
// #endif
//     }
// }
