using System.Collections;
using UnityEngine;

public class AIDetectorBox : AIDetector
{
    [Header("Field of View Settings")] [SerializeField]
    public Vector2 detectorSize = Vector2.one;
    
    public override void RunDetection()
    {
        Collider2D collider2D = Physics2D.OverlapBox(
            (Vector2)detectorOrigin.position + detectorOriginOffset,
            detectorSize, 0, detectorLayerMask);
        
        if (collider2D != null)
        {
            Target = collider2D.gameObject;
        }
        else
        {
            Target = null;
        }
    }

    private void OnDrawGizmos()
    {
        if (showGizmos && detectorOrigin != null)
        {
            if (TargetInSight)
            {
                Gizmos.color = gizmoDetectedColor;
            }
            else
            {
                Gizmos.color = gizmoIdleColor;
            }
            
            Gizmos.DrawCube((Vector2)detectorOrigin.position + detectorOriginOffset, detectorSize);
        }
    }
}