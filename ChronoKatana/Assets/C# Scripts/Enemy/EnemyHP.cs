using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    const float STUN_CD = 0.7f;

    [SerializeField] private float hp;

    private float stun_time = STUN_CD;
    private Rigidbody2D rb;
    private float cur_hp;
    private bool _stunned = false;
    public bool _Stunned {  get { return _stunned; } }

    public void Awake()
    {
        EnemyController.AddEnemy(gameObject);
    }

    public void OnEnable()
    {
        
        cur_hp = hp;
    }

    public void Update()
    {
        if (_stunned)
        {
            stun_time -= Time.deltaTime;
            if (stun_time < 0)
                _stunned = false;
        }

    }

    public void TakeDamage(float damage) {
        cur_hp -= damage;
        Debug.Log(this.name + " hp: " + cur_hp);
        _stunned = true;
        stun_time = STUN_CD;
        if (cur_hp <= 0)
            Die();
    }

    private void Die() {
        gameObject.SetActive(false);
        Debug.Log(this.name + " killed");
        //SaveLoadManager.SaveGame();
    }
}
