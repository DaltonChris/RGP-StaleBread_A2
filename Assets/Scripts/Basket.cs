using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour
{
    public bool isPlayerBasket; // Set this in the inspector to identify if it's the player's basket
    public SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    public CharacterAnimation charAnim;

    private int basketSpriteNumber = 4; //the set number of bread sprites to add to the basket
    //public List<Sprite> basketSprites; // A list of sprites to swap out as the score increases
    private int[] scoreThresholds = { 5, 10, 20, 30 }; // Score thresholds for updating the sprite
    private int currentSpriteIndex = 0;

    private void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
{
    // Check if the item matches any score item tag in the ScoreManager
    int score = ScoreManager.Instance.GetScoreForItem(other.tag);

    if (score > 0)
    {
        // If score is set, update the score based on whether it's the player's or AI's basket
        if (isPlayerBasket)
        {
            ScoreManager.Instance.AddScore(score, true);
            UpdateBasketSprite(ScoreManager.Instance.GetPlayerScore());
        }
        else
        {
            ScoreManager.Instance.AddScore(score, false);
            UpdateBasketSprite(ScoreManager.Instance.GetAIScore());
        }

        Destroy(other.gameObject);
        return; // Exit after finding a matching item
    }

    // Check if the item matches any obstacle tag in the ScoreManager
    float stunDuration = ScoreManager.Instance.GetStunDurationForObstacle(other.tag);

    if (stunDuration > 0)
    {
        // Apply stun based on whether it's the player's or AI's basket
        if (isPlayerBasket)
        {
            PlayerController player = GetComponentInParent<PlayerController>();
            if (player != null)
            {
                player.StunPlayer(stunDuration);
            }
        }
        else
        {
            AIController ai = GetComponentInParent<AIController>();
            if (ai != null)
            {
                ai.StunAI(stunDuration);
            }
        }

        Destroy(other.gameObject);
    }
}

    // This function checks the score and updates the sprite if a threshold is reached
    private void UpdateBasketSprite(int currentScore)
    {
        if (currentSpriteIndex < basketSpriteNumber && currentScore >= scoreThresholds[currentSpriteIndex])
        {
            charAnim.FillBasket(currentSpriteIndex);
            currentSpriteIndex++; // Move to the next sprite
        }
    }
}
