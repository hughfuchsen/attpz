

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPathFollower : MonoBehaviour
{
    [Header("Grid")]
    public GridGenerator gridGenerator;

    [Header("Patrol Nodes")]
    public List<GridNode> patrolNodes = new List<GridNode>();
    int patrolIndex = 0;

    [Header("Movement")]
    public float pauseTime = 0.1f;

    Queue<GridNode> pathQueue = new Queue<GridNode>();

    Vector3 gridOffset = new Vector3(-8, 28, 0);

    public GridNode currentNode;

    CharacterMovement npcCM;
    CharacterDialogueScript npcCD;
    CharacterMovement playerCM;

    Coroutine pathFollowCoro;

    void Start()
    {
        npcCM = GetComponent<CharacterMovement>();
        npcCD = GetComponent<CharacterDialogueScript>();
        playerCM = GameObject.FindGameObjectWithTag("Player")
            .GetComponent<CharacterMovement>();

        gridGenerator = FindObjectOfType<GridGenerator>();

        currentNode = GetClosestNode(transform.position - gridOffset);
        
        gridGenerator.UpdateNodeViability(this.gameObject, npcCM.currentLevel);

        if (patrolNodes.Count > 0)
        {
            SetTargetNode(patrolNodes[0]);;
        }
    }

    public void SetTargetNode(GridNode target)
    {
        if (target == null || currentNode == null)
            return;

        // gridGenerator.UpdateNodeViability(this.gameObject, npcCM.currentLevel);

        bool[,] blockedGrid =
            new bool[gridGenerator.width, gridGenerator.height];

        for (int x = 0; x < gridGenerator.width; x++)
        {
            for (int y = 0; y < gridGenerator.height; y++)
            {
                blockedGrid[x, y] =
                    gridGenerator.nodes[x, y] == null ||
                    gridGenerator.nodes[x, y].isBlocked;
            }
        }

        List<Vector2Int> pathIndices =
            Pathfinding.FindPath(
                blockedGrid,
                gridGenerator.width,
                gridGenerator.height,
                currentNode.GridPosition,
                target.GridPosition
            );
        
        if (pathIndices == null || pathIndices.Count == 0)
            return;

        pathIndices = ExtractCorners(pathIndices);
        pathIndices = SmoothPath(pathIndices);

        pathQueue.Clear();

        if (currentNode == null)
        {
            currentNode = GetClosestNode(transform.position - gridOffset);
        }

        foreach (Vector2Int pos in pathIndices)
        {
            if (pos.x < 0 || pos.y < 0 ||
                pos.x >= gridGenerator.width ||
                pos.y >= gridGenerator.height)
            {
                Debug.LogWarning($"Path node out of bounds: {pos}");
                continue;
            }

            GridNode node = gridGenerator.nodes[pos.x, pos.y];

            if (node != null)
                pathQueue.Enqueue(node);
        }

        if (pathFollowCoro != null)
            StopCoroutine(pathFollowCoro);

        pathFollowCoro = StartCoroutine(FollowPath());
    }

    IEnumerator FollowPath()
    {
        while (pathQueue.Count > 0)
        {
            GridNode node = pathQueue.Dequeue();
        
            Vector3 targetPos = node.transform.position + gridOffset;

            while (Vector3.Distance(transform.position, targetPos) > 1f)
            {
                Vector3 dir = (targetPos - transform.position).normalized;

                if ((playerCM.currentRoom != null &&
                     playerCM.currentRoom.roomIsMoving) ||
                    npcCD.staring)
                {
                    npcCM.change = Vector3.zero;
                }
                else
                {
                    npcCM.change = dir;
                }

                yield return null;
            }

            transform.position = targetPos;

            npcCM.change = Vector3.zero;

            currentNode = node;

            yield return new WaitForSeconds(pauseTime);
        }

        NextPatrolNode();
    }

    void NextPatrolNode()
    {
        if (patrolNodes.Count == 0) return;

        patrolIndex++;

        if (patrolIndex >= patrolNodes.Count)
            patrolIndex = 0;

        SetTargetNode(patrolNodes[patrolIndex]);
    }

    GridNode GetClosestNode(Vector3 worldPos)
    {
        GridNode closest = null;
        float minDist = float.MaxValue;

        for (int x = 0; x < gridGenerator.width; x++)
        {
            for (int y = 0; y < gridGenerator.height; y++)
            {
                GridNode node = gridGenerator.nodes[x, y];
                if (node == null) continue;

                float dist = Vector3.Distance(worldPos, node.transform.position);

                if (dist < minDist)
                {
                    minDist = dist;
                    closest = node;
                }
            }
        }

        return closest;
    }


    List<Vector2Int> ExtractCorners(List<Vector2Int> path)
    {
        List<Vector2Int> result = new List<Vector2Int>();

        if (path.Count == 0)
            return result;

        result.Add(path[0]);

        Vector2Int prevDir = Vector2Int.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2Int dir = path[i] - path[i - 1];

            if (dir != prevDir)
            {
                result.Add(path[i - 1]);
            }

            prevDir = dir;
        }

        result.Add(path[path.Count - 1]);

        return result;
    }

    List<Vector2Int> SmoothPath(List<Vector2Int> path)
    {
        if (path.Count <= 2)
            return path;

        List<Vector2Int> result = new List<Vector2Int>();
        result.Add(path[0]);

        int currentIndex = 0;

        while (currentIndex < path.Count - 1)
        {
            int nextIndex = currentIndex + 1;

            for (int i = path.Count - 1; i > nextIndex; i--)
            {
                if (HasLineOfSight(path[currentIndex], path[i]))
                {
                    nextIndex = i;
                    break;
                }
            }

            result.Add(path[nextIndex]);
            currentIndex = nextIndex;
        }

        return result;
    }

    bool HasLineOfSight(Vector2Int a, Vector2Int b)
    {
        int dx = Mathf.Abs(b.x - a.x);
        int dy = Mathf.Abs(b.y - a.y);

        int sx = a.x < b.x ? 1 : -1;
        int sy = a.y < b.y ? 1 : -1;

        int err = dx - dy;

        int x = a.x;
        int y = a.y;

        while (true)
        {
            if (x < 0 || y < 0 || x >= gridGenerator.width || y >= gridGenerator.height)
                return false;

            GridNode node = gridGenerator.nodes[x, y];

            if (node == null || node.isBlocked)
                return false;

            if (x == b.x && y == b.y)
                break;

            int e2 = 2 * err;

            if (e2 > -dy)
            {
                err -= dy;
                x += sx;
            }

            if (e2 < dx)
            {
                err += dx;
                y += sy;
            }
        }

        return true;
    }
}