using UnityEngine;
using UnityEngine.Events;

public class HideTilReq : InteractBase {

    [Header("Hide parameters")]

    public Renderer targetComponent;
    public UnityEvent Action;
    private bool isEnabled = false;

    public override void Execute (bool isLeftAction = true)
    {
        if(isEnabled)
        {
            Action.Invoke();
        }
    }

    private void Update() 
    {
        if(GameController.current.database.GetProgressionState(reqID) && !isEnabled)
        {
            isEnabled = true;
            targetComponent.enabled = true;
        }
    }
    
}