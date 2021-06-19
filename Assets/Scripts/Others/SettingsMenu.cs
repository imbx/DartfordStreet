using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void setVolume (float volumeChosed)
    {
        audioMixer.SetFloat("GlobalVolume", volumeChosed);
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

}
