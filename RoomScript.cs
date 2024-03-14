using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    public List<RoomScript> roomsSameOrAbove = new List<RoomScript>();
    public List<RoomScript> roomsBelow = new List<RoomScript>();
    private List<Transform> childColliders = new List<Transform>(); // Separate list for child colliders
    public List<RoomThresholdColliderScript> doorsBelow = new List<RoomThresholdColliderScript>();
    private List<Coroutine> doorsBelowCoros = new List<Coroutine>();
    private float doorBelowAlpha = 0.15f;
    public int wallHeight = 30;
    public float displaceSpeed = 100;
    public float fadeSpeed = 100f;
    private Vector3 initialPosition;
    private List<Vector3> childColliderInitialPositions = new List<Vector3>();
    private Coroutine currentMotionCoroutine;
    private Coroutine currentColliderMotionCoroutine;

    void Start()
    {
        initialPosition = this.gameObject.transform.localPosition;

        FindColliderObjects(transform);
    }

    private void FindColliderObjects(Transform transform)
    {
        Stack<Transform> stack = new Stack<Transform>();
        stack.Push(transform);

        while (stack.Count > 0)
        {
            Transform current = stack.Pop();

            foreach (Transform child in current)
            {
                if (child.CompareTag("Collider") || child.CompareTag("Trigger"))
                {
                    childColliders.Add(child);
                    childColliderInitialPositions.Add(child.position); // Store the initial local position
                }
                stack.Push(child);
            }
        }
    }
// public void EnterRoom(bool shouldWait, float? waitTime)
// {
//     // ...
//     if (waitTime.HasValue)
//     {
//         roomsBelow[i].MoveDown(shouldWait, waitTime.Value);
//     }
//     else
//     {
//         // Handle the case where waitTime is null
//         // You might want to add some default behavior or log an error here.
//     }
//     // ...
// }

    public void EnterRoom(bool shouldWait, float? waitTime)
    {
        for (int i = 0; i < roomsSameOrAbove.Count; i++)
        {
            roomsSameOrAbove[i].MoveUp();
        }

        for (int i = 0; i < roomsBelow.Count; i++)
        {
            if (waitTime.HasValue)
            {
                roomsBelow[i].MoveDown(shouldWait, waitTime.Value);
            }
            else
            {
                roomsBelow[i].MoveDown(false, 0f);
            }        
        }

        for (int i = 0; i < doorsBelow.Count; i++)
        {
            doorsBelow[i].SetPlayerIsInRoomAbove(true);
            doorsBelow[i].SetPlayerIsInDoorway(false);
        }
    }
    
    public void ExitRoom() 
    {
        for (int i = 0; i < doorsBelow.Count; i++)
        {
            doorsBelow[i].SetPlayerIsInRoomAbove(false);
            doorsBelow[i].SetPlayerIsInDoorway(false);
        }
    }
    
    public void ResetRooms() //move rooms to initial positions
    {
        Debug.Log("YESSY");
        for (int i = 0; i < roomsSameOrAbove.Count; i++)
        {
            roomsSameOrAbove[i].MoveUp();
        }

        for (int i = 0; i < roomsBelow.Count; i++)
        {
            roomsBelow[i].MoveUp();
        }
    }

    private void MoveUp()
    {
        if (currentMotionCoroutine != null)
        {
            StopCoroutine(currentMotionCoroutine);
        }

        currentMotionCoroutine = StartCoroutine(Displace(false, null, this.gameObject, initialPosition));
    }
    
    public void MoveDown(bool shouldWait, float? waitTime)
    {
        if (currentMotionCoroutine != null)
        {
            StopCoroutine(currentMotionCoroutine);
        }

        if (waitTime.HasValue)
        {
            // Move the parent object down
            currentMotionCoroutine = StartCoroutine(Displace(shouldWait, waitTime.Value, this.gameObject, initialPosition + new Vector3(0, -wallHeight, 0)));//, 1));
        }
        else
        {
            // Handle the case where waitTime is null (optional)
            // Debug.LogError("waitTime is null. You should handle this case appropriately.");
        }
    }


    IEnumerator Displace(bool shouldWait, float? waitTime, GameObject obj, Vector3 targetPosition)
    {
        Vector3 currentPosition = obj.transform.localPosition;

        float distance = (currentPosition - targetPosition).magnitude;

        float timeToReachTarget = 0.3f;

        float elapsedTime = 0f;

        if(shouldWait)
        {
            yield return new WaitForSeconds(waitTime.Value);
        }
        
        while (elapsedTime < timeToReachTarget)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / timeToReachTarget);

            // Rearranged LERP:
            // displacements = target - initial
            // initial + displacements * t
            // initial + (target - initial) * t
            // initial + target * t - initial * t
            // initial * (1 - t) + target * t
            obj.transform.localPosition = currentPosition * (1 - t) + targetPosition * t;

            for (int i = 0; i < childColliders.Count; i++)
            {
                childColliders[i].position = childColliderInitialPositions[i];
            }

            yield return null;
        }

        obj.transform.localPosition = targetPosition; // Ensure the object reaches the exact target position

        for (int i = 0; i < childColliders.Count; i++)
        {
            childColliders[i].position = childColliderInitialPositions[i]; // Ensure the object reaches the exact target position
        }
    }

    private IEnumerator Fade(SpriteRenderer sr, float fadeTo)
    {
        for (float t = 0.0f; t < 1; t += Time.deltaTime) 
        {
            float currentAlpha = Mathf.Lerp(sr.color.a, fadeTo, t * fadeSpeed);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, fadeTo);
            yield return null;
        }
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, fadeTo);
    }

    private void ResetDoorsBelowCoros()
    {
        for (int i = 0; i < doorsBelowCoros.Count; i++)
        {
            StopCoroutine(doorsBelowCoros[i]);
        }
        doorsBelowCoros = new List<Coroutine>();
    }
}

