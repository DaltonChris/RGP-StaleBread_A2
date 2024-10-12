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
    public float collectDelay = 2f; // Base delay time when AI decides to wait before collecting more items

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Collider2D targetItem;
    private bool isCollecting = false;
    private bool isStunned = false;
    private int itemsCollected = 0;
    private int itemsToCollectBeforeDelay;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetRandomItemsToCollect();
    }

    void Update()
    {
        if (!isCollecting && !isStunned)
        {
            FindAndMoveTowardsItem();
        }
        FlipSprite();
    }

    void FixedUpdate()
    {
        if (!isStunned)
        {
            bool isGroundedLeft = Physics2D.Raycast(leftGroundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
            bool isGroundedRight = Physics2D.Raycast(rightGroundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

            if ((rb.velocity.x < 0 && !isGroundedLeft) || (rb.velocity.x > 0 && !isGroundedRight))
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
    }

     public void StunAI(float duration)
    {
        StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;
        rb.velocity = new Vector2(0, rb.velocity.y); // Stop AI movement while stunned
        yield return new WaitForSeconds(duration);
        isStunned = false;
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
                // Stop moving when directly underneath the item and increment the count
                rb.velocity = new Vector2(0, rb.velocity.y);
                itemsCollected++;
                
                if (itemsCollected >= itemsToCollectBeforeDelay)
                {
                    StartCoroutine(CollectItem());
                }
                else
                {
                    targetItem = null; // Reset the target to look for the next item
                }
            }
        }
        else
        {
            // No item found, stop moving
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    IEnumerator CollectItem()
    {
        // Set collecting state to true
        isCollecting = true;

        // Simulate collection delay
        yield return new WaitForSeconds(collectDelay);

        // Reset target and collecting state after delay
        targetItem = null;
        isCollecting = false;

        // Reset the items collected counter and choose a new random number of items to collect before the next delay
        itemsCollected = 0;
        SetRandomItemsToCollect();
    }

    void SetRandomItemsToCollect()
    {
        // Set a random number between 1 and 4 for the next items to collect before the AI waits
        itemsToCollectBeforeDelay = Random.Range(1, 5);
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
