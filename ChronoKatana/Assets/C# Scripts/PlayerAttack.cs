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

    public bool inputReceived;
    public bool canReceiveInput = true;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            Attack();
    }

    public void Attack()
    {
        if (!canReceiveInput)
            return;

        
        canReceiveInput = false;
        inputReceived = true;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
            enemy.GetComponent<Enemy>().TakeDamage(damage);
    }

    public void InputManager() {
        Debug.Log("Inp Man");
        canReceiveInput = !canReceiveInput; }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
