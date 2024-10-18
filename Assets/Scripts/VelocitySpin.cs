using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocitySpin : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;

    // Update is called once per frame
    void Update()
    {
        anim.speed = rb.velocity.y*-0.25f;
    }
}
