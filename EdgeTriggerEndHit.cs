using UnityEngine;
using System.Collections;

public class EdgeTriggerEndHit : MonoBehaviour
{
    public float centerThreshold = 3f;  // Adjust this value to define how close to the center the player needs to be
    public float checkInterval = 0.1f;  // Optional: check interval for position in seconds
    private EdgeCollider2D edgeCollider;
    private Coroutine checkCenterCoroutine;

    [SerializeField] GameObject player;
    [SerializeField] BoxCollider2D playerCollider;
    
    private Vector2 playerPosition;
    private Vector2 playerOffset = new Vector2(8f, -28f);  // Offset for player's position (X: 8, Y: -28)
    private Vector2 playerPositionWithCollisionPointOffest;
    Vector2 collisionPointOffset;
    public bool aboveThreshold;  
    public bool leftDiagThreshold;

    Vector2 leftEnd;
    Vector2 rightEnd;
    Vector2 centerPoint;

    CharacterMovement playerMovement; 

    void Start()
    {
        // Get references for player, player movement, and player collider
        edgeCollider = GetComponent<EdgeCollider2D>();
        player = GameObject.FindWithTag("Player");
        playerMovement = player.GetComponent<CharacterMovement>();
        playerCollider = player.GetComponent<BoxCollider2D>();

        leftEnd = transform.TransformPoint(edgeCollider.points[0]);
        // Debug.Log("left end" + leftEnd);

        rightEnd = transform.TransformPoint(edgeCollider.points[edgeCollider.pointCount - 1]);
        // Debug.Log("right end" + rightEnd);


        centerPoint = (leftEnd + rightEnd) / 2;
        // Debug.Log("centre point" + centerPoint);

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            playerMovement.velocityLock = false;  // Reset movement restrictions

            if (checkCenterCoroutine != null)
            {
                StopCoroutine(checkCenterCoroutine);
                checkCenterCoroutine = null;
            }
           
            // Update the player's position during runtime
            playerPosition = (Vector2)player.transform.position + playerOffset;
            // Debug.Log(GetColliderContactPoint(playerCollider));
            
            
            
            if(!playerMovement.playerOnThresh)
            {
                playerMovement.ResetPlayerMovement();  // Reset movement restrictions

                // Handle movement restrictions based on thresholds
                HandleMovementRestrictions(playerPosition, centerPoint);

                // Start the coroutine to check if the player is at the center
                checkCenterCoroutine = StartCoroutine(CheckIfPlayerIsAtCenter());


            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            if (checkCenterCoroutine != null)
            {
                StopCoroutine(checkCenterCoroutine);
                checkCenterCoroutine = null;
            }
            if(!playerMovement.playerOnThresh)
            {
                playerMovement.ResetPlayerMovement();  // Reset movement restrictions
            }
        }
    }

    private Vector2 GetColliderContactPoint(BoxCollider2D playerCollider)
    {
        ContactPoint2D[] contactPoints = new ContactPoint2D[10]; // Increased array size
        int contactCount = playerCollider.GetContacts(contactPoints);
        // Debug.Log(contactCount);
        if (contactCount > 0)
        {
            // Debug.Log("Contact Point: " + contactPoints[0].point);
            return contactPoints[0].point;  // Return the first point of contact
        }
        else
        {
            Debug.LogWarning("No collision points found!");
            return Vector2.zero;  // Return zero if no contact points are found
        }
    }

    // private IEnumerator CheckIfPlayerIsAtCenter()
    // {
    //     while (true)
    //     {
    //         // Update the player's position during runtime
    //         playerPosition = (Vector2)player.transform.position + playerOffset;            
    //         float distanceToCenter = Vector2.Distance(playerPosition.x, centerPoint);
    //         Debug.Log("dist to center:" + distanceToCenter);
    //         Debug.Log("playerPos:" + playerPosition);

    //         if (distanceToCenter <= centerThreshold)
    //         {
    //             // Debug.Log("Player is at the center of the edge collider.");
    //             playerMovement.ResetPlayerMovement();  // Reset restrictions once at center
    //             yield break;
    //         }

    //         yield return new WaitForSeconds(checkInterval);
    //     }
    // }


    private IEnumerator CheckIfPlayerIsAtCenter()
    {
        while (true)
        {
            // Update the player's position during runtime
            playerPosition = (Vector2)player.transform.position + playerOffset;
            float distanceToCenterX = Mathf.Abs(playerPosition.x - centerPoint.x); // Check X distance only
            // Debug.Log("Distance to center (X): " + distanceToCenterX);
            // Debug.Log("Player position: " + playerPosition);

            if (distanceToCenterX <= centerThreshold)
            {
                // playerMovement.ResetPlayerMovement(); // Reset restrictions once at center
                HandleMovementRestrictionsIntoThresh();
                yield break;
            }

            yield return new WaitForSeconds(checkInterval);
        }
    }


