// using System.Collections;
// using UnityEngine;

// public class NPCPathFollower : MonoBehaviour
// {
//     public PathDrawer path;
//     public float pauseTime = 0.3f;
//     public bool stopAtEnd = false; // ✅ new toggle for testing
//     public bool stopMoving = false; // ✅ new toggle for testing

//     private CharacterMovement npcCM;
//     private CharacterDialogueScript npcCD;
//     private CharacterMovement myCM;
//     private IsoSpriteSorting isoSpriteSortingScript;

//     public GridNode currentNode;

//     private void Start()
//     {
//         StartCoroutine(LateStart());
//     }

//     public IEnumerator LateStart()
//     {
//         yield return null; // wait one frame for components to init
//         npcCM = GetComponent<CharacterMovement>();
//         npcCD = GetComponent<CharacterDialogueScript>();
//         isoSpriteSortingScript = GetComponent<IsoSpriteSorting>();
//         myCM = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();


//         if (path == null || path.pathPoints.Count < 2)
//         {
//             Debug.LogWarning("NPCPathFollower: Path is missing or too short!");
//             yield break;
//         }

//         // ✅ if stopAtEnd = true, only go forward once
//         if (stopAtEnd)
//         {
//             yield return StartCoroutine(FollowPath(forward: true));
//             yield break;
//         }

//         // otherwise, loop continuously
//         while (true)
//         {
//             yield return StartCoroutine(FollowPath(forward: true));   // go forward
//             yield return StartCoroutine(FollowPath(forward: false));  // then backward
//         }
//     }

//     private IEnumerator FollowPath(bool forward)
//     {
//         int startIndex = forward ? 0 : path.pathPoints.Count - 1;
//         int endIndex = forward ? path.pathPoints.Count - 1 : 0;
//         int step = forward ? 1 : -1;

//         for (int i = startIndex; i != endIndex; i += step)
//         {
//             Vector3 start = path.transform.TransformPoint(path.pathPoints[i]);
//             Vector3 end = path.transform.TransformPoint(path.pathPoints[i + step]);

//             // Optional offset adjustment (depending on sprite setup)
//             end -= new Vector3(8, -28, 0);

//             // --- Move toward endpoint ---
//             while (Vector3.Distance(transform.position, end) > 1f)
//             {
//                 Vector3 dir = (end - transform.position).normalized;

//                 // --- Handle temporary pauses ---
//                 if ((myCM.currentRoom != null && myCM.currentRoom.roomIsMoving) 
//                     || 
//                 (myCM.currentInclineThreshold != null && myCM.currentInclineThreshold == npcCM.currentInclineThreshold)
//                     ||
//                 npcCD.staring == true
//                     || 
//                 stopMoving == true)
//                 {
//                     npcCM.change = Vector3.zero; // freeze during room motion
//                 }
//                 else
//                 {
//                     npcCM.change = dir; // normal path following
//                 }

//                 yield return null;
//             }

//             // Snap exactly to end
//             transform.position = end;

//             // Pause at each waypoint
//             if (npcCM != null)
//                 npcCM.change = Vector3.zero;

//             yield return new WaitForSeconds(pauseTime);
//         }

//         // ✅ Stop completely when reaching end (for testing)
//         if (stopAtEnd && forward)
//         {
//             if (npcCM != null)
//                 npcCM.change = Vector3.zero;
//         }
//     }
// }


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPathFollower : MonoBehaviour
{
    [Header("Grid Setup")]
    public GridGenerator gridGenerator;    // Assign your grid
    public char startColumn = 'a';
    public int startRow = 1;
    public char targetColumn = 'd';
    public int targetRow = 4;

    [Header("Movement Settings")]
    public float pauseTime = 0.1f;
    public bool stopAtEnd = false;         // Stop at end or loop
    public bool stopMoving = false;        // Freeze NPC

    private Queue<GridNode> pathQueue = new Queue<GridNode>();

    // Character scripts
    private CharacterMovement npcCM;
    private CharacterDialogueScript npcCD;
    private CharacterMovement myCM;


    private void Start()
    {
        // Get required components
        npcCM = GetComponent<CharacterMovement>();
        npcCD = GetComponent<CharacterDialogueScript>();
        myCM = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();

        gridGenerator = FindObjectOfType<GridGenerator>();
        // Initialize the path
        InitializePath();

        if (pathQueue.Count > 0)
            StartCoroutine(FollowPathCoroutine());
    }

    private void InitializePath()
    {
        GridNode startNode = gridGenerator.nodes[startColumn - 'a', startRow - 1];
        GridNode targetNode = gridGenerator.nodes[targetColumn - 'a', targetRow - 1];

        if (startNode == null || targetNode == null)
        {
            Debug.LogWarning("Start or Target node is null!");
            return;
        }

        // Build blocked map
        bool[,] blockedGrid = new bool[gridGenerator.width, gridGenerator.height];
        for (int x = 0; x < gridGenerator.width; x++)
            for (int y = 0; y < gridGenerator.height; y++)
                blockedGrid[x, y] = gridGenerator.nodes[x, y] == null || gridGenerator.nodes[x, y].isBlocked;

        List<Vector2Int> pathIndices = Pathfinding.FindPath(blockedGrid, gridGenerator.width, gridGenerator.height,
                                                           new Vector2Int(startColumn - 'a', startRow - 1),
                                                           new Vector2Int(targetColumn - 'a', targetRow - 1));

        if (pathIndices == null || pathIndices.Count == 0)
        {
            Debug.LogWarning("No path found!");
            return;
        }

        pathQueue.Clear();
        foreach (Vector2Int pos in pathIndices)
        {
            pathQueue.Enqueue(gridGenerator.nodes[pos.x, pos.y]);
        }

        // Snap NPC to start node
        transform.position = startNode.transform.position;
    }

    private IEnumerator FollowPathCoroutine()
    {
        do
        {
            Queue<GridNode> tempQueue = new Queue<GridNode>(pathQueue); // copy for looping if needed

            while (tempQueue.Count > 0)
            {
                GridNode node = tempQueue.Dequeue();
                Vector3 targetPos = node.transform.position;

                while (Vector3.Distance(transform.position, targetPos) > 0.1f)
                {
                    Vector3 dir = (targetPos - transform.position).normalized;

                    // Handle temporary pauses
                    if ((myCM.currentRoom != null && myCM.currentRoom.roomIsMoving)
                        || (myCM.currentInclineThreshold != null && myCM.currentInclineThreshold == npcCM.currentInclineThreshold)
                        || npcCD.staring
                        || stopMoving)
                    {
                        npcCM.change = Vector3.zero;
                    }
                    else
                    {
                        npcCM.change = dir;
                    }

                    yield return null;
                }

                // Snap exactly to node
                transform.position = targetPos;
                if (npcCM != null)
                    npcCM.change = Vector3.zero;

                yield return new WaitForSeconds(pauseTime);
            }

            if (!stopAtEnd)
            {
                // reverse path for back-and-forth looping
                Queue<GridNode> reversed = new Queue<GridNode>(pathQueue);
                var arr = reversed.ToArray();
                System.Array.Reverse(arr);
                pathQueue = new Queue<GridNode>(arr);
            }

        } while (!stopAtEnd);
    }
}