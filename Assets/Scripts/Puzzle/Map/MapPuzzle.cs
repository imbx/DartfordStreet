using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using BoxScripts;

public class MapPuzzle : PuzzleBase
{
    public List<GameObject> PinList;
    public List<GameObject> MarkerList;

    private void OnEnable() {
        PinList = new List<GameObject>();
    }

    public override void Execute(bool isLeftAction = true)
    {
        base.Execute();
        GetComponent<BoxCollider>().enabled = false;
    }

    private void Update() {
        if(isInteractingThis && controller.isEscapePressed)
        {
            Debug.Log("[PuzzleBase] Called Escape");
            this.OnExit();
        }
    }
}