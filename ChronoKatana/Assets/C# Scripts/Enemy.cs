using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp;
    private float cur_hp;

    private void OnEnable()
    {
        cur_hp = hp;
    }

    public void TakeDamage()
    {
        cur_hp -= FindObjectOfType<PlayerController>().damage;
        Debug.Log(cur_hp);
        if (cur_hp <= 0)
            Die();
    }

    private void Die()
    {
        gameObject.SetActive(false);
        Debug.Log("смерть");
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("u");
        if (collider.isTrigger && collider.tag == "Player")
            TakeDamage();
    }
}
