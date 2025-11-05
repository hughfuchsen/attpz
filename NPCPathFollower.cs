using System.Collections;
using UnityEngine;

public class NPCPathFollower : MonoBehaviour
{
    public PathDrawer path;
    public float pauseTime = 0.3f;

    private CharacterMovement characterMovement;
    IsoSpriteSorting isoSpriteSortingScript;
    private void Start()
    {
        StartCoroutine(LateStart());
    }

    private IEnumerator LateStart()
    {
        // Wait a frame to ensure CharacterMovement is initialized
        yield return null;
        characterMovement = GetComponent<CharacterMovement>();
        isoSpriteSortingScript = GetComponent<IsoSpriteSorting>();

        if (path == null || path.pathPoints.Count < 2)
        {
            Debug.LogWarning("NPCPathFollower: Path is missing or too short!");
            yield break;
        }

        yield return StartCoroutine(FollowPath());
    }

    private IEnumerator FollowPath()
    {

        for (int i = 0; i < path.pathPoints.Count - 1; i++)
        {
            Vector3 start = path.transform.TransformPoint(path.pathPoints[i]);
            Vector3 end = path.transform.TransformPoint(path.pathPoints[i + 1]);

            // Optional: adjust for sorting offset
            end -= isoSpriteSortingScript.SorterPositionOffset;

            while (Vector3.Distance(transform.position, end) > 0.5f)
            {
                Vector3 dir = (end - transform.position).normalized;
                
                // Apply to character movement
                if (characterMovement != null)
                    characterMovement.change = dir;
                    // characterMovement.MoveCharacter();
                yield return null;
            }

            // Snap exactly to the endpoint
            transform.position = end;


            // Stop moving briefly
            if (characterMovement != null)
                characterMovement.change = Vector3.zero;
                // characterMovement.MoveCharacter();

            yield return new WaitForSeconds(pauseTime);
        }

    }

}
