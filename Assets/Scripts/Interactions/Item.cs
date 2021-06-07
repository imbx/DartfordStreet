using UnityEngine;
using BoxScripts;

[RequireComponent(typeof (Movement))]
public class Item : InteractBase {
    public bool NoEffects = false;
    public bool CanPickup = true;
    public bool HasItemInside = false;
    protected bool isLeftAction = true;
    [SerializeField] protected PrimaryController controller;
    protected Movement movement;
    protected TransformData startTransform;
    [SerializeField] protected GameObject Son = null;

    public float DistanceToCamera = 0.5f;

    [SerializeField] private float zoomedDistanceToCamera = 0.25f;
    [SerializeField] private float zoomSpeed = 0.05f;
    private Vector3 destination;
    private Vector3 zoomedVector = Vector3.zero;
    private float accumulatedZoomedFloat = 0;

    [FMODUnity.EventRef]
    public string itemSound = "event:/cogerObject2d";

    protected GameObject LookAtPoint;

    void Start()
    {
        movement = GetComponent<Movement>();
    }

    public override void Execute(bool isLeftAction = true) {
        if(hasRequirement && !GameController.current.database.GetProgressionState(reqID)) return;
        GameController.current.music.playMusic(itemSound);

        isInteractingThis = true;
        if(Son) Son.GetComponent<Item>().isInteractingThis = true;
        if(isLeftAction && gameControllerObject.state != GameState.LOOKITEM){
            if(NoEffects) {
                OnEnd();
                return;
            }
            
            if(startTransform == null) 
            {
                tag = "Item";
                if(Son) {
                    BoxUtils.SetLayerRecursively(gameObject, 8);
                    gameObject.layer = LayerMask.NameToLayer("Blocked");
                }
                else gameObject.layer = LayerMask.NameToLayer("Focus");

                if(Son) Son.tag = "Item";
                zoomedVector = Vector3.zero;
                accumulatedZoomedFloat = 0;
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
                destination = GameController.current.gameCObject.camera.transform.position +
                        (GameController.current.gameCObject.camera.transform.forward * DistanceToCamera);
                LookAtPoint.GetComponent<Movement>().SetParameters(pointCamera, pointTransform);
                movement.SetParameters(tCamera, startTransform);
            } 
            else
            {
                zoomedVector = Vector3.zero;
                accumulatedZoomedFloat = 0;

                movement.Invert();
                LookAtPoint.GetComponent<Movement>().Invert();
                startTransform = default;
            }
            if(HasItemInside) GetComponent<BoxCollider>().enabled = false;
            gameControllerObject.ChangeState(GameState.LOOKITEM);
        }

        if(movement.isAtDestination) {
            if(isLeftAction && CanPickup)
            {
                BoxUtils.SetLayerRecursively(gameObject, 6);
                OnEnd();
                return;
            }
            this.isLeftAction = isLeftAction;
        }
    }

    protected override void OnEnd(bool destroyGameObject = false)
    {
        isInteractingThis = false;
        if(Son) Son.GetComponent<Item>().isInteractingThis = false;
        isLeftAction = false;
        GameController.current.database.EditProgression(_id, true);
        gameControllerObject.ChangeState(GameState.ENDLOOKITEM);
        GameController.current.database.SaveGame();
        if(achievementType != AchievementType.None)
        {
            GameController.current.textManager.SpawnAchievement(achievementType);
        }
        if(NoEffects || CanPickup) Destroy(gameObject);
    }

    public override void OnExit()
    {
        isInteractingThis = false;
        tag = "BasicInteraction";
        // base.OnExit();
        if(!NoEffects) {
            BoxUtils.SetLayerRecursively(gameObject, 6);
            gameObject.layer = LayerMask.NameToLayer("Interactuable");
            movement.Invert();
        }
        gameControllerObject.ChangeState(GameState.ENDLOOKITEM);
        if(HasItemInside) {
            if(Son) {
                GetComponent<BoxCollider>().enabled = true;
                Son.GetComponent<Item>().isInteractingThis = false;
            }
            else this.enabled = false;
        }
    }

    private void Update()
    {
        if(hasRequirement)
        {
            if(GameController.current.database.GetProgressionState(reqID))
                transform.tag = "BasicInteraction";
        }

        if(isInteractingThis)
        {
            if(movement.isAtDestination)
            {
                if(HasItemInside && Son == null) OnExit();

                if(controller.Axis.x != 0)
                {
                    float zoomDirection = 0f;
                    Vector3 cameraPosition = GameController.current.gameCObject.camera.transform.position;
                    Vector3 cameraForward = GameController.current.gameCObject.camera.transform.forward;
                    Debug.Log("[Item] Zoomed magnitude is : " + zoomedVector.magnitude);
                    if(controller.Axis.x > 0 && accumulatedZoomedFloat < zoomedDistanceToCamera)
                        zoomDirection = - zoomSpeed * Time.deltaTime;
                    else if (controller.Axis.x < 0 && accumulatedZoomedFloat > -zoomedDistanceToCamera)
                        zoomDirection = zoomSpeed * Time.deltaTime;

                    accumulatedZoomedFloat -= zoomDirection;
                    zoomedVector += (cameraForward.normalized * zoomDirection);
                    transform.position = destination + zoomedVector;
                }
                if(!isLeftAction && controller.isInput2Hold)
                {
                    transform.Rotate(Vector3.up, -controller.CameraAxis.x);
                    transform.Rotate(Vector3.right, controller.CameraAxis.y);
                    
                    // transform.localEulerAngles += new Vector3(-controller.CameraAxis.y, controller.CameraAxis.x, 0);
                }
                else if(isLeftAction)
                {
                    isLeftAction = false;
                }
            }
            else
            {
                transform.LookAt(LookAtPoint.transform);
            }

            if(gameControllerObject.state == GameState.LOOKITEM && controller.isEscapePressed && !NoEffects){
                Debug.Log("[Item] Called Escape");
                OnExit();
            }
        }
    }

    void OnDrawGizmos(){
        #if UNITY_EDITOR
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(
                transform.position,
                transform.position + transform.forward.normalized * DistanceToCamera
            );
            Gizmos.color = Color.white;
        #endif
    }
    
}