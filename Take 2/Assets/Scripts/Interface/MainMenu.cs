using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour {
    [SerializeField]
    private string coopLevelName = "PaulTesting";

    [SerializeField]
    private string singlePlayerLevelName = "";

    [SerializeField]
    private GameObject HowToPlay;

    [SerializeField]
    private GameObject Credits;

    [SerializeField]
    private GameObject ModeSelection;

    [SerializeField]
    private GameObject PlayButton;

    [SerializeField]
    private GameObject SinglePlayer;

    [SerializeField]
    private EventSystem eventSystem;
    
    public  void BTN_Begin()
    {
        ModeSelection.SetActive(true);
        PlayButton.SetActive(false);
        eventSystem.SetSelectedGameObject(SinglePlayer);
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

    public void BTN_SinglePlayer()
    {
        SceneManager.LoadScene(singlePlayerLevelName);
    }

    public void BTN_Coop()
    {
        SceneManager.LoadScene(coopLevelName);
    }
}
