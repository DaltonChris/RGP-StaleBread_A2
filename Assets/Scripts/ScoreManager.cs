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

    [Header("Item Scores")]
    public List<string> itemTags; // List of item tags
    public List<int> itemScores; // Corresponding scores for each item tag

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

        UpdatePlayerScoreUI();
        UpdateAIScoreUI();
    }

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
        // Find the index of the item tag in the list
        int index = itemTags.IndexOf(itemTag);
        if (index != -1 && index < itemScores.Count)
        {
            return itemScores[index];
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

    public int GetPlayerScore()
    {
        return playerScore;
    }

    public int GetAIScore()
    {
        return aiScore;
    }

}
