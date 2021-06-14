using UnityEngine;

public class ThoughtsInteract : InteractBase {

    public override void Execute(bool isLeftAction = false)
    {
        
        GameController.current.textManager.SpawnThought(dialogueID);
        // Destroy(this);
    }    


    private void Update() {
        if(GameController.current.database.GetProgressionState(reqID))
            Destroy(this);
    }
}