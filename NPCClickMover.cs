using UnityEngine;

public class NPCClickMover : MonoBehaviour
{
    public NPCPathFollower npc;

    [HideInInspector] public Camera mainCamera;
    [HideInInspector] public GridGenerator gridGenerator;
    [HideInInspector] public ActivateCharacterUITrigger characterCustomizer;


    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (gridGenerator == null)
            gridGenerator = FindObjectOfType<GridGenerator>();
        
        if (characterCustomizer == null)
            characterCustomizer = FindObjectOfType<ActivateCharacterUITrigger>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !characterCustomizer.playerInRange)
        {
            Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0f;

            GridNodeData closestNode = GetClosestNode(mouseWorld);

            if (closestNode != null && !closestNode.isBlocked)
            {
                npc.SetTargetNode(closestNode);
            }
        }
    }

    GridNodeData GetClosestNode(Vector3 worldPos)
    {
        GridNodeData closest = null;
        float minDist = float.MaxValue;

        for (int x = 0; x < gridGenerator.width; x++)
        {
            for (int y = 0; y < gridGenerator.height; y++)
            {
                GridNodeData node = gridGenerator.nodes[x, y];
                if (node == null || node.isBlocked) continue;

                float dist = Vector3.Distance(worldPos, node.worldPos);

                if (dist < minDist)
                {
                    minDist = dist;
                    closest = node;
                }
            }
        }
        return closest;
    }

}