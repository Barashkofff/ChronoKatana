using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowText : MonoBehaviour
{
    [SerializeField] GameObject text;


    void Start()
    {
        text.SetActive(false);
        
    }


    //public void OnMouseOver()
    //{
    //    text.SetActive(true);
    //    GetComponent<SpriteRenderer>().enabled = false;
    //}

    //public void OnMouseExit()
    //{
    //    text.SetActive(false);
    //    GetComponent<SpriteRenderer>().enabled = true;
    //}

    //private void OnCollisionStay(Collision collision)
    //{

    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        text.SetActive(true);
    //        GetComponent<SpriteRenderer>().enabled = false;
    //    }
    //    else
    //    {
    //        text.SetActive(false);
    //        GetComponent<SpriteRenderer>().enabled = true;
    //    }

    //}

    private void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log("asda");
            text.SetActive(true);
            gameObject.GetComponent<SpriteRenderer>().enabled=false;
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
     
          
            text.SetActive(false);
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
   
           
    }
}
