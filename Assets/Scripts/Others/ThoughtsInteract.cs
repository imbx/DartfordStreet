using UnityEngine;

public class ThoughtsInteract : InteractBase {
    public int dialogueId = 5;

    public override void Execute(bool isLeftAction = false)
    {
        GameController.current.textManager.SpawnThought(dialogueId);
        Destroy(this);
    }    
}