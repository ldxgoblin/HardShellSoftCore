using UnityEngine;

public class AIDetectorCircle : AIDetector
{
    [Header("Field of View Settings")] [SerializeField]
    public float detectorRadius = 1f;

    private void OnDrawGizmos()
    {
        if (showGizmos && detectorOrigin != null)
        {
            if (TargetInSight)
            {
                Gizmos.color = gizmoDetectedColor;
                Gizmos.DrawLine(detectorOrigin.position, Target.transform.position);
            }
            else
            {
                Gizmos.color = gizmoIdleColor;
            }

            Gizmos.DrawWireSphere(detectorOrigin.position, detectorRadius);
        }
    }

    public override void RunDetection()
    {
        var collider2D = Physics2D.OverlapCircle(
            (Vector2)detectorOrigin.position + detectorOriginOffset,
            detectorRadius, detectorLayerMask);

        if (collider2D != null)
            Target = collider2D.gameObject;
        else
            Target = null;
    }
}