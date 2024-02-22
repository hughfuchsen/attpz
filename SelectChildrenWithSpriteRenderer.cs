using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class SelectChildren : MonoBehaviour {
	private static List<GameObject> childGOs;

	[MenuItem( "Tools/Select Children &d" )]
	private static void NewMenuOption() {
		GameObject obj = Selection.activeGameObject;
		Transform t = obj.transform;
		childGOs = new List<GameObject>();
		GetAllChildren( t );
		GameObject[] gOs = childGOs.ToArray();
		Selection.objects = gOs;
	}

	static void GetAllChildren( Transform t ) {
		foreach( Transform childT in t ) {
            if (childT.GetComponent<SpriteRenderer>() != null)
            {
			    childGOs.Add( childT.gameObject );
            }
            else
            {
                GetAllChildren(childT);
            }
		}
	}
}