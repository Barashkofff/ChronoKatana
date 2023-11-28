using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    #region Singleton

    public static PlayerAttack instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    public Animator animator;

    public float damage;
    public Transform attackPoint;
    public float attackRange;
    public LayerMask enemyLayers;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            Attack();
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
