using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [HideInInspector] public float damage;

    private bool isDeflected = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if (other.isTrigger)
            return;
        switch (other.tag)
        {
            case "Player":
                other.GetComponent<PlayerController>().TakeDamage(damage);
                Destroy(gameObject);
                break;
            case "Enemy":
                if (!isDeflected)
                    return;
                other.GetComponent<EnemyHP>().TakeDamage(damage * 2);
                Destroy(gameObject);
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }

    public void Deflected()
    {
        isDeflected = true;
        GetComponent<Rigidbody2D>().velocity *= -1;
    }
}
