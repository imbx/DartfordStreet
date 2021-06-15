using System.Collections;
using UnityEngine;

public class FilmProjector : InteractBase {

    public Material TargetMaterial;
    public string TextureName;
    public Texture2D[] ProjectorTextures;

    public Texture2D EmptyTexture;

    public Transform ProjectorTop;

    public float RotationSpeed = 2f;

    private bool isRotating = false;

    private int CurrentProjectorTexture = 0;
    

    [FMODUnity.EventRef]
    public string ruedaSound = "event:/sonidosRadio2d";



    public override void Execute(bool isLeftAction = true)
    {
        if(isRotating) return;
        // base.Execute();

        StartCoroutine(RotateProjector());

    }

    IEnumerator RotateProjector()
    {
        float timer = 0;
        float startAngle = ProjectorTop.localEulerAngles.z;
        float targetAngle = startAngle + (360f / ProjectorTextures.Length);
        isRotating = true;
        GameController.current.music.playMusic(ruedaSound);
        CurrentProjectorTexture++;
        if (CurrentProjectorTexture > ProjectorTextures.Length - 1) CurrentProjectorTexture = 0;
        TargetMaterial.SetTexture(TextureName, EmptyTexture);
        while(timer < 1f)
        {
            timer += RotationSpeed * Time.deltaTime;
            ProjectorTop.localEulerAngles = new Vector3(
                ProjectorTop.localEulerAngles.x,
                ProjectorTop.localEulerAngles.y,
                Mathf.Lerp(startAngle, targetAngle, timer));

            yield return null;
        }
        TargetMaterial.SetTexture(TextureName, ProjectorTextures[CurrentProjectorTexture]);
        ProjectorTop.localEulerAngles = new Vector3(
                ProjectorTop.localEulerAngles.x,
                ProjectorTop.localEulerAngles.y,
                targetAngle);

        isRotating = false;

        yield return null;
    }

    public void SetImage()
    {
        TargetMaterial.SetTexture(TextureName, ProjectorTextures[CurrentProjectorTexture]);
    }
}