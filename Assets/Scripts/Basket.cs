using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour
{
    public List<string> itemTags; // List of tags for items this basket can collect
    public bool isPlayerBasket; // Set this in the inspector to identify if it's the player's basket
    public SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    public List<Sprite> basketSprites; // A list of sprites to swap out as the score increases
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
        // Check if the item matches any of the tags in the list
        for (int i = 0; i < itemTags.Count; i++)
        {
            if (other.CompareTag(itemTags[i]))
            {
                int score = ScoreManager.Instance.GetScoreForItem(itemTags[i]);

                // If score is set, update the score based on whether it's the player's or AI's basket
                if (score > 0)
                {
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
                }
                return; // Exit after finding a matching item
            }
        }
    }

    // This function checks the score and updates the sprite if a threshold is reached
    private void UpdateBasketSprite(int currentScore)
    {
        if (currentSpriteIndex < basketSprites.Count && currentScore >= scoreThresholds[currentSpriteIndex])
        {
            spriteRenderer.sprite = basketSprites[currentSpriteIndex];
            currentSpriteIndex++; // Move to the next sprite
        }
    }
}
