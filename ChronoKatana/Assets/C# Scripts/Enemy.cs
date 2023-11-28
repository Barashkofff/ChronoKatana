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

    public void TakeDamage(float damage)
    {
        cur_hp -= damage;
        Debug.Log(cur_hp);
        if (cur_hp <= 0)
            Die();
    }

    private void Die()
    {
        gameObject.SetActive(false);
        Debug.Log("смерть");
    }
}
