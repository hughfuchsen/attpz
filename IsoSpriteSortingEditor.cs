// #if UNITY_EDITOR
// using UnityEditor;
// using UnityEngine;

// #pragma warning disable 618

// [CustomEditor(typeof(IsoSpriteSorting))]
// public class IsoSpriteSortingEditor : Editor
// {
//     public void OnSceneGUI()
//     {
//         IsoSpriteSorting myTarget = (IsoSpriteSorting)target;

//         myTarget.SorterPositionOffset = Handles.FreeMoveHandle(
//             myTarget.transform.position + myTarget.SorterPositionOffset,
//             Quaternion.identity,
//             0.08f * HandleUtility.GetHandleSize(myTarget.transform.position),
//             Vector3.zero,
//             Handles.DotHandleCap
//         ) - myTarget.transform.position;
//         if (myTarget.sortType == IsoSpriteSorting.SortType.Line)
//         {
//             myTarget.SorterPositionOffset2 = Handles.FreeMoveHandle(
//                 myTarget.transform.position + myTarget.SorterPositionOffset2,
//                 Quaternion.identity,
//                 0.08f * HandleUtility.GetHandleSize(myTarget.transform.position),
//                 Vector3.zero,
//                 Handles.DotHandleCap
//             ) - myTarget.transform.position;
//             Handles.DrawLine(myTarget.transform.position + myTarget.SorterPositionOffset, myTarget.transform.position + myTarget.SorterPositionOffset2);
//         }
//         if (GUI.changed)
//         {
//             Undo.RecordObject(target, "Updated Sorting Offset");
//             EditorUtility.SetDirty(target);
//         }
//     }

//     public override void OnInspectorGUI()
//     {
//         DrawDefaultInspector();
//         IsoSpriteSorting myScript = (IsoSpriteSorting)target;
//         if (GUILayout.Button("Sort Visible Scene"))
//         {
//             IsoSpriteSorting.SortScene();
//         }
//     }
// }
// #endif


#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

#pragma warning disable 618

[CustomEditor(typeof(IsoSpriteSorting))]
public class IsoSpriteSortingEditor : Editor
{
    private const float defaultSnap = 0.25f;
    private const float altSnap = 0.5f;

    public void OnSceneGUI()
    {
        IsoSpriteSorting myTarget = (IsoSpriteSorting)target;

        // --- DETERMINE SNAP MODE
        float snap = defaultSnap;

        if (Event.current.shift)
        {
            snap = 0f; // no snapping
        }
        else if (Event.current.control)
        {
            snap = altSnap; // bigger snap
        }

        // --- DRAW GRID
        // DrawGrid(myTarget.transform.position, 10f, defaultSnap);

        // --- HANDLE 1
        myTarget.SorterPositionOffset = DrawSnappedHandle(
            myTarget,
            myTarget.SorterPositionOffset,
            snap
        );

        // --- HANDLE 2 (if line mode)
        if (myTarget.sortType == IsoSpriteSorting.SortType.Line)
        {
            myTarget.SorterPositionOffset2 = DrawSnappedHandle(
                myTarget,
                myTarget.SorterPositionOffset2,
                snap
            );

            Handles.DrawLine(
                myTarget.transform.position + myTarget.SorterPositionOffset,
                myTarget.transform.position + myTarget.SorterPositionOffset2
            );
        }

        // --- UNDO / DIRTY
        if (GUI.changed)
        {
            Undo.RecordObject(target, "Updated Sorting Offset");
            EditorUtility.SetDirty(target);
        }
    }

    private Vector3 DrawSnappedHandle(IsoSpriteSorting target, Vector3 offset, float snap)
    {
        Vector3 worldPos = target.transform.position + offset;

        Handles.color = new Color(1f, 1f, 1f, 1f); // white, 40% opacity

        // draw handle and get the new world position
        Vector3 newWorldPos = Handles.FreeMoveHandle(
            worldPos,
            Quaternion.identity,
            0.08f * HandleUtility.GetHandleSize(worldPos),
            Vector3.zero,
            Handles.DotHandleCap
        );

        // only apply snap *after* dragging
        if (snap > 0f && newWorldPos != worldPos)
        {
            newWorldPos = SnapToGrid(newWorldPos, snap);
        }

        // return offset relative to parent
        return newWorldPos - target.transform.position;
    }

    // private Vector3 DrawSnappedHandle(IsoSpriteSorting target, Vector3 offset, float snap)
    // {
    //     Transform t = target.transform;

    //     // 🔥 handle negative scale (flip compensation)
    //     Vector3 scaleSign = new Vector3(
    //         Mathf.Sign(t.localScale.x),
    //         Mathf.Sign(t.localScale.y),
    //         1f
    //     );

    //     // apply sign so flipping doesn't move handle
    //     Vector3 correctedOffset = Vector3.Scale(offset, scaleSign);

    //     Vector3 worldPos = t.position + correctedOffset;

    //     Handles.color = new Color(1f, 1f, 1f, 1f);

    //     Vector3 newWorldPos = Handles.FreeMoveHandle(
    //         worldPos,
    //         Quaternion.identity,
    //         0.08f * HandleUtility.GetHandleSize(worldPos),
    //         Vector3.zero,
    //         Handles.DotHandleCap
    //     );

    //     if (snap > 0f && newWorldPos != worldPos)
    //     {
    //         newWorldPos = SnapToGrid(newWorldPos, snap);
    //     }

    //     // convert back into local offset space (reapply flip)
    //     Vector3 newOffset = newWorldPos - t.position;
    //     newOffset = Vector3.Scale(newOffset, scaleSign);

    //     return newOffset;
    // }

    private Vector3 SnapToGrid(Vector3 position, float gridSize)
    {
        return new Vector3(
            Mathf.Round(position.x / gridSize) * gridSize,
            Mathf.Round(position.y / gridSize) * gridSize,
            0f
        );
    }

    private void DrawGrid(Vector3 center, float size, float spacing)
    {
        Handles.color = new Color(1f, 1f, 1f, 0.08f);

        float half = size * 0.5f;

        for (float x = -half; x <= half; x += spacing)
        {
            Vector3 start = center + new Vector3(x, -half, 0);
            Vector3 end = center + new Vector3(x, half, 0);
            Handles.DrawLine(start, end);
        }

        for (float y = -half; y <= half; y += spacing)
        {
            Vector3 start = center + new Vector3(-half, y, 0);
            Vector3 end = center + new Vector3(half, y, 0);
            Handles.DrawLine(start, end);
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        IsoSpriteSorting myScript = (IsoSpriteSorting)target;

        GUILayout.Space(10);

        EditorGUILayout.LabelField("Scene Controls", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("• Drag handles = move points");
        EditorGUILayout.LabelField("• Hold SHIFT = free move (no snap)");
        EditorGUILayout.LabelField("• Hold CTRL = snap to 0.5");

        GUILayout.Space(10);

        if (GUILayout.Button("Sort Visible Scene"))
        {
            IsoSpriteSorting.SortScene();
        }
    }
}
#endif