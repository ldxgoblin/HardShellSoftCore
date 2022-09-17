using UnityEngine;

public class MechIndicator : MonoBehaviour
{
    [SerializeField] private Transform indicatorTarget;
    private Transform indicatorOrigin;

    private void Awake()
    {
        indicatorOrigin = transform;
    }

    // Update is called once per frame
    void Update()
    {
        var relativePos = indicatorTarget.position - indicatorOrigin.position;
        var angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;
    }
}