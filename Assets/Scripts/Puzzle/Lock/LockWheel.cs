using UnityEngine;

public class LockWheel : InteractBase {
    private bool isHold = false;
    private float CurrentRotation;
    private float RotationLerp = 0;
    [Header("Rounded Lock Parameters")]
    [SerializeField] private PrimaryController controller;
    public Vector2 Interval = new Vector2(0, 9f);

    public int Number = 0;

    [FMODUnity.EventRef]
    public string eventoSound = "event:/weels2d";
    private bool canInteractThis = true;
    private Vector3 savedRotation;

    public float angleOffset = 0;

    public override void Execute(bool isLeftAction = true)
    {
        base.Execute(isLeftAction);
        isInteractingThis = true;
        GameController.current.music.playMusic(eventoSound);

    }
    private void Update() {
        if(!isInteractingThis) return;
        if(!canInteractThis) return;
        else if(!(controller.isInput2Pressed || controller.isInputPressed))
        {
            // transform.localEulerAngles = new Vector3(CurrentRotation * 360f / 9, 0, 0);
            isInteractingThis = false;
            isHold = false;
            return;
        }
        isHold = controller.isInputHold || controller.isInput2Hold;


        int mouseDir = -BoxScripts.BoxUtils.ConvertTo01((int)controller.CameraAxis.y);
        CurrentRotation = (int) Mathf.Lerp(Interval.y, Interval.x, RotationLerp);
        RotationLerp += mouseDir * 0.35f * Time.deltaTime;

        if(RotationLerp > 1f ) RotationLerp = 0f;
        if(RotationLerp < 0f) RotationLerp = 1f;

        Number = (int) CurrentRotation;
        int calculatedAngle = (int)Mathf.Lerp(0, 360f, RotationLerp);
        if(calculatedAngle < 0) calculatedAngle = 360 - calculatedAngle;

        transform.localEulerAngles = new Vector3(calculatedAngle + angleOffset, 0, 0);
        savedRotation = new Vector3(calculatedAngle + angleOffset, 0, 0);
        // Debug.Log("[LookWheel] Current rot : " + CurrentRotation);
    }

    public void SetRightRotation(int nmb, float timer)
    {
        canInteractThis = false;
        float targetAngle = ((360f / Interval.y) * nmb);
        
        if(targetAngle > 180f) targetAngle -= 360f;
        if(savedRotation.x > 180f) savedRotation.x -= 360f;

        Debug.Log("[LockWheel] LockWheel" + nmb + " has this savedRotation: " + savedRotation.x + " and needs to go to " + -targetAngle);
        transform.localEulerAngles = new Vector3(Mathf.Lerp(savedRotation.x, -targetAngle, timer), 0, 0);
    }

}