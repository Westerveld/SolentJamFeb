using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class EndScreenScript : MonoBehaviour {
    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private Text[] letters;

    [SerializeField]
    private Text highScoresText;

    [SerializeField]
    private GameObject newHighScorePanel;

    void Start()
    {
        scoreText.text = PlayerPrefs.GetInt("FinalScore").ToString();
        int myScore = PlayerPrefs.GetInt("FinalScore");
        string highScores = "";
        for(int i = 0; i < Leaderboard.EntryCount; i++)
        {
            Leaderboard.ScoreEntry entry = Leaderboard.GetEntry(i);
            highScores += (i + 1) + "." + entry.mName + ", " + entry.mScore + "\n";
        }
        highScoresText.text = highScores;

        if (Leaderboard.CheckScore(myScore))
        {
            newHighScorePanel.SetActive(true);
        }


    }


	public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ReplayLevel()
    {      
      SceneManager.LoadScene("PaulTesting");
    }
    
    public void SubmitScore()
    {
        string name = "";
        for(int i = 0; i< letters.Length; i++)
        {
            name += letters[i].text;
        }

        Leaderboard.Record(name, PlayerPrefs.GetInt("FinalScore"));

        newHighScorePanel.SetActive(false);
    }
}
