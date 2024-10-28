// using UnityEngine;
// using System.Collections;

// public class EdgeTriggerEndHit : MonoBehaviour
// {
//     public float centerThreshold = 3f;  // Adjust this value to define how close to the center the player needs to be
//     public float checkInterval = 0.1f;  // Optional: check interval for position in seconds
//     private EdgeCollider2D edgeCollider;
//     private Coroutine checkCenterCoroutine;

//     private Vector2 playerPosition;
//     private Vector2 playerOffset = new Vector2(8f, -28f);  // Offset for player's position (X: 8, Y: -28)
//     private Vector2 playerPositionCollisionPointOffset;
//     Vector2 collisionPointOffset;
//     public bool aboveThreshold;
//     public bool leftDiagThreshold;

//     PlayerMovement playerMovement;

//     // Player offset values

//     void Start()
//     {
//         edgeCollider = GetComponent<EdgeCollider2D>();
//         playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
//         playerPosition = (Vector2)playerMovement.transform.position + playerOffset;

//     }

//     void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             playerMovement.cantGoLeftDownMustGoLeftUp = false;
//             playerMovement.cantGoLeftDownMustGoRightDown = false;
//             playerMovement.cantGoLeftUpMustGoLeftDown = false;
//             playerMovement.cantGoLeftUpMustGoRightUp = false;
//             playerMovement.cantGoRightDownMustGoLeftDown = false;
//             playerMovement.cantGoRightDownMustGoRightUp = false;
//             playerMovement.cantGoRightUpMustGoLeftUp = false;
//             playerMovement.cantGoRightUpMustGoRightDown = false;

//             // Stop the coroutine when the player exits the trigger
//             if (checkCenterCoroutine != null)
//             {
//                 StopCoroutine(checkCenterCoroutine);
//                 checkCenterCoroutine = null;
//             }
//             // Get the exact collision point of the player's collider
//             collisionPointOffset = GetColliderContactPoint(other);

//             playerPositionCollisionPointOffset = playerPosition - collisionPointOffset;
            
//             // Start the coroutine to check if the player is at the center
//             checkCenterCoroutine = StartCoroutine(CheckIfPlayerIsAtCenter());



//             Vector2 leftEnd = transform.TransformPoint(edgeCollider.points[0]);
//             Vector2 rightEnd = transform.TransformPoint(edgeCollider.points[edgeCollider.pointCount - 1]);

//             Vector2 centerPoint = (leftEnd + rightEnd) / 2;

//             if(leftDiagThreshold)
//             {
//                 if (IsLeftOfCenter(playerPosition, centerPoint))
//                 {
//                     if(aboveThreshold)
//                     {
//                         playerMovement.cantGoRightDownMustGoRightUp = true;
//                     }
//                     else
//                     {
//                         playerMovement.cantGoLeftUpMustGoRightUp = true;
//                     }
//                 }
//                 else
//                 {
//                     if(aboveThreshold)
//                     {
//                         playerMovement.cantGoRightDownMustGoLeftDown = true;
//                     }
//                     else
//                     {
//                         playerMovement.cantGoLeftUpMustGoLeftDown = true;
//                     }
//                 }
//             }
//             else
//             {
//                 if (IsLeftOfCenter(playerPosition, centerPoint))
//                 {
//                     if(aboveThreshold)
//                     {
//                         playerMovement.cantGoLeftDownMustGoRightDown = true;
//                     }
//                     else
//                     {
//                         playerMovement.cantGoRightUpMustGoRightDown = true;
//                     }
//                 }
//                 else
//                 {
//                     if(aboveThreshold)
//                     {
//                         playerMovement.cantGoLeftDownMustGoLeftUp = true;
//                     }
//                     else
//                     {
//                         playerMovement.cantGoRightUpMustGoLeftUp = true;
//                     }
//                 }
//             }
//         }
//     }

//     void OnTriggerExit2D(Collider2D other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             playerMovement.cantGoLeftDownMustGoLeftUp = false;
//             playerMovement.cantGoLeftDownMustGoRightDown = false;
//             playerMovement.cantGoLeftUpMustGoLeftDown = false;
//             playerMovement.cantGoLeftUpMustGoRightUp = false;
//             playerMovement.cantGoRightDownMustGoLeftDown = false;
//             playerMovement.cantGoRightDownMustGoRightUp = false;
//             playerMovement.cantGoRightUpMustGoLeftUp = false;
//             playerMovement.cantGoRightUpMustGoRightDown = false;

//             if (checkCenterCoroutine != null)
//             {
//                 StopCoroutine(checkCenterCoroutine);
//                 checkCenterCoroutine = null;
//             }
//         }
//     }

//     private Vector2 GetColliderContactPoint(Collider2D playerCollider)
//     {
//         // If using a BoxCollider2D, CircleCollider2D, or any other 2D collider
//         ContactPoint2D[] contactPoints = new ContactPoint2D[1];
//         playerCollider.GetContacts(contactPoints);
//         // Debug.Log(contactPoints[0].point);
//         return contactPoints[0].point;  // Returns the exact point of contact
//     }

