// using System.Collections.Generic;
// using UnityEngine;

// [RequireComponent(typeof(Collider2D))]
// [RequireComponent(typeof(Rigidbody2D))]
// public class GridNode : MonoBehaviour
// {
//     [Header("Grid Position")]
//     public char column;   // a, b, c, d
//     public int row;       // 1, 2, 3, 4

//     [Header("Connections")]
//     public List<GridNode> neighbours = new List<GridNode>();

//     [Header("Obstacle")]
//     public bool isBlocked = false; // automatically updated

//     private int blockingColliders = 0; // counts overlapping obstacles

//     public Vector2Int GridPosition => new Vector2Int(column - 'a', row - 1);

//     void Awake()
//     {
//         // Ensure we have a Kinematic Rigidbody2D for trigger detection
//         Rigidbody2D rb = GetComponent<Rigidbody2D>();
//         rb.bodyType = RigidbodyType2D.Kinematic;
//         rb.simulated = true;

//         // Detect static walls at start
//         int wallLayer = LayerMask.GetMask("Wall"); // put all walls in a "Wall" layer
//         Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.5f, wallLayer);

//         if (hits.Length > 0)
//         {
//             isBlocked = true;
//             blockingColliders = hits.Length;
//             Debug.Log($"{name} blocked at start by {hits.Length} wall(s)");
//         }
//     }


//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.CompareTag("Dot") || other.CompareTag("GridNode")) return;

//         blockingColliders++;
//         isBlocked = true;
//         Debug.Log($"{name} blocked by {other.name}");
//     }

//     private void OnTriggerExit2D(Collider2D other)
//     {
//         if (other.CompareTag("Dot") || other.CompareTag("GridNode")) return;

//         blockingColliders--;
//         if (blockingColliders <= 0)
//         {
//             blockingColliders = 0;
//             isBlocked = false;
//             Debug.Log($"{name} unblocked after {other.name} left");
//         }
//     }

//     private void OnDrawGizmos()
//     {
//         // Node color: red = blocked, green = free
//         Gizmos.color = isBlocked ? Color.red : Color.green;
//         Gizmos.DrawSphere(transform.position, 0.05f);

//         // Draw connections to neighbours
//         Gizmos.color = Color.blue;
//         foreach (var n in neighbours)
//         {
//             if (n != null) Gizmos.DrawLine(transform.position, n.transform.position);
//         }
//     }
// }


using System.Collections.Generic;
using UnityEngine;

public class GridNode : MonoBehaviour
{
    [Header("Grid Position")]
    public char column;   // a, b, c, d
    public int row;       // 1, 2, 3, 4

    [Header("Connections")]
    public List<GridNode> neighbours = new List<GridNode>();

    [Header("Obstacle")]
    public bool isBlocked = false;

    public Vector2Int GridPosition => new Vector2Int(column - 'a', row - 1);

    private void OnDrawGizmos()
    {
        // Node color: red = blocked, green = free
        Gizmos.color = isBlocked ? Color.red : Color.green;
        Gizmos.DrawSphere(transform.position, 0.05f);

        // Draw connections to neighbours
        Gizmos.color = Color.blue;
        foreach (var n in neighbours)
        {
            if (n != null)
                Gizmos.DrawLine(transform.position, n.transform.position);
        }
    }
}
