using UnityEngine;

public class GlobalTriggerVisualizer : MonoBehaviour
{
    [Header("Gizmo Settings")]
    public Color triggerGizmoColor = Color.red;
    public Color lvlOneColliderGizmoColor = Color.cyan;
    public Color lvlTwoColliderGizmoColor = Color.magenta;

    private void OnDrawGizmos()
    {
        // Gizmos.color = triggerGizmoColor;
        Collider2D[] colliders = FindObjectsOfType<Collider2D>();

        foreach (Collider2D collider in colliders)
        {
            if (collider.isTrigger)
            {
                Gizmos.color = triggerGizmoColor;
                Matrix4x4 originalGizmosMatrix = Gizmos.matrix;
                Gizmos.matrix = collider.transform.localToWorldMatrix;

                if (collider is BoxCollider2D box)
                {
                    Gizmos.DrawWireCube(box.offset, box.size);
                }
                else if (collider is CircleCollider2D circle)
                {
                    Gizmos.DrawWireSphere(circle.offset, circle.radius);
                }
                else if (collider is PolygonCollider2D polygon)
                {
                    DrawPolygonGizmo(polygon);
                }
                else if (collider is CapsuleCollider2D capsule)
                {
                    Gizmos.DrawWireSphere(capsule.offset, Mathf.Max(capsule.size.x, capsule.size.y) / 2);
                }

                Gizmos.matrix = originalGizmosMatrix;
            }
            else if (collider.gameObject.layer == LayerMask.NameToLayer("Level1"))
            {
                Gizmos.color = lvlOneColliderGizmoColor;
                Matrix4x4 originalGizmosMatrix = Gizmos.matrix;
                Gizmos.matrix = collider.transform.localToWorldMatrix;

                if (collider is BoxCollider2D box)
                {
                    Gizmos.DrawWireCube(box.offset, box.size);
                }
                else if (collider is CircleCollider2D circle)
                {
                    Gizmos.DrawWireSphere(circle.offset, circle.radius);
                }
                else if (collider is PolygonCollider2D polygon)
                {
                    DrawPolygonGizmo(polygon);
                }
                else if (collider is CapsuleCollider2D capsule)
                {
                    Gizmos.DrawWireSphere(capsule.offset, Mathf.Max(capsule.size.x, capsule.size.y) / 2);
                }

                Gizmos.matrix = originalGizmosMatrix;
            }
            else if (collider.gameObject.layer == LayerMask.NameToLayer("Level2"))
            {
                Gizmos.color = lvlTwoColliderGizmoColor;
                Matrix4x4 originalGizmosMatrix = Gizmos.matrix;
                Gizmos.matrix = collider.transform.localToWorldMatrix;

                if (collider is BoxCollider2D box)
                {
                    Gizmos.DrawWireCube(box.offset, box.size);
                }
                else if (collider is CircleCollider2D circle)
                {
                    Gizmos.DrawWireSphere(circle.offset, circle.radius);
                }
                else if (collider is PolygonCollider2D polygon)
                {
                    DrawPolygonGizmo(polygon);
                }
                else if (collider is CapsuleCollider2D capsule)
                {
                    Gizmos.DrawWireSphere(capsule.offset, Mathf.Max(capsule.size.x, capsule.size.y) / 2);
                }

                Gizmos.matrix = originalGizmosMatrix;
            }
        }
    }

    private void DrawPolygonGizmo(PolygonCollider2D polygon)
    {
        Vector2[] points = polygon.points;
        for (int i = 0; i < points.Length; i++)
        {
            Vector2 current = points[i] + polygon.offset;
            Vector2 next = points[(i + 1) % points.Length] + polygon.offset;
            Gizmos.DrawLine(current, next);
        }
    }
}
