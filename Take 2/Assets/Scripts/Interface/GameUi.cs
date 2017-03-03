using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GameUi : MonoBehaviour
{

    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text highScoreText;
    private int currentHighScore;
    [SerializeField]
    private Text waveNumberText;
    [SerializeField]
    private Image[] frozenIcons;


    [SerializeField]
    private Image fireRateBar;
    [SerializeField]
    private Text fireRateText;
    [SerializeField]
    private Image damageBar;
    [SerializeField]
    private Text damageText;
    [SerializeField]
    private Image freezeDurationBar;
    [SerializeField]
    private Text freezeDurationText;
    [SerializeField]
    private Image enginePowerBar;
    [SerializeField]
    private Text enginePowerText;
    [SerializeField]
    private Image healthBar;
    [SerializeField]
    private Text healthText;


 
    // Use this for initialization
    void Start()
    {
        GameManager.OnScoreChanged += UpdateScoreDisplay;
        GameManager.OnWaveChanged += UpdateWaveDisplay;
        ShipController.OnFreezeChargeUsed += UpdateFreezeDisplay;
        ShipController.OnStatsChange += UpdateUiStats;
        currentHighScore = PlayerPrefs.GetInt("HighestScore");
        highScoreText.text = currentHighScore.ToString();
    }
    void OnDestroy()
    {
        GameManager.OnScoreChanged -= UpdateScoreDisplay;
        GameManager.OnWaveChanged -= UpdateWaveDisplay;
        ShipController.OnFreezeChargeUsed -= UpdateFreezeDisplay;
        ShipController.OnStatsChange -= UpdateUiStats;
    }

    void UpdateScoreDisplay(int score)
    {
        scoreText.text = score.ToString();
        if(score > currentHighScore)
        {
            highScoreText.text = score.ToString();
        }
    }


    void UpdateWaveDisplay(int waveNumber)
    {
        waveNumberText.text = waveNumber.ToString();
    }

    void UpdateFreezeDisplay(int freezeCharges)
    {
        int i = 0;
        foreach (Image image in frozenIcons)
        {
            if (i < freezeCharges)
            {
                image.gameObject.SetActive(true);
            }
            else
            {
                image.gameObject.SetActive(false);
            }
            i++;
        }
    }

    float CalculateBarFillValue(float currentValue, float maxValue)
    {
        return Mathf.Clamp(currentValue / maxValue, 0, 1);
    }


    void UpdateUiStats(float value, float maxValue, PowerUpType powerUpType)
    {
       float barValue = CalculateBarFillValue(value, maxValue);

        switch (powerUpType)
        {
            case PowerUpType.FireRate:
                fireRateText.text = (1 / value).ToString("F2") + "s/s";
               // fireRateBar.fillAmount = CalculateBarFillValue((1/maxValue), (1 / value));
               
                break;
            case PowerUpType.TurretDamage:
                damageText.text = value.ToString();
                damageBar.fillAmount = barValue;
                break;
            case PowerUpType.FreezeTime:
                freezeDurationText.text = value.ToString() + " sec";
                freezeDurationBar.fillAmount = barValue;
                break;
            case PowerUpType.MoveSpeed:
                enginePowerText.text = value.ToString();
                enginePowerBar.fillAmount = barValue;
                break;
            case PowerUpType.MaxHealth:
                healthText.text = value.ToString() + " / " + maxValue.ToString();
                healthBar.fillAmount = barValue;
                break;
            case PowerUpType.Health:
                healthText.text =  value.ToString() + " / " + maxValue.ToString();
                healthBar.fillAmount = barValue;
                break;
            case PowerUpType.FreezeCharge:
                UpdateFreezeDisplay((int)value);
                break;
            default:
                break;
        }
    }

}
