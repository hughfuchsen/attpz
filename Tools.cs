using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

public class Tools
{
    [MenuItem("Tools/Select Thresholds")]
    // static void SelectThresholds()
    // {
    //     // Find all RoomThresholdColliderScript components in the scene
    //     RoomThresholdColliderScript[] allThresholds = GameObject.FindObjectsOfType<RoomThresholdColliderScript>();
    //     // BuildingThreshColliderScript[] allThresholds = GameObject.FindObjectsOfType<BuildingThreshColliderScript>();

    //     var selectedObjects = new System.Collections.Generic.List<GameObject>();

    //     foreach (var threshold in allThresholds)
    //     {
    //         Transform parent = threshold.transform.parent;

    //         if (
    //             parent != null &&
    //             parent.localScale.x > 0 &&
    //             (threshold.GetComponent<RoomThresholdColliderScript>() != null) &&
    //             threshold.GetComponent<InclineThresholdColliderScript>() == null &&
    //             threshold.gameObject.name.ToLower().Contains("door") &&
    //             atLeastOneSibling.SpriteRendererFileName.ToLower().Contains("26"))
    //         {
    //             selectedObjects.Add(parent.gameObject);
    //         }
    //     }

    //     // Select them in the editor
    //     Selection.objects = selectedObjects.ToArray();

    //     Debug.Log($"Selected {selectedObjects.Count} GameObjects with plyrCrsngLeft == true");
    // }



    static void SelectThresholds()
    {
        // Find all RoomThresholdColliderScript components in the scene
        RoomThresholdColliderScript[] allThresholds = GameObject.FindObjectsOfType<RoomThresholdColliderScript>();

        var selectedObjects = new System.Collections.Generic.List<GameObject>();

        foreach (var threshold in allThresholds)
        {
            Transform parent = threshold.transform.parent;

            if (parent == null || parent.localScale.x <= 0)
                continue;

            // if (threshold.GetComponent<InclineThresholdColliderScript>() != null)
            //     continue;
          
            if (threshold.GetComponent<BuildingThreshColliderScript>() != null && !threshold.GetComponent<BuildingThreshColliderScript>().enabled) 
                continue;

            if (!threshold.gameObject.name.ToLower().Contains("door"))
                continue;
            
            if (threshold.gameObject.name.ToLower().Contains(@"/"))
                continue;
            
            if (parent.gameObject.name.ToLower().Contains(@"/"))
                continue;
            
            if (parent.gameObject.name.ToLower().Contains("right"))
                continue;

            // Check siblings for a sprite with "26" in the file name
            bool hasMatchingSibling = false;
            foreach (Transform sibling in parent)
            {
                if (sibling == threshold.transform) // skip self
                    continue;

                SpriteRenderer sr = sibling.GetComponent<SpriteRenderer>();
                if (sr != null && sr.sprite != null)
                {
                    string fileName = sr.sprite.name.ToLower();
                    if (fileName.Contains("6x26"))
                    {
                        hasMatchingSibling = true;
                        break;
                    }
                }
            }

            if (hasMatchingSibling)
                selectedObjects.Add(parent.gameObject);
        }

        // Select them in the editor
        Selection.objects = selectedObjects.ToArray();

        Debug.Log($"Selected {selectedObjects.Count} GameObjects with matching siblings");
    }


    // static void SelectThresholds()
    // {
    //     // Find all RoomThresholdColliderScript components in the scene
    //     // RoomThresholdColliderScript[] allThresholds = GameObject.FindObjectsOfType<RoomThresholdColliderScript>();
    //     SpriteRenderer[] allThresholds = GameObject.FindObjectsOfType<SpriteRenderer>();

    //     var selectedObjects = new System.Collections.Generic.List<GameObject>();

    //     foreach (var threshold in allThresholds)
    //     {
    //         Transform parent = threshold.transform.parent;

    //         // if (
    //             // parent != null &&
    //             // parent.localScale.x > 0 &&
    //             // (threshold.GetComponent<RoomThresholdColliderScript>() != null && !threshold.GetComponent<RoomThresholdColliderScript>().plyrCrsngLeft) &&
    //             // threshold.GetComponent<InclineThresholdColliderScript>() == null &&
    //             // threshold.gameObject.name.ToLower().Contains("door"))
    //         // {
    //             selectedObjects.Add(threshold.gameObject);
    //         // }
    //     }

    //     // Select them in the editor
    //     Selection.objects = selectedObjects.ToArray();

