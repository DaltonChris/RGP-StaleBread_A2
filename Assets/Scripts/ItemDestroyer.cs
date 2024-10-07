using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDestroyer : MonoBehaviour
{
    // Make sure to set the tag for your ground objects in Unity inspector or it wont trigger.
    public string groundTag = "Ground";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the item collides with the ground
        if (collision.collider.CompareTag(groundTag))
        {
            Destroy(gameObject);
        }
    }
}
