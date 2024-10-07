using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI aiScoreText;

    // Set the points for each item type in the inspector
    [Header("Item Scores")]
    public int item1Score = 1;
    public int item2Score = 2;

    private int playerScore = 0;
    private int aiScore = 0;

    private void Awake()
    {
        // Singleton pattern to access the ScoreManager globally
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Updates the UI on awake so it displays the scores and not "NewText"
        UpdatePlayerScoreUI();
        UpdateAIScoreUI();
    }

    // Adds the score to the UI.
    public void AddScore(int score, bool isPlayer)
    {
        if (isPlayer)
        {
            playerScore += score;
            UpdatePlayerScoreUI();
        }
        else
        {
            aiScore += score;
            UpdateAIScoreUI();
        }
    }

    public int GetScoreForItem(string itemTag)
    {
        // Return the score value based on the item tag
        if (itemTag == "Item1")
        {
            return item1Score;
        }
        else if (itemTag == "Item2")
        {
            return item2Score;
        }

        // If no matching tag is found, return 0
        return 0;
    }

    private void UpdatePlayerScoreUI()
    {
        playerScoreText.text = "Player Score: " + playerScore;
    }

    private void UpdateAIScoreUI()
    {
        aiScoreText.text = "AI Score: " + aiScore;
    }
}
