using UnityEngine;
using BoxScripts;

public class Movement : MonoBehaviour {
    private TransformData from;
    private TransformData target;

    private bool hasToRotate = false;

    private bool rotateOnInvert = false;

    private float Speed = 2f;
    [HideInInspector] public bool hasParameters = false;
    private float timer = 0f;
    [HideInInspector] public bool HasToDestroy = false;
    [SerializeField] public bool isAtDestination = true;

    private bool isInverted = false;

    void Update()
    {
        if(!isAtDestination && hasParameters)
        { 
            if(timer < 1f)
                timer += Time.deltaTime * Speed;
            else {
                Debug.Log("Is here");
                isAtDestination = true;
                
                hasParameters = false;
                if(HasToDestroy) Destroy(this);
            }
            if(hasToRotate) {
                if((isInverted && rotateOnInvert) || !rotateOnInvert) transform.eulerAngles = from.LerpAngle(target, timer);
            }
            transform.position = from.LerpDistance(target, timer);
        }
    }

    public void SetConfig(float speed = 2f, bool rotate = false, bool rotateOnInvert = false)
    {
        Speed = speed;
        hasToRotate = rotate;
        this.rotateOnInvert = rotateOnInvert;
    }

    public void SetParameters(Transform target, Transform _from = null)
    {
        this.target = new TransformData(target);
        this.from = _from ? new TransformData(_from) : new TransformData(transform);
        isInverted = false;
        isAtDestination = false;
        hasParameters = true;
        timer = 0f;
    }

    public void SetParameters(TransformData target, TransformData _from = null)
    {
        this.target = target;
        this.from = _from != null ? _from : new TransformData(transform);
        isInverted = false;
        isAtDestination = false;
        hasParameters = true;
        timer = 0f;
    }

    public void Invert()
    {
        timer = 0;
        if(rotateOnInvert) target.SetEuler(transform.eulerAngles);
        TransformData tempTransform = from;
        from = new TransformData(transform);
        target = tempTransform;
        isInverted = true;
        isAtDestination = false;
        hasToRotate = true;
        hasParameters = true;
        Debug.Log("[Movement] Is inverting");
    }

    void OnDrawGizmos(){
        #if UNITY_EDITOR
            Gizmos.color = Color.green;
            Gizmos.DrawLine(
                transform.position,
                transform.position + transform.forward.normalized
            );
            Gizmos.color = Color.white;
        #endif
    }
}