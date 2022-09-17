using UnityEngine;

public class AIDetectorBox : AIDetector
{
    [Header("Field of View Settings")] [SerializeField]
    public Vector2 detectorSize = Vector2.one;

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

            Gizmos.DrawWireCube((Vector2)detectorOrigin.position + detectorOriginOffset, detectorSize);
        }
    }

    public override void RunDetection()
    {
        var collider2D = Physics2D.OverlapBox(
            (Vector2)detectorOrigin.position + detectorOriginOffset,
            detectorSize, 0, detectorLayerMask);

        if (collider2D != null)
            Target = collider2D.gameObject;
        else
            Target = null;
    }
}