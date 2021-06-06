using UnityEngine;

public class ThoughtsCollider : MonoBehaviour {
    public int dialogueId = 1;
    public bool giveNote = false;
    public int noteId = -1;

    public bool removeWithReq = false;
    public int req;
    private void OnTriggerEnter(Collider other) {
        if(removeWithReq){
            if(GameController.current.database.GetProgressionState(req))
            {
                Destroy(gameObject);
                return;
            }
        }
        GameController.current.textManager.SpawnThought(dialogueId);
        if(giveNote){
            GameController.current.database.EditProgression(noteId, true);
            GameController.current.textManager.SpawnAchievement(BoxScripts.AchievementType.Note);
            //GameController.current.ui.ForceDiaryPage(noteId);
        }
        Destroy(gameObject);
    }
}