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
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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
        Transform plat = spriteRenderer.transform;
        Vector3 pos = plat.position;
        int count = 9;
        while (timer < time)
        {
            if (count == 0)
            {
                float x = Random.Range(-1, 1); float y = Random.Range(-1, 1);
                float a = .0625f;
                plat.position = new Vector3(pos.x + x * a, pos.y + y * a, pos.z);
                count = 9;
            }

            timer += Time.deltaTime; count--;
            yield return new WaitForEndOfFrame();
        }
        plat.position = pos;

        spriteRenderer.color = new Color(1, 1, 1, 0);
        GetComponent<BoxCollider2D>().enabled = false;

        timer = 0;
        while (timer < re_time / 4)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        timer = 0;
        while (timer < re_time / 4)
        {
            spriteRenderer.color = new Color(1, 1, 1, timer / (re_time / 4) / 4);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        timer = 0;
        while (timer < re_time / 4)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.25f - (timer / (re_time / 4) / 2));
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        timer = 0;
        while (timer < re_time / 4)
        {
            spriteRenderer.color = new Color(1, 1, 1, timer / (re_time / 4));
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        GetComponent<BoxCollider2D>().enabled = true;
    }
}
