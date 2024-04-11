using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : Entity
{
    


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

    [Header("Attack info")]
    [SerializeField]
    private float comboTime  = 0.3f;
    private float comboTimeWindow;
    private bool isAttacking;
    private int comboCounter;

    private float xInput;



    protected override void Start()
    {
      base.Start();
    }

    protected override void Update()
    {
        base.Update();



        Movement();

        CheckInput();

        CollisionChecks();


        dashTime -=Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;
        comboTimeWindow -= Time.deltaTime;



        FlipController();

        AnimatorControllers();
    }

    public void AttackOver()
    {
        isAttacking = false;

        comboCounter++;

        if(comboCounter > 2 )
            comboCounter = 0;


    }




    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.X))
        {
            StartAttackEvent();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer < 0)
        {
            DashAbility();
        }
    }

    private void StartAttackEvent()
    {
        if (comboTimeWindow < 0)
            comboCounter = 0;

        if(isGrounded)
        {
            isAttacking = true;
            comboTimeWindow = comboTime;
        }

    }

    private void DashAbility()
    {
        if(dashCooldownTimer < 0 && !isAttacking)
        {
            dashCooldownTimer = dashCooldown;
            dashTime = dashDuration;
        }
    }

    private void Movement()
    {

        if(isAttacking && isGrounded)
        {
            rb.velocity = new Vector2(0,0);
        }
        else if(dashTime > 0)
        {
            rb.velocity = new Vector2(facingDir * dashSpeed, 0);
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
        anim.SetFloat("yVelocity", rb.velocity.y);

        anim.SetBool("isMoving",isMoving);
        anim.SetBool("isDashing", dashTime > 0);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetInteger("comboCounter",comboCounter);
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


}
