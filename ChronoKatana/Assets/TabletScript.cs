using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TabletScript : MonoBehaviour
{
    [SerializeField] GameObject button;
    [SerializeField] Sprite active;
    [SerializeField] Sprite deactive;
    private Animator button_animator;
    private bool _isTrigger = false;
    private int ind;

    private void Start()
    {
        button_animator = button.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (PlayerController.instance.CurTable == ind)
                return;
            _isTrigger = true;
            button_animator.SetBool("Enable", true);
        }
    }

    private void Update()
    {
        if (!_isTrigger)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            button_animator.SetBool("Enable", false);
            
            PlayerController.instance.CurTable = ind;
            IntroObjects.instance.UpdateTables();
            SaveLoadManager.SaveGame();
            Debug.Log("Game Saved");

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isTrigger = false;
            button_animator.SetBool("Enable", false);
        }
    }

    public void UpdateSprite()
    {
        ind = Array.IndexOf(IntroObjects.instance.tables, transform);
        if (PlayerController.instance.CurTable == ind)
            GetComponent<SpriteRenderer>().sprite = active;
        else
            GetComponent<SpriteRenderer>().sprite = deactive;
    }
}
