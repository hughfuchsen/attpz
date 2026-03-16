
// using System.Collections.Generic;
// using UnityEngine;

// public class GridNode : MonoBehaviour
// {
//     [Header("Grid Position")]
//     public char column;   // a, b, c, d
//     public int row;       // 1, 2, 3, 4

//     [Header("Connections")]
//     public List<GridNode> neighbours = new List<GridNode>();

//     [Header("Obstacle")]
//     public bool isBlocked = false;

//     public Vector2Int GridPosition => new Vector2Int(column - 'a', row - 1);

//     // private void OnDrawGizmos()
//     // {
//     //     // Node color: red = blocked, green = free
//     //     Gizmos.color = isBlocked ? Color.red : Color.green;
//     //     Gizmos.DrawSphere(transform.position, 0.05f);

//     //     // Draw connections to neighbours
//     //     Gizmos.color = Color.blue;
//     //     foreach (var n in neighbours)
//     //     {
//     //         if (n != null)
//     //             Gizmos.DrawLine(transform.position, n.transform.position);
//     //     }
//     // }
// }


using UnityEngine;

public class GridNode : MonoBehaviour
{
    [Header("Grid Position")]
    public char column;
    public int row;

    [Header("Obstacle")]
    public bool isBlocked = false;

    public Vector2Int GridPosition => new Vector2Int(column - 'a', row - 1);
}