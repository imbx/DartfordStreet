using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_R : MonoBehaviour
{
    public void LoadScene(string nivel)
    {
        SceneController.LoadGame = false;
        SceneManager.LoadScene(nivel);
    
    }

    public void LoadSceneAndGame(string nivel)
    {
        SceneController.LoadGame = true;
        SceneManager.LoadScene(nivel);
    }

    public void Exit()
    {
        Application.Quit();
    }


}
