using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] private float hp;
    private float cur_hp;
    private Rigidbody2D rb;
    private float HorizontalMove = 0f;
    [SerializeField] private bool FacingRight;
    private bool DoubleJumpEnable = true;

    public CameraController camera_controller;
    public float speed = 1f;
    public float jumpForce = 8f;
    public bool isGrounded = false;
    public float checkGroundOffsetY = -1.8f;
    public float checkGroundRadius = 0.3f;
    public Animator animator;
    private bool _isDashing;
    [SerializeField] private float _dashTime = 0.5f;
    [SerializeField] private float _dashSpeed;
    [SerializeField] private AnimationCurve _dashSpeedCurve;
    [SerializeField] private LayerMask notGround;

    [SerializeField] private bool _ableDoubleJump;
    [SerializeField] private bool _ableDash;

    [SerializeField] private Slider HPBar;

    public void dash_SetTrue() { _ableDash = true; }
    public void doubleJump_SetTrue() { _ableDoubleJump = true; }

    void Start()
    {
        if (!FacingRight) {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }


        rb = GetComponent<Rigidbody2D>();
        cur_hp = hp;
        UpdateHpBar();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded) { 
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                animator.Play("Player_Jump");
                animator.Play("Legs_Jump");
            }
            else if (_ableDoubleJump && DoubleJumpEnable) {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                DoubleJumpEnable = false;
                animator.Play("Player_Jump");
                animator.Play("Legs_Jump");
            }
        }

       

        if (Input.GetKeyDown(KeyCode.S))
            camera_controller.offset.y -= 3;
        if (Input.GetKeyUp(KeyCode.S))
            camera_controller.offset.y += 3;

        HorizontalMove = Input.GetAxisRaw("Horizontal") * speed;

        animator.SetFloat("HorizontalMove", Mathf.Abs(HorizontalMove));

        animator.SetBool("InAir", !isGrounded);


        CheckGround();


        //__________________________________________
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("UI");
        //__________________________________________
        if (Input.GetKeyDown(KeyCode.LeftShift) && _ableDash)
        {
            //rb.AddForce(new Vector2((GetFacing() ? 1 : -1) * 500, 0), ForceMode2D.Impulse);
            StartCoroutine(Dash(new Vector2(GetFacing() ? 1 : -1, 0)));
        }
        if (_isDashing) return;
        if (HorizontalMove < 0 && FacingRight || HorizontalMove > 0 && !FacingRight)
            Flip();

        Vector2 targetVelocity = new Vector2(HorizontalMove * 10, rb.velocity.y);
        rb.velocity = targetVelocity;
    }

    public void TakeDamage(float damage)
    {
        cur_hp -= damage;
        UpdateHpBar();
        Debug.Log(this.name + " hp: " + cur_hp);
        if (cur_hp <= 0)
            Die();
    }

    private void Die()
    {
        gameObject.SetActive(false);
        Debug.Log("You are killed");
    }

    private void Flip()
    {
        FacingRight = !FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y + checkGroundOffsetY), checkGroundRadius, ~notGround);

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


    private IEnumerator Dash(Vector2 direction)
    {
        if (direction == Vector2.zero) yield break;
        if (_isDashing) yield break;

        _isDashing = true;

        var elapsedTime = 0f;
        while (elapsedTime < _dashTime)
        {
            var velocityMultiplier = _dashSpeed * _dashSpeedCurve.Evaluate(elapsedTime);

            ApplyVelocity(direction, velocityMultiplier);

            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        _isDashing = false;
        yield break;
    }

    private void ApplyVelocity(Vector3 desiredVelocity, float multiplier) // Дублирующийся код всегда выносить в отдельный метод
    {
        var velocity = rb.velocity;

        velocity.y = desiredVelocity.y == 0 ? velocity.y : desiredVelocity.y * multiplier;// чтобы не ломать физику, скорость по Y будет изменяться только если это нужно
        velocity.x = desiredVelocity.x * multiplier;
        

        rb.velocity = velocity;
    }

    private void UpdateHpBar()
    {
        HPBar.value = (hp - cur_hp) / hp;
    }
}

