using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI WinnerText;
    float RunTime;
    float TimeLimit = 33.33f;
    public Canvas GameOverCanvas;
    public ScoreManager ScoreManager;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        RunTime = TimeLimit;
        TimerText.text = "Time Left: " + RunTime.ToString("F2");
        GameOverCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (RunTime <= 0) 
        {
            RunTime = 0;
            TimerText.text = "GameOver"; // Display game over message
            GameOverCanvas.enabled = true; // Enable the GameOver canvas
            Time.timeScale = 0.1f;
            WinnerText.text = (ScoreManager.GetPlayerScore() > ScoreManager.GetAIScore())
                    ? "Winner: Player"
                    : (ScoreManager.GetPlayerScore() == ScoreManager.GetAIScore()
                        ? "It's a Tie!"
                        : "Winner: AI");

        }
        else
        {
            RunTime -= Time.deltaTime;
            TimerText.text = "Time Left: " + RunTime.ToString("F2"); ;

            //Running out of time vfx
            if(RunTime <= 5.1f)
            {
                TimerText.color = new Color(1.0f, 0.25f, 0.25f, 1.0f);
                TimerText.fontSize += 0.005f;
            }

        }
    }

    
}
