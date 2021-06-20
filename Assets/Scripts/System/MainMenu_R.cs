using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu_R : MonoBehaviour
{
    public GameObject AllUI;
    public GameObject MenuUI;
    public GameObject OptionsUI;
    [Header("Start Game Parameters")]
    public Animator AnimatorPortada;
    public Animator CameraAnimator;


    [Header("Load Game Parameters")]
    public EntityData playerData;
    public Image FadeImage;


    public void LoadScene(string nivel)
    {
        SceneController.LoadGame = false;
        StartCoroutine(NewGame());
        // SceneManager.LoadScene(nivel);
    
    }

    public void LoadSceneAndGame(string nivel)
    {
        AllUI.SetActive(false);
        playerData.isLoadingData = true;
        SceneController.LoadGame = true;
        StartCoroutine(FadeInLoad(nivel)); 
    }

    public void OpenSettings()
    {
        OptionsUI.SetActive(true);
        MenuUI.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }


    IEnumerator FadeInLoad(string nivel)
    {

        float timer = 0;

         while(timer <= 1f)
        {
            timer += Time.deltaTime * 2f;
            FadeImage.color = new Color(0, 0, 0, Mathf.Lerp(0f, 1f, timer));

            yield return null;
        }
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadScene(nivel);
        yield return null;
    }

    IEnumerator NewGame()
    {

        AllUI.SetActive(false);
        AnimatorPortada.SetTrigger("PasarPortada");
        yield return new WaitForSeconds(0.25f);
        CameraAnimator.SetTrigger("AnimCamera");
        yield return null;
    }

}
