using System;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class BossBody : MonoBehaviour
{
    [SerializeField] private Transform bossHeadTransform, bossClawsTransform, bossLegsTransform;
    [SerializeField] private AudioClip bossIntroScream, bossDeathgrowl;
    
    private CinemachineImpulseSource bossImpulseSource;
    private Transform bossBodyTransform;
    [SerializeField] private AudioSource audioSource;

    public static event Action<Transform> OnAddBossTarget;

    private void Awake()
    {
        bossBodyTransform = transform;
        bossBodyTransform.DOMoveY(83, 2).SetEase(Ease.OutElastic).OnComplete(Intimidate);
        
        bossClawsTransform.DOScaleY(1.1f, 3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        bossLegsTransform.DOScaleY(1.6f, 3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);

        bossImpulseSource = GetComponent<CinemachineImpulseSource>();
        OnAddBossTarget?.Invoke(bossBodyTransform);
    }

    private void Intimidate()
    {
        audioSource.PlayOneShot(bossIntroScream);
        bossImpulseSource.GenerateImpulse(transform.position);
    }
}
