using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField, Range(0.1f, 5f)] private float fadeDuration = 0.25f;
    [SerializeField, Range(0.1f, 1f)] private float fadeVolume = 1f;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        WaveManager.onWaveMusicStart += FadeInTrack;
        WaveManager.onWaveMusicEnd += FadeOutTrack;
    }

    private void OnDisable()
    {
        WaveManager.onWaveMusicStart -= FadeInTrack;
        WaveManager.onWaveMusicEnd -= FadeOutTrack;
    }

    private void FadeInTrack(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
        audioSource.DOFade(fadeVolume, fadeDuration);
    }
    
    private void FadeOutTrack()
    {
        audioSource.DOFade(0, fadeDuration);
    }
    
}
