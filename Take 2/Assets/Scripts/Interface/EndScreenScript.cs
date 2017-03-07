using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EndScreenScript : MonoBehaviour {
    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private Text[] letters;

    [SerializeField]
    private Text highScoresText;

    [SerializeField]
    private GameObject newHighScorePanel;

    [SerializeField]
    private GameObject replayLevelBtn;

    private int myScore;

    [SerializeField]
    private EventSystem eventSystem;

    void Start()
    {
        scoreText.text = PlayerPrefs.GetInt("FinalScore").ToString();
        myScore = PlayerPrefs.GetInt("FinalScore");
        string highScores = "";
        for(int i = 0; i < Leaderboard.EntryCount; i++)
        {
            Leaderboard.ScoreEntry entry = Leaderboard.GetEntry(i);
            highScores += (i + 1) + ". " + entry.mName + ", " + entry.mScore + "\n";
        }
        highScoresText.text = highScores;

        if (Leaderboard.CheckScore(myScore))
        {
            newHighScorePanel.SetActive(true);
        }

        PlayerPrefs.SetInt("FinalScore", 0);

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
            name += letters[i].text.ToString();
        }
        Leaderboard.Record(name, myScore);

        newHighScorePanel.SetActive(false);
        string highScores = "";
        for (int i = 0; i < Leaderboard.EntryCount; i++)
        {
            Leaderboard.ScoreEntry entry = Leaderboard.GetEntry(i);
            highScores += (i + 1) + ". " + entry.mName + ", " + entry.mScore + "\n";
        }
        highScoresText.text = highScores;
        eventSystem.SetSelectedGameObject(replayLevelBtn);

    }
}
