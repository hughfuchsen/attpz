using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class PathDrawer : MonoBehaviour
{
    public List<Vector3> pathPoints = new List<Vector3>();
    public Color pathColor = Color.yellow;
    public float handleSize = 0.2f;
    public bool snapToGrid = true;

    private void OnDrawGizmos()
    {
        Gizmos.color = pathColor;

        for (int i = 0; i < pathPoints.Count; i++)
        {
            Vector3 worldPos = transform.TransformPoint(pathPoints[i]);
            Gizmos.DrawSphere(worldPos, handleSize);

            if (i < pathPoints.Count - 1)
            {
                Vector3 nextPos = transform.TransformPoint(pathPoints[i + 1]);
                Gizmos.DrawLine(worldPos, nextPos);
            }
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PathDrawer))]
public class PathDrawerEditor : Editor
{
    private void OnSceneGUI()
    {
        PathDrawer pathDrawer = (PathDrawer)target;

        for (int i = 0; i < pathDrawer.pathPoints.Count; i++)
        {
            // Convert point to world space
            Vector3 worldPos = pathDrawer.transform.TransformPoint(pathDrawer.pathPoints[i]);

            // Create draggable handle
            EditorGUI.BeginChangeCheck();
            Vector3 newWorldPos = Handles.PositionHandle(worldPos, Quaternion.identity);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(pathDrawer, "Move Path Point");
                // Convert back to local space
                Vector3 localPos = pathDrawer.transform.InverseTransformPoint(newWorldPos);

                // Snap to nearest whole number
                if (pathDrawer.snapToGrid)
                {
                    localPos.x = Mathf.Round(localPos.x);
                    localPos.y = Mathf.Round(localPos.y);
                    localPos.z = Mathf.Round(localPos.z);
                }

                pathDrawer.pathPoints[i] = localPos;
                EditorUtility.SetDirty(pathDrawer);
            }
        }
    }
}
#endif
