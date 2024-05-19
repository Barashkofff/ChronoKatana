using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{
    public float STUN_CD = 0.7f;

    [SerializeField] private float hp;

    public bool isBoss = false;

    private float stun_time;
    private Rigidbody2D rb;
    private float cur_hp;
    private bool _stunned = false;
    public bool _Stunned {  get { return _stunned; } }

    public float Hp { get { return hp; } }
    public float Cur_Hp { get { return cur_hp; } }

    [SerializeField] public Slider HPBar;

    public void Awake()
    {
        EnemyController.AddEnemy(gameObject);
    }

    public void OnEnable()
    {
        stun_time = STUN_CD;
        cur_hp = hp;
        UpdateHpBar();
    }

    private void UpdateHpBar()
    {
        HPBar.value = (hp - cur_hp) / hp;
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
        if (isBoss)
            UpdateHpBar();
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
