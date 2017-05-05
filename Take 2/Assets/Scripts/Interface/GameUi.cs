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

    [SerializeField]
    private Text nextWaveText;

    [SerializeField]
    private Text multiplierText;

    [SerializeField]
    private Animator[] powerUpText;

    // Starting Multiplier
    private int multiplier = 1;
 
    // Use this for initialization
    void Start()
    {
        GameManager.OnScoreChanged += UpdateScoreDisplay;
        GameManager.OnWaveChanged += UpdateWaveDisplay;
        GameManager.OnWaveEnded += WaveIncoming;
        ShipController.OnMultiplierChanged += UpdateMultiplierDisplay;
        ShipController.OnFreezeChargeUsed += UpdateFreezeDisplay;
        //ShipController.OnStatsChange += UpdateUiStats;
        ShipController.OnHealthChanged += UpdatePlayerHealth;
        ShipController.OnPowerUpCollected += ShowPowerUpText;
        currentHighScore = PlayerPrefs.GetInt("HighestScore");
        highScoreText.text = currentHighScore.ToString();
    }
    void OnDestroy()
    {
        GameManager.OnScoreChanged -= UpdateScoreDisplay;
        GameManager.OnWaveChanged -= UpdateWaveDisplay;
        GameManager.OnWaveEnded -= WaveIncoming;
        ShipController.OnMultiplierChanged -= UpdateMultiplierDisplay;
        ShipController.OnFreezeChargeUsed -= UpdateFreezeDisplay;
        ShipController.OnHealthChanged -= UpdatePlayerHealth;
        ShipController.OnPowerUpCollected -= ShowPowerUpText;
        //ShipController.OnStatsChange -= UpdateUiStats;
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


   /* void UpdateUiStats(float value, float maxValue, PowerUpType powerUpType)
    {
       float barValue = CalculateBarFillValue(value, maxValue);

        switch (powerUpType)
        {
            case PowerUpType.FireRate:
                fireRateText.text = value.ToString("F2") + "s/s";
                fireRateBar.fillAmount = barValue;
               
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
    }*/

    void UpdateMultiplierDisplay(int amount)
    {
        multiplier += amount;
        multiplierText.text = multiplier + "x";
    }

    void UpdatePlayerHealth(float currentHealth)
    {
        float barValue = CalculateBarFillValue(currentHealth, 100);

        healthText.text = currentHealth.ToString() + " / 100";
        healthBar.fillAmount = barValue;
    }

    void WaveIncoming()
    {
        StartCoroutine(WaitForTimer());   
    }

    IEnumerator WaitForTimer()
    {
        yield return new WaitForSeconds(5f);
        nextWaveText.GetComponent<Animator>().SetTrigger("NextWave");
    }

    void ShowPowerUpText(PowerUpType type)
    {
        switch(type)
        {
            case PowerUpType.Health:
                powerUpText[0].SetTrigger("pickedUp");
                break;
            /*case PowerUpType.Invun:
                powerUpText[1].SetTrigger("pickedUp");
                break;*/
            case PowerUpType.Damage:
                powerUpText[2].SetTrigger("pickedUp");
                break;
            case PowerUpType.Explosion:
                powerUpText[3].SetTrigger("pickedUp");
                break;
            case PowerUpType.Freeze:
                powerUpText[4].SetTrigger("pickedUp");
                break;
            default:
                break;
        }
    }
}
