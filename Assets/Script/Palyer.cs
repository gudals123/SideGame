using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Palyer : MonoBehaviour
{
    private Rigidbody2D rb;
    public Animator anim;


    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpforce;


    [Header("Dash Info")]
    [SerializeField]
    private float dashSpeed;
    [SerializeField]
    private float dashDuration;
    
    private float dashTime;


    [SerializeField]
    private float dashCooldown;

    private float dashCooldownTimer;

    [Header("Collision info")]
    [SerializeField]
    private float groundCheckDistance;
    [SerializeField]
    private LayerMask whatIsGround;
    private bool isGrounded;

    private float xInput;

    private int facingDir = 1;
    private bool facingRight = true;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Movement();

        CheckInput();

        CollisionChecks();


        dashTime -=Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;




        FlipController();

        AnimatorControllers();
    }

    private void CollisionChecks()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer < 0)
        {
            DashAbility();
        }
    }

    private void DashAbility()
    {
        dashCooldownTimer = dashCooldown;
        dashTime = dashDuration;
    }

    private void Movement()
    {
        if(dashTime > 0)
        {
            rb.velocity = new Vector2(xInput * dashSpeed, 0);
        }
        else
        {
            rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
        }
    }

    private void Jump()
    {
        if(isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
        }
    }

    private void AnimatorControllers()
    {
        bool isMoving = rb.velocity.x != 0;

        anim.SetBool("isMoving",isMoving);
        anim.SetBool("isDashing", dashTime > 0);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isGrounded", isGrounded);

    }

    private void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }


    private void FlipController()
    {
        if(rb.velocity.x > 0 && !facingRight)
        {
            Flip();
        }
        else if(rb.velocity.x < 0 && facingRight)
        {
            Flip();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x,transform.position.y - groundCheckDistance));
    }

}
