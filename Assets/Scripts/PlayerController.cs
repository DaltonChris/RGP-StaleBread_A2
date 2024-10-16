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
    private CharacterAnimation playerAnim;
    private bool isStunned = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<CharacterAnimation>();
    }

    void Update()
    {
        if (!isStunned)
        {
            moveInput = Input.GetAxis("Horizontal");
            playerAnim.FlipSprite(moveInput);
        }
    }

    void FixedUpdate()
    {
        if (!isStunned)
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

            if (moveInput != 0)
            {
                playerAnim.ChangeAnimation(CharacterAnimation.AnimationState.RUN);
            }
            else
            {
                playerAnim.ChangeAnimation(CharacterAnimation.AnimationState.IDLE);
            }
        }
    }

    public void StunPlayer(float duration)
    {
        StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;
        playerAnim.ChangeAnimation(CharacterAnimation.AnimationState.STUNNED);
        rb.velocity = new Vector2(0, rb.velocity.y); // Stop player movement while stunned
        yield return new WaitForSeconds(duration);
        isStunned = false;
        playerAnim.ChangeAnimation(CharacterAnimation.AnimationState.IDLE);
    }


    //DEPRECIATED: FlipSprite is now a function on the CharacterAnimation script, due to the need to flip 6 sprites at once
/*     void FlipSprite()
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
    } */
}
