using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    public List<SpriteRenderer> characterSprites;
    public GameObject[] breadSprites;
    
    private Animator anim;
    
    public enum AnimationState {IDLE, RUN, STUNNED};
    AnimationState currentAnim = AnimationState.IDLE;


    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    //Method to use on other scripts to change animation
    public void ChangeAnimation(AnimationState incomingAnim)
    {
        //Skip running the code if the animation hasn't changed
        if (incomingAnim == currentAnim)
        {
            return;
        }

        //Directly play the corresponding animation
        switch (incomingAnim)
        {
            case AnimationState.RUN:
                anim.Play("Run");
                break;
            case AnimationState.STUNNED:
                anim.Play("Stunned");
                break;
            default:
                anim.Play("Idle");
                break;
        }

        currentAnim = incomingAnim;

    }

    public void FlipSprite(float moveInput)
    {
        // Flip the sprite based on the movement direction
        if (moveInput < 0)
        {
            foreach(SpriteRenderer sprite in characterSprites)
            {
                sprite.flipX = true;
            }
            
        }
        else if (moveInput > 0)
        {
            foreach(SpriteRenderer sprite in characterSprites)
            {
                sprite.flipX = false;
            }
        }
    }

    public void FillBasket(int basketIndex)
    {
        print(basketIndex);
        breadSprites[basketIndex].SetActive(true);
    }
}
