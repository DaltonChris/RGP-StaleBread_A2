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

    [Header("Obstacle Stun Durations")]
    public List<string> obstacleTags; // List of obstacle tags
    public List<float> obstacleStunDurations; // Corresponding stun durations for each obstacle

    private int playerScore = 0;
    private int aiScore = 0;

    [Header("Particle Effects")]
    public GameObject scoreParticlePrefab; // particle effect

    [Header("Particle Effect Spawn Locations")]
    public Transform playerBasketSpawnLocation; // Player particle spawn object
    public Transform aiBasketSpawnLocation; // AI basket particle spawn objext

    private void Awake()
    {
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

            // Trigger particle effect basket
            TriggerScoreParticle(true);
        }
        else
        {
            aiScore += score;
            UpdateAIScoreUI();

            // Trigger particle effect basket
            TriggerScoreParticle(false);
        }
    }

    private void TriggerScoreParticle(bool isPlayer)
    {
        if (scoreParticlePrefab != null)
        {
            // if only plyr scores, trigger particle effect there
            if (isPlayer && playerBasketSpawnLocation != null)
            {
                Instantiate(scoreParticlePrefab, playerBasketSpawnLocation.position, Quaternion.identity);
            }
            // If AI scores, trigger particle effect there
            else if (!isPlayer && aiBasketSpawnLocation != null)
            {
                Instantiate(scoreParticlePrefab, aiBasketSpawnLocation.position, Quaternion.identity);
            }
        }
    }

    public int GetScoreForItem(string itemTag)
    {
        int index = itemTags.IndexOf(itemTag);
        if (index != -1 && index < itemScores.Count)
        {
            return itemScores[index];
        }

        return 0;
    }

    public float GetStunDurationForObstacle(string obstacleTag)
    {
        int index = obstacleTags.IndexOf(obstacleTag);
        if (index != -1 && index < obstacleStunDurations.Count)
        {
            return obstacleStunDurations[index];
        }
        return 0;
    }

    private void UpdatePlayerScoreUI()
    {
        playerScoreText.text = ":" + playerScore;
    }

    private void UpdateAIScoreUI()
    {
        aiScoreText.text = aiScore + ": ";
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
