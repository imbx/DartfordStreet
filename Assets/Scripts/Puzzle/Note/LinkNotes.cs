using UnityEngine;
using System.Collections.Generic;

public class LinkNotes : PuzzleBase {
    public List<Note> notes;
    public override void Execute(bool isLeftAction = true)
    {
        base.Execute();
        GetComponent<BoxCollider>().enabled = false;
    }

    public void RemoveNote(Note nt)
    {
        notes.Remove(nt);
        Destroy(nt);
    }

    protected override void OnEnd(bool destroyGameObject = false)
    {
        GameController.current.textManager.SpawnThought(5);
        GameController.current.database.EditProgression(_id, true);
        GameController.current.ui.ForceDiaryPage(_id);
        base.OnEnd(true);
    }

    private void Update() {
        if(!isInteractingThis) return;
        if(notes.Count <= 0) this.OnEnd();
        if(controller.isEscapePressed)
        {
            Debug.Log("[LinkNotes] Escape pressed");
            this.OnExit();
        }
    }
}