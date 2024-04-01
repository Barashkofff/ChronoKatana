using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBotn : MonoBehaviour
{
    public void StartCoroutineButton()
    {
        GameObject.Find("InputManager").GetComponent<InputManager>().StartCoroutine(“SetButton", gameObject);
    }    
}
