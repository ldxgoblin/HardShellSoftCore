using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreCountText;
    [SerializeField] private TextMeshProUGUI comboCountText;

    [SerializeField] [Range(2.5f, 5f)] private float rumbleIntensity = 2.5f;
    [SerializeField] [Range(0.25f, 1f)] private float rumbleFallOff;

    private float comboRumbleIntensity;
    private float scoreRumbleIntensity;
    
    private Vector3 comboTextBasePosition;
    private Vector3 scoreTextBasePosition;

    [SerializeField] private Material hpDisplay;
    
    private void Awake()
    {
        comboTextBasePosition = comboCountText.transform.localPosition;
        scoreTextBasePosition = scoreCountText.transform.localPosition;

        ComboTracker.onCombo += UpdateComboCountText;
        ComboTracker.onComboEnded += HideComboCountText;
        
        MissionTracker.onScoreChange += UpdateScoreText;
    }

    private void OnDisable()
    {
        ComboTracker.onCombo -= UpdateComboCountText;
        ComboTracker.onComboEnded -= HideComboCountText;

        MissionTracker.onScoreChange -= UpdateScoreText;
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

    private void SetHpDisplaySegments(float segmentCount)
    {
        // changes everytime we switch from mech to ball state
        hpDisplay.SetFloat("SegmentCount", segmentCount);
    }

    private void RemoveHpDisplaySegment(int currentHp, int maxHp)
    {
        var segmentCount = currentHp - maxHp;
        hpDisplay.SetFloat("SegmentCount", segmentCount);
    }
 }