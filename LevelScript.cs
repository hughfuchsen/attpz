using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript : MonoBehaviour
{
    public List<LevelScript> roomsSameOrAbove = new List<LevelScript>();
    public List<LevelScript> roomsBelow = new List<LevelScript>();
    private List<Transform> childColliders = new List<Transform>(); // Separate list for child colliders
    public List<ThresholdColliderScript> doorsBelow = new List<ThresholdColliderScript>();
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
        initialPosition = this.gameObject.transform.position;

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


    public void EnterRoom()
    {
        for (int i = 0; i < roomsSameOrAbove.Count; i++)
        {
            roomsSameOrAbove[i].MoveUp();
        }

        for (int i = 0; i < roomsBelow.Count; i++)
        {
            roomsBelow[i].MoveDown();
        }

        for (int i = 0; i < doorsBelow.Count; i++)
        {
            doorsBelow[i].SetPlayerIsInRoomAbove(true);
        }
    }
    
    public void ExitRoom()
    {
        for (int i = 0; i < doorsBelow.Count; i++)
        {
            doorsBelow[i].SetPlayerIsInRoomAbove(false);
        }
    }
    public void ExitBuilding()
    {
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

        currentMotionCoroutine = StartCoroutine(Displace(this.gameObject, initialPosition));
    }
    
    private void MoveDown()
    {
        if (currentMotionCoroutine != null)
        {
            StopCoroutine(currentMotionCoroutine);
        }

        // Move the parent object down
        currentMotionCoroutine = StartCoroutine(Displace(this.gameObject, initialPosition + new Vector3(0, -wallHeight, 0)));//, 1));
    }

    private IEnumerator Displace(GameObject obj, Vector3 targetPosition)
    {
        Vector3 currentPosition = obj.transform.position;

        float distance = (currentPosition - targetPosition).magnitude;

        float timeToReachTarget = distance / displaceSpeed;

        float elapsedTime = 0f;
        
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
            obj.transform.position = currentPosition * (1 - t) + targetPosition * t;

            for (int i = 0; i < childColliders.Count; i++)
            {
                childColliders[i].position = childColliderInitialPositions[i];
            }

            yield return null;
        }

        obj.transform.position = targetPosition; // Ensure the object reaches the exact target position

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

