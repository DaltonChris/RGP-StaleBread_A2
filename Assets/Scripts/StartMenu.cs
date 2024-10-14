using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1.0f;
    }

    public void StartMenuScene()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }
}