    private void HandleMovementRestrictions(Vector2 playerPosition, Vector2 centerPoint)
    {
        if (leftDiagThreshold)
        {
            if (IsLeftOfCenter(playerPosition, centerPoint))
            {
                if (aboveThreshold)
                {
                    // Debug.Log("leftAbove");
                    playerMovement.controlDirectionToPlayerDirection[Direction.DownFacingRight] = Direction.UpRight;
                    playerMovement.controlDirectionToPlayerDirection[Direction.RightDown] = Direction.UpRight;
                    playerMovement.controlDirectionToPlayerDirection[Direction.Right] = Direction.UpRight;
                }
                else
                {
                    // Debug.Log("leftBelow");
                    playerMovement.controlDirectionToPlayerDirection[Direction.UpLeft] = Direction.UpRight;
                    playerMovement.controlDirectionToPlayerDirection[Direction.UpFacingLeft] = Direction.UpRight;
                    playerMovement.controlDirectionToPlayerDirection[Direction.UpFacingRight] = Direction.UpRight;
                }
            }
            else // is right of center
            {
                if (aboveThreshold) // if this trigger sits above the thresh
                {
                    // Debug.Log("rightAbove");
                    playerMovement.controlDirectionToPlayerDirection[Direction.RightDown] = Direction.DownLeft;
                    playerMovement.controlDirectionToPlayerDirection[Direction.DownFacingRight] = Direction.DownLeft;
                    playerMovement.controlDirectionToPlayerDirection[Direction.DownFacingLeft] = Direction.DownLeft;
                }
                else
                {
                    // Debug.Log("rightBelow");
                    playerMovement.controlDirectionToPlayerDirection[Direction.UpFacingLeft] = Direction.DownLeft;
                    playerMovement.controlDirectionToPlayerDirection[Direction.UpLeft] = Direction.DownLeft;
                    playerMovement.controlDirectionToPlayerDirection[Direction.Left] = Direction.DownLeft;
                }
            }
        }
        else // right threshold direction
        {
            if (IsLeftOfCenter(playerPosition, centerPoint))
            {
                if (aboveThreshold)
                {
                    // Debug.Log("leftAbove");
                    playerMovement.controlDirectionToPlayerDirection[Direction.DownLeft] = Direction.RightDown;
                    playerMovement.controlDirectionToPlayerDirection[Direction.DownFacingLeft] = Direction.RightDown;
                    playerMovement.controlDirectionToPlayerDirection[Direction.DownFacingRight] = Direction.RightDown;
                }
                else
                {
                    // Debug.Log("leftBelow");
                    playerMovement.controlDirectionToPlayerDirection[Direction.UpFacingRight] = Direction.RightDown;
                    playerMovement.controlDirectionToPlayerDirection[Direction.UpRight] = Direction.RightDown;
                    playerMovement.controlDirectionToPlayerDirection[Direction.Right] = Direction.RightDown;
                }
            }
            else // is right of centre
            {
                if (aboveThreshold)
                {
                    // Debug.Log("rightAbove");
                    playerMovement.controlDirectionToPlayerDirection[Direction.DownFacingLeft] = Direction.UpLeft;
                    playerMovement.controlDirectionToPlayerDirection[Direction.DownLeft] = Direction.UpLeft;
                    playerMovement.controlDirectionToPlayerDirection[Direction.Left] = Direction.UpLeft;

                }
                else
                {
                    // Debug.Log("rightBelow");x
                    playerMovement.controlDirectionToPlayerDirection[Direction.UpRight] = Direction.UpLeft;
                    playerMovement.controlDirectionToPlayerDirection[Direction.UpFacingRight] = Direction.UpLeft;
                    playerMovement.controlDirectionToPlayerDirection[Direction.UpFacingLeft] = Direction.UpLeft;
                }
            }
        }
    }
    private void HandleMovementRestrictionsIntoThresh()
    {
        if (leftDiagThreshold)
        {
            playerMovement.controlDirectionToPlayerDirection[Direction.Left] = Direction.UpLeft;
            playerMovement.controlDirectionToPlayerDirection[Direction.UpLeft] = Direction.UpLeft;
            playerMovement.controlDirectionToPlayerDirection[Direction.UpFacingLeft] = Direction.UpLeft;
            playerMovement.controlDirectionToPlayerDirection[Direction.UpFacingRight] = Direction.UpLeft;
            // playerMovement.controlDirectionToPlayerDirection[Direction.UpRight] = Direction.UpLeft;
            playerMovement.controlDirectionToPlayerDirection[Direction.Right] = Direction.RightDown;
            playerMovement.controlDirectionToPlayerDirection[Direction.RightDown] = Direction.RightDown;
            playerMovement.controlDirectionToPlayerDirection[Direction.DownFacingRight] = Direction.RightDown;
            playerMovement.controlDirectionToPlayerDirection[Direction.DownFacingLeft] = Direction.RightDown;
            // playerMovement.controlDirectionToPlayerDirection[Direction.DownLeft] = Direction.RightDown;
        }
        else // right threshold direction
        {
            playerMovement.controlDirectionToPlayerDirection[Direction.Left] = Direction.DownLeft;
            // playerMovement.controlDirectionToPlayerDirection[Direction.UpLeft] = Direction.UpRight;
            playerMovement.controlDirectionToPlayerDirection[Direction.UpFacingLeft] = Direction.UpRight;
            playerMovement.controlDirectionToPlayerDirection[Direction.UpFacingRight] = Direction.UpRight;
            playerMovement.controlDirectionToPlayerDirection[Direction.UpRight] = Direction.UpRight;
            playerMovement.controlDirectionToPlayerDirection[Direction.Right] = Direction.UpRight;
            // playerMovement.controlDirectionToPlayerDirection[Direction.RightDown] = Direction.DownLeft;
            playerMovement.controlDirectionToPlayerDirection[Direction.DownFacingRight] = Direction.DownLeft;
            playerMovement.controlDirectionToPlayerDirection[Direction.DownFacingLeft] = Direction.DownLeft;
            playerMovement.controlDirectionToPlayerDirection[Direction.DownLeft] = Direction.DownLeft;
        }
    }



    private bool IsLeftOfCenter(Vector2 playerPosition, Vector2 centerPoint)
    {
        return playerPosition.x < centerPoint.x;
    }
}
