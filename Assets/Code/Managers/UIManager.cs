using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentScoreText, currentComboText, 
        comboMessageText, missionEndHeadline, finalDamageCountText, finalAccuracyCountText, 
        finalComboCountText, bonusScoreText, finalScoreCountText, finalClearTimeText, rankText, 
        waveMessageText;
    
    private Vector3 scoreTextBasePosition, comboMessageBasePosition, comboTextBasePosition;
    private float scoreRumbleIntensity, comboRumbleIntensity;
    
    [SerializeField] [Range(2.5f, 5f)] private float rumbleIntensity = 2.5f;
    [SerializeField] [Range(0.25f, 1f)] private float rumbleFallOff;

    [SerializeField] private Sprite[] hpDisplayPortraitSprites;
    [SerializeField] private Material hpDisplay;
    [SerializeField] private Image hpDisplayPortrait, playerDamageEffect, missionEndBackgroundImage, heroImage;
    [SerializeField] private Sprite heroFailSprite, heroWinSprite;

    [SerializeField] private RectTransform comboPanel, comboMessagePanel, wavePanel, rankBubble, heroImagePanel, infoPanel;
    [SerializeField] private Color failColor, winColor;
    
    private Vector2 waveWarningBasePosition;
    
    public static event Action OnStartSpawning;

    private HitPoints ballStateHp, mechStateHp;
    
    private void Awake()
    {
        ballStateHp = GameObject.FindWithTag("Player").GetComponent<Player>().HitPoints;
        if (ballStateHp == null) Debug.LogError("Ball State HP Object not found!");

        mechStateHp = GameObject.FindWithTag("Mech").GetComponent<Mech>().HitPoints;
        if (mechStateHp == null) Debug.LogError("Mech State HP Object not found!");

        comboTextBasePosition = currentComboText.transform.localPosition;
        comboMessageBasePosition = comboMessageText.transform.localPosition;
        scoreTextBasePosition = currentScoreText.transform.localPosition;

        ComboTracker.OnCombo += UpdateComboCountText;
        ComboTracker.OnComboEnded += HideComboCountText;
        ComboTracker.OnComboMessage += UpdateComboMessage;

        MissionTracker.OnScoreChange += UpdateScoreText;
        MissionTracker.OnBlastAllStatistics += SetFinalStats;

        MechAttachPoint.OnMechActivation += SwitchToMechHitPointsUI;
        MechAttachPoint.OnMechDeactivation += SwitchToBallStateHitPointsUi;

        // TODO: not optimal, needs references to the lambdas in order to unsubscribe later but i wanted to try the syntax lol
        BasicEnemyProjectile.OnBallStateDamage += () => RemoveHitPointsUiSegment(ballStateHp);
        BasicEnemyProjectile.OnMechStateDamage += () => RemoveHitPointsUiSegment(mechStateHp);

        waveWarningBasePosition = wavePanel.anchoredPosition;
        WaveManager.OnWaveStarting += ShowWaveStartWarning;
        WaveManager.OnWavesCleared += MissionAccomplished;

        Player.OnPlayerDamage += ShowPlayerDamageEffect;
        Player.OnPlayerDeath += MissionFailed;

        heroImage = heroImagePanel.GetComponent<Image>();
        
        
    }

    private void Start()
    {
        DOTween.Init();
        UpdateHitPointsUi(ballStateHp);
    }

    private void Update()
    {
        var randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;

        if (comboRumbleIntensity > 0)
        {
            RumbleCounter(currentComboText, comboTextBasePosition, randomDirection, comboRumbleIntensity);
            RumbleCounter(comboMessageText, comboMessageBasePosition, randomDirection, comboRumbleIntensity);
            comboRumbleIntensity -= rumbleFallOff * Time.deltaTime;
        }
        else
        {
            HideComboCountText();
        }

        if (scoreRumbleIntensity > 0)
        {
            RumbleCounter(currentScoreText, scoreTextBasePosition, randomDirection, scoreRumbleIntensity);
            scoreRumbleIntensity -= rumbleFallOff * Time.deltaTime;
        }
    }

    private void OnDisable()
    {
        ComboTracker.OnCombo -= UpdateComboCountText;
        ComboTracker.OnComboEnded -= HideComboCountText;
        ComboTracker.OnComboMessage -= UpdateComboMessage;
        
        MissionTracker.OnScoreChange -= UpdateScoreText;

        MechAttachPoint.OnMechActivation -= SwitchToMechHitPointsUI;
        MechAttachPoint.OnMechDeactivation -= SwitchToBallStateHitPointsUi;
        
        WaveManager.OnWaveStarting -= ShowWaveStartWarning;
        WaveManager.OnWavesCleared -= MissionAccomplished;
        
        Player.OnPlayerDamage -= ShowPlayerDamageEffect;
        Player.OnPlayerDeath -= MissionFailed;
        
        MissionTracker.OnBlastAllStatistics -= SetFinalStats;
    }

    private void RumbleCounter(TextMeshProUGUI counterTextObject, Vector3 basePosition, Vector3 direction,
        float intensity)
    {
        counterTextObject.transform.localPosition = basePosition + direction * intensity;
    }

    private void UpdateScoreText(int score)
    {
        scoreRumbleIntensity = rumbleIntensity;
        currentScoreText.SetText(score.ToString());
    }

    private void UpdateComboCountText(int hits)
    {
        comboPanel.DOAnchorPos(new Vector2(-50, -70), 0.25f).SetEase(Ease.OutExpo);

        comboRumbleIntensity = rumbleIntensity;
        currentComboText.SetText($"{hits}");
    }

    private void UpdateComboMessage(string message)
    {
        comboMessageText.SetText(message);
    }

    private void HideComboCountText()
    {
        comboPanel.DOAnchorPos(new Vector2(500, -70), 0.25f).SetEase(Ease.InExpo).OnComplete(()=> UpdateComboMessage(""));
    }

    private void UpdateHitPointsUi(HitPoints hitPoints)
    {
        // changes everytime we switch from mech to ball state
        hpDisplay.SetFloat("_SegmentCount", hitPoints.maxHitPoints);
        hpDisplay.SetFloat("_RemovedSegments", hitPoints.currentHitPoints - hitPoints.maxHitPoints);

        RemoveHitPointsUiSegment(hitPoints);
    }

    private void SwitchToMechHitPointsUI()
    {
        UpdateHitPointsUi(mechStateHp);
    }

    private void SwitchToBallStateHitPointsUi()
    {
        UpdateHitPointsUi(ballStateHp);
    }

    private void RemoveHitPointsUiSegment(HitPoints hitPoints)
    {
        if (hitPoints == null) return;

        var current = hitPoints.currentHitPoints;
        var max = hitPoints.maxHitPoints;

        UpdateHitPointsPortrait(current, max);

        var removedSegments = Mathf.Abs(current - max);
        hpDisplay.SetFloat("_RemovedSegments", removedSegments);
    }

    private void UpdateHitPointsPortrait(int current, int max)
    {
        var percentage = 100 * ((float)current / max);

        // TODO refactor this mess
        if (percentage > 80)
            // 100percentSprite
            hpDisplayPortrait.sprite = hpDisplayPortraitSprites[4];
        else if (percentage is <= 80 and > 60)
            // 75percentSprite
            hpDisplayPortrait.sprite = hpDisplayPortraitSprites[3];
        else if (percentage is <= 60 and >= 40)
            // 50percentSprite
            hpDisplayPortrait.sprite = hpDisplayPortraitSprites[2];
        else if (percentage is < 40 and > 0)
            // 25percentSprite
            hpDisplayPortrait.sprite = hpDisplayPortraitSprites[1];
        else if (percentage == 0)
            // 0percentSprite
            hpDisplayPortrait.sprite = hpDisplayPortraitSprites[0];
    }

    private void ShowWaveStartWarning(string waveMessage, float duration)
    {
        wavePanel.anchoredPosition = waveWarningBasePosition;
        
        waveMessageText.SetText(waveMessage);

        var sequence = DOTween.Sequence();
        
        sequence.Append(wavePanel.DOAnchorPosX(0, 0.25f))
            .Append(wavePanel.DOPunchPosition(new Vector3(2,2,0),duration))
            .Append(wavePanel.DOAnchorPosX(-1800, 0.25f))
            .OnComplete(() =>
            {
                OnStartSpawning?.Invoke();
            })
            .SetEase(Ease.InOutExpo);
    }

    private void ShowPlayerDamageEffect(float duration)
    {
        playerDamageEffect.color = Color.red;
        playerDamageEffect.DOFade(0, duration);
    }

    private void MissionFailed()
    {
        ShowMissionEndPanel(false);
    }

    private void MissionAccomplished()
    {
        ShowMissionEndPanel(true);
    }
    
    private void ShowMissionEndPanel(bool state)
    {
        var duration = 0.75f;
        var color = new Color();

        if (state)
        {
            missionEndHeadline.SetText("FUCK YEAH!");
            heroImage.sprite = heroWinSprite;
            color = winColor;
        }
        else
        {
            missionEndHeadline.SetText("YOU DIED!");
            heroImage.sprite = heroFailSprite;
            color = failColor;
        }
        
        rankBubble.localScale = Vector3.zero;
        
        var sequence = DOTween.Sequence();
        sequence.Append(missionEndBackgroundImage.DOColor(color, duration/2))
            .Append(infoPanel.DOAnchorPosX(280, duration))
            .Append(heroImagePanel.DOAnchorPosY(-70, duration))
            .Append(rankBubble.DOScale(new Vector3(1.2f,1.2f, 0), duration))
            .SetEase(Ease.InOutExpo);
    }

    private void SetFinalStats(int damage, float accuracy, int combos, int score, int bonusScore, string clearTime, string rank)
    {
        finalDamageCountText.text = damage.ToString();
        finalAccuracyCountText.text = accuracy + " %"; 
        finalComboCountText.text = combos.ToString();
        finalScoreCountText.text = score.ToString();
        finalClearTimeText.text = clearTime;

        bonusScoreText.text = bonusScore.ToString();
        rankText.text = rank;
    }
}