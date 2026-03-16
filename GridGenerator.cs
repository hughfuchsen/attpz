// using System.Collections.Generic;
// using UnityEngine;

// public class GridGenerator : MonoBehaviour
// {
//     public GameObject gridNodePrefab; // prefab with GridNode + Trigger Collider2D + Rigidbody2D Kinematic
//     public int width = 4;
//     public int height = 4;

//     public bool[,] map; // true = walkable
//     public GridNode[,] nodes; 

//     [Header("Isometric Settings")]
//     public float tileWidth = 10f; 
//     public float tileHeight = 5f;


//     [Header("Obstacle Detection")]
//     public LayerMask wallLayer;
//     // public float obstacleCheckRadius = 0.1f;

//     Vector3 GridToIso(int x, int y)
//     {
//         float isoX = (x - y) * tileWidth * 1f;
//         float isoY = (x + y) * tileHeight * 1f;
//         return new Vector3(isoX, isoY, 0f);
//     }

//     void Awake()
//     {
//         if (map == null || map.Length == 0)
//         {
//             map = new bool[width, height];
//             for (int x = 0; x < width; x++)
//                 for (int y = 0; y < height; y++)
//                     map[x, y] = true;
//         }

//         nodes = new GridNode[width, height];
//         GenerateNodes();
//         GenerateNeighbours();
//     }


//     void GenerateNodes()
// {
//     for (int x = 0; x < width; x++)
//     {
//         for (int y = 0; y < height; y++)
//         {
//             if (!map[x, y]) continue;

//             Vector3 isoPos = GridToIso(x, y);

//             GameObject nodeObj = Instantiate(
//                 gridNodePrefab,
//                 isoPos,
//                 Quaternion.identity,
//                 transform
//             );

//             GridNode node = nodeObj.GetComponent<GridNode>();
//             node.column = (char)('a' + x);
//             node.row = y + 1;

//             // Do NOT determine blocked yet — will be dynamic per NPC level
//             node.isBlocked = false;
//             node.transform.position += transform.position;
//             nodes[x, y] = node;
//         }
//     }
// }

//     public void UpdateNodeViability(GameObject character, LevelScript currentLevel)
//     {
//         int levelLayer;

//         if (currentLevel == null)
//         {
//             levelLayer = character.layer;
//         }
//         else
//         {
//             levelLayer = currentLevel.gameObject.layer;
//         }

            

//         for (int x = 0; x < width; x++)
//         {
//             for (int y = 0; y < height; y++)
//             {
//                 GridNode node = nodes[x, y];
//                 if (node == null) continue;

//                 // Check obstacles dynamically
//                 Collider2D[] hits = Physics2D.OverlapCircleAll(node.transform.position, tileWidth);
//                 bool blocked = false;

//                 foreach (var hit in hits)
//                 {
//                     if (hit == null) continue;

//                     // Ignore triggers
//                     if (hit.isTrigger) continue;

//                     // Ignore Player/NPC colliders
//                     if (hit.gameObject.CompareTag("PlayerCollider") || hit.gameObject.CompareTag("NPCCollider"))
//                         continue;

//                     // Only consider colliders on this level
//                     if (hit.gameObject.layer != levelLayer)
//                         continue;

//                     // Anything else on this level counts as obstacle
//                     blocked = true;
//                     break;
//                 }

//                 node.isBlocked = blocked;
//             }
//         }
//     }

//     void GenerateNeighbours()
//     {
//         for (int x = 0; x < width; x++)
//         {
//             for (int y = 0; y < height; y++)
//             {
//                 GridNode node = nodes[x, y];
//                 if (node == null || node.isBlocked) continue;

//                 TryAddNeighbour(node, x - 1, y);
//                 TryAddNeighbour(node, x + 1, y);
//                 TryAddNeighbour(node, x, y - 1);
//                 TryAddNeighbour(node, x, y + 1);
//             }
//         }
//     }

//     void TryAddNeighbour(GridNode node, int x, int y)
//     {
//         if (x < 0 || y < 0 || x >= width || y >= height)
//             return;

//         GridNode neighbour = nodes[x, y];
//         if (neighbour == null || neighbour.isBlocked)
//             return;

//         node.neighbours.Add(neighbour);
//     }

//     // void OnDrawGizmosSelected()
//     // {
//     //     if (nodes == null) return;

//     //     Gizmos.color = Color.yellow;
//     //     foreach (var node in nodes)
//     //     {
//     //         if (node != null)
//     //             Gizmos.DrawWireSphere(node.transform.position, tileWidth);
//     //     }
//     // }

