using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    #region Singleton

    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    private Rigidbody2D rb;
    private float HorizontalMove = 0f;
    private bool FacingRight = true;
    private bool DoubleJumpEnable = true;

    public CameraController camera_controller;
    public float speed = 1f;
    public float jumpForce = 8f;
    public bool isGrounded = false;
    public float checkGroundOffsetY = -1.8f;
    public float checkGroundRadius = 0.3f;
    public Animator animator;    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded) { 
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                animator.Play("Player_Jump");
            }
            else if (DoubleJumpEnable) {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                DoubleJumpEnable = false;
                animator.Play("Player_Jump");
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
            camera_controller.offset.y -= 3;
        if (Input.GetKeyUp(KeyCode.S))
            camera_controller.offset.y += 3;

        HorizontalMove = Input.GetAxisRaw("Horizontal") * speed;

        animator.SetFloat("HorizontalMove", Mathf.Abs(HorizontalMove));

        animator.SetBool("InAir", !isGrounded);



        if (HorizontalMove < 0 && FacingRight || HorizontalMove > 0 && !FacingRight)
            Flip();

        Vector2 targetVelocity = new Vector2(HorizontalMove * 10, rb.velocity.y);
        rb.velocity = targetVelocity;

        CheckGround();

        //__________________________________________
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("UI");
        //__________________________________________
    }

    //private void FixedUpdate()
    //{

    //}

    private void Flip()
    {
        FacingRight = !FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y + checkGroundOffsetY), checkGroundRadius);

        if (colliders.Length > 1)
        {
            DoubleJumpEnable = true;
            isGrounded = true;
        }
        else
            isGrounded = false;
    }

    public bool GetFacing()
    {
        return FacingRight;
    }
}
