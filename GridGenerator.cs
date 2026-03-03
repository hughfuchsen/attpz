using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject gridNodePrefab; // prefab with GridNode + Trigger Collider2D + Rigidbody2D Kinematic
    public int width = 4;
    public int height = 4;

    public bool[,] map; // true = walkable
    public GridNode[,] nodes; 

    [Header("Isometric Settings")]
    public float tileWidth = 10f;
    public float tileHeight = 5f;


    [Header("Obstacle Detection")]
    public LayerMask wallLayer;
    public float obstacleCheckRadius = 0.1f;

    Vector3 GridToIso(int x, int y)
    {
        float isoX = (x - y) * tileWidth * 1f;
        float isoY = (x + y) * tileHeight * 1f;
        return new Vector3(isoX, isoY, 0f);
    }

    void Awake()
    {
        if (map == null || map.Length == 0)
        {
            map = new bool[width, height];
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    map[x, y] = true;
        }

        nodes = new GridNode[width, height];
        GenerateNodes();
        GenerateNeighbours();
    }

    void GenerateNodes()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (!map[x, y]) continue;

                Vector3 isoPos = GridToIso(x, y);

                GameObject nodeObj = Instantiate(
                    gridNodePrefab,
                    isoPos,
                    Quaternion.identity,
                    transform
                );

                GridNode node = nodeObj.GetComponent<GridNode>();
                node.column = (char)('a' + x);
                node.row = y + 1;

                // One-time obstacle check
                bool blocked = Physics2D.OverlapCircle(
                    isoPos,
                    obstacleCheckRadius,
                    wallLayer
                );

                node.isBlocked = blocked;

                nodes[x, y] = node;
            }
        }
    }


    void GenerateNeighbours()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GridNode node = nodes[x, y];
                if (node == null || node.isBlocked) continue;

                TryAddNeighbour(node, x - 1, y);
                TryAddNeighbour(node, x + 1, y);
                TryAddNeighbour(node, x, y - 1);
                TryAddNeighbour(node, x, y + 1);
            }
        }
    }

    void TryAddNeighbour(GridNode node, int x, int y)
    {
        if (x < 0 || y < 0 || x >= width || y >= height)
            return;

        GridNode neighbour = nodes[x, y];
        if (neighbour == null || neighbour.isBlocked)
            return;

        node.neighbours.Add(neighbour);
    }

    // void OnDrawGizmosSelected()
    // {
    //     if (nodes == null) return;

    //     Gizmos.color = Color.yellow;
    //     foreach (var node in nodes)
    //     {
    //         if (node != null)
    //             Gizmos.DrawWireSphere(node.transform.position, obstacleCheckRadius);
    //     }
    // }


}
