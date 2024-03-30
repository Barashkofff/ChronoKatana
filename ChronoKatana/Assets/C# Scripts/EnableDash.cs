using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDash : MonoBehaviour
{
    [SerializeField] GameObject text;

    void Start()
    {
        text.SetActive(false);
        //PlayerController.instance.dash_SetTrue();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController.instance.dash_SetTrue();
        text.SetActive(true);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        text.SetActive(false);
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }
}
