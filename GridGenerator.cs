using UnityEngine;

// [ExecuteAlways]
public class GridGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    public int width = 20;
    public int height = 20;

    [Header("Isometric Settings")]
    public float tileWidth = 4f;
    public float tileHeight = 2f;

    [Header("Debug")]
    public bool drawGrid = true;
    public bool drawBlockedOnly = false;

    // Data-only grid
    public GridNodeData[,] nodes;

    void Awake()
    {
        GenerateGrid();
    }

    #if UNITY_EDITOR
    void OnValidate()
    {
        if (!Application.isPlaying)
            GenerateGrid();
    }
    #endif

    void GenerateGrid()
    {
        if (width <= 0 || height <= 0)
            return;

        nodes = new GridNodeData[width, height];

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

    // =========================================================
    // NODE VIABILITY
    // =========================================================

    public void UpdateNodeViability(
        GameObject character
    )
    {
        // int levelLayer = character.layer;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GridNodeData node = nodes[x, y];

                if (node == null)
                    continue;

                Collider2D[] hits =
                    Physics2D.OverlapCircleAll(
                        node.worldPos,
                        tileHeight
                    );

                bool blocked = false;

                foreach (var hit in hits)
                {
                    if (hit == null)
                        continue;

                    // Ignore thresholds
                    // if (
                    //     hit.GetComponent<RoomThresholdColliderScript>() != null ||
                    //     hit.GetComponent<LevelThreshColliderScript>() != null ||
                    //     hit.GetComponent<BuildingThreshColliderScript>() != null
                    // )
                    // {
                    //     continue;
                    // }

                    // Ignore characters
                    if (
                        hit.CompareTag("PlayerCollider") ||
                        hit.CompareTag("NPCCollider") ||
                        hit.CompareTag("NPC")
                    )
                    {
                        continue;
                    }

                    // Ignore triggers
                    if (hit.isTrigger)
                        continue;

                    // Wrong layer
                    // if (hit.gameObject.layer != levelLayer)
                    //     continue;

                    blocked = true;
                    break;
                }

                node.isBlocked = blocked;
            }
        }
    }

    // =========================================================
    // GIZMOS
    // =========================================================

    void OnDrawGizmos()
    {
        if (!drawGrid)
            return;

        // Generate temporary grid in editor
        if (nodes == null ||
            nodes.GetLength(0) != width ||
            nodes.GetLength(1) != height)
        {
            GenerateGrid();
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GridNodeData node = nodes[x, y];

                if (node == null)
                    continue;

                if (drawBlockedOnly && !node.isBlocked)
                    continue;

                Gizmos.color =
                    node.isBlocked
                    ? Color.red
                    : new Color(1f, 1f, 1f, 0.15f);

                Gizmos.DrawSphere(
                    node.worldPos,
                    tileHeight * 0.25f
                );
            }
        }
    }
}

// =========================================================
// DATA-ONLY NODE
// =========================================================

public class GridNodeData
{
    public Vector2Int gridPos;
    public Vector3 worldPos;
    public bool isBlocked;
}