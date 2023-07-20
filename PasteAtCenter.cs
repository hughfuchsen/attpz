using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PasteAtCenter : EditorWindow
{
    [MenuItem("GameObject/Paste At Center %b", false, 0)]
    private static void PasteAtCenterCommand(MenuCommand menuCommand)
    {
        string copiedObjectData = EditorGUIUtility.systemCopyBuffer;
        if (!string.IsNullOrEmpty(copiedObjectData))
        {
            Object copiedObject = JsonUtility.FromJson<Object>(copiedObjectData);
            if (copiedObject != null)
            {
                SceneView sceneView = SceneView.lastActiveSceneView;
                if (sceneView != null)
                {
                    Vector3 center = sceneView.pivot;
                    GameObject pastedObject = PrefabUtility.InstantiatePrefab(copiedObject) as GameObject;
                    if (pastedObject != null)
                    {
                        pastedObject.transform.position = center;

                        // Find the root object of the active scene
                        GameObject sceneRoot = SceneManager.GetActiveScene().GetRootGameObjects()[0];

                        // Set the root object as the parent of the pasted object
                        pastedObject.transform.parent = sceneRoot.transform;

                        Selection.activeObject = pastedObject;
                    }
                }
            }
        }
    }
}
