using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    public PauseMenu pauseMenu;
    public GameObject EndPanel;

    private void OnTriggerEnter2D(Collider2D other)
    {
        EndPanel.SetActive(true);
        pauseMenu.enabled = false;
        EndPan();
    }

    public void EndPan()
    {
        pauseMenu.enabled = false;
        EndPanel.SetActive(true);
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        AudioListener.pause = false;
    }


    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        AudioListener.pause = false;
    }
}
