using UnityEngine;

public class ThoughtOnSee : MonoBehaviour {
    public List<int> DialogueIds;
    public void SpawnThought()
    {

        if(DialogueIds)
            if(DialogueIds.Count > 0)
            {
                foreach(int i in DialogueIds)
                    GameController.current.textManager.SpawnThought(i);
            }
        
        Destroy(this);
    }
}