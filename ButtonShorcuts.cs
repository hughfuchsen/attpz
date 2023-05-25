

 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 using UnityEditor;
 
 public class ButtonShorcuts : EditorWindow {
 
     [MenuItem("Edit/Select parent &c")]
        static void SelectParentOfObject() {
            Selection.activeGameObject = Selection.activeGameObject.transform.parent.gameObject;
        }

    // [MenuItem("MyMenu/Execute Button Shortcut %&#x")] // Defines the shortcut combination (alt/shift/x)
    //     static void SortTheScene() {
    //         // Call the function or perform the actions associated with your button here
    //         // For example, if your button's functionality is in another script, you can call its method:
    //         IsoSpriteSorting.SortScene();}  
 }