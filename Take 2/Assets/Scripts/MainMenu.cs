using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    [SerializeField]
    private string beginLevelName = "PaulTesting";
  public  void BTN_Begin()
    {
        SceneManager.LoadScene(beginLevelName);
    }

    public void BTN_HowToPlay()
    {

    }

    public void BTN_Credits()
    {

    }

    public void Exit()
    {

    }
}
