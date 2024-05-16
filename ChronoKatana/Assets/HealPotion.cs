using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class HealPotion : MonoBehaviour
{
    [SerializeField] GameObject button;
    [SerializeField] float heal;
    private Rigidbody2D rb;
    private BoxCollider2D _collider;
    private Animator button_animator;
    private bool _isTrigger = false;

    private void Start()
    {
        button_animator = button.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        _collider = GetComponentsInChildren<BoxCollider2D>().First(x => !x.isTrigger);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isTrigger = true;
            button_animator.SetBool("Enable", true);
        }
    }
    private void Update()
    {
        if (!_isTrigger)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            button_animator.SetBool("Enable", false);
            StartCoroutine(GoToPlayer());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isTrigger = false;
            button_animator.SetBool("Enable", false);
        }
    }

    private IEnumerator GoToPlayer()
    {
        button_animator.SetBool("Enable", false);
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
