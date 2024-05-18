using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndTrigger : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            GetComponent<Platform>().enabled = true;
            if (collision.transform.position.y < transform.position.y || Mathf.Abs(transform.position.x - collision.transform.position.x) > GetComponent<BoxCollider2D>().size.x / 2)
                return;

            collision.transform.parent = transform;
            
        }
    }
}
