using UnityEngine;
using UnityEngine.Events;
using BoxScripts;
public class PowerfulItem : Item {
    public UnityEvent Action;

    public override void Execute(bool isLeftAction = true) {
        if(hasRequirement && !GameController.current.database.GetProgressionState(reqID)) return;
        Debug.Log("[PowerfulItem] Executing");
        isInteractingThis = true;
        if(NoEffects)
        {
            Debug.Log("[PowerfulItem] Picking up");
            GameController.current.database.EditProgression(_id, true);
            Action.Invoke();
            OnEnd();
            return;
        }
        if(isLeftAction && gameControllerObject.state != GameState.LOOKITEM) {
            if(startTransform == null) 
            {
                tag = "Item";
                if(Son) {
                    BoxUtils.SetLayerRecursively(gameObject, 8);
                    gameObject.layer = LayerMask.NameToLayer("Blocked");
                }
                else gameObject.layer = LayerMask.NameToLayer("Focus");

                if(Son) Son.tag = "Item";

                if(!LookAtPoint){
                    LookAtPoint = new GameObject(name + "_LookAtPoint", typeof(Movement));
                    LookAtPoint.transform.position = transform.position + transform.forward.normalized * 0.5f;
                    LookAtPoint.transform.SetParent(transform);
                }
                TransformData pointTransform = new TransformData(LookAtPoint.transform);
                startTransform = new TransformData(transform);

                TransformData pointCamera = 
                    new TransformData(
                        GameController.current.gameCObject.camera.transform.position +
                        (GameController.current.gameCObject.camera.transform.forward * (DistanceToCamera * 0.5f)),
                        -GameController.current.gameCObject.camera.transform.eulerAngles
                    );
                TransformData tCamera = 
                    new TransformData(
                        GameController.current.gameCObject.camera.transform.position +
                        (GameController.current.gameCObject.camera.transform.forward * DistanceToCamera),
                        -GameController.current.gameCObject.camera.transform.eulerAngles
                    );
                LookAtPoint.GetComponent<Movement>().SetConfig(4f);
                movement.SetConfig(2f, true, true);
                
                LookAtPoint.GetComponent<Movement>().SetParameters(pointCamera, pointTransform);
                movement.SetParameters(tCamera, startTransform);
            } 
            else
            { 
                movement.Invert();
                LookAtPoint.GetComponent<Movement>().Invert();
                startTransform = default;
            }
            gameControllerObject.ChangeState(GameState.LOOKITEM);
        }
        
        if(movement.isAtDestination) {
            Debug.Log("[PowerfulItem] Is at destination, execute");
            if(isLeftAction && CanPickup)
            {
                Debug.Log("[PowerfulItem] Picking up");
                GameController.current.database.EditProgression(_id, true);
                Action.Invoke();
                OnEnd();
                return;
            }
            this.isLeftAction = isLeftAction;
        }
    }
    
}