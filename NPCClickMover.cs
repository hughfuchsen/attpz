using UnityEngine;

public class NPCClickMover : MonoBehaviour
{
    public NPCPathFollower npc;
    public Camera mainCamera;
    public GridGenerator gridGenerator;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (gridGenerator == null)
            gridGenerator = FindObjectOfType<GridGenerator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0f;

            GridNode closestNode = GetClosestNode(mouseWorld);

            if (closestNode != null && !closestNode.isBlocked)
            {
                npc.SetTargetNode(closestNode);
            }
        }
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
                if (node == null || node.isBlocked) continue;

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

}