//     // void OnDrawGizmosSelected()
//     // {
//     //     Gizmos.color = Color.yellow;

//     //     for (int x = 0; x < width; x++)
//     //     {
//     //         for (int y = 0; y < height; y++)
//     //         {
//     //             Vector3 pos = GridToIso(x, y) + transform.position;

//     //             Gizmos.DrawWireSphere(pos, tileWidth * 0.25f);
//     //         }
//     //     }
//     // }

//     // void OnDrawGizmos()
//     // {
//     //     if (width <= 0 || height <= 0)
//     //         return;

//     //     for (int x = 0; x < width; x++)
//     //     {
//     //         for (int y = 0; y < height; y++)
//     //         {
//     //             Vector3 center = GridToIso(x, y) + transform.position;

//     //             Gizmos.color = Color.yellow;
//     //             DrawIsoTile(center);
//     //         }
//     //     }
//     // }

//     // void DrawIsoTile(Vector3 center)
//     // {
//     //     Vector3 top = center + new Vector3(0, tileHeight, 0);
//     //     Vector3 right = center + new Vector3(tileWidth, 0, 0);
//     //     Vector3 bottom = center + new Vector3(0, -tileHeight, 0);
//     //     Vector3 left = center + new Vector3(-tileWidth, 0, 0);

//     //     Gizmos.DrawLine(top, right);
//     //     Gizmos.DrawLine(right, bottom);
//     //     Gizmos.DrawLine(bottom, left);
//     //     Gizmos.DrawLine(left, top);
//     // }

// }


// using UnityEngine;

// public class GridGenerator : MonoBehaviour
// {
//     public GameObject gridNodePrefab;

//     public int width = 4;
//     public int height = 4;

//     public bool[,] map;
//     public GridNode[,] nodes;

//     [Header("Isometric Settings")]
//     public float tileWidth = 2f;
//     public float tileHeight = 1f;

//     Vector3 GridToIso(int x, int y)
//     {
//         float isoX = (x - y) * tileWidth;
//         float isoY = (x + y) * tileHeight;

//         return new Vector3(isoX, isoY, 0);
//     }

//     void Awake()
//     {
//         if (map == null || map.Length == 0)
//         {
//             map = new bool[width, height];

//             for (int x = 0; x < width; x++)
//                 for (int y = 0; y < height; y++)
//                     map[x, y] = true;
//         }

//         nodes = new GridNode[width, height];

//         GenerateNodes();
//     }

//     void GenerateNodes()
//     {
//         for (int x = 0; x < width; x++)
//         {
//             for (int y = 0; y < height; y++)
//             {
//                 if (!map[x, y]) continue;

//                 Vector3 isoPos = GridToIso(x, y) + transform.position;

//                 GameObject obj = Instantiate(
//                     gridNodePrefab,
//                     isoPos,
//                     Quaternion.identity,
//                     transform
//                 );

//                 GridNode node = obj.GetComponent<GridNode>();

//                 node.column = (char)('a' + x);
//                 node.row = y + 1;

//                 nodes[x, y] = node;
//             }
//         }
//     }

//     public void UpdateNodeViability(GameObject character, LevelScript currentLevel)
//     {
//         int levelLayer = currentLevel != null ? currentLevel.gameObject.layer : character.layer;

//         for (int x = 0; x < width; x++)
//         {
//             for (int y = 0; y < height; y++)
//             {
//                 GridNode node = nodes[x, y];
//                 if (node == null) continue;

//                 Collider2D[] hits = Physics2D.OverlapCircleAll(node.transform.position, tileHeight);

//                 bool blocked = false;
//                 bool touchingThreshold = false;

//                 foreach (var hit in hits)
//                 {
//                     if (hit == null) continue;

//                     // 🔹 If a threshold touches the node, it overrides everything
//                     if  
//                     (hit.GetComponent<RoomThresholdColliderScript>() != null 
//                     || hit.GetComponent<LevelThreshColliderScript>() != null
//                     || hit.GetComponent<BuildingThreshColliderScript>() != null)
//                     {
//                         touchingThreshold = true;
//                         break;
//                     }

//                     // Ignore player/NPC colliders
//                     if (hit.CompareTag("PlayerCollider") || hit.CompareTag("NPCCollider") || hit.CompareTag("NPC"))
//                         continue;
                   
//                     if (hit.isTrigger)
//                         continue;

//                     // Only consider objects on the current level
//                     if (hit.gameObject.layer != levelLayer)
//                         continue;

