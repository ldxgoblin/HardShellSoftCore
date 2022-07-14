using System.Collections;
using UnityEngine;

public class AIDetector : MonoBehaviour
{
    [Header("Field of View Settings")] [SerializeField]
    protected Transform detectorOrigin;

    public Vector2 detectorOriginOffset = Vector2.zero;

    public float detectionDelay = 0.3f;
    public LayerMask detectorLayerMask;

    [Header("Gizmo Settings")] public Color gizmoIdleColor = Color.green;

    public Color gizmoDetectedColor = Color.red;
    public bool showGizmos;

    protected GameObject target;
    [field: SerializeField] public bool TargetInSight { get; private set; }

    public GameObject Target
    {
        get => target;
        protected set
        {
            target = value;
            TargetInSight = target != null;
        }
    }

    private void Start()
    {
        StartCoroutine(DetectionCoroutine());
    }

    private IEnumerator DetectionCoroutine()
    {
        yield return new WaitForSeconds(detectionDelay);
        RunDetection();
        StartCoroutine(DetectionCoroutine());
    }

    public virtual void RunDetection()
    {
    }
}