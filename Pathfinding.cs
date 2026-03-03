using System.Collections.Generic;
using UnityEngine;

public static class Pathfinding
{
    private class PathNode
    {
        public int x;
        public int y;

        public int gCost;
        public int hCost;

        public int FCost => gCost + hCost;

        public PathNode parent;

        public PathNode(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public static List<Vector2Int> FindPath( 
        bool[,] blocked,
        int width,
        int height,
        Vector2Int start,
        Vector2Int goal)
    {
        List<PathNode> openSet = new List<PathNode>();
        HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();

        PathNode startNode = new PathNode(start.x, start.y);
        startNode.gCost = 0;
        startNode.hCost = Heuristic(start, goal);

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            PathNode current = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < current.FCost ||
                (openSet[i].FCost == current.FCost &&
                    openSet[i].hCost < current.hCost))
                {
                    current = openSet[i];
                }
            }

            if (current.x == goal.x && current.y == goal.y)
            {
                return ReconstructPath(current);
            }

            openSet.Remove(current);
            closedSet.Add(new Vector2Int(current.x, current.y));

            // Check neighbours
            CheckNeighbour(current, -1, 0);
            CheckNeighbour(current, 1, 0);
            CheckNeighbour(current, 0, -1);
            CheckNeighbour(current, 0, 1);
        }

        return null; // no path found

        void CheckNeighbour(PathNode current, int dx, int dy)
        {
            int nx = current.x + dx;
            int ny = current.y + dy;

            if (nx < 0 || ny < 0 || nx >= width || ny >= height)
                return;

            if (blocked[nx, ny])
                return;

            Vector2Int neighbourPos = new Vector2Int(nx, ny);
            if (closedSet.Contains(neighbourPos))
                return;

            int tentativeG = current.gCost + 1;

            PathNode neighbour = openSet.Find(n => n.x == nx && n.y == ny);

            if (neighbour == null)
            {
                neighbour = new PathNode(nx, ny);
                neighbour.gCost = tentativeG;
                neighbour.hCost = Heuristic(neighbourPos, goal);
                neighbour.parent = current;
                openSet.Add(neighbour);
            }
            else if (tentativeG < neighbour.gCost)
            {
                neighbour.gCost = tentativeG;
                neighbour.parent = current;
            }
        }
    }
    static int Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    static List<Vector2Int> ReconstructPath(PathNode node)
    {
        List<Vector2Int> path = new List<Vector2Int>();

        while (node != null)
        {
            path.Add(new Vector2Int(node.x, node.y));
            node = node.parent;
        }

        path.Reverse();
        return path;
    }
}