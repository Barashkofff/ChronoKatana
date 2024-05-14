using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private float re_time;

    private SpriteRenderer spriteRenderer;
    private float _half_width;

    // Start is called before the first frame update
    void Start()
    {
        _half_width = GetComponent<BoxCollider2D>().size.x / 2;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag != "Player")
            return;

        if (collision.transform.position.y < transform.position.y || Mathf.Abs(transform.position.x - collision.transform.position.x) > _half_width)
            return;

        StartCoroutine(Breaking());
    }

    private IEnumerator Breaking()
    {
        float timer = 0;
        while (timer < time)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        spriteRenderer.enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;

        timer = 0;
        while (timer < re_time)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        spriteRenderer.enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
