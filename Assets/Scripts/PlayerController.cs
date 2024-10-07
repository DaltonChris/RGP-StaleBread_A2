using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public LayerMask groundLayer;
    public Transform leftGroundCheck;
    public Transform rightGroundCheck;
    public float groundCheckDistance = 0.1f;

    private Rigidbody2D rb;
    private float moveInput;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        moveInput = Input.GetAxis("Horizontal");
        FlipSprite();
    }

    void FixedUpdate()
    {
        // Check for ground beneath the left and right ground checks
        bool isGroundedLeft = Physics2D.Raycast(leftGroundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        bool isGroundedRight = Physics2D.Raycast(rightGroundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

        // Determine if movement is allowed based on ground detection and input direction
        if ((moveInput < 0 && isGroundedLeft) || (moveInput > 0 && isGroundedRight) || moveInput == 0)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
        else
        {
            // Keep velocity on the y-axis but stop horizontal movement if there's no ground in the direction
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    void FlipSprite()
    {
        // Flip the sprite based on the movement direction
        if (moveInput < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveInput > 0)
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
