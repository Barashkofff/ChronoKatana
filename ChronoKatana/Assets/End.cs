using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    public static End instance;
    public string nextScene;
    public PauseMenu pauseMenu;
    public Animator fadePanel;

    private void Awake()
    {
        instance = this;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            fadePanel.enabled = true;
            Repository.DeleteData<EnemyData[]>();
            Repository.DeleteData<TableData>();
            Repository.DeleteData<PlayerData>();
            Repository.SaveState();
            EnemyController.Clear();
            fadePanel.Play("FadeIn");
        }
    }
}
