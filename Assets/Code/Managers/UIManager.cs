using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreCountText, comboCountText, comboMessageText, 
        missionEndHeadline, damageCountText, accuracyCountText, comboCounterText, 
        scoreCounterText, clearTimeCounterText, rankText, waveMessage;
    private Vector3 scoreTextBasePosition, comboMessageBasePosition, comboTextBasePosition;
    private float scoreRumbleIntensity, comboRumbleIntensity;
    
    [SerializeField] [Range(2.5f, 5f)] private float rumbleIntensity = 2.5f;
    [SerializeField] [Range(0.25f, 1f)] private float rumbleFallOff;

    [SerializeField] private Sprite[] hpDisplayPortraitSprites;
    [SerializeField] private Material hpDisplay;
    [SerializeField] private Image hpDisplayPortrait, playerDamageEffect, missionEndBackgroundImage;
    [SerializeField] private Sprite heroFailSprite, heroWinSprite;
    
    [SerializeField] private ComboMessage[] comboMessages;
    
    [SerializeField] private RectTransform comboPanel, wavePanel, rankBubble, heroImagePanel, quitButton, restartButton, infoPanel;
    [SerializeField] private Color failColor, winColor;
    
    private Vector2 waveWarningBasePosition;
    
    public static Action onStartSpawning;

    private HitPoints ballStateHp, mechStateHp;
    
    private void Awake()
    {
        ballStateHp = GameObject.FindWithTag("Player").GetComponent<Player>().HitPoints;
        if (ballStateHp == null) Debug.LogError("Ball State HP Object not found!");

        mechStateHp = GameObject.FindWithTag("Mech").GetComponent<Mech>().HitPoints;
        if (mechStateHp == null) Debug.LogError("Mech State HP Object not found!");

        comboTextBasePosition = comboCountText.transform.localPosition;
        comboMessageBasePosition = comboMessageText.transform.localPosition;
        scoreTextBasePosition = scoreCountText.transform.localPosition;

        ComboTracker.OnCombo += UpdateComboCountText;
        ComboTracker.OnComboEnded += HideComboCountText;

        MissionTracker.OnScoreChange += UpdateScoreText;

        MechAttachPoint.OnMechActivation += SwitchToMechHitPointsUI;
        MechAttachPoint.OnMechDeactivation += SwitchToBallStateHitPointsUi;

        // TODO: not optimal, needs references to the lambdas in order to unsubscribe later but i wanted to try the syntax lol
        EnemyProjectile.OnBallStateDamage += () => RemoveHitPointsUiSegment(ballStateHp);
        EnemyProjectile.OnMechStateDamage += () => RemoveHitPointsUiSegment(mechStateHp);

        waveWarningBasePosition = wavePanel.anchoredPosition;
        WaveManager.onWaveStarting += ShowWaveStartWarning;

        Player.onPlayerDamage += ShowPlayerDamageEffect;
        Player.onPlayerDeath += ShowMissionEndPanel;
    }

    private void Start()
    {
        DOTween.Init();
        UpdateHitPointsUi(ballStateHp);

        ShowMissionEndPanel();
    }

    private void Update()
    {
        var randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;

        if (comboRumbleIntensity > 0)
        {
            RumbleCounter(comboCountText, comboTextBasePosition, randomDirection, comboRumbleIntensity);
            RumbleCounter(comboMessageText, comboMessageBasePosition, randomDirection, comboRumbleIntensity);
            comboRumbleIntensity -= rumbleFallOff * Time.deltaTime;
        }
        else
        {
            HideComboCountText();
        }

        if (scoreRumbleIntensity > 0)
        {
            RumbleCounter(scoreCountText, scoreTextBasePosition, randomDirection, scoreRumbleIntensity);
            scoreRumbleIntensity -= rumbleFallOff * Time.deltaTime;
        }
    }

    private void OnDisable()
    {
        ComboTracker.OnCombo -= UpdateComboCountText;
        ComboTracker.OnComboEnded -= HideComboCountText;

        MissionTracker.OnScoreChange -= UpdateScoreText;

        MechAttachPoint.OnMechActivation -= SwitchToMechHitPointsUI;
        MechAttachPoint.OnMechDeactivation -= SwitchToBallStateHitPointsUi;
        
        WaveManager.onWaveStarting -= ShowWaveStartWarning;
        
        Player.onPlayerDamage -= ShowPlayerDamageEffect;
        Player.onPlayerDeath -= ShowMissionEndPanel;
    }

    private void RumbleCounter(TextMeshProUGUI counterTextObject, Vector3 basePosition, Vector3 direction,
        float intensity)
    {
        counterTextObject.transform.localPosition = basePosition + direction * intensity;
    }

    private void UpdateScoreText(int score)
    {
        scoreRumbleIntensity = rumbleIntensity;
        scoreCountText.SetText(score.ToString());
    }

    private void UpdateComboCountText(int hits)
    {
        comboPanel.DOAnchorPos(new Vector2(-50, -70), 0.25f);

        comboRumbleIntensity = rumbleIntensity;
        comboCountText.SetText($"{hits}");
    }

    private void HideComboCountText()
    {
        comboPanel.DOAnchorPos(new Vector2(500, -70), 0.25f);
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

    private void ShowWaveStartWarning(string waveMessageText, float duration)
    {
        wavePanel.anchoredPosition = waveWarningBasePosition;
        
        waveMessage.SetText(waveMessageText);
        
        wavePanel.DOAnchorPosX(0, 0.75f).SetEase(Ease.InElastic);
        wavePanel.DOAnchorPosX(-1800, 0.75f)
            .SetDelay(duration)
            .OnComplete(() =>
        {
            onStartSpawning?.Invoke();
        });
    }

    private void ShowPlayerDamageEffect(float duration)
    {
        playerDamageEffect.color = Color.red;
        playerDamageEffect.DOFade(0, duration);
    }

    private void ShowMissionEndPanel()
    {
        bool gameWon = true;
        var duration = 0.75f;
        var jumpPower = 2;
        var strength = new Vector2(0, -100);

        if (gameWon)
        {
            // Update Text & Counters
            missionEndHeadline.SetText("FUCK YEAH!");
            
            var sequence = DOTween.Sequence();
            sequence.Append(missionEndBackgroundImage.DOColor(winColor, duration))
                .Append(infoPanel.DOAnchorPosX(280, duration).SetEase(Ease.OutCubic))
                .Append(heroImagePanel.DOPunchAnchorPos(new Vector2(0, 150), duration))
                .Append(rankBubble.DOPunchAnchorPos(strength, duration));
        }
        

    }
}

[Serializable]
public class ComboMessage
{
    public int hits;
    public string message;
}