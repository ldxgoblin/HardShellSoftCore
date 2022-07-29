using System.Collections;
using Cinemachine;
using UnityEngine;

public class SmashGroupCamera : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup targetGroup;
    [SerializeField, Range(0.1f, 0.9f)] private float easeFactor;
    [SerializeField] private float targetWeight;
    [SerializeField] private float targetRadius;

    private void Awake()
    {
        Enemy.onEnemyAddToGroup += AddTarget;
        Enemy.onEnemyRemoveFromGroup += RemoveTarget;
    }

    private void OnDisable()
    {
        Enemy.onEnemyAddToGroup -= AddTarget;
        Enemy.onEnemyRemoveFromGroup -= RemoveTarget;
    }

    private void AddTarget(Transform target)
    {
        if (targetGroup != null)
            if (targetGroup.FindMember(target) == -1)
            {
                targetGroup.AddMember(target, 0, targetRadius);
                StartCoroutine(EaseIntoTargetGroup(target));
            }
    }

    private IEnumerator EaseIntoTargetGroup(Transform targetTransform)
    {
        var index = targetGroup.FindMember(targetTransform);
        var target = targetGroup.m_Targets[index];

        while (target.weight < targetWeight)
        {
            target.weight = Mathf.MoveTowards(target.weight, targetWeight, easeFactor * Time.smoothDeltaTime);
            index = targetGroup.FindMember(targetTransform);
            if (index >= 0) targetGroup.m_Targets[index] = target;

            yield return new WaitForSeconds(0.01f);
        }

        target.weight = targetWeight;
    }

    private void RemoveTarget(Transform targetTransform)
    {
        if (targetGroup != null)
            if (targetGroup.FindMember(targetTransform) != -1)
                StartCoroutine(EaseOutOfTargetGroup(targetTransform));
    }

    private IEnumerator EaseOutOfTargetGroup(Transform targetTransform)
    {
        var index = targetGroup.FindMember(targetTransform);
        var target = targetGroup.m_Targets[index];
        while (target.weight > 0f)
        {
            target.weight = Mathf.MoveTowards(target.weight, 0, easeFactor * Time.smoothDeltaTime);
            index = targetGroup.FindMember(targetTransform);
            if (index >= 0) targetGroup.m_Targets[index] = target;

            yield return new WaitForSeconds(0.01f);
        }

        target.weight = 0;
        targetGroup.RemoveMember(targetTransform);
    }
}