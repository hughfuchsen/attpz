// using System.Collections.Generic;
// using UnityEngine;

// public class PathFollowerScript : MonoBehaviour
// {
//     public float speed = 2f;
//     private GridNodeData targetNode;

//     private Queue<GridNodeData> pathQueue = new Queue<GridNodeData>();
//     private Vector3 moveTarget;
//     private bool isMoving = false;

//     public GridNodeData currentNode;
//     public GridGenerator gridGenerator;
//     public char startColumn = 'a';
//     public int startRow = 1;

//     void Start()
//     {
//         if (gridGenerator != null)
//         {
//             currentNode = gridGenerator.nodes[startColumn - 'a', startRow - 1];
//             transform.position = currentNode.worldPos;
//         }
//     }

//     void Update()
//     {
//         if (!isMoving) return;

//         transform.position = Vector3.MoveTowards(
//             transform.position,
//             moveTarget,
//             speed * Time.deltaTime
//         );


//         // Snap when close enough
//         if (Vector3.Distance(transform.position, moveTarget) < 0.001f)
//         {
//             transform.position = moveTarget;

//             if (pathQueue.Count > 0)
//             {
//                 currentNode = pathQueue.Dequeue();
//                 moveTarget = currentNode.worldPos;
//             }
//             else
//             {
//                 isMoving = false;
//             }
//         }
//     }

//     public void MoveTo(GridNodeData target)
//     {
//         if (currentNode == null || target == null) return;

//         targetNode = target;
//         RecalculatePath();
//     }

//     private void RecalculatePath()
//     {
//         if (gridGenerator == null) return;

//         // Convert GridNodeDatas to Grid indices
//         int width = gridGenerator.width;
//         int height = gridGenerator.height;
//         bool[,] blockedGrid = new bool[width, height];

//         for (int x = 0; x < width; x++)
//         {
//             for (int y = 0; y < height; y++)
//             {
//                 blockedGrid[x, y] = gridGenerator.nodes[x, y] == null || gridGenerator.nodes[x, y].isBlocked;
//             }
//         }

//         Vector2Int startPos = new Vector2Int(currentNode.column - 'a', currentNode.row - 1);
//         Vector2Int endPos = new Vector2Int(targetNode.column - 'a', targetNode.row - 1);

//         List<Vector2Int> path = Pathfinding.FindPath(blockedGrid, width, height, startPos, endPos);

//         pathQueue.Clear();

//         if (path != null && path.Count > 0)
//         {
//             foreach (Vector2Int nodePos in path)
//             {
//                 GridNodeData node = gridGenerator.nodes[nodePos.x, nodePos.y];
//                 if (node != null)
//                     pathQueue.Enqueue(node);
//             }

//             // Start moving immediately
//             currentNode = pathQueue.Dequeue();
//             moveTarget = currentNode.transform.position;
//             isMoving = true;
//         }
//     }
// }
// // using System.Collections.Generic;
// // using UnityEngine;

// // public class PathFollowerScript : MonoBehaviour
// // {
// //     public float speed = 2f;
// //     private GridNodeData targetNode;
// //     private Queue<GridNodeData> pathQueue = new Queue<GridNodeData>();
// //     private Vector3 moveTarget;
// //     private bool isMoving = false;

// //     public GridNodeData currentNode;
// //     public GridGenerator gridGenerator;
// //     public char startColumn = 'a';
// //     public int startRow = 1;

// //     // Cache paths: key = start+end node IDs, value = path list
// //     private static Dictionary<string, List<Vector2Int>> pathCache = new Dictionary<string, List<Vector2Int>>();

// //     void Start()
// //     {
// //         if (gridGenerator != null)
// //         {
// //             currentNode = gridGenerator.nodes[startColumn - 'a', startRow - 1];
// //             transform.position = currentNode.transform.position;
// //         }
// //     }

// //     void Update()
// //     {
// //         if (!isMoving) return;

// //         transform.position = Vector3.MoveTowards(transform.position, moveTarget, speed * Time.deltaTime);

// //         if (Vector3.Distance(transform.position, moveTarget) < 0.001f)
// //         {
// //             transform.position = moveTarget;

// //             if (pathQueue.Count > 0)
// //             {
// //                 currentNode = pathQueue.Dequeue();
// //                 moveTarget = currentNode.transform.position;
// //             }
// //             else
// //             {
// //                 isMoving = false;
// //             }
// //         }
// //     }

// //     public void MoveTo(GridNodeData target)
// //     {
// //         if (currentNode == null || target == null || gridGenerator == null) return;

// //         targetNode = target;
// //         RecalculatePath();
// //     }

// //     private void RecalculatePath()
// //     {
// //         // Generate key for caching
// //         string key = $"{currentNode.column}{currentNode.row}-{targetNode.column}{targetNode.row}";

// //         List<Vector2Int> path;

// //         if (!pathCache.TryGetValue(key, out path))
// //         {
// //             // Convert grid to blocked map
// //             int width = gridGenerator.width;
// //             int height = gridGenerator.height;
// //             bool[,] blockedGrid = new bool[width, height];

// //             for (int x = 0; x < width; x++)
// //                 for (int y = 0; y < height; y++)
// //                     blockedGrid[x, y] = gridGenerator.nodes[x, y] == null || gridGenerator.nodes[x, y].isBlocked;

// //             Vector2Int startPos = new Vector2Int(currentNode.column - 'a', currentNode.row - 1);
// //             Vector2Int endPos = new Vector2Int(targetNode.column - 'a', targetNode.row - 1);

// //             path = Pathfinding.FindPath(blockedGrid, width, height, startPos, endPos);

// //             // Store in cache
// //             if (path != null)
// //                 pathCache[key] = path;
// //         }

// //         // Convert path indices to GridNodeData queue
// //         pathQueue.Clear();
// //         if (path != null && path.Count > 0)
// //         {
// //             foreach (Vector2Int nodePos in path)
// //             {
// //                 GridNodeData node = gridGenerator.nodes[nodePos.x, nodePos.y];
// //                 if (node != null)
// //                     pathQueue.Enqueue(node);
// //             }

// //             // Start moving
// //             currentNode = pathQueue.Dequeue();
// //             moveTarget = currentNode.transform.position;
// //             isMoving = true;
// //         }
// //     }
// // }