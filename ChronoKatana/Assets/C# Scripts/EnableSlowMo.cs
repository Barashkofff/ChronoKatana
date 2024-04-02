using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableSlowMo : MonoBehaviour
{
    [SerializeField] GameObject text;
    void Start()
    {
        text.SetActive(false);
        //SlowMo.instance.slowMo_SetTrue();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag!="Player")
            return;
        SlowMo.instance.slowMo_SetTrue();
        text.SetActive(true);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag != "Player")
            return;
        text.SetActive(false);
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }
}
