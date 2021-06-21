using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject UIBack;
    public GameObject UIMenu;
    public GameObject UISettings;

    [Header("Pause Parameters")]
    public AudioMixer audioMixer;
    public GameControllerObject gObject;
    public CameraSettings cSettings;

    [Header("UI Parameters")]
    public Slider AudioSlider;
    public Slider mouseX;
    public Slider mouseY;

    private void UpdateValues()
    {
        audioMixer.GetFloat("GlobalVolume", out float audioValue);
        AudioSlider.value = audioValue;
        mouseX.value = cSettings.YawSpeed;
        mouseY.value = cSettings.PitchSpeed;
    }

    public void EnableMenu(bool active = true)
    {
        if(active) gObject.ChangeState(BoxScripts.GameState.SETTINGS);
        UIBack.SetActive(active);
        UIMenu.SetActive(active);

        UISettings.SetActive(!active);
    }

    public void ReturnToGame()
    {
        EnableMenu(false);
        UISettings.SetActive(false);
        gObject.ChangeState(BoxScripts.GameState.PLAYING);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("01_MainMenu");
    }
    
    public void ActiveSettings(bool active = true)
    {
        UpdateValues();
        UIMenu.SetActive(!active);
        UISettings.SetActive(active);
    }
    public void setVolume (float volumeChosed)
    {
        audioMixer.SetFloat("GlobalVolume", volumeChosed);
    }

    public void SetMouseX (float mouseX)
    {
        cSettings.YawSpeed = mouseX; 
    }
    public void SetMouseY (float mouseY)
    {
        cSettings.PitchSpeed = mouseY; 
    }

}
