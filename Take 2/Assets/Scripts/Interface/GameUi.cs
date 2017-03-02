using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GameUi : MonoBehaviour
{

    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text healthText;
    [SerializeField]
    private Text waveNumberText;
    [SerializeField]
    private Image frozenIcon;

    // Use this for initialization
    void Start()
    {
        GameManager.OnScoreChanged += UpdateScoreDisplay;
        ShipController.OnPlayerHit += UpdateHealthDisplay;
        GameManager.OnWaveChanged += UpdateWaveDisplay;
        ShipController.OnFreezeChargeUsed += UpdateFreezeDisplay;
    }
    void OnDestroy()
    {
        GameManager.OnScoreChanged -= UpdateScoreDisplay;
        ShipController.OnPlayerHit -= UpdateHealthDisplay;
        GameManager.OnWaveChanged -= UpdateWaveDisplay;
    }

    void UpdateScoreDisplay(int score)
    {
        scoreText.text = score.ToString();
    }
   void UpdateHealthDisplay(float health)
    {
        healthText.text = health.ToString();
    }

    void UpdateWaveDisplay(int waveNumber)
    {
        waveNumberText.text = waveNumber.ToString();
    }

    void UpdateFreezeDisplay(float freezeDuration)
    {       Color color = new Color(1, 1, 1, 0.5f);
            frozenIcon.color = color;
    }

}
