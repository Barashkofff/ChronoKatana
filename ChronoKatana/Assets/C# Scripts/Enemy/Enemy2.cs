using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.GraphicsBuffer;

class Enemy2 : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float proj_speed;
    [SerializeField] private Transform[] patrolMarks;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private LayerMask IgnoreRC;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCD;
    [SerializeField] private float targetDist_x; [SerializeField] private float targetDist_y;
    [SerializeField] private float damage;
    [SerializeField] private float speed;

    private int mark_i;
    private float cur_CD = -1;
    private bool is_targeted;
    private bool isAttacking;

    private float HorizontalMove = 0f;
    private bool FacingRight = true;
    private Rigidbody2D rb;
    private Transform player;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        player = PlayerController.instance.transform;
    }

    private void OnEnable() {
        cur_CD = attackCD;
    }

    void Update() {
        if (cur_CD >= 0)
            cur_CD -= Time.deltaTime;

        if (!is_targeted) {
            Patrol();
            return;
        }
        
        if (Mathf.Abs(attackPoint.position.x - player.position.x) > attackRange)
            MoveTo(player.position);
        else {
            if (CheckRayCast() && cur_CD < 0) {
                
                if ((player.position - transform.position).x > 0 ^ FacingRight)
                    Flip();
                isAttacking = true;
                rb.velocity = new Vector2(0, rb.velocity.y);
                animator.Play("PrepAttack");
                cur_CD = attackCD;
            }
        }
    }

    private bool CheckRayCast()
    {
        var hit = Physics2D.Raycast(transform.position, (player.position - transform.position).normalized * attackRange, Vector2.Distance(player.position, transform.position), ~IgnoreRC);
        
        if (hit.collider != null)
            return hit.collider.gameObject == player.gameObject;
        else 
            return false;
    }

    private void MoveTo(Vector2 tar_pos) {
        if (isAttacking)
            return;
        Vector2 targetVec = tar_pos - (Vector2)transform.position;
        HorizontalMove = Mathf.Sign(targetVec.x);
        animator.SetFloat("HorizontalMove", Mathf.Abs(HorizontalMove));
        if (HorizontalMove < 0 && FacingRight || HorizontalMove > 0 && !FacingRight)
            Flip();
        rb.velocity = new Vector2(HorizontalMove * speed, rb.velocity.y);
    }

    private void Flip() {
        FacingRight = !FacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void Patrol()
    {
        if (patrolMarks.Length == 0)
        {
            CheckTarget();
            return;
        }
            
        if (Mathf.Abs(patrolMarks[mark_i].position.x - transform.position.x) < 0.1f) {
            mark_i++;
            if (mark_i == patrolMarks.Length)
                mark_i = 0;
        }

        CheckTarget();
        MoveTo(patrolMarks[mark_i].position);
    }

    private void CheckTarget() {
        Vector2 tarToVec = player.transform.position - transform.position;
        if (Mathf.Abs(tarToVec.y) < targetDist_y * 0.5f && Mathf.Abs(tarToVec.x) < targetDist_x * 0.5f)
        {
            is_targeted = true;
            return;
        }
        else if (tarToVec.x * HorizontalMove >= 0 && Mathf.Abs(tarToVec.y) < targetDist_y && Mathf.Abs(tarToVec.x) < targetDist_x)
        {
            is_targeted = true;
            return;
        }
        

        if (Mathf.Abs(tarToVec.y) > targetDist_y * 2 && Mathf.Abs(tarToVec.x) > targetDist_x * 2)
            is_targeted = false;
    }

    public void Attack() {
        Debug.Log(gameObject);
        Vector3 dir = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        GameObject proj = Instantiate(projectile, transform.position, Quaternion.Euler(new Vector3(0, 0, angle)));
        //if (splashRange == 0)
        //proj.GetComponent<ProjectileScript>().damage = damage;
        proj.GetComponent<Rigidbody2D>().AddForce(dir * proj_speed, ForceMode2D.Impulse);
        proj.GetComponent<Rigidbody2D>().AddTorque(5, ForceMode2D.Impulse);
        proj.GetComponent<ProjectileScript>().damage = damage;

    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void StopAttack() { isAttacking = false; }
}