//                     blocked = true;
//                 }

//                 // Trigger presence overrides blocking
//                 node.isBlocked = touchingThreshold ? false : blocked;
//             }
//         }
//     }

    // void OnDrawGizmos()
    // {
    //     if (width <= 0 || height <= 0)
    //         return;

    //     for (int x = 0; x < width; x++)
    //     {
    //         for (int y = 0; y < height; y++)
    //         {
    //             Vector3 pos = GridToIso(x, y) + transform.position;

    //             // Draw tile diamond
    //             Gizmos.color = Color.yellow;
    //             DrawIsoTile(pos);

    //             // Draw node state if nodes exist
    //             if (nodes != null &&
    //                 x < nodes.GetLength(0) &&
    //                 y < nodes.GetLength(1))
    //             {
    //                 GridNode node = nodes[x, y];

    //                 if (node != null)
    //                 {
    //                     Gizmos.color = node.isBlocked ? Color.red : Color.green;
    //                     Gizmos.DrawWireSphere(node.transform.position, tileHeight);
    //                 }
    //             }
    //         }
    //     }
    // }

    // void DrawIsoTile(Vector3 center)
    // {
    //     Vector3 top = center + new Vector3(0, tileHeight, 0);
    //     Vector3 right = center + new Vector3(tileWidth, 0, 0);
    //     Vector3 bottom = center + new Vector3(0, -tileHeight, 0);
    //     Vector3 left = center + new Vector3(-tileWidth, 0, 0);

    //     Gizmos.DrawLine(top, right);
    //     Gizmos.DrawLine(right, bottom);
    //     Gizmos.DrawLine(bottom, left);
    //     Gizmos.DrawLine(left, top);
    // }
// }

using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    public int width = 1000;
    public int height = 1000;

    [Header("Isometric Settings")]
    public float tileWidth = 4f;
    public float tileHeight = 2f;

    // Data-only grid
    public GridNodeData[,] nodes;

    void Awake()
    {
        nodes = new GridNodeData[width, height];

        GenerateNodes();
    }

    void GenerateNodes()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                nodes[x, y] = new GridNodeData
                {
                    gridPos = new Vector2Int(x, y),
                    worldPos = GridToIso(x, y) + transform.position,
                    isBlocked = false
                };
            }
        }
    }

    Vector3 GridToIso(int x, int y)
    {
        float isoX = (x - y) * tileWidth;
        float isoY = (x + y) * tileHeight;
        return new Vector3(isoX, isoY, 0f);
    }

    // Updates blocking info for pathfinding
    public void UpdateNodeViability(GameObject character, LevelScript currentLevel)
    {
        int levelLayer = currentLevel != null ? currentLevel.gameObject.layer : character.layer;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GridNodeData node = nodes[x, y];
                if (node == null) continue;

                Collider2D[] hits = Physics2D.OverlapCircleAll(node.worldPos, tileHeight);

                bool blocked = false;
                bool touchingThreshold = false;

                foreach (var hit in hits)
                {
                    if (hit == null) continue;

                    // Thresholds override blocking
                    if (hit.GetComponent<RoomThresholdColliderScript>() != null ||
                        hit.GetComponent<LevelThreshColliderScript>() != null ||
                        hit.GetComponent<BuildingThreshColliderScript>() != null)
                    {
                        touchingThreshold = true;
                        break;
                    }

                    if (hit.CompareTag("PlayerCollider") || hit.CompareTag("NPCCollider") || hit.CompareTag("NPC"))
                        continue;

                    if (hit.isTrigger)
                        continue;

                    if (hit.gameObject.layer != levelLayer)
                        continue;

                    blocked = true;
                }

                node.isBlocked = touchingThreshold ? false : blocked;
            }
        }
    }

    // Optional: draw debug grid
    // void OnDrawGizmos()
    // {
    //     if (nodes == null) return;

    //     for (int x = 0; x < width; x++)
    //     {
    //         for (int y = 0; y < height; y++)
    //         {
    //             GridNodeData node = nodes[x, y];
    //             if (node == null) continue;

    //             Gizmos.color = node.isBlocked ? Color.red : Color.green;
    //             Gizmos.DrawWireSphere(node.worldPos, tileHeight * 0.25f);
    //         }
    //     }
    // }
}

// Lightweight data-only node class
public class GridNodeData
{
    public Vector2Int gridPos; // node coordinates
    public Vector3 worldPos;   // actual world position
    public bool isBlocked;     // blocked for pathfinding
}