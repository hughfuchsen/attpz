// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class NPCPathInitializer : MonoBehaviour
// {
//     public GridGenerator gridGenerator;    // Assign your GridGenerator in Inspector
//     public NPCPathFollower npcFollower;    // Assign the NPC in Inspector
//     public char targetColumn = 'd';        // Target column
//     public int targetRow = 4;              // Target row

//     void Start()
//     {
//         if (gridGenerator == null || npcFollower == null)
//         {
//             Debug.LogError("Assign gridGenerator and npcFollower!");
//             return;
//         }

//         StartCoroutine(InitPath());
//     }

//     private IEnumerator InitPath()
//     {
//         yield return null; // wait one frame for all components to initialize

//         // Get the NPC's current node
//         GridNode currentNode = npcFollower.currentNode;

//         if (currentNode == null)
//         {
//             Debug.LogWarning("NPC currentNode not assigned!");
//             yield break;
//         }

//         // Get target node
//         GridNode targetNode = gridGenerator.nodes[targetColumn - 'a', targetRow - 1];
//         if (targetNode == null)
//         {
//             Debug.LogWarning("Target node is null!");
//             yield break;
//         }

//         // Generate path as PathDrawer
//         PathDrawer pd = CreatePathDrawerFromNodes(currentNode, targetNode, gridGenerator);

//         if (pd == null || pd.pathPoints.Count < 2)
//         {
//             Debug.LogWarning("No valid path found for NPC!");
//             yield break;
//         }

//         // Assign path to NPCPathFollower
//         npcFollower.path = pd;

//         // Restart coroutine to follow the path
//         npcFollower.StopAllCoroutines();
//         npcFollower.StartCoroutine(npcFollower.LateStart());
//     }

//     private PathDrawer CreatePathDrawerFromNodes(GridNode start, GridNode target, GridGenerator grid)
//     {
//         // Convert GridGenerator nodes to blocked map
//         bool[,] blockedGrid = new bool[grid.width, grid.height];
//         for (int x = 0; x < grid.width; x++)
//             for (int y = 0; y < grid.height; y++)
//                 blockedGrid[x, y] = grid.nodes[x, y] == null || grid.nodes[x, y].isBlocked;

//         Vector2Int startPos = new Vector2Int(start.column - 'a', start.row - 1);
//         Vector2Int endPos = new Vector2Int(target.column - 'a', target.row - 1);

//         List<Vector2Int> pathIndices = Pathfinding.FindPath(blockedGrid, grid.width, grid.height, startPos, endPos);

//         if (pathIndices == null || pathIndices.Count == 0)
//             return null;

//         // Create PathDrawer on the fly
//         GameObject pathObj = new GameObject("DynamicPath");
//         PathDrawer pd = pathObj.AddComponent<PathDrawer>();
//         pd.pathPoints = new List<Vector3>();

//         foreach (Vector2Int nodePos in pathIndices)
//         {
//             GridNode node = grid.nodes[nodePos.x, nodePos.y];
//             pd.pathPoints.Add(node.transform.position);
//         }

//         return pd;
//     }
// }