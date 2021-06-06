using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Knot : InteractBase {
    [Header("Knot Parameters")]
    public int KnotID = -1;
    public bool hasWire = false;
    [SerializeField] private PrimaryController controller;

    public UnityEvent<Knot> Action;

    public float RotationSpeed = 2f;

    public override void Execute(bool isLeftAction = true)  
    {
        base.Execute(isLeftAction);
    } 

    public void SetWire (Wire w)
    {
        if(hasRequirement && !GameController.current.database.GetProgressionState(reqID)) return;
        if(w.WireID != KnotID) return;
        w.SetPoint(transform.position);

        hasWire = true;
        GetComponent<BoxCollider>().enabled = false;
        StartCoroutine(RotateKnot());
    }

    IEnumerator RotateKnot()
    {
        float timer = 0f;
        float startRotationZ = transform.localEulerAngles.z;
        while(timer < 1f)
        {
            transform.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(startRotationZ, startRotationZ + 180f, timer));
            timer += Time.deltaTime * RotationSpeed;
            yield return null;
        }
        Action.Invoke(this);
        yield return null;
    }
    
}