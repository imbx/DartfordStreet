using System;
using System.Collections;
using UnityEngine;
using BoxScripts;

public class InteractBase : MonoBehaviour {
    [Header("Interact Base Parameters")]
    public int _id = 0;
    public bool dontDestroyIfGotten = true;
    public bool hasRequirement = false;
    public bool hideIfReq = false;
    public int reqID = -1;
    [SerializeField] protected GameControllerObject gameControllerObject;

    private bool hasCheckedState = false;

    protected bool isInteractingThis = false;
    protected string MainTag = "";

    public AchievementType achievementType = AchievementType.None;
    public int dialogueID = -1;
    

    private void Awake() {
        GameController.current.SubscribeInteraction(this);
    }
    void OnEnable()
    {
    }

    public void OnLoad() {
        if(!hasCheckedState)
        {
            MainTag = transform.tag;
            if(transform.tag != "Picture") transform.tag = "BasicInteraction";
            Debug.Log("[InteractBase] " + name);
            if(GameController.current){
                if(hasRequirement && !GameController.current.database.GetProgressionState(reqID))
                {
                    tag = "Requirement";
                    if(hideIfReq) gameObject.SetActive(false);
                }

                hasCheckedState = true;
                if(_id != 0)
                    if(GameController.current.database.ProgressionExists(_id) && !dontDestroyIfGotten){
                        Debug.Log("Deactivating " + name);
                        this.gameObject.SetActive(!GameController.current.database.GetProgressionState(_id));
                    }
                    else GameController.current.database.AddProgressionID(_id);
            }
        }
    }

    private void Update() {
    }

    protected virtual void OnStart(){

    }

    protected virtual void OnEnd(bool destroyGameObject = false) {
        isInteractingThis = false;

        if(!gameControllerObject)
            GameController.current.ChangeState(BoxScripts.GameState.ENDINTERACTING);
        else {
            gameControllerObject.ChangeState(BoxScripts.GameState.ENDINTERACTING);
            gameControllerObject.requireFocus = true;
        }
        BoxUtils.SetLayerRecursively(gameObject, 6);
        
        GameController.current.database.EditProgression(_id);
        GameController.current.database.SaveGame();
        
        if(dialogueID != -1) GameController.current.textManager.SpawnThought(dialogueID);

        if(destroyGameObject) Destroy(gameObject);
        if(transform.tag != "Picture") transform.tag = "BasicInteraction";
    }

    public virtual void OnExit() {
        isInteractingThis = false;

        if(!gameControllerObject)
            GameController.current.ChangeState(BoxScripts.GameState.ENDINTERACTING);
        else {
            gameControllerObject.ChangeState(BoxScripts.GameState.ENDINTERACTING);
            gameControllerObject.requireFocus = true;
        }
        BoxUtils.SetLayerRecursively(gameObject, 6);

        if(transform.tag != "Picture") transform.tag = "BasicInteraction";
    }

    public virtual void Execute(bool isLeftAction = true)
    {
        if(hasRequirement && !GameController.current.database.GetProgressionState(reqID)) return;
        
        isInteractingThis = true;

        transform.tag = MainTag;

        if(!gameControllerObject)
            GameController.current.ChangeState(GameState.INTERACTING);
        else {
            gameControllerObject.requireFocus = false;
            gameControllerObject.ChangeState(GameState.INTERACTING);
        }

        BoxUtils.SetLayerRecursively(gameObject, 8);
    }

    public virtual void DestroyInteraction()
    {
        if(GetComponent<BoxCollider>()) Destroy(GetComponent<BoxCollider>());

        Destroy(this);
    }
}