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
    public enum Move { doNothing, moveLeft, moveRight, jumpLeft, jumpRight, heightReduce, jumpIncrease}
    private State state = State.idle;
    public Move move;

    //sound track
    public AudioSource jumpAudio;
    public AudioSource collectableAudio;
    public AudioSource moveAudio;

    [SerializeField] private LayerMask ground;
    [SerializeField]float moveForce = 8f;
    float jumpForce = 14;
    private float hurtForce = 10f;
    private float heightReduceRatio = 0.6f;


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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Cherry")
        {
            collectableAudio.Play();
            Destroy(collision.gameObject);


        }
        
    }

    private void Movement()
    {
        switch (move)
        {
            case Move.moveLeft:
                MoveLeft();
                move = Move.doNothing;
                break;
            case Move.moveRight:
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
            case Move.heightReduce:
                HeightReduce();
                move = Move.doNothing;
                break;
            case Move.jumpIncrease:
                JumpIncrease();
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
        transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        rb.velocity = new Vector2(moveForce, 0);  //rb.velocity.y
    }

    public void MoveLeft()
    {
        transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        rb.velocity = new Vector2(-moveForce, 0); //holds x&y value 
    }

    public void JumpRight()
    {
        jumpAudio.Play();
        transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        rb.velocity = new Vector2(moveForce/2, jumpForce);
        state = State.jumping;
    }

    public void JumpLeft()
    {
        jumpAudio.Play();
        transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        rb.velocity = new Vector2(-moveForce/2, jumpForce);
        state = State.jumping;
    }

    public void HeightReduce()
    {
        transform.localScale = new Vector2(heightReduceRatio * transform.localScale.x, heightReduceRatio * transform.localScale.y);
    }

    public void JumpIncrease()
    {
        jumpForce *= 1.5f;
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

    public void PlayMoveAudio()
    {
        moveAudio.Play();
    }
}
