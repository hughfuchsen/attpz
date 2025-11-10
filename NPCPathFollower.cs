using System.Collections;
using UnityEngine;

public class NPCPathFollower : MonoBehaviour
{
    public PathDrawer path;
    public float pauseTime = 0.3f;

    private CharacterMovement characterMovement;
    private CharacterMovement myCM;
    private IsoSpriteSorting isoSpriteSortingScript;

    private void Start()
    {
        StartCoroutine(LateStart());
    }

    private IEnumerator LateStart()
    {
        yield return null; // wait one frame for components to init
        characterMovement = GetComponent<CharacterMovement>();
        isoSpriteSortingScript = GetComponent<IsoSpriteSorting>();
        myCM = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();


        if (path == null || path.pathPoints.Count < 2)
        {
            Debug.LogWarning("NPCPathFollower: Path is missing or too short!");
            yield break;
        }

        // loop forever
        while (true)
        {
            yield return StartCoroutine(FollowPath(forward: true));   // go forward
            yield return StartCoroutine(FollowPath(forward: false));  // then backward
        }
    }

    private IEnumerator FollowPath(bool forward)
    {
        int startIndex = forward ? 0 : path.pathPoints.Count - 1;
        int endIndex   = forward ? path.pathPoints.Count - 1 : 0;
        int step       = forward ? 1 : -1;

        for (int i = startIndex; i != endIndex; i += step)
        {
            Vector3 start = path.transform.TransformPoint(path.pathPoints[i]);
            Vector3 end   = path.transform.TransformPoint(path.pathPoints[i + step]);

            // Optional: adjust for sorting offset
            end -= new Vector3(8, -28, 0);

            // Move toward end point
            while (Vector3.Distance(transform.position, end) > 0.5f)
            {
                Vector3 dir = (end - transform.position).normalized;

                if (characterMovement != null && !myCM.currentRoom.isMovingg)
                    characterMovement.change = dir;
                else if (characterMovement != null)
                    characterMovement.change = Vector3.zero;

                yield return null;
            }

            // snap exactly to endpoint
            transform.position = end;

            // pause briefly at each point
            if (characterMovement != null)
                characterMovement.change = Vector3.zero;

            yield return new WaitForSeconds(pauseTime);
        }
    }
}
