using UnityEngine;
using UnityEngine.Events;

public class AntiwireKnot : InteractBase {
    public int WireId = -1;
    public UnityEvent<int> Action;
    public UnityEvent Action2;


    public override void Execute(bool isLeftAction = true)
    {
        Action.Invoke(WireId);
        Action2.Invoke();
        
        
    }
    
}