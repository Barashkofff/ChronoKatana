using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [HideInInspector] public float damage;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject);
        switch (other.tag)
        {
            case "Player":
                other.GetComponent<PlayerController>().TakeDamage(damage);
                Destroy(gameObject);
                break;
            case "Enemy":
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }
}
