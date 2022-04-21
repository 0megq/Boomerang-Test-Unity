using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform boomerang;

    public float boomerangRange = 3f;
    public float increment = 0.5f;
    public float jumpHeight;

    public Rigidbody2D rb;

    private bool holding = true;

    public LayerMask groundLayer;

    public float speed;

    private bool facingRight = true;

    private float moveInput;

    private int facing = 1;

    public int maxThrows;
    private int throwCounter;

    private bool isGrounded = true;
    public float groundCheckRadius;
    public Transform groundCheck;

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (holding)
            {
                ThrowBoomerang();
            }
            else
            {
                CatchBoomerang();
            }
        }
    }

    void FixedUpdate()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(speed * Time.deltaTime * moveInput, rb.velocity.y);

        isGrounded = Physics2D.Raycast(transform.position + new Vector3(0, -0.5f * transform.localScale.y, 0), -Vector3.up, 0.1f, groundLayer) || Physics2D.Raycast(transform.position + new Vector3(0.5f * transform.localScale.x, -0.5f * transform.localScale.y, 0), -Vector3.up, 0.1f, groundLayer) || Physics2D.Raycast(transform.position + new Vector3(-0.5f * transform.localScale.x, -0.5f * transform.localScale.y, 0), -Vector3.up, 0.1f, groundLayer);

        if (isGrounded)
        {
            throwCounter = 0;
        }

        if (facingRight == false && moveInput > 0)
        {
            Flip();
        }else if(facingRight == true && moveInput < 0)
        {
            Flip();
        }

        if(facingRight)
        {
            facing = 1;
        }
        else
        {
            facing = -1;
        }

        
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (!holding && coll.transform == boomerang)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            CatchBoomerang();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    void ThrowBoomerang()
    {
        if (!isGrounded)
        {
            throwCounter++;
            if(throwCounter> maxThrows)
            {
                return;
            }
        }

        float currentRange = boomerangRange;
        RaycastHit2D hit = Physics2D.Raycast(boomerang.position, Vector2.right * facing, currentRange + 0.5f, groundLayer);

        while (hit)
        {
            currentRange -= increment;
            if (currentRange <= 0.6f)
            {
                CatchBoomerang();
                return;
            }
            hit = Physics2D.Raycast(boomerang.position, Vector2.right * facing, currentRange+0.5f, groundLayer);
        }

        if (!hit)
        {
            boomerang.parent = null;
            boomerang.position += Vector3.right * (currentRange) * facing;
            holding = false;
        }
    }

    void CatchBoomerang()
    {
        
        boomerang.parent = transform;
        boomerang.localPosition = new Vector2(0.5f, -0.15f);
        holding = true;
    }

}
