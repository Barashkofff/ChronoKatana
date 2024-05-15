using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private bool rotate;
    private bool opened;
    private Animator animator;

    public bool Opened { get { return opened; }  }

    public void Open()
    {
        opened = true;
        animator.SetTrigger("Open");
        
        if (itemPrefab != null)
            StartCoroutine(Drop());
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !opened)
        {
            Open();
        }
    }

    private IEnumerator Drop()
    {
        yield return new WaitForSeconds(0.5f);

        GameObject item = Instantiate(itemPrefab, transform.position + new Vector3(0, 0.25f, 0), rotate ? Quaternion.identity * Quaternion.Euler(0, 0, 90) : Quaternion.identity);
        float x_impulse = Random.Range(-0.4f, 0.4f);
        float y_impulse = Random.Range(0.5f, 0.7f);

        item.GetComponent<Rigidbody2D>().AddForce(new Vector2(x_impulse, y_impulse) * 5, ForceMode2D.Impulse);

        if (!rotate)
            yield break;

        Quaternion start_rot = item.transform.rotation;
        float timer = 0.25f;

        while (timer > 0)
        {
            item.transform.rotation = Quaternion.Lerp(start_rot, Quaternion.identity, 1 - timer * 4);
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        item.transform.rotation = Quaternion.identity;
    }
}
