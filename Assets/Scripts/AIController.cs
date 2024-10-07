using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public LayerMask groundLayer;
    public Transform leftGroundCheck;
    public Transform rightGroundCheck;
    public float groundCheckDistance = 0.1f;

    private Rigidbody2D rb;
    private float moveDirection = 0;
    private float moveTimer = 0;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetRandomDirection();
    }

    void Update()
    {
        // Countdown the timer
        moveTimer -= Time.deltaTime;

        // If the timer reaches zero, set a new random direction and duration
        if (moveTimer <= 0)
        {
            SetRandomDirection();
        }

        FlipSprite();
    }

    void FixedUpdate()
    {
        // Check for ground beneath the left and right ground checks
        bool isGroundedLeft = Physics2D.Raycast(leftGroundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        bool isGroundedRight = Physics2D.Raycast(rightGroundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

        // Determine if movement is allowed based on ground detection and movement direction
        if ((moveDirection < 0 && isGroundedLeft) || (moveDirection > 0 && isGroundedRight))
        {
            rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
        }
        else
        {
            // Stop horizontal movement if there's no ground in the direction
            rb.velocity = new Vector2(0, rb.velocity.y);
            SetRandomDirection(); // Change direction if there's no ground
        }
    }

    void SetRandomDirection()
    {
        // Randomly choose a direction (-1 for left, 1 for right)
        moveDirection = Random.Range(0, 2) == 0 ? -1 : 1;
        // Set a random time interval between 0.5 and 1 second for this movement
        moveTimer = Random.Range(0.5f, 1f);
    }

    void FlipSprite()
    {
        // Flip the sprite based on the movement direction
        if (moveDirection < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveDirection > 0)
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
    }
}
