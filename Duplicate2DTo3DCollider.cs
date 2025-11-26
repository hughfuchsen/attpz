using UnityEngine;
using UnityEditor;

public class Duplicate2DTo3DCollider : EditorWindow
{
    [MenuItem("Tools/Duplicate BoxCollider2D → BoxCollider (3D)")]
    static void DuplicateColliders()
    {
        // Get all BoxCollider2Ds in the scene
        BoxCollider2D[] all2D = GameObject.FindObjectsOfType<BoxCollider2D>();

        if (all2D.Length == 0)
        {
            Debug.LogWarning("No BoxCollider2D components found in the scene.");
            return;
        }

        Undo.RegisterSceneUndo("Duplicate 2D Colliders");

        foreach (var col2D in all2D)
        {
            GameObject original = col2D.gameObject;

            // Duplicate the object
            GameObject duplicate = Instantiate(original, original.transform.parent);
            duplicate.name = original.name + "_COLLIDER3D";

            // Remove BoxCollider2D on duplicate
            BoxCollider2D c2d = duplicate.GetComponent<BoxCollider2D>();
            if (c2d != null) Undo.DestroyObjectImmediate(c2d);

            // Add 3D BoxCollider
            BoxCollider col3d = Undo.AddComponent<BoxCollider>(duplicate);

            // Convert parameters
            col3d.isTrigger = col2D.isTrigger;
            col3d.size = new Vector3(col2D.size.x, col2D.size.y, 0.1f);
            col3d.center = new Vector3(col2D.offset.x, col2D.offset.y, 0f);
            // col3d.material = col2D.sharedMaterial;  

            // Optional: shrink Z depth to be tiny
            col3d.size = new Vector3(col3d.size.x, col3d.size.y, 0.01f);

            Debug.Log($"Duplicated {original.name} → {duplicate.name}");
        }

        Debug.Log("Finished duplicating all BoxCollider2D objects.");
    }
}
