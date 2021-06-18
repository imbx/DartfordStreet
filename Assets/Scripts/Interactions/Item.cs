using UnityEngine;
using BoxScripts;

[RequireComponent(typeof (Movement))]
public class Item : InteractBase {
    [SerializeField] protected PrimaryController controller;

    [Header("Item Parameters")]
    public bool NoEffects = false;
    public bool CanPickup = true;
    public bool HasItemInside = false;
    protected Movement movement;
    protected TransformData startTransform;
    [SerializeField] protected GameObject Son = null;
    [Header("Movement Parameters")]
    public bool rotateOnlyHorizontally = true;
    public bool doLookAtPoint = true;
    public float DistanceToCamera = 0.5f;
    [SerializeField] private float zoomedDistanceToCamera = 0.25f;
    [SerializeField] private float zoomSpeed = 0.05f;
    private Vector3 destination;
    private Vector3 zoomedVector = Vector3.zero;
    private float accumulatedZoomedFloat = 0;
    public bool ThoughtAlsoOnExit = false;
    
    [Header("Sound")]

    [FMODUnity.EventRef]
    public string itemSound = "event:/cogerObject2d";

    protected GameObject LookAtPoint;

     protected bool isLeftAction = true;

     private Transform FirstParent;

    void Start()
    {
        movement = GetComponent<Movement>();
        FirstParent = transform.parent;
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
                // transform.SetParent(GameController.current.gameCObject.camera.transform);
                BoxUtils.SetLayerRecursively(gameObject, 8);
                Debug.Log("[Item] Moving in");
                if(CanPickup)
                    tag = "Item";
                else tag = "ItemNoPick";
                if(Son) {
                    
                    // gameObject.layer = LayerMask.NameToLayer("Blocked");
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
                LookAtPoint.GetComponent<Movement>().SetConfig(2.1f);
                movement.SetConfig(2f, doLookAtPoint, doLookAtPoint);
                destination = GameController.current.gameCObject.camera.transform.position +
                        (GameController.current.gameCObject.camera.transform.forward * DistanceToCamera);
                LookAtPoint.GetComponent<Movement>().SetParameters(pointCamera, pointTransform);
                movement.SetParameters(tCamera, startTransform);
            } 
            else
            {

                InvertMovement();
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

    private void InvertMovement()
    {
        zoomedVector = Vector3.zero;
        accumulatedZoomedFloat = 0;

        movement.Invert();
        // LookAtPoint.GetComponent<Movement>().Invert();
        transform.SetParent(FirstParent);
        startTransform = default;
    }

    protected override void OnEnd(bool destroyGameObject = false)
    {
        isInteractingThis = false;
        if(Son) Son.GetComponent<Item>().isInteractingThis = false;
        tag = "Untagged";
        isLeftAction = false;
        GameController.current.database.EditProgression(_id, true);
        gameControllerObject.ChangeState(GameState.ENDLOOKITEM);
        GameController.current.database.SaveGame();
        Destroy(LookAtPoint);

        if(dialogueID != -1) GameController.current.textManager.SpawnThought(dialogueID);

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
            InvertMovement();
        }
        Destroy(LookAtPoint);

        if(ThoughtAlsoOnExit && dialogueID != -1) GameController.current.textManager.SpawnThought(dialogueID); 
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
            {
                transform.tag = "BasicInteraction";
                hasRequirement = false;
            } 
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
                    if(! rotateOnlyHorizontally)transform.Rotate(Vector3.right, -controller.CameraAxis.y);
                    
                    // transform.localEulerAngles += new Vector3(-controller.CameraAxis.y, controller.CameraAxis.x, 0);
                }
                else if(isLeftAction)
                {
                    isLeftAction = false;
                }
            }
            else if(doLookAtPoint)
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