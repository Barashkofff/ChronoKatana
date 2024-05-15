using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HealPotion : MonoBehaviour
{
    [SerializeField] float heal;
    private Rigidbody2D rb;
    private BoxCollider2D _collider;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _collider = GetComponentsInChildren<BoxCollider2D>().First(x => !x.isTrigger);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(GoToPlayer());
        }
    }

    private IEnumerator GoToPlayer()
    {
        rb.gravityScale = 0f;
        _collider.enabled = false;
        Transform player_tf = PlayerController.instance.transform;
        while (Mathf.Abs(player_tf.position.x - transform.position.x) > 0.1)
        {
            Vector2 vec = (player_tf.position - transform.position).normalized;
            rb.AddForce(vec*5, ForceMode2D.Impulse);

            yield return new WaitForEndOfFrame();
        }
        PlayerController.instance.Heal(heal);
        gameObject.SetActive(false);
    }
}
