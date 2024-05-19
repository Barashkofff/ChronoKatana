using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samurai1 : MonoBehaviour
{
    [SerializeField] private Transform[] patrolMarks;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCD;
    [SerializeField] private float targetDist_x; [SerializeField] private float targetDist_y;
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private bool fixedOnWP;

    private int mark_i;
    private float cur_CD = -1;
    private bool is_targeted;
    private bool isAttacking;
    public bool _stunned = false;

    private EnemyHP hp_script;
    private float HorizontalMove = 0f;
    [SerializeField] private bool FacingRight = true;
    private Rigidbody2D rb;
    private Transform player;

    void Start()
    {
        hp_script = GetComponent<EnemyHP>();
        rb = GetComponent<Rigidbody2D>();
        player = PlayerController.instance.transform;
    }

    private void OnEnable()
    {
        cur_CD = attackCD;
    }

    void Update()
    {
        if (hp_script._Stunned)
            if (isAttacking) { Stun(); }
        //if (hp_script._Stunned)
        //{
        //    if (isAttacking) { Stun(); }
        //    animator.SetFloat("HorizontalMove", 0);
        //    return;
        //}
        if (cur_CD >= 0)
            cur_CD -= Time.deltaTime;
        CheckTarget();
        if (!is_targeted)
        {
            Patrol();
            return;
        }
        Debug.Log(2);
        if (Mathf.Abs(attackPoint.position.x - player.position.x) > attackRange)
            MoveToPlayer(player.position);
        else
        {
            if (cur_CD < 0)
            {
                Attack();
                cur_CD = attackCD;
            }
        }
    }

    private void MoveToWP(Vector2 tar_pos)
    {
        if (isAttacking)
            return;
        Vector2 targetVec = tar_pos - (Vector2)transform.position;
        HorizontalMove = targetVec.x;
        animator.SetFloat("HorizontalMove", Mathf.Abs(HorizontalMove));
        if (HorizontalMove < 0 && FacingRight || HorizontalMove > 0 && !FacingRight)
            Flip();
        rb.velocity = new Vector2(Mathf.Sign(HorizontalMove) * speed, rb.velocity.y);
    }

    private void MoveToPlayer(Vector2 tar_pos)
    {
        Debug.Log(3);
        
        if (fixedOnWP)
        {
            float x = FindDistToClosestMark();
            if (x < 0.5f)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                animator.SetFloat("HorizontalMove", 0);
                return;
            }
        }
        if (isAttacking)
            return;
        Vector2 targetVec = tar_pos - (Vector2)transform.position;
        HorizontalMove = targetVec.x;
        Debug.Log(HorizontalMove);
        animator.SetFloat("HorizontalMove", Mathf.Abs(HorizontalMove));
        if (HorizontalMove < 0 && FacingRight || HorizontalMove > 0 && !FacingRight)
            Flip();
        rb.velocity = new Vector2(Mathf.Sign(HorizontalMove) * speed, rb.velocity.y);
    }

    private float FindDistToClosestMark()
    {
        float res = Mathf.Abs(transform.position.x - patrolMarks[mark_i].position.x);
        foreach (var mark in patrolMarks)
        {
            if (((mark.position.x - transform.position.x) * (mark.position.x - player.position.x)) < 0)
                return mark.position.x;
        }

        return res;
    }


    private void Flip()
    {
        if (is_targeted && Mathf.Abs(player.position.x - transform.position.x) < 0.5)
            return;
        FacingRight = !FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void Patrol()
    {
        if (patrolMarks.Length == 0)
            return;

        if (Mathf.Abs(patrolMarks[mark_i].position.x - transform.position.x) < 0.1f)
        {
            mark_i++;
            if (mark_i == patrolMarks.Length)
                mark_i = 0;
        }

        MoveToWP(patrolMarks[mark_i].position);
    }

    private void CheckTarget()
    {
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

    private void Attack()
    {
        HorizontalMove = 0;
        animator.SetFloat("HorizontalMove", Mathf.Abs(HorizontalMove));
        isAttacking = true;
        rb.velocity = new Vector2(0, rb.velocity.y);
        animator.Play("Attack");

        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, layerMask);
        Debug.Log("att");
        foreach (Collider2D player in hitPlayer)
            player.GetComponent<PlayerController>().TakeDamage(damage);
    }

    public void Attack2()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, layerMask);
        Debug.Log("att");
        foreach (Collider2D player in hitPlayer)
            player.GetComponent<PlayerController>().TakeDamage(damage);
        isAttacking = false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y + targetDist_y, 0), new Vector3(transform.position.x + (FacingRight ? targetDist_x : -targetDist_x), transform.position.y + targetDist_y, 0));
        Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y - targetDist_y, 0), new Vector3(transform.position.x + (FacingRight ? targetDist_x : -targetDist_x), transform.position.y - targetDist_y, 0));
    }
#endif

    
    public void Stun()
    {
        isAttacking = false;
        animator.Play("Idle");
    }
}
