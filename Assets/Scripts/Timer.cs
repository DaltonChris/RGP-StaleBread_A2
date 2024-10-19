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

    public AudioClip GameStart;
    public AudioClip YouLose;
    public AudioClip YouWin;

    private bool GameOverSFXplayed = false;
    private AudioSource gameMusic;


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        RunTime = TimeLimit;
        TimerText.text = "Time Left: " + RunTime.ToString("F2");
        GameOverCanvas.enabled = false;

        gameMusic = GameObject.FindWithTag("GameMusic").GetComponent<AudioSource>();
        gameMusic.volume = 0.4f;
        gameMusic.pitch = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (RunTime <= 0)
        {
            RunTime = 0;
            TimerText.text = "GameOver"; // Display game over message
            GameOverCanvas.enabled = true; // Enable the GameOver canvas

            int playerScore = ScoreManager.GetPlayerScore();
            int aiScore = ScoreManager.GetAIScore();
            int pointDifference = Mathf.Abs(playerScore - aiScore);

            Time.timeScale = 0.1f;

            // Check winner and display point difference
            WinnerText.text = (playerScore > aiScore)
                ? $"Winner: Player\nBeat AI by {pointDifference} points"
                : (playerScore == aiScore
                    ? "It's a Tie!"
                    : $"Winner: AI\nBeat Player by {pointDifference} points");
        }
        else
        {
            RunTime -= Time.deltaTime;
            TimerText.text = "Time Left: \n" + RunTime.ToString("F2"); ;

            //Running out of time vfx
            if(RunTime <= 5.1f)
            {
                TimerText.color = new Color(1.0f, 0.25f, 0.25f, 1.0f);
                TimerText.fontSize += 0.02f;
            }

        }
    }

    void FixedUpdate()
    {
        //Must be on fixed time to work! Otherwise the sfx play slowly and is horrible
        //End of game sfx won't play unless starting from the start scene, due to the unfound gameMusic
        //The GameOverSFXplayed bool prevents it from playing every frame
        if (RunTime <= 0 && GameOverSFXplayed == false)
        {
            gameMusic.volume = 0.1f;

            if(ScoreManager.GetPlayerScore() > ScoreManager.GetAIScore())
            {
                SfxManager.Instance.PlaySfx(YouWin);
                gameMusic.pitch = 2.0f;
                GameOverSFXplayed = true;
            }
            else
            {
                SfxManager.Instance.PlaySfx(YouLose);
                gameMusic.pitch = 0.5f;
                GameOverSFXplayed = true;
            }
        }
    }

    
}
