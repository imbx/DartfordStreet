using UnityEngine;
using UnityEngine.Events;

public class HideTilReq : InteractBase {

    [Header("Hide parameters")]
    public UnityEvent Action;
    private bool isEnabled = false;

    public override void Execute (bool isLeftAction = true)
    {
        if(isEnabled)
        {
            Action.Invoke();
            Destroy(gameObject);
        }
    }

    private void Update() 
    {
        if(GameController.current.database.GetProgressionState(reqID) && !isEnabled)
        {
            isEnabled = true;
        }
    }
    
}