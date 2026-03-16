using System.Collections.Generic;
using UnityEngine;

public static class LinePathfinder
{
    // Tries to reach the target using as few straight segments as possible
    public static List<Vector2Int> FindPath(
        GridNode[,] nodes,
        bool[,] blocked,
        Vector2Int start,
        Vector2Int target)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        path.Add(start);

        Vector2Int current = start;

        // Safety limit to avoid infinite loops
        int maxSteps = nodes.GetLength(0) * nodes.GetLength(1);
        int steps = 0;

        while (current != target && steps < maxSteps)
        {
            Vector2Int next = GetFurthestVisible(current, target, nodes, blocked);
            if (next == current)
            {
                Debug.LogWarning("LinePathfinder: stuck, cannot reach target directly");
                break;
            }

            path.Add(next);
            current = next;
            steps++;
        }

        return path;
    }

    // Finds the furthest node along a straight line to the target that is not blocked
    private static Vector2Int GetFurthestVisible(
        Vector2Int from,
        Vector2Int to,
        GridNode[,] nodes,
        bool[,] blocked)
    {
        int dx = Mathf.Clamp(to.x - from.x, -1, 1);
        int dy = Mathf.Clamp(to.y - from.y, -1, 1);

        Vector2Int current = from;
        Vector2Int lastGood = from;

        while (current != to)
        {
            Vector2Int next = new Vector2Int(current.x + dx, current.y + dy);

            // Out of bounds
            if (next.x < 0 || next.y < 0 || next.x >= nodes.GetLength(0) || next.y >= nodes.GetLength(1))
                break;

            if (blocked[next.x, next.y] || nodes[next.x, next.y] == null)
                break;

            lastGood = next;
            current = next;
        }

        return lastGood;
    }
}