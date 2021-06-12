using UnityEngine;

public class ThoughtsSeen : MonoBehaviour {
    public int DialogueId = 0;
    public void SpawnThought()
    {
        GameController.current.textManager.SpawnThought(DialogueId);
        Destroy(this);
    }
}