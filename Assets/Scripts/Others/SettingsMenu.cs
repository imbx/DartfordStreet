using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public GameControllerObject gObject;
    public CameraSettings cSettings;

    [Header("Options Parameters")]

    public GameObject MainMenu;

    public Slider AudioSlider;
    public Slider mouseX;
    public Slider mouseY;

    private void OnEnable()
    {
        audioMixer.GetFloat("GlobalVolume", out float audioValue);
        AudioSlider.value = audioValue;
        mouseX.value = cSettings.YawSpeed;
        mouseY.value = cSettings.PitchSpeed;
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

    public void SetFullScreen (bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetCamera (bool invertCam)
    {
        if (invertCam)
        {
            //CameraSettings.
            //si el boton esta activo, se tiene que invertir la camara
        }
    }

    public void Return()
    {
        MainMenu.SetActive(true);
        gameObject.SetActive(false);
    }

}
