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

    private int phase = 0;
    private float checkHp;

    private float phase_cd;
    private float phase_speed;

    [SerializeField] private GameObject[] ranges;
    [SerializeField] private GameObject end_panel;

    void Start()
    {
        phase_cd = attackCD * 2;
        phase_speed = speed * 0.5f;
        animator.speed = 0.5f;
        hp_script = GetComponent<EnemyHP>();
        rb = GetComponent<Rigidbody2D>();
        player = PlayerController.instance.transform;
        checkHp = hp_script.Hp / 4 * (4 - phase);
    }

    private void OnEnable()
    {
        cur_CD = phase_cd;
    }

    public void OnDisable()
    {
        end_panel.SetActive(true);
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }

    void Update()
    {
        if (hp_script.Cur_Hp <= checkHp)
        {
            NextPhase();
        }
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

        if (!is_targeted)
            CheckTarget();
        
        if (!is_targeted)
        {
            Patrol();
            return;
        }
        if (Mathf.Abs(attackPoint.position.x - player.position.x) > attackRange && !isAttacking)
            MoveToPlayer(player.position);
        else
        {
            if (cur_CD < 0)
            {
                Attack();
                cur_CD = phase_cd;
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
        rb.velocity = new Vector2(Mathf.Sign(HorizontalMove) * phase_speed, rb.velocity.y);
    }

    private void MoveToPlayer(Vector2 tar_pos)
    {
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
        animator.SetFloat("HorizontalMove", Mathf.Abs(HorizontalMove));
        if (HorizontalMove < 0 && FacingRight || HorizontalMove > 0 && !FacingRight)
            Flip();
        rb.velocity = new Vector2(Mathf.Sign(HorizontalMove) * phase_speed, rb.velocity.y);
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
            GetComponent<EnemyHP>().HPBar.gameObject.SetActive(true);
            return;
        }
        else if (tarToVec.x * HorizontalMove >= 0 && Mathf.Abs(tarToVec.y) < targetDist_y && Mathf.Abs(tarToVec.x) < targetDist_x)
        {
            is_targeted = true;
            GetComponent<EnemyHP>().HPBar.gameObject.SetActive(true);
            return;
        }


    }

    private void Attack()
    {
        HorizontalMove = 0;
        animator.SetFloat("HorizontalMove", Mathf.Abs(HorizontalMove));
        isAttacking = true;
        rb.velocity = new Vector2(0, rb.velocity.y);
        animator.Play("StartAttack");
    }

    public void Attack1()
    {
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
        //isAttacking = false;
        //animator.Play("Idle");
    }

    private void NextPhase()
    {
        phase++;
        checkHp = hp_script.Hp / 4 * (4 - phase);
        switch (phase)
        {
            case 2:
                SpawnRanges(1);
                break;
            case 3:
                animator.speed = 0.7f;
                phase_speed = speed * 0.7f;
                phase_cd = attackCD * 1.5f;
                SpawnRanges(2);
                break;
            case 4:
                animator.speed = 1f;
                phase_speed = speed;
                phase_cd = attackCD;
                SpawnRanges(4);
                break;
            default:
                return;
        }
    }

    private void SpawnRanges(int i)
    {
        switch (i)
        {
            case 1:
                ranges[Random.Range(0, 4)].SetActive(true);
                break;
            case 2:
                ranges[Random.Range(0, 2)].SetActive(true);
                ranges[Random.Range(3, 4)].SetActive(true);
                break;
            case 4:
                ranges[0].SetActive(true);
                ranges[1].SetActive(true);
                ranges[3].SetActive(true);
                ranges[4].SetActive(true);
                break;
            default:
                return;
        }
    }
}
