using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class FishBoundaryScript : MonoBehaviour
{
    private PolygonCollider2D poly;

    private void Awake()
    {
        poly = GetComponent<PolygonCollider2D>();
    }

    // Checks if a world-space point is inside the polygon
    public bool IsInside(Vector2 worldPoint)
    {
        // Convert world to local space for collider comparison
        Vector2 localPoint = transform.InverseTransformPoint(worldPoint);
        return poly.OverlapPoint(worldPoint); // simpler & faster
    }

    // Push point back inside polygon if it's outside
    public Vector2 ClampInside(Vector2 worldPoint)
    {
        if (IsInside(worldPoint))
            return worldPoint;

        Vector2 localPoint = transform.InverseTransformPoint(worldPoint);
        Vector2 center = (Vector2)transform.TransformPoint(poly.bounds.center);

        // move slightly toward center until inside
        Vector2 dir = (center - worldPoint).normalized;

        for (int i = 0; i < 200; i++)
        {
            worldPoint += dir * 0.01f;
            if (IsInside(worldPoint))
                break;
        }

        return worldPoint;
    }

    private void OnDrawGizmos()
    {
        PolygonCollider2D p = GetComponent<PolygonCollider2D>();
        if (!p) return;

        Gizmos.color = Color.cyan;
        Vector2[] pts = p.points;

        for (int i = 0; i < pts.Length; i++)
        {
            Vector2 a = transform.TransformPoint(pts[i]);
            Vector2 b = transform.TransformPoint(pts[(i + 1) % pts.Length]);
            Gizmos.DrawLine(a, b);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.TransformPoint(p.bounds.center), 0.05f);
    }
}
