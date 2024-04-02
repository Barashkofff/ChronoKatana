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

    [HideInInspector]
    public bool inputReceived = false;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.K))
        {
            if (!animator.GetBool("IsAttackStart"))
                animator.Play("Attack_1");
            else
                inputReceived = true;
        }
    }

    public void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
            enemy.GetComponent<EnemyHP>().TakeDamage(damage);
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
