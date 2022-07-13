using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDetectorCircle : AIDetector
{
    [Header("Field of View Settings")] [SerializeField]
    public float detectorRadius = 1f;
    
    public override void RunDetection()
    {
        Collider2D collider2D = Physics2D.OverlapCircle(
            (Vector2)detectorOrigin.position + detectorOriginOffset,
            detectorRadius, detectorLayerMask);
        
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
            
            Gizmos.DrawSphere((Vector2)detectorOrigin.position + detectorOriginOffset, detectorRadius);
        }
    }
}
