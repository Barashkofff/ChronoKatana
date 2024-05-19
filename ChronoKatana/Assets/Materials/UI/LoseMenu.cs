using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class LoseMenu : MonoBehaviour
{

    public PauseMenu pauseMenu;
    public GameObject LosePanel;

    public void Lose()
    {
        Repository.DeleteData<EnemyData[]>();
        pauseMenu.enabled = false;
        LosePanel.SetActive(true);
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //int index = PlayerController.instance.CurTable;
        //PlayerController.instance.transform.position = IntroObjects.instance.tables[index].position - new Vector3(2, 0, 0);
        AudioListener.pause = false;
    }


    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        AudioListener.pause = false;
    }


}
