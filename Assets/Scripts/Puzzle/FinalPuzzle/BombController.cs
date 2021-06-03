using System.Collections.Generic;
using UnityEngine;

public class BombController : PuzzleBase {


    [Header("AntiWire Puzzle Parameters")]
    public bool isBombDeactivated = false;

    public bool isBombExploding = false;
    public GameObject Tapa;
    [SerializeField] private List<AntiwireKnot> tornillos;
    public List<int> WireOrder;

    public override void Execute(bool isLeftAction = true)
    {
        base.Execute();
        if(Tapa) Tapa.SetActive(false);
        GetComponent<BoxCollider>().enabled = false;
    }

    private void Update() {
        if(isInteractingThis && controller.isEscapePressed)
        {
            Debug.Log("[PuzzleBase] Called Escape");
            this.OnEnd();
        }
    }

    public void CutCable(int wireId)
    {
        if(WireOrder.Count <= 0) return;

        if(wireId == WireOrder[0]) WireOrder.Remove(wireId);
        else 
        {
            isBombExploding = true;
            return;
        }

        if(WireOrder.Count == 0) isBombDeactivated = true;
    }
    
}