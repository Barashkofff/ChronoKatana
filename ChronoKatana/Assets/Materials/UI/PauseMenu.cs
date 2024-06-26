using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;

    
    void Update()
    {
        //if (!settingsMenuUI.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
    }


    public void Resume()
    {
        Debug.Log("fdsdfsdf");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        AudioListener.pause = false;
    }


    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        AudioListener.pause = true;
    }


    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        AudioListener.pause = false;
    }


    public void LoadMenu()
    {
        Time.timeScale = 1f;
        Repository.LoadState();
        if (SceneManager.GetActiveScene().name != "Learning" && SceneManager.GetActiveScene().name != "Level3")
        {
            EnemySaveLoader esl = new EnemySaveLoader();
            (esl as ISaveLoader).SaveData();
            Repository.SaveState();
        }
        Debug.Log("ffdsfds");
        GameIsPaused = false;
        SceneManager.LoadScene("UI", LoadSceneMode.Single);
        AudioListener.pause = false;
    }

}
