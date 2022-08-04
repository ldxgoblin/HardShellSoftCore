using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreCountText;
    private Vector3 scoreTextBasePosition;
    private float scoreRumbleIntensity;
    
    [SerializeField] private TextMeshProUGUI comboCountText;
    [SerializeField] private TextMeshProUGUI comboMessageText;
    private Vector3 comboMessageBasePosition;
    private Vector3 comboTextBasePosition;
    private float comboRumbleIntensity;

    [SerializeField] [Range(2.5f, 5f)] private float rumbleIntensity = 2.5f;
    [SerializeField] [Range(0.25f, 1f)] private float rumbleFallOff;

    [SerializeField] private Sprite[] hpDisplayPortraitSprites;
    [SerializeField] private Material hpDisplay;
    [SerializeField] private Image hpDisplayPortrait;
    [SerializeField] private Image playerDamageEffect;
    
    [SerializeField] private ComboMessage[] comboMessages;
    
    [SerializeField] private RectTransform comboPanel;
    
    [SerializeField] private RectTransform wavePanel;
    [SerializeField] private TextMeshProUGUI waveMessage;
    private Vector3 waveWarningBasePosition;

    [SerializeField] private RectTransform missionEndPanel;
    
    public static Action onStartSpawning;

    private HitPoints ballStateHp;
    private HitPoints mechStateHp;


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

        waveWarningBasePosition = wavePanel.position;

        WaveManager.onWaveStarting += ShowWaveStartWarning;

        Player.onPlayerDamage += ShowPlayerDamageEffect;
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
        wavePanel.position = waveWarningBasePosition;
        
        waveMessage.SetText(waveMessageText);
        
        wavePanel.DOAnchorPosX(0, 0.5f).SetEase(Ease.OutFlash);
        wavePanel.DOAnchorPosX(-1800, 0.5f)
            .SetEase(Ease.InFlash)
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
        
    }
}

[Serializable]
public class ComboMessage
{
    public int hits;
    public string message;
}