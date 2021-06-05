using UnityEngine;

public class ThoughtsCollider : MonoBehaviour {
    public int dialogueId = 1;

    public bool giveNote = false;
    public int noteId = -1;
    private void OnTriggerEnter(Collider other) {
        GameController.current.textManager.SpawnThought(dialogueId);
        if(giveNote){
            GameController.current.database.EditProgression(noteId, true);
            GameController.current.ui.ForceDiaryPage(noteId);
        }
        Destroy(gameObject);
    }
}