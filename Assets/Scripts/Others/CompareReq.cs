using UnityEngine;
using UnityEngine.Events;

public class CompareReq : MonoBehaviour {
    public int Req1 = -1;
    public bool req1NeedsToBeActive = true;

    public int Req2 = -1;
    public bool req2NeedsToBeActive = false;

    public UnityEvent Action;

    private void Update()
    {
        if(GameController.current.database.GetProgressionState(Req1) == req1NeedsToBeActive && GameController.current.database.GetProgressionState(Req2) == req2NeedsToBeActive)
        {
            Action.Invoke();
            Destroy(this);
        }
    }
}