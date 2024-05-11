using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FallingSpike : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float trigger_dist;
    [SerializeField] private float prepare_time;
    private Transform player_pos;
    private Rigidbody2D rb;
    private Vector2 start_pos;

    private bool _triggered = false;
    private bool _falling = false;

    void Start()
    {
        start_pos = transform.parent.position;
        player_pos = PlayerController.instance.transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (_triggered)
            return;

        if (Mathf.Abs(transform.position.x - player_pos.position.x) < trigger_dist)
            StartCoroutine(Fall());
    }

    IEnumerator Fall()
    {
        _triggered = true;
        float timer = prepare_time;
        while (timer >= 0)
        {
            timer -= 0.15f;
            transform.parent.position = new Vector2(start_pos.x + Random.Range(-0.03f, 0.03f), start_pos.y + Random.Range(0, 0.02f));

            yield return new WaitForSeconds(0.15f);
        }

        transform.parent.position = start_pos;
        rb.simulated = true;
        _falling = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_falling)
            return;

        switch (collision.transform.tag)
        {
            case "Player":
                collision.transform.GetComponent<PlayerController>().TakeDamage(damage);
                break;
        }

        gameObject.SetActive(false);

    }
    //    collision.xuipisipopa
    //        true
    //        switch
    //}DebugDictionary
    //        geei

}