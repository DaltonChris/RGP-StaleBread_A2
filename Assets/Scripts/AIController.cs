using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public LayerMask groundLayer;
    public LayerMask itemLayer;
    public Transform leftGroundCheck;
    public Transform rightGroundCheck;
    public float groundCheckDistance = 0.1f;
    public Transform basket; // Reference to the AI's basket
    public float stopDistance = 0.1f; // Distance threshold for the AI to stop moving when under an item
    public Collider2D platformArea; // Defines the AI's platform area

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Collider2D targetItem;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        FindAndMoveTowardsItem();
        FlipSprite();
    }

    void FixedUpdate()
    {
        // Check for ground beneath the left and right ground checks
        bool isGroundedLeft = Physics2D.Raycast(leftGroundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        bool isGroundedRight = Physics2D.Raycast(rightGroundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

        // Restrict movement if there's no ground in the direction the AI is moving
        if ((rb.velocity.x < 0 && !isGroundedLeft) || (rb.velocity.x > 0 && !isGroundedRight))
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    void FindAndMoveTowardsItem()
    {
        if (targetItem == null || !targetItem.gameObject.activeInHierarchy)
        {
            // Find the nearest item within the AI's platform area
            Collider2D[] items = Physics2D.OverlapCircleAll(transform.position, 10f, itemLayer);
            targetItem = FindNearestItem(items);
        }

        if (targetItem != null)
        {
            float distanceToItem = Mathf.Abs(targetItem.transform.position.x - basket.position.x);

            // Move towards the item if not within the stop distance
            if (distanceToItem > stopDistance)
            {
                float direction = Mathf.Sign(targetItem.transform.position.x - basket.position.x);
                rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
            }
            else
            {
                // Stop moving when directly underneath the item
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
        else
        {
            // No item found, stop moving
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    Collider2D FindNearestItem(Collider2D[] items)
    {
        Collider2D nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider2D item in items)
        {
            // Check if the item is within the AI's platform area
            if (platformArea.bounds.Contains(item.transform.position))
            {
                float distance = Vector2.Distance(basket.position, item.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = item;
                }
            }
        }

        return nearest;
    }

    void FlipSprite()
    {
        // Flip the sprite based on the movement direction
        if (rb.velocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (rb.velocity.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    void OnDrawGizmos()
    {
        // Visualize the ground check rays in the scene view
        bool isGroundedLeft = Physics2D.Raycast(leftGroundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        bool isGroundedRight = Physics2D.Raycast(rightGroundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

        // Draw the left ray
        Gizmos.color = isGroundedLeft ? Color.green : Color.red;
        Gizmos.DrawLine(leftGroundCheck.position, leftGroundCheck.position + Vector3.down * groundCheckDistance);

        // Draw the right ray
        Gizmos.color = isGroundedRight ? Color.green : Color.red;
        Gizmos.DrawLine(rightGroundCheck.position, rightGroundCheck.position + Vector3.down * groundCheckDistance);

        // Draw the platform area if assigned
        if (platformArea != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(platformArea.bounds.center, platformArea.bounds.size);
        }
    }
}
