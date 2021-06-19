using System.Collections.Generic;
using UnityEngine;

public class TriggerObjects : MonoBehaviour {
    public List<GameObject> ObjectsToDisable;
    public List<GameObject> ObjectsToEnable;
    public GameControllerObject gObject;

    public bool isFirstFloor = false;

    private void Start() {
        if(gObject.isFirstFloor == isFirstFloor)
        {
            ObjectsToDisable.ForEach(x => x.SetActive(false));
            ObjectsToEnable.ForEach(x => x.SetActive(true));
        }
    }
    
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player")
        {
            gObject.isFirstFloor = isFirstFloor;
            ObjectsToDisable.ForEach(x => x.SetActive(false));
            ObjectsToEnable.ForEach(x => x.SetActive(true));
        }
    }
}