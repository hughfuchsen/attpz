using UnityEngine;
using UnityEditor;

public class ThresholdSelectorMenu
{
    [MenuItem("Tools/Select Thresholds with plyrCrsngLeft")]
    static void SelectThresholds()
    {
        // Find all RoomThresholdColliderScript components in the scene
        RoomThresholdColliderScript[] allThresholds = GameObject.FindObjectsOfType<RoomThresholdColliderScript>();

        var selectedObjects = new System.Collections.Generic.List<GameObject>();

        foreach (var threshold in allThresholds)
        {
           if (!threshold.plyrCrsngLeft && 
                threshold.transform.parent != null && 
                threshold.transform.parent.localScale.x < 0)
            {
                selectedObjects.Add(threshold.gameObject);
            }
        }

        // Select them in the editor
        Selection.objects = selectedObjects.ToArray();

        Debug.Log($"Selected {selectedObjects.Count} GameObjects with plyrCrsngLeft == true");
    }
}