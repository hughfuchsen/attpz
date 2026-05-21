using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPathFollower : MonoBehaviour
{
    // =========================================================
    // GRID
    // =========================================================

    [Header("Grid")]
    public GridGenerator gridGenerator;
    public RemixScript remixScript;

    // =========================================================
    // AI
    // =========================================================

    [Header("AI Targets")]
    public Transform prey;
    public Transform predator;

    [Header("AI Weights")]
    public float chaseWeight = 1f;
    public float avoidWeight = 1f;

    [Header("AI")]
    private float thinkInterval = 0.05f;
    public float decisionRadius = 2f;


    [Header("Panic Burst")]
    public float panicDistance = 20f;
    private float panicWeight = 30f;
    public float panicMultiplier = 2f;
    public float panicBurstDuration = 6f;
    public Vector2 panicBurstIntervalRange = new Vector2(1f, 5f);

    bool panicBurstActive = false;

    GridNodeData currentGoal;

    [HideInInspector] public Coroutine goCoroutine;

    // =========================================================
    // MOVEMENT
    // =========================================================

    [Header("Movement")]
    Queue<GridNodeData> pathQueue = new Queue<GridNodeData>();


    public GridNodeData currentNode;

    CharacterMovement cm;
    CharacterAnimation preyType;
    CharacterAnimation predatorType;

    Coroutine pathFollowCoro;

    // =========================================================
    // START
    // =========================================================
    void Awake()
    {
        gridGenerator = FindObjectOfType<GridGenerator>();
        // remixScript = FindObjectOfType<RemixScript>();

        cm = GetComponent<CharacterMovement>();

        preyType =
        prey.GetComponent<CharacterAnimation>();

        predatorType =
        predator.GetComponent<CharacterAnimation>();
    }
    void Start()
    {
        currentNode = GetClosestNode(transform.position);

        gridGenerator.UpdateNodeViability(
            this.gameObject
        );

        StartCoroutine(PanicBurstLoop());
    }

    // =========================================================
    // THINK LOOP
    // =========================================================

    public IEnumerator ThinkLoop()
    {
        if (CompareTag("Player")) yield break;

        while (true)
        {
            GridNodeData bestNode = GetBestNode();

            if (bestNode != null &&
                bestNode != currentGoal)
            {
                currentGoal = bestNode;

                SetTargetNode(bestNode, false);
            }


            // gridGenerator.UpdateNodeViability(this.gameObject, cm.currentLevel);


            yield return new WaitForSeconds(thinkInterval);
        }
    }
    public void GoToInitialPosition(Vector3 homePos)
    {
        if (remixScript == null || remixScript.chosenPlayer == null)
        {
            return;
        }
        
        GridNodeData homeNode = null;

        if (CompareTag("Player"))
            return;

        if (goCoroutine != null)
        {
            StopCoroutine(goCoroutine);
            goCoroutine = null;
        }
        if(remixScript.chosenPlayer.GetComponent<CharacterMovement>().characterOnThresh)
        {
            homeNode =
            GetClosestNode(homePos + new Vector3(0, -60, 0));
            remixScript.chosenPlayer.GetComponent<CharacterMovement>().currentArea.areaType = AreaType.train;
        }
        else if (remixScript.chosenPlayer.GetComponent<CharacterMovement>().currentArea.areaType == AreaType.platform)
        {
            homeNode =
            GetClosestNode(homePos);
        }
        else
        {
            homeNode =
            GetClosestNode(homePos + new Vector3(0, -60, 0));
        }


        if (homeNode != null)
        {
            currentGoal = homeNode;
            SetTargetNode(homeNode, true);
        }
    }

    // =========================================================
    // PANIC BURST
    // =========================================================

    IEnumerator PanicBurstLoop()
    {
        if (remixScript.chosenPlayer != this.gameObject) yield break;

        while (true)
        {
            float waitTime =
                Random.Range(
                    panicBurstIntervalRange.x,
                    panicBurstIntervalRange.y
                );

            yield return new WaitForSeconds(waitTime);

            panicBurstActive = true;

            yield return new WaitForSeconds(panicBurstDuration);

            panicBurstActive = false;
        }
    }

    // =========================================================
    // NODE SCORING
    // =========================================================

    float currentGoalScore = float.MinValue;

    GridNodeData GetBestNode()
    {
        GridNodeData best = null;

        float bestScore = float.MinValue;

        GridNodeData myNode =
            GetClosestNode(transform.position); // get's the closest node to this object

        if (myNode == null)
            return null;

        // if (remixScript.chosenPlayer == this.gameObject && cm.change == Vector3.zero)
        // {
        //     predator.GetComponent<NPCPathFollower>().chaseWeight = 0f; // chase it's plaey 100% 
        //     prey.GetComponent<NPCPathFollower>().avoidWeight = 1f; // avoid it's predator 100% 
        // }
        // else
        // {
        //     predator.GetComponent<NPCPathFollower>().chaseWeight = 1f;
        //     prey.GetComponent<NPCPathFollower>().avoidWeight = 0f;
        // }
        avoidWeight = 1f;
        chaseWeight = 1f;

        for (int x = 0; x < gridGenerator.width; x++)
        {
            for (int y = 0; y < gridGenerator.height; y++) // iterates through all of the nodes of the grid
            {
                GridNodeData node =
                    gridGenerator.nodes[x, y];

                if (node == null)
                    continue;

                if (node.isBlocked)
                    continue;

                float distFromSelf =
                    Vector2Int.Distance(
                        myNode.gridPos,
                        node.gridPos
                    );

                // Local tactical search only
                if (distFromSelf > decisionRadius)
                    continue;

                Vector3 preyPos = prey.position;

                Vector3 predatorPos = predator.position;

                float distToPrey =
                    Vector3.Distance(
                        node.worldPos,
                        preyPos
                    );

                float distToPred =
                    Vector3.Distance(
                        node.worldPos,
                        predatorPos
                    );
                
                // float effectiveAvoid = avoidWeight;
                // float effectiveChase = chaseWeight;

                // if(cm.currentArea == predator.GetComponent<CharacterMovement>().currentArea)
                // {
                //     if (distToPred < panicDistance)
                //     {
                //         effectiveAvoid *= panicMultiplier;
                //     }
                // }
                // if(cm.currentArea == prey.GetComponent<CharacterMovement>().currentArea)
                // {
                //     if (distToPrey > panicDistance)
                //     {
                //         effectiveChase *= panicMultiplier;
                //     }
                // }

                float score =
                    (distToPred * avoidWeight)
                    -
                    (distToPrey * chaseWeight);

                if (score > bestScore)
                {
                    bestScore = score;
                    best = node;
                }
            }
        }

        // float commitmentThreshold = 5f;

        // if (
        //     currentGoal != null &&
        //     bestScore < currentGoalScore + commitmentThreshold
        // )
        // {
        //     currentGoalScore = bestScore;
        //     return currentGoal;
        // }


        return best;
    }

    // =========================================================
    // PATHFINDING
    // =========================================================

    public void SetTargetNode(GridNodeData target, bool initializing = false)
    {
        if (target == null)
            return;


        currentNode =
            GetClosestNode(transform.position);

        if (currentNode == null)
            return;


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
                currentNode.gridPos,
                target.gridPos
            );

        if (pathIndices == null ||
            pathIndices.Count == 0)
            return;

        pathIndices = ExtractCorners(pathIndices);
        pathIndices = SmoothPath(pathIndices);

        if (pathIndices.Count > 1)
            pathIndices.RemoveAt(0);

        pathQueue.Clear();

        foreach (Vector2Int pos in pathIndices)
        {
            if (pos.x < 0 ||
                pos.y < 0 ||
                pos.x >= gridGenerator.width ||
                pos.y >= gridGenerator.height)
            {
                continue;
            }

            GridNodeData node =
                gridGenerator.nodes[pos.x, pos.y];

            if (node != null)
            {
                pathQueue.Enqueue(node);
            }
        }

        if (pathFollowCoro != null)
        {
            StopCoroutine(pathFollowCoro);
        }

        pathFollowCoro =
            StartCoroutine(FollowPath(initializing));
    }

    // =========================================================
    // FOLLOW PATH
    // =========================================================

    IEnumerator FollowPath(bool initializing = false)
    {
        while (pathQueue.Count > 0)
        {
            GridNodeData node =
                pathQueue.Dequeue();

            Vector3 targetPos =
                node.worldPos;

            while (
                Vector3.Distance(
                    transform.position,
                    targetPos
                ) > 3f
            )
            {
                Vector3 dir = (targetPos - transform.position).normalized;

                cm.movementSpeed = initializing ? 150 : cm.initialmovementSpeed;

                cm.change = dir;

                yield return null;
            }

            transform.position = targetPos;

            cm.change = Vector3.zero;
            
            cm.movementSpeed = cm.initialmovementSpeed;

            currentNode = node;

            yield return null;
        }
    }

    // =========================================================
    // CLOSEST NODE
    // =========================================================

    GridNodeData GetClosestNode(Vector3 worldPos)
    {
        GridNodeData closest = null;

        float minDist = float.MaxValue;

        for (int x = 0; x < gridGenerator.width; x++)
        {
            for (int y = 0; y < gridGenerator.height; y++)
            {
                GridNodeData node =
                    gridGenerator.nodes[x, y];

                if (node == null)
                    continue;

                float dist =
                    Vector3.Distance(
                        worldPos,
                        node.worldPos
                    );

                if (dist < minDist)
                {
                    minDist = dist;
                    closest = node;
                }
            }
        }

        return closest;
    }

    // =========================================================
    // EXTRACT CORNERS
    // =========================================================

    List<Vector2Int> ExtractCorners(
        List<Vector2Int> path
    )
    {
        List<Vector2Int> result =
            new List<Vector2Int>();

        if (path.Count == 0)
            return result;

        result.Add(path[0]);

        Vector2Int prevDir = Vector2Int.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2Int dir =
                path[i] - path[i - 1];

            if (dir != prevDir)
            {
                result.Add(path[i - 1]);
            }

            prevDir = dir;
        }

        result.Add(path[path.Count - 1]);

        return result;
    }

    // =========================================================
    // SMOOTH PATH
    // =========================================================

    List<Vector2Int> SmoothPath(
        List<Vector2Int> path
    )
    {
        if (path.Count <= 2)
            return path;

        List<Vector2Int> result =
            new List<Vector2Int>();

        result.Add(path[0]);

        int currentIndex = 0;

        while (currentIndex < path.Count - 1)
        {
            int nextIndex = currentIndex + 1;

            for (
                int i = path.Count - 1;
                i > nextIndex;
                i--
            )
            {
                if (
                    HasLineOfSight(
                        path[currentIndex],
                        path[i]
                    )
                )
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

    // =========================================================
    // LINE OF SIGHT
    // =========================================================

    bool HasLineOfSight(
        Vector2Int a,
        Vector2Int b
    )
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
            if (
                x < 0 ||
                y < 0 ||
                x >= gridGenerator.width ||
                y >= gridGenerator.height
            )
            {
                return false;
            }

            GridNodeData node =
                gridGenerator.nodes[x, y];

            if (node == null || node.isBlocked)
            {
                return false;
            }

            if (x == b.x && y == b.y)
            {
                break;
            }

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