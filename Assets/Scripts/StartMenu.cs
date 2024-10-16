using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void StartGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);
    }

    public void StartMenuScene()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }
}
