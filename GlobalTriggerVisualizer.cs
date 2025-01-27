using UnityEngine;

public class GlobalTriggerVisualizer : MonoBehaviour
{
    [Header("Gizmo Settings")]
    public Color triggerGizmoColor = Color.red;

    private void OnDrawGizmos()
    {
        // Set the gizmo color
        Gizmos.color = triggerGizmoColor;

        // Find all Collider2D components in the scene
        Collider2D[] colliders = FindObjectsOfType<Collider2D>();

        foreach (Collider2D collider in colliders)
        {
            // Only process colliders marked as triggers
            if (collider.isTrigger)
            {
                // Save the original Gizmos matrix
                Matrix4x4 originalGizmosMatrix = Gizmos.matrix;

                // Adjust Gizmos to match the collider's transform
                Gizmos.matrix = collider.transform.localToWorldMatrix;

                // Draw the appropriate gizmo based on collider type
                if (collider is BoxCollider2D box)
                {
                    Gizmos.DrawWireCube(box.offset, box.size);
                }
                else if (collider is CircleCollider2D circle)
                {
                    Gizmos.DrawWireSphere(circle.offset, circle.radius);
                }
                else if (collider is CapsuleCollider2D capsule)
                {
                    // Capsule is approximated as a circle for simplicity
                    Gizmos.DrawWireSphere(capsule.offset, Mathf.Max(capsule.size.x, capsule.size.y) / 2);
                }

                // Restore the original Gizmos matrix
                Gizmos.matrix = originalGizmosMatrix;
            }
        }
    }
}
