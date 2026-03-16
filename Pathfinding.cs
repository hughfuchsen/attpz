using System.Collections.Generic;
using UnityEngine;

public static class Pathfinding
{
    const int MAX_SEARCH_RADIUS = 80;

    private class PathNode
    {
        public int x;
        public int y;

        public int gCost;
        public int hCost;

        public int FCost => gCost + hCost * 2;

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
        Vector2Int diff = goal - start;
        int manhattan = Mathf.Abs(diff.x) + Mathf.Abs(diff.y);

        if (manhattan > MAX_SEARCH_RADIUS)
        {
            // Scale dx/dy proportionally
            float scale = (float)MAX_SEARCH_RADIUS / manhattan;
            int clampedX = Mathf.RoundToInt(diff.x * scale);
            int clampedY = Mathf.RoundToInt(diff.y * scale);
            goal = start + new Vector2Int(clampedX, clampedY);
        }


        List<PathNode> openSet = new List<PathNode>();
        Dictionary<Vector2Int, PathNode> openLookup = new Dictionary<Vector2Int, PathNode>();

        HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();

        PathNode startNode = new PathNode(start.x, start.y);
        startNode.gCost = 0;
        startNode.hCost = Heuristic(start, goal);

        openSet.Add(startNode);
        openLookup[start] = startNode;

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
                return ReconstructPath(current);

            openSet.Remove(current);
            openLookup.Remove(new Vector2Int(current.x, current.y));
            closedSet.Add(new Vector2Int(current.x, current.y));

            CheckNeighbour(current, -1, 0);
            CheckNeighbour(current, 1, 0);
            CheckNeighbour(current, 0, -1);
            CheckNeighbour(current, 0, 1);

            void CheckNeighbour(PathNode node, int dx, int dy)
            {
                int nx = node.x + dx;
                int ny = node.y + dy;

                if (nx < 0 || ny < 0 || nx >= width || ny >= height)
                    return;

                int distFromStart = Mathf.Abs(nx - start.x) + Mathf.Abs(ny - start.y);
                if (distFromStart > MAX_SEARCH_RADIUS)
                    return;

                if (blocked[nx, ny])
                    return;

                Vector2Int pos = new Vector2Int(nx, ny);
                if (closedSet.Contains(pos))
                    return;

                int tentativeG = node.gCost + 1;

                PathNode neighbour;
                openLookup.TryGetValue(pos, out neighbour);

                if (neighbour == null)
                {
                    neighbour = new PathNode(nx, ny);
                    neighbour.gCost = tentativeG;
                    neighbour.hCost = Heuristic(pos, goal);
                    neighbour.parent = node;

                    openSet.Add(neighbour);
                    openLookup[pos] = neighbour;
                }
                else if (tentativeG < neighbour.gCost)
                {
                    neighbour.gCost = tentativeG;
                    neighbour.parent = node;
                }
            }
        }

        return null;
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