using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            int ind = Array.IndexOf(IntroObjects.instance.tables, transform);
            if (PlayerController.instance.CurTable == ind)
                return;
            PlayerController.instance.CurTable = ind;
            SaveLoadManager.SaveGame();
            Debug.Log("Game Saved");
        }
    }
}
