using System;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class BossBody : MonoBehaviour
{
    [SerializeField] private Transform bossHeadTransform;
    
    private CinemachineImpulseSource bossImpulseSource;
    private Transform bossBodyTransform;

    public static event Action<Transform> OnAddBossTarget;

    private void Awake()
    {
        bossBodyTransform = transform;
        bossBodyTransform.DOMoveY(77, 2).SetEase(Ease.OutBounce).SetDelay(1);

        bossImpulseSource = GetComponent<CinemachineImpulseSource>();
        OnAddBossTarget?.Invoke(bossBodyTransform);
    }

    private void Start()
    {
        bossImpulseSource.GenerateImpulse(new Vector3(500, 500, 500));
    }
}
