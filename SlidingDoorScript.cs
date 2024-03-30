// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class SlidingDoorScript : MonoBehaviour
// {
//     public List<GameObject> doors = new List<GameObject>();

//     private Coroutine slidingDoorOpenCoro;
//     private Coroutine slidingDoorCloseCoro;
//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     void OnTriggerEnter2D()
//     {
//         if(this.slidingDoorOpenCoro != null)
//         {
//             StopCoroutine(this.slidingDoorOpenCoro);
//         }
//         if(this.slidingDoorCloseCoro != null)
//         {
//             StopCoroutine(this.slidingDoorCloseCoro);
//         }
//     }
//     void OnTriggerExit2D()
//     {
//         if(this.slidingDoorOpenCoro != null)
//         {
//             StopCoroutine(this.slidingDoorOpenCoro);
//         }
//         if(this.slidingDoorCloseCoro != null)
//         {
//             StopCoroutine(this.slidingDoorCloseCoro);
//         }
//     }

//  private IEnumerator Displace(GameObject obj, Vector3 targetPosition)
//     {
//         Vector3 currentPosition = obj.transform.localPosition;
//         float displaceDistace = (currentPosition - targetPosition).magnitude;
//         float timeToReachTarget = 0.3f;
//         float elapsedTime = 0f;

//         while (elapsedTime < timeToReachTarget)
//         {
//             elapsedTime += Time.deltaTime;

//             float t = Mathf.Clamp01(elapsedTime / timeToReachTarget);

//             // Rearranged LERP:
//             // displacements = target - initial
//             // initial + displacements * t
//             // initial + (target - initial) * t
//             // initial + target * t - initial * t
//             // initial * (1 - t) + target * t
//             obj.transform.localPosition = currentPosition * (1 - t) + targetPosition * t;

//             for (int i = 0; i < childColliders.Count; i++)
//             {
//                 childColliders[i].position = childColliderInitialPositions[i];
//             }
//             yield return null;
//         }

//         obj.transform.localPosition = targetPosition; // Ensure the object reaches the exact target position
//     }   
// }
