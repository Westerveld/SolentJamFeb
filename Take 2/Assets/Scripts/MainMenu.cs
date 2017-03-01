using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    [SerializeField]
    private string beginLevelName = "PaulTesting";
    [SerializeField]
    private GameObject HowToPlay;
    [SerializeField]
    private GameObject Credits;
    public  void BTN_Begin()
    {
        SceneManager.LoadScene(beginLevelName);
    }

    public void BTN_HowToPlay()
    {
        HowToPlay.SetActive(true);
        Credits.SetActive(false);
    }

    public void BTN_Credits()
    {
        Credits.SetActive(true);
        HowToPlay.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
