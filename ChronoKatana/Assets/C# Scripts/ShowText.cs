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


    public void OnMouseOver()
    {
        text.SetActive(true);
    }

    public void OnMouseExit()
    {
        text.SetActive(false);
    }
}
