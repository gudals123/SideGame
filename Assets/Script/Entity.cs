using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    protected Animator anim;
    protected Rigidbody2D rb;

    [Header("Collision info")]
    [SerializeField]
    protected Transform groundCheck;
    [SerializeField]
    protected float groundCheckDistance;
    [SerializeField]
    protected LayerMask whatIsGround;
    [SerializeField]
    protected Transform wallCheck;
    [SerializeField]
    protected float wallCheckDistance;
    [SerializeField]
    protected LayerMask whatIsWall;


    protected bool isGrounded;
    protected bool isWallDetected;

    protected int facingDir = 1;
    protected bool facingRight = true;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();


        if(wallCheck ==null)
        {
            wallCheck = transform;
        }
    }



    protected virtual void Update()
    {
        CollisionChecks();
    }

    protected virtual void CollisionChecks()
    {
        isGrounded = Physics2D.Raycast(groundCheck.position, 
            Vector2.down, groundCheckDistance, whatIsGround);

        
        isWallDetected = Physics2D.Raycast(wallCheck.position,
            Vector2.right, wallCheckDistance * facingDir, whatIsWall);

    }

    protected virtual void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position,
            new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));

        Gizmos.DrawLine(wallCheck.position,
            new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));

    }
}
