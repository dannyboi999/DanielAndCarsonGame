using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //variables for the left right movement of the player 
    private float horizontal;
    private float speed = 8f;
    private float jumpingheight = 8f;
    private bool isFacingRight = true;
    
    //jump variables for the player
    private float time = 0.2f;
    private float cyoteTime = 0.2f;
    public int extraJumps = 0;
    int count = 0;
    private bool jump = false;
    float time1 = 0.2f;


    //attatches hitboxes to this script so it knows what they are basically
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundlayer;

    //connects me to the animator
    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        //gets and sets the value of basically left or right
        horizontal = Input.GetAxisRaw("Horizontal");

        //makes run and jump animations work
        animator.SetFloat("Speed", Math.Abs(horizontal));
        animator.SetBool("IsGrounded", IsGrounded());

        //jump time when not touching ground (called cyote time google it)
        if (IsGrounded() && jump == true)
        {
            Jump();
        }
        if (!IsGrounded() && count < extraJumps + 1 && jump == true)
        {
            Jump();
        }
        if (!IsGrounded() && count == 0 && jump == true)
        {
            time -= Time.deltaTime;

            if (time > 0)
            {
                Jump();
            }
            else
            {
                count ++;
            }
        }

        //resets jump variable when you 
        if(rb.velocity.y == 0f)
        {
            jump = false;
            count = 0;
            time = cyoteTime;
        }

        //jump calculations when person presses jump key
        if (Input.GetKeyDown(KeyCode.Space) && count < extraJumps + 1)
        {
            jump = true;
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        Flip();
    }

    //this moves the player left and right
    /// <summary>
    /// the reason it is in fixed update and not update
    /// basically while void update runs based on frames the person can see
    /// and fixed runs on a set amount of frames per second no matter what
    /// </summary>
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    //checks if the player is on ground
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.02f, groundlayer);
    }

    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpingheight);
        jump = false;
        count++;
    }

    //when switching form walking right to left the flips the sprite
    private void Flip()
    {
        if(isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
