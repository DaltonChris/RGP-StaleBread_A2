using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Basket : MonoBehaviour
{
   public List<string> itemTags; // List of tags for items this basket can collect
    public bool isPlayerBasket; // Set this in the inspector to identify if it's the player's basket

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
                    }
                    else
                    {
                        ScoreManager.Instance.AddScore(score, false);
                    }

                    Destroy(other.gameObject);
                }
                return; // Exit after finding a matching item
            }
        }
    }
}
