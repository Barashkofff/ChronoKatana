using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3f;
    public float jumpForce = 7f;
    
    Rigidbody2D rb;
    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float movement = Input.GetAxis("Horizontal");
        transform.position += new Vector3(movement, 0, 0) * speed * Time.deltaTime;
    
    
        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(rb.velocity.y) < 0.05f)
            rb .AddForce(new Vector2 (0, jumpForce), ForceMode2D.Impulse);
    
        sr.flipX = movement < 0 ? true : false;
    }
}
