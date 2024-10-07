using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Basket : MonoBehaviour
{
    public string item1Tag = "Item1"; // Tag for item 1
    public string item2Tag = "Item2"; // Tag for item 2
    // Add more items later on.
    public bool isPlayerBasket; // Set this in the inspector to identify if it's the player's basket if the prefab isnt set already. (Should be a little tick box)

    private void OnTriggerEnter2D(Collider2D other)
    {
        int score = 0;

        // Check if the object entering the trigger is item1 or item2 and get the score value from ScoreManager
        if (other.CompareTag(item1Tag))
        {
            score = ScoreManager.Instance.GetScoreForItem(item1Tag);
        }
        else if (other.CompareTag(item2Tag))
        {
            score = ScoreManager.Instance.GetScoreForItem(item2Tag);
        }

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
    }
}
