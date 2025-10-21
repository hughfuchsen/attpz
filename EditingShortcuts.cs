

 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;

 #if UNITY_EDITOR
 using UnityEditor;
 
 public class EditingShortcuts : EditorWindow {
	private static List<GameObject> childSpriteRendererGameObjects;
	private static List<GameObject> childColliderTriggerGameObjects;
    private static List<GameObject> selectSpriteRendererSiblingGameObjects;
    private static List<GameObject> siblingsWithSameColor;



     [MenuItem("Tools/Select Parent &c")]
        static void SelectParentOfObject() {
            Selection.activeGameObject = Selection.activeGameObject.transform.parent.gameObject; 
        }


	[MenuItem( "Tools/Select Children With ColliderTrigger &f" )]
	private static void NewMenuOptionCldr() {
		GameObject obj = Selection.activeGameObject;
		Transform t = obj.transform;
		childColliderTriggerGameObjects = new List<GameObject>();
		GetAllChildrenColliders( t );
		GameObject[] gOs = childColliderTriggerGameObjects.ToArray();
		Selection.objects = gOs;
	}


    [MenuItem( "Tools/Select Children W Sprite Rndrs &d" )]
	private static void NewMenuOptionSR() {
		GameObject obj = Selection.activeGameObject;
		Transform t = obj.transform;
		childSpriteRendererGameObjects = new List<GameObject>();
		GetAllChildrenSpriteRenderers( t );
		GameObject[] gOs = childSpriteRendererGameObjects.ToArray();
		Selection.objects = gOs;
	}

	[MenuItem( "Tools/Select Siblings &s" )]
	private static void NewMenuOptionSiblingSpriteRenderer() {
		GameObject obj = Selection.activeGameObject;
		Transform t = obj.transform;
		selectSpriteRendererSiblingGameObjects = new List<GameObject>();
		GetAllSiblings( t.parent );
		GameObject[] gOs = selectSpriteRendererSiblingGameObjects.ToArray();
		Selection.objects = gOs;
	}


    [MenuItem("Tools/Select Siblings With Same Color &a")]
    private static void SelectSiblingsWithSameColor() {
        GameObject obj = Selection.activeGameObject;
        Transform parent = obj.transform.parent;

        if (parent != null) {
            SpriteRenderer currentSpriteRenderer = obj.GetComponent<SpriteRenderer>();
            siblingsWithSameColor = new List<GameObject>();

            if (currentSpriteRenderer != null) {
                Color currentColor = currentSpriteRenderer.color;

                foreach (Transform sibling in parent) {
                    SpriteRenderer siblingSpriteRenderer = sibling.GetComponent<SpriteRenderer>();

                    if (siblingSpriteRenderer != null && siblingSpriteRenderer.color == currentColor) {
                        siblingsWithSameColor.Add(sibling.gameObject);
                    }
                }

                if (siblingsWithSameColor.Count > 0) {
                    Selection.objects = siblingsWithSameColor.ToArray();
                } else {
                    Debug.Log("No siblings found with the same color as the selected GameObject.");
                }
            } else {
                Debug.Log("Selected GameObject does not have a SpriteRenderer component.");
            }
        } else {
            Debug.Log("Selected GameObject does not have a parent.");
        }
    }


    [MenuItem("Tools/Round Off Values &g")]
    public static void RoundOffValues(MenuCommand command)
    {
        Transform[] selectedTransforms = Selection.transforms;

        foreach (Transform transform in selectedTransforms)
        {
            if (transform.GetComponent<SpriteRenderer>() != null || transform.GetComponent<BoxCollider2D>() == null)
            {
                Undo.RecordObject(transform, "Round Off " + transform.name);

                transform.localPosition = RoundVector3(transform.localPosition);
                transform.localEulerAngles = RoundVector3(transform.localEulerAngles);
                transform.localScale = RoundVector3(transform.localScale);
                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            }
            // }
        }
    }



    private static Vector3 RoundVector3( Vector3 v ) {
        return new Vector3( Mathf.Round( v.x ), Mathf.Round( v.y ), Mathf.Round( v.z ) );
    }

	static void GetAllSiblings( Transform t ) {
		foreach( Transform childT in t ) {
            if (childT.GetComponent<SpriteRenderer>() != null)
            {
			    selectSpriteRendererSiblingGameObjects.Add( childT.gameObject );
            }
		}
	}

	static void GetAllChildrenSpriteRenderers( Transform t ) {
		foreach( Transform childT in t ) {
            if (childT.GetComponent<SpriteRenderer>() != null)
            {
			    childSpriteRendererGameObjects.Add( childT.gameObject );
            }
            else
            {
                GetAllChildrenSpriteRenderers(childT);
            }
		}
	}

	static void GetAllChildrenColliders( Transform t ) {
		foreach( Transform childT in t ) {
            if (childT.GetComponent<BoxCollider2D>() != null || childT.GetComponent<PolygonCollider2D>() != null)
            {
			    childColliderTriggerGameObjects.Add( childT.gameObject );
            }
            else
            {
                GetAllChildrenColliders(childT);
            }
		}
	}

 }
 #endif