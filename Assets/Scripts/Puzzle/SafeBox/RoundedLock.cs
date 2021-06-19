using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundedLock : InteractBase
{
    private bool isHold = false;
    private float CurrentRotation;
    private float RotationLerp = 0;
    [Header("Rounded Lock Parameters")]
    [SerializeField] private PrimaryController controller;
    public Vector2 Interval = new Vector2(0, 99.99f);
    [SerializeField] private SafeBox parent;
    public float rotationMultiplier = 0.20f;

    public override void Execute(bool isLeftAction = true)
    {
        base.Execute(isLeftAction);
        isInteractingThis = true;
    }

    void Update()
    {
        if(!isInteractingThis) return;
        else if(!(controller.isInput2Pressed || controller.isInputPressed))
        {
            isInteractingThis = false;
            isHold = false;
            return;
        }
        isHold = controller.isInputHold || controller.isInput2Hold;


        int mouseDir = BoxScripts.BoxUtils.ConvertTo01((int)controller.CameraAxis.x);
        CurrentRotation = (int) Mathf.Lerp(Interval.x, Interval.y, RotationLerp);
        RotationLerp += mouseDir * rotationMultiplier * Time.deltaTime;

        if(RotationLerp > 1f ) RotationLerp = 0f;
        if(RotationLerp < 0f) RotationLerp = 1f;

        transform.localEulerAngles = new Vector3(0, (int)Mathf.Lerp(0, 360f, RotationLerp), 0);
        
        if(controller.isInputHold) parent.FirstNumber = (int) CurrentRotation;
        else if(controller.isInput2Hold) parent.SecondNumber = (int) CurrentRotation;

        Debug.Log("[RoundedLock] Current rot : " + CurrentRotation + " First Number is " + parent.FirstNumber + " and second is " + parent.SecondNumber);
    
    }

    public void DestroyComponent()
    {
        Destroy(GetComponent<BoxCollider>());
        Destroy(this);
    }
}
