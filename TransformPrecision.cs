using UnityEditor;
using UnityEngine;


public class TransformPrecision {
    /// <summary>
    /// Replacement for default Unity Duplicate function that avoids floating point precision errors
    /// Usage : Alt-D
    /// </summary>
    // [ MenuItem( "GameObject/Precise Duplicate &d" ) ]
    // public static void PreciseDuplicate( ) {
    //     if ( Selection.activeGameObject != null ) {

    //         GameObject newObj = GameObject.Instantiate<GameObject>( Selection.activeGameObject );
    //         newObj.name = Selection.activeGameObject.name;
    //         newObj.transform.SetParent( Selection.activeGameObject.transform.parent, false );

    //         Undo.RegisterCreatedObjectUndo( newObj, "Precise Duplicate " + Selection.activeGameObject.name );
    //         Selection.activeGameObject = newObj;
    //     }
    // }

    // /// <summary>
    // /// Rounds off all Transform and RectTransform values to nearest integer
    // /// Usage: Transform context menu (gear icon)
    // /// </summary>
    // [ MenuItem( "CONTEXT/Transform/Round Off Values" ) ]
    // public static void RoundOffValues( MenuCommand command ) {
    //     // Handle rounding slightly differently for Transform vs. RectTransform
    //     if (Selection.activeGameObject.GetComponent<SpriteRenderer>() != null) {
    //         if ( command.context is RectTransform ) {
    //             RectTransform rt = command.context as RectTransform;
    //             Undo.RecordObject( rt, "Round Off " + rt.name );

    //             rt.anchoredPosition = RoundVector2( rt.anchoredPosition );
    //             rt.localEulerAngles = RoundVector3( rt.localEulerAngles );
    //             rt.localScale = RoundVector3( rt.localScale );
    //             rt.sizeDelta = RoundVector2( rt.sizeDelta );
    //         } else {
    //             // Regular Transform
    //             Transform t = command.context as Transform;
    //             Undo.RecordObject( t, "Round Off " + t.name );

    //             t.localPosition = RoundVector2( t.localPosition );
    //             t.localEulerAngles = RoundVector3( t.localEulerAngles );
    //             t.localScale = RoundVector3( t.localScale );
    //         }
    //     }    
    // }

    [MenuItem("CONTEXT/Transform/Round Off Values")]
    public static void RoundOffValues(MenuCommand command)
    {
        Transform[] selectedTransforms = Selection.transforms;

        foreach (Transform transform in selectedTransforms)
        {
            if (transform.GetComponent<SpriteRenderer>() != null)
            {
                Undo.RecordObject(transform, "Round Off " + transform.name);

                transform.localPosition = RoundVector3(transform.localPosition);
                transform.localEulerAngles = RoundVector3(transform.localEulerAngles);
                transform.localScale = RoundVector3(transform.localScale);
            }
        }
    }



    private static Vector3 RoundVector3( Vector3 v ) {
        return new Vector3( Mathf.Round( v.x ), Mathf.Round( v.y ), Mathf.Round( v.z ) );
    }
}