//     private bool IsLeftOfCenter(Vector2 playerPosition, Vector2 centerPoint)
//     {
//         return playerPosition.x < centerPoint.x;
//     }

//     private IEnumerator CheckIfPlayerIsAtCenter()
//     {
//         Vector2 leftEnd = transform.TransformPoint(edgeCollider.points[0]);
//         Vector2 rightEnd = transform.TransformPoint(edgeCollider.points[edgeCollider.pointCount - 1]);
//         Vector2 centerPoint = (leftEnd + rightEnd) / 2;

//         while (true)
//         {
//             // Vector2 playerPosition = (Vector2)playerTransform.position + playerOffset;
//             float distanceToCenter = Vector2.Distance(playerPosition + playerPositionCollisionPointOffset, centerPoint);

//             if (distanceToCenter <= centerThreshold)
//             {
//                 Debug.Log("Player is at the center of the edge collider.");
//                 playerMovement.cantGoLeftDownMustGoLeftUp = false;
//                 playerMovement.cantGoLeftDownMustGoRightDown = false;
//                 playerMovement.cantGoLeftUpMustGoLeftDown = false;
//                 playerMovement.cantGoLeftUpMustGoRightUp = false;
//                 playerMovement.cantGoRightDownMustGoLeftDown = false;
//                 playerMovement.cantGoRightDownMustGoRightUp = false;
//                 playerMovement.cantGoRightUpMustGoLeftUp = false;
//                 playerMovement.cantGoRightUpMustGoRightDown = false;  
//                 yield break;
//             }

//             yield return new WaitForSeconds(checkInterval);
//         }
//     }
// }


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
    private Vector2 playerPositionCollisionPointOffset;
    Vector2 collisionPointOffset;
    public bool aboveThreshold;
    public bool leftDiagThreshold;

    PlayerMovement playerMovement;

    void Start()
    {
        // Get references for player, player movement, and player collider
        edgeCollider = GetComponent<EdgeCollider2D>();
        player = GameObject.FindWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        playerCollider = player.GetComponent<BoxCollider2D>();
        
 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement.ResetPlayerMovement();  // Reset movement restrictions

            if (checkCenterCoroutine != null)
            {
                StopCoroutine(checkCenterCoroutine);
                checkCenterCoroutine = null;
            }
            
            Vector2 leftEnd = transform.TransformPoint(edgeCollider.points[0]);
            Vector2 rightEnd = transform.TransformPoint(edgeCollider.points[edgeCollider.pointCount - 1]);
            Vector2 centerPoint = (leftEnd + rightEnd) / 2;

            // Handle movement restrictions based on thresholds
            HandleMovementRestrictions(playerPosition, centerPoint);
           
            // Start the coroutine to check if the player is at the center
            checkCenterCoroutine = StartCoroutine(CheckIfPlayerIsAtCenter());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement.ResetPlayerMovement();  // Reset movement restrictions

            if (checkCenterCoroutine != null)
            {
                StopCoroutine(checkCenterCoroutine);
                checkCenterCoroutine = null;
            }
        }
    }

    private Vector2 GetColliderContactPoint(BoxCollider2D playerCollider)
    {
        ContactPoint2D[] contactPoints = new ContactPoint2D[10]; // Increased array size
        int contactCount = playerCollider.GetContacts(contactPoints);

        if (contactCount > 0)
        {
            Debug.Log("Contact Point: " + contactPoints[0].point);
            return contactPoints[0].point;  // Return the first point of contact
        }
        else
        {
            Debug.LogWarning("No collision points found!");
            return Vector2.zero;  // Return zero if no contact points are found
        }
    }

    private IEnumerator CheckIfPlayerIsAtCenter()
    {
        Vector2 leftEnd = transform.TransformPoint(edgeCollider.points[0]);
        Vector2 rightEnd = transform.TransformPoint(edgeCollider.points[edgeCollider.pointCount - 1]);
        Vector2 centerPoint = (leftEnd + rightEnd) / 2;
        Debug.Log("centerpoint" + centerPoint);


        while (true)
        {
            // Update the player's position during runtime
            playerPosition = (Vector2)player.transform.position + playerOffset;            
            collisionPointOffset = GetColliderContactPoint(playerCollider);
            Vector2 adjustedPlayerPosition = playerPosition + collisionPointOffset;
            float distanceToCenter = Vector2.Distance(adjustedPlayerPosition, centerPoint);
            Debug.Log("dist to center:" + distanceToCenter);

            if (distanceToCenter <= centerThreshold)
            {
                Debug.Log("Player is at the center of the edge collider.");
                playerMovement.ResetPlayerMovement();  // Reset restrictions once at center
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
            else
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



    private bool IsLeftOfCenter(Vector2 playerPosition, Vector2 centerPoint)
    {
        return playerPosition.x < centerPoint.x;
    }
}
