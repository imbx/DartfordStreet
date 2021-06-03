using UnityEngine;
using UnityEngine.Events;

public class AntiwireKnot : InteractBase {
    public int WireId = -1;
    public UnityEvent<int> Action;

    public override void Execute(bool isLeftAction = true)
    {
        Action.Invoke(WireId);
        Destroy(gameObject);
    }
    
}