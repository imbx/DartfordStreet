using UnityEngine;

public class ThoughtsCheckReq : MonoBehaviour {
    public int Id = -1;
    public int DialogueId = -1;

    public void Show()
    {
        if(!GameController.current.database.GetProgressionState(Id))
            GameController.current.textManager.SpawnThought(DialogueId);
    }
}