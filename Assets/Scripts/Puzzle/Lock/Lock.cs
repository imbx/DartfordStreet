using System.Collections.Generic;
using UnityEngine;

public class Lock : PuzzleBase {

    public List<LockWheel> keys;

    public string FinalCombination = "123";

    [FMODUnity.EventRef]
    public string candadoSound = "event:/candado2d";

    private float innerTimer = 0f;

    public override void Execute(bool isLeftAction = true)
    {
        if(hasRequirement && !GameController.current.database.GetProgressionState(reqID)) return;
        else 
        {
            base.Execute();
            GetComponent<BoxCollider>().enabled = false;
        }
        
    }

    void Update()
    {
        if(hasRequirement)
        {
            if(GameController.current.database.GetProgressionState(reqID))
            {
                Debug.Log("[Lock] Unlocked");
                hasRequirement = false;
                tag = "BasicInteraction";
            }
        }
        if (isInteractingThis && GetCurrentCombination().Equals(FinalCombination))
        {
            if(innerTimer < 1f)
                for(int i = 0; i < keys.Count; i++)
                {
                    int targetNumber = int.Parse(FinalCombination[i].ToString());
                    keys[i].SetRightRotation(targetNumber, innerTimer);
                }
            innerTimer += Time.deltaTime;
            if(innerTimer >= 2f)
            {
                OnEnd(true);
                GameController.current.music.playMusic(candadoSound);
            }
        }

        if(isInteractingThis && controller.isEscapePressed)
        {
            Debug.Log("[Safebox] Escape pressed");
            this.OnExit();
        }
        
    }
    protected override void OnEnd(bool destroyGameObject = false)
    {
        foreach(LockWheel lw in keys)
        {
            lw.DestroyInteraction();
        }
        base.OnEnd(destroyGameObject);
    }

    private string GetCurrentCombination()
    {
        string combination = "";

        foreach(LockWheel lcc in keys)
        {
            combination += lcc.Number.ToString();
            
        }
        Debug.Log("[Lock] Current combination is " + combination);
        return combination;
    }
    
}