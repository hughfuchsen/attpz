using System.Collections;
using UnityEngine;

public class NPCPathFollower : MonoBehaviour
{
    public PathDrawer path;
    public float pauseTime = 0.3f;
    public bool stopAtEnd = false; // ✅ new toggle for testing
    public bool stopMoving = false; // ✅ new toggle for testing

    private CharacterMovement npcCM;
    private CharacterMovement myCM;
    private IsoSpriteSorting isoSpriteSortingScript;

    private void Start()
    {
        StartCoroutine(LateStart());
    }

    private IEnumerator LateStart()
    {
        yield return null; // wait one frame for components to init
        npcCM = GetComponent<CharacterMovement>();
        isoSpriteSortingScript = GetComponent<IsoSpriteSorting>();
        myCM = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();

        if (path == null || path.pathPoints.Count < 2)
        {
            Debug.LogWarning("NPCPathFollower: Path is missing or too short!");
            yield break;
        }

        // ✅ if stopAtEnd = true, only go forward once
        if (stopAtEnd)
        {
            yield return StartCoroutine(FollowPath(forward: true));
            yield break;
        }

        // otherwise, loop continuously
        while (true)
        {
            yield return StartCoroutine(FollowPath(forward: true));   // go forward
            yield return StartCoroutine(FollowPath(forward: false));  // then backward
        }
    }

    private IEnumerator FollowPath(bool forward)
    {
        int startIndex = forward ? 0 : path.pathPoints.Count - 1;
        int endIndex = forward ? path.pathPoints.Count - 1 : 0;
        int step = forward ? 1 : -1;

        for (int i = startIndex; i != endIndex; i += step)
        {
            Vector3 start = path.transform.TransformPoint(path.pathPoints[i]);
            Vector3 end = path.transform.TransformPoint(path.pathPoints[i + step]);

            // Optional offset adjustment (depending on sprite setup)
            end -= new Vector3(8, -28, 0);

            // --- Move toward endpoint ---
            while (Vector3.Distance(transform.position, end) > 1f)
            {
                Vector3 dir = (end - transform.position).normalized;

                // --- Handle temporary pauses ---
                if ((myCM.currentRoom != null && myCM.currentRoom.roomIsMoving) 
                    || 
                (myCM.currentInclineThreshold != null && myCM.currentInclineThreshold == npcCM.currentInclineThreshold)
                || stopMoving == true)
                {
                    npcCM.change = Vector3.zero; // freeze during room motion
                }
                else
                {
                    npcCM.change = dir; // normal path following
                }

                yield return null;
            }

            // Snap exactly to end
            transform.position = end;

            // Pause at each waypoint
            if (npcCM != null)
                npcCM.change = Vector3.zero;

            yield return new WaitForSeconds(pauseTime);
        }

        // ✅ Stop completely when reaching end (for testing)
        if (stopAtEnd && forward)
        {
            if (npcCM != null)
                npcCM.change = Vector3.zero;
        }
    }
}
