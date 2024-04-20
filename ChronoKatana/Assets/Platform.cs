using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Platform : MonoBehaviour
{
    [SerializeField] private bool _vertical;
    [SerializeField] private float shift;
    [SerializeField] private float speed;

    private Vector2 start_pos;
    private Vector2 fin_pos;
    private Vector2 tar_pos;
    private float _half_width;

    // Start is called before the first frame update
    void Start()
    {
        _half_width = GetComponent<BoxCollider2D>().size.x / 2;
        start_pos = transform.position;
        tar_pos = fin_pos = start_pos + (_vertical ? new Vector2(0, shift) : new Vector2(shift, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, tar_pos) < 0.05f)
        {
            transform.position = tar_pos;
            shift *= -1;
            tar_pos = tar_pos + (_vertical ? new Vector2(0, shift) : new Vector2(shift, 0));
        }
        else
        {
            Vector2 dir = tar_pos - (Vector2)transform.position;
            transform.Translate(dir.normalized * Time.deltaTime * speed, Space.World);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag != "Player")
            return;

        if (collision.transform.position.y < transform.position.y || Mathf.Abs(transform.position.x - collision.transform.position.x) > _half_width)
            return;

        collision.transform.parent = transform;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag != "Player")
            return;

        transform.DetachChildren();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + (_vertical ? new Vector2(0, shift) : new Vector2(shift, 0)));
    }
}
