using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI TimerText;
    float RunTime;
    public float TimeLimit;

    // Start is called before the first frame update
    void Start()
    {
        RunTime = TimeLimit;
        TimerText.text = "Time Left: " + RunTime.ToString("F2"); ;
    }

    // Update is called once per frame
    void Update()
    {
        if (RunTime < 0)
        {
            TimerText.text = "GameOver";
        }
        else
        {
            RunTime -= Time.deltaTime;
            TimerText.text = "Time Left: " + RunTime.ToString("F2"); ;
        }
    }
}
