using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreCountText;
    [SerializeField] private TextMeshProUGUI comboCountText;

    [SerializeField] [Range(2.5f, 5f)] private float rumbleIntensity = 2.5f;
    [SerializeField] [Range(0.25f, 1f)] private float rumbleFallOff;

    [SerializeField] private Material hpDisplay;
    
    [SerializeField] private Image hpDisplayPortrait;
    [SerializeField] private Sprite[] hpDisplayPortraitSprites;

    private HitPoints ballStateHp;
    private HitPoints mechStateHp;

    private float comboRumbleIntensity;
    private float scoreRumbleIntensity;
    
    private Vector3 comboTextBasePosition;
    private Vector3 scoreTextBasePosition;

    private void Awake()
    {
        ballStateHp = GameObject.FindWithTag("Player").GetComponent<Player>().HitPoints;
        if(ballStateHp == null) Debug.LogError("Ball State HP Object not found!");
        
        mechStateHp = GameObject.FindWithTag("Mech").GetComponent<Mech>().HitPoints;
        if(mechStateHp == null) Debug.LogError("Mech State HP Object not found!");

        comboTextBasePosition = comboCountText.transform.localPosition;
        scoreTextBasePosition = scoreCountText.transform.localPosition;

        ComboTracker.onCombo += UpdateComboCountText;
        ComboTracker.onComboEnded += HideComboCountText;

        MissionTracker.onScoreChange += UpdateScoreText;

        MechAttachPoint.OnMechActivation += SwitchToMechHitPointsUI;
        MechAttachPoint.OnMechDeactivation += SwitchToBallStateHitPointsUi;
        
        // TODO: not optimal, needs references to the lambdas in order to unsubscribe later but i wanted to try the syntax lol
        EnemyProjectile.OnBallStateDamage += () => RemoveHitPointsUiSegment(ballStateHp);
        EnemyProjectile.OnMechStateDamage += () => RemoveHitPointsUiSegment(mechStateHp);
        
    }

    private void Start()
    {
        UpdateHitPointsUi(ballStateHp);
    }

    private void Update()
    {
        var randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;

        if (comboRumbleIntensity > 0)
        {
            RumbleCounter(comboCountText, comboTextBasePosition, randomDirection, comboRumbleIntensity);
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
        ComboTracker.onCombo -= UpdateComboCountText;
        ComboTracker.onComboEnded -= HideComboCountText;

        MissionTracker.onScoreChange -= UpdateScoreText;
        
        MechAttachPoint.OnMechActivation -= SwitchToMechHitPointsUI;
        MechAttachPoint.OnMechDeactivation -= SwitchToBallStateHitPointsUi;
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
        comboCountText.enabled = true;
        comboRumbleIntensity = rumbleIntensity;
        comboCountText.SetText($"{hits}HIT");
    }

    private void HideComboCountText()
    {
        comboCountText.enabled = false;
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
        var current = hitPoints.currentHitPoints;
        var max = hitPoints.maxHitPoints;
        
        UpdateHitPointsPortrait(current, max);
        
        var removedSegments = Mathf.Abs(current - max);
        hpDisplay.SetFloat("_RemovedSegments", removedSegments);
    }

    private void UpdateHitPointsPortrait(int current, int max)
    {
        if (current <= 0) return;
        
        var percentage = 100 * ((float) current / max);
        
        Debug.Log($"{percentage}% of MaxHealth left!");
        
        // TODO refactor this mess
        if (percentage > 80)
        {
            // 100percentSprite
            hpDisplayPortrait.sprite = hpDisplayPortraitSprites[4];
        }
        else if (percentage is <= 80 and > 60)
        {
            // 75percentSprite
            hpDisplayPortrait.sprite = hpDisplayPortraitSprites[3];
        }
        else if (percentage is <= 60 and >= 40)
        {
            // 50percentSprite
            hpDisplayPortrait.sprite = hpDisplayPortraitSprites[2];
        }
        else if (percentage is < 40 and > 0)
        {
            // 25percentSprite
            hpDisplayPortrait.sprite = hpDisplayPortraitSprites[1];
        }
        else if (percentage == 0)
        {
            // 0percentSprite
            hpDisplayPortrait.sprite = hpDisplayPortraitSprites[0];
        }
    }
}