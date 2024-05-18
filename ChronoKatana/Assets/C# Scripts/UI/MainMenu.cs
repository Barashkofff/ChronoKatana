using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public SettingsMenu settingsMenu;

    private void Start()
    {
        settingsMenu.StartSetSettings();
    }

    public void PlayGame()
    {
        Repository.LoadState();
        LevelSaveLoader lsl = new LevelSaveLoader();
        (lsl as ISaveLoader).LoadData();
    }
    public void Learning()
    {
        SceneManager.LoadScene("Learning", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Debug.Log("конец");
        Application.Quit();
    }
}
