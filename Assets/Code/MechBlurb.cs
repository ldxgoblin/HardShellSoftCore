using System;
using DG.Tweening;
using UnityEngine;

public class MechBlurb : MonoBehaviour
{
    [SerializeField] private float animationSpeed = 1f;
    private Transform blurbTransform;

    private void Awake()
    {
        blurbTransform = transform;
    }

    private void OnEnable()
    {
        blurbTransform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), animationSpeed).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDisable()
    {
        blurbTransform.DOKill();
    }


}
