using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private RectTransform startButton, quitButton, gameLogo;
    [SerializeField] private Image backGroundImage;
    [SerializeField] private Color backGroundImageColor;

    [SerializeField] private float tweenDuration;
    
    [SerializeField] private AudioClip mainMenuMusic;
    private AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(mainMenuMusic);
        
        AnimateElements();
    }

    private void AnimateElements()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(backGroundImage.DOColor(backGroundImageColor, tweenDuration/2))
            .Append(startButton.DOAnchorPosX(-548, tweenDuration))
            .Append(quitButton.DOAnchorPosX(-548, tweenDuration))
            .Append(gameLogo.DOAnchorPosY(-182, tweenDuration))
            .SetEase(Ease.InOutExpo);
    }
}
