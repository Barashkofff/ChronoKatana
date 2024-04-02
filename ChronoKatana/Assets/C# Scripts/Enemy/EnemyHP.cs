using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField] private float hp;

    private float cur_hp;

    public void OnEnable()
    {
        cur_hp = hp;
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
}
