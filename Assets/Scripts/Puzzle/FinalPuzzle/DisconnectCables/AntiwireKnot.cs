using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AntiwireKnot : InteractBase
{
    public int WireId = -1;
    public UnityEvent<int> Action;
    public Vector3 targetRotation;
    public float SpeedToDeactivate = 4f;
    private Vector3 startRotation;

    public Transform RealWire;

    private bool isActivated = false;


    public override void Execute(bool isLeftAction = true)
    {
        if (!isActivated)
        {
            isActivated = true;
            Action.Invoke(WireId); 
            Destroy(RealWire.gameObject);
            Destroy(this);
            //StartCoroutine(Deactivate());
        }
    }

    void Update()
    {
        if (!isActivated)
        {
            startRotation = RealWire.localEulerAngles;
        }
    }


    IEnumerator Deactivate()
    {

        float timer = 0;

        while (timer < 1f)
        {
            timer += Time.deltaTime * SpeedToDeactivate;

            RealWire.localEulerAngles = new Vector3(
                Mathf.Lerp(startRotation.x, startRotation.x + targetRotation.x, timer),
                Mathf.Lerp(startRotation.y, startRotation.y + targetRotation.y, timer),
                Mathf.Lerp(startRotation.z, startRotation.z + targetRotation.z, timer)
            );
            yield return null;
        }
        Destroy(this);
        yield return null;
    }
}

/*using UnityEngine;
using UnityEngine.Events;

public class AntiwireKnot : InteractBase {
    public int WireId = -1;
    public UnityEvent<int> Action;
   
    /*public UnityEvent Action2;
    Animation cablesAnimation;
    AnimationClip cable;

private void awake()
    {
        gameObject.SetActive(false);
        //cablesAnimation = GetComponent<Animation>();
        //cable = cablesAnimation.GetClip("cablerojo");
    }

    public override void Execute(bool isLeftAction = true)
    {
        Action.Invoke(WireId);
        Destroy(gameObject);  
        //cablesAnimation.CrossFade("cablerojo");//cable.name
        //Action2.Invoke();
             
        
        
    }
    public void ActivateWires()
    {
        gameObject.SetActive(true);
    }


}*/