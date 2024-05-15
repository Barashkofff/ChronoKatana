using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;
using System.Linq;

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
    public float speed = 10f;
    public float jumpForce = 8f;
    public bool isGrounded = false;
    public float checkGroundOffsetY = -1.8f;
    public float checkGroundRadius = 0.3f;
    public Animator animator;
    private bool _isDashing;
    [SerializeField] private float _dashTime = 0.5f;
    [SerializeField] private float _dashSpeed;
    [SerializeField] private AnimationCurve _dashSpeedCurve;
    [SerializeField] private Transform legs;
    [SerializeField] private LayerMask notGround;

    [SerializeField] private bool _ableDoubleJump;
    [SerializeField] private bool _ableDash;

    [SerializeField] private Slider HPBar;
    [SerializeField] private LoseMenu _loseMenu;

    private Vector2 save_pos;
    private float save_pos_time = 0.5f;
    private float save_pos_timer = 0.5f;
    private float coyoteTime = 0.1f;
    private float coyoteCounter;

    private int curTable;
    public void dash_SetTrue() { _ableDash = true; }
    public void doubleJump_SetTrue() { _ableDoubleJump = true; }
    public int CurTable { get { return curTable; } set { curTable = value; } }

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
            if (coyoteCounter > 0) { 
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                if (!animator.GetBool("IsAttackStart"))
                    animator.Play("Player_Jump");
                animator.Play("Legs_Jump");
                coyoteCounter = 0;
            }
            else if (_ableDoubleJump && DoubleJumpEnable) {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                DoubleJumpEnable = false;
                if (!animator.GetBool("IsAttackStart"))
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
        
        //__________________________________________
        if (Input.GetKeyDown(KeyCode.LeftShift) && _ableDash)
        {
            //rb.AddForce(new Vector2((GetFacing() ? 1 : -1) * 500, 0), ForceMode2D.Impulse);
            StartCoroutine(Dash(new Vector2(GetFacing() ? 1 : -1, 0)));
        }
        if (_isDashing) return;
        if (HorizontalMove < 0 && FacingRight || HorizontalMove > 0 && !FacingRight)
            Flip();

        Vector2 targetVelocity = new Vector2(HorizontalMove, rb.velocity.y);
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

    public void Heal(float heal)
    {
        cur_hp = (cur_hp + heal > hp) ? hp : cur_hp + heal;
        UpdateHpBar();
    }

    private void Die()
    {
        gameObject.SetActive(false);
        _loseMenu.Lose();
        Debug.Log("You are killed");
        SaveLoadManager.SaveGame();
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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y + checkGroundOffsetY), checkGroundRadius, ~notGround).Where(x => !x.isTrigger).ToArray();

        if (save_pos_timer >= save_pos_time)
        {
            if (isGrounded)
                save_pos = transform.position;
            save_pos_timer = 0;
        }
        save_pos_timer += Time.deltaTime;

        if (colliders.Length > 1)
        {
            DoubleJumpEnable = true;
            isGrounded = true;
            coyoteCounter = coyoteTime;
        }
        else
        {
            isGrounded = false;
            coyoteCounter -= Time.deltaTime;
        }
    }

    public bool GetFacing()
    {
        return FacingRight;
    }


    private IEnumerator Dash(Vector2 direction)
    {
        animator.SetBool("isDashing", true);
        animator.Play("Legs_Dash");
        if (direction == Vector2.zero) yield break;
        if (_isDashing) yield break;

        Physics2D.IgnoreLayerCollision(gameObject.layer, 6, true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, 10, true);
        _isDashing = true;

        var elapsedTime = 0f;
        while (elapsedTime < _dashTime)
        {
            var velocityMultiplier = _dashSpeed * _dashSpeedCurve.Evaluate(elapsedTime);

            ApplyVelocity(direction, velocityMultiplier);

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Physics2D.IgnoreLayerCollision(gameObject.layer, 6, false);
        Physics2D.IgnoreLayerCollision(gameObject.layer, 10, false);
        _isDashing = false;
        animator.SetBool("isDashing", false);
        yield break;
    }

    private void ApplyVelocity(Vector3 desiredVelocity, float multiplier) // ������������� ��� ������ �������� � ��������� �����
    {
        var velocity = rb.velocity;

        velocity.y = desiredVelocity.y == 0 ? velocity.y : desiredVelocity.y * multiplier;// ����� �� ������ ������, �������� �� Y ����� ���������� ������ ���� ��� �����
        velocity.x = desiredVelocity.x * multiplier;
        

        rb.velocity = velocity;
    }

    private void UpdateHpBar()
    {
        HPBar.value = (hp - cur_hp) / hp;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Spikes")
        {
            DieFromSpikes();
        }
    }

    private void DieFromSpikes()
    {
        transform.position = save_pos;
        TakeDamage(10);
    }

    public Transform GetLegsTransform() { return legs; }
}

