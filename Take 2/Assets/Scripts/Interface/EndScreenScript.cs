using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class EndScreenScript : MonoBehaviour {
    [SerializeField]
    private Text scoreText;

    void Start()
    {
        scoreText.text = PlayerPrefs.GetInt("FinalScore").ToString();
        PlayerPrefs.SetInt("FinalScore", -666);
        PlayerPrefs.Save();
    }


	public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ReplayLevel()
    {      
      SceneManager.LoadScene("PaulTesting");
    }
}
