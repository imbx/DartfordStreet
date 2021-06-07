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

    [FMODUnity.EventRef]
    public string itemSound = "event:/cogerObject2d";



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
                startTransform = new TransformData(transform);
                movement.SetConfig(2f, true);
                movement.SetParameters(new TransformData(GameController.current.gameCObject.camera.transform.position + (GameController.current.gameCObject.camera.transform.forward * DistanceToCamera), Vector3.zero), startTransform);
            } else { 
                movement.Invert();
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
                if(!isLeftAction && controller.isInput2Hold)
                {
                    transform.localEulerAngles += new Vector3(-controller.CameraAxis.y, controller.CameraAxis.x, 0);
                }
                else if(isLeftAction)
                {
                    isLeftAction = false;
                }
            }

            if(gameControllerObject.state == GameState.LOOKITEM && controller.isEscapePressed && !NoEffects){
                Debug.Log("[Item] Called Escape");
                OnExit();
            }
        }
    }
    
}