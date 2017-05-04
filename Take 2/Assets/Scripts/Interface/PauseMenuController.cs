using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public static System.Action OnGameResumed;

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ReplayLevel()
    {
        
        SceneManager.LoadScene(GameManager.currentScene);
    }

    public void Resume()
    {
        OnGameResumed();
    }
}
