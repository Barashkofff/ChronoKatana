using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame(string x)
    {
        SceneManager.LoadScene(x);
    }
    public void ExitGame()
    {
        Debug.Log("�����");
        Application.Quit();
    }
}