    //     Debug.Log($"Selected {selectedObjects.Count} GameObjects with plyrCrsngLeft == true");
    // }




    [MenuItem("Tools/Set Selected Sprites To 50% Saturation")]
    public static void SetSelectedSpritesToMidSaturation()
    {
        // Gets all SpriteRenderers in selected objects + children
        SpriteRenderer[] sprites = Selection.GetFiltered<SpriteRenderer>(SelectionMode.Deep);

        foreach (SpriteRenderer sr in sprites)
        {
            Color color = sr.color;

            Color.RGBToHSV(color, out float h, out float s, out float v);

            s = 0.5f;

            Color newColor = Color.HSVToRGB(h, s, v);
            newColor.a = color.a;

            sr.color = newColor;

            EditorUtility.SetDirty(sr);
        }

        Debug.Log($"Updated {sprites.Length} SpriteRenderers to 50% saturation.");
    }
    




    [MenuItem("Tools/Hue Shift Selected Sprites (+15°)")]
    public static void ShiftHue15()
    {
        ShiftHue(15f);
    }

    [MenuItem("Tools/Hue Shift Selected Sprites (-15°)")]
    public static void ShiftHueMinus15()
    {
        ShiftHue(-15f);
    }

    private static void ShiftHue(float degrees)
    {
        // Convert degrees → 0–1 range
        float hueOffset = degrees / 360f;

        SpriteRenderer[] sprites = Selection.GetFiltered<SpriteRenderer>(SelectionMode.Deep);

        foreach (SpriteRenderer sr in sprites)
        {
            Color color = sr.color;

            Color.RGBToHSV(color, out float h, out float s, out float v);

            // Shift hue and wrap around
            h += hueOffset;
            if (h > 1f) h -= 1f;
            if (h < 0f) h += 1f;

            Color newColor = Color.HSVToRGB(h, s, v);
            newColor.a = color.a;

            sr.color = newColor;

            EditorUtility.SetDirty(sr);
        }

        Debug.Log($"Hue shifted {sprites.Length} sprites by {degrees} degrees.");
    }

    public class SelectByTagTool
    {
        [MenuItem("Tools/Select All BuildingExterior")]
        public static void SelectAllHouseExterior()
        {
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            List<GameObject> matches = new List<GameObject>();

            foreach (GameObject go in allObjects)
            {
                if (go.CompareTag("BuildingExterior"))
                {
                    matches.Add(go);
                }
            }

            Selection.objects = matches.ToArray();

            Debug.Log($"Selected {matches.Count} objects with tag 'BuildingExterior'.");
        }
    }

    [MenuItem("Tools/Select Floor Parents Only")]
    static void SelectFloorParents()
    {
        SpriteRenderer[] sprites = GameObject.FindObjectsOfType<SpriteRenderer>();
        HashSet<GameObject> results = new HashSet<GameObject>();

        foreach (var sr in sprites)
        {
            Transform floorParent = sr.transform.parent;

            if (floorParent != null)
            {
                // make sure the parent itself does NOT have a SpriteRenderer
                if (floorParent.GetComponent<SpriteRenderer>() == null && floorParent.name.ToLower().Contains("floor")
                && !floorParent.name.ToLower().Contains("lamp")
                && !floorParent.name.ToLower().Contains("lay"))
                    results.Add(floorParent.gameObject);
            }
        }

        Selection.objects = new List<GameObject>(results).ToArray();
        Debug.Log("Selected " + results.Count + " floor parent objects.");
    }

    public class SpriteMaterialResetTool
{
    [MenuItem("Tools/Reset All SpriteRenderer Materials")]
    static void ResetMaterials()
    {
        SpriteRenderer[] renderers = Object.FindObjectsOfType<SpriteRenderer>(true);

        GameObject[] objects = renderers
            .Select(r => r.gameObject)
            .Distinct()
            .ToArray();

        Selection.objects = objects;

        Debug.Log($"Selected {objects.Length} GameObjects with SpriteRenderers");
    }
}

    // recursively walk up to find a parent that matches "floor" but not "lamp"
    static Transform FindFloorParent(Transform t)
    {
        while (t != null)
        {
            if (IsFloorButNotLamp(t.name))
                return t;

            t = t.parent;
        }
        return null;
    }

    static bool IsFloorButNotLamp(string name)
    {
        string lower = name.ToLower();
        return lower.Contains("floor");
    }
    
}