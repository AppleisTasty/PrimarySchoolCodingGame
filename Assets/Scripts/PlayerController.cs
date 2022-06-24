using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // all arguments
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;
    private enum State { idle, running, jumping, falling, hurt }
    public enum Move { doNothing, left, right, jumpLeft, jumpRight}
    private State state = State.idle;
    public Move move;

    [SerializeField] private LayerMask ground;
    float moveForce = 8.5f;
    float jumpForce = 15;
    private float hurtForce = 10f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame

    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        switch (move)
        {
            case Move.left:
                MoveLeft();
                move = Move.doNothing;
                break;
            case Move.right:
                MoveRight();
                move = Move.doNothing;
                break;
            case Move.jumpLeft:
                JumpLeft();
                move = Move.doNothing;
                break;
            case Move.jumpRight:
                JumpRight();
                move = Move.doNothing;
                break;
            default:
                move = Move.doNothing;
                break;
        }
        AnimationState();
        anim.SetInteger("state", (int)state); // set corresponding animation
    }
    public void MoveRight()
    {
        transform.localScale = new Vector2(1, 1);
        rb.velocity = new Vector2(moveForce, 0);  //rb.velocity.y
    }

    public void MoveLeft()
    {
        transform.localScale = new Vector2(-1, 1);
        rb.velocity = new Vector2(-moveForce, 0); //holds x&y value 
    }

    public void JumpRight()
    {
        rb.velocity = new Vector2(moveForce, jumpForce);
        state = State.jumping;
    }

    public void JumpLeft()
    {
        rb.velocity = new Vector2(-moveForce, jumpForce);
        state = State.jumping;
    }

    private void AnimationState()
    {
        if (state == State.jumping)
        {
            if (rb.velocity.y < 0.1f)
            {
                state = State.falling;
            }
        }
        else if (state == State.falling)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }
        else if (state == State.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                state = State.idle;
            }
        }
        else if (Mathf.Abs(rb.velocity.x) > 1f)
        {
            //going right
            state = State.running;
        }

        else
        {
            state = State.idle;
        }

    }
}
