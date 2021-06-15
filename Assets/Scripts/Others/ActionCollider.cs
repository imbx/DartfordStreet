using UnityEngine;
using UnityEngine.Events;

public class ActionCollider : MonoBehaviour {

    public UnityEvent Action;
    private void OnTriggerEnter(Collider other) {
        Action.Invoke();
        Destroy(this);
    }
}