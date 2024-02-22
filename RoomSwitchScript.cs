using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSwitchScript : MonoBehaviour
{
    public RoomScript room;
    public List<GameObject> doorsBelow = new List<GameObject>(); 

    void OnTriggerEnter2D()
    {
        room.EnterRoom(false, 0f);
        // roomBelow.ExitRoom();
    }
    void OnTriggerExit2D()
    {
        room.ExitRoom();
        // roomBelow.EnterRoom(false, 0f);s
    }

//     void SetTreeAlpha(GameObject treeNode, float alpha, string[] tagsToExclude = null) 
//     {
//         if (treeNode == null) {
//             return; // TODO: remove this
//         }
//         SpriteRenderer sr = treeNode.GetComponent<SpriteRenderer>();
//         if (sr != null && (tagsToExclude == null || !Array.Exists(tagsToExclude, element => element == treeNode.tag)))
//         {
//             sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
//         }
//         foreach (Transform child in treeNode.transform) 
//         {
//             SetTreeAlpha(child.gameObject, alpha, tagsToExclude);
//         }
//     }  

//     IEnumerator treeFade(
//       GameObject obj,
//       float fadeTo,
//       float fadeSpeed) 
//     {
//         float fadeFrom = obj.GetComponent<SpriteRenderer>().color.a;

//         for (float t = 0.0f; t < 1; t += Time.deltaTime) 
//         {
//             float currentAlpha = Mathf.Lerp(fadeFrom, fadeTo, t * fadeSpeed);
//             SetTreeAlpha(obj, currentAlpha);
//             yield return null;
//         }
//     }
}
