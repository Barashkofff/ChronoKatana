using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCD;
    [SerializeField] private float hp;
    [SerializeField] private float damage;
    [SerializeField] private float speed;

    private float cur_CD;
    private float cur_hp;

    private float HorizontalMove = 0f;
    private bool FacingRight = true;
    private Rigidbody2D rb;
    private Transform player;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        player = PlayerController.instance.transform;
    }

    private void OnEnable() {
        cur_hp = hp;
        cur_CD = attackCD;
    }

    void Update() {
        if (Vector2.Distance((Vector2)player.position, (Vector2)transform.position) < attackRange) {
            Attack();
        }

        HorizontalMove = rb.velocity.x;
        if (HorizontalMove < 0 && FacingRight || HorizontalMove > 0 && !FacingRight)
            Flip();

        cur_CD -= Time.deltaTime;
        if (cur_CD < 0) {
            Attack();
            cur_CD = attackCD;
        }
        //rb.velocity = new Vector2(HorizontalMove * 10, rb.velocity.y);
    }

    private void Flip() {
        FacingRight = !FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void Attack() {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, layerMask);

        foreach (Collider2D player in hitPlayer)
        {
            player.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }

    public void TakeDamage(float damage) {
        cur_hp -= damage;
        Debug.Log(this.name + " hp: " + cur_hp);
        if (cur_hp <= 0)
            Die();
    }

    private void Die() {
        gameObject.SetActive(false);
        Debug.Log(this.name + " killed");
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
