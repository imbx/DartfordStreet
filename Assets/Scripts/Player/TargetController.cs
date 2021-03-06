using UnityEngine;
using BoxScripts;
public class TargetController : MonoBehaviour {
    public GameControllerObject gcObject;
    public EntityData m_CVars;
    public PrimaryController m_PlayerMovement;
    private Vector3 m_ROrigin;
    private float _isPressedCd = 0f;
    [SerializeField] private float InteractRange = 2f;

    public float TimeSeenThought = 1f;

    private float TargetThoughtTimer = 0f;
    void Update() 
    {
        if(StatesToAvoid()) return;
        RaycastHit hit;
        
        m_ROrigin = gcObject.camera.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));

        Vector3 direction = gcObject.camera.transform.forward;
        if(!m_CVars.CanLook) {
            Ray mouseHit = Camera.main.ScreenPointToRay(m_PlayerMovement.Mouse);
            m_ROrigin = Camera.main.ScreenToWorldPoint(m_PlayerMovement.Mouse);
            direction = mouseHit.direction;
        }

        if(_isPressedCd > 0) _isPressedCd -= Time.deltaTime;
        LayerMask layerMask = LayerMask.GetMask("Blocked") | LayerMask.GetMask("Focus") | LayerMask.GetMask("Interactuable");
        if(gcObject.state == GameState.INTERACTING || gcObject.state == GameState.LOOKITEM) layerMask = LayerMask.GetMask("Blocked") | LayerMask.GetMask("Focus");
        if(gcObject.targetAllLayers) layerMask = -1;
        if(Physics.Raycast(m_ROrigin, direction, out hit, m_CVars.VisionRange, layerMask)){
            
            // Debug.Log("[TargetController] Passed check");
            if(Mathf.Abs((hit.transform.position - transform.position).magnitude) >
                InteractRange && !gcObject.targetAllLayers)
            {
                if(!((m_PlayerMovement.isInputHold || m_PlayerMovement.isInput2Hold) && gcObject.playerTargetTag != "")) gcObject.playerTargetTag = "";
                // if(gcObject.state == BoxScripts.GameState.TARGETING) gcObject.ChangeState(BoxScripts.GameState.PLAYING);
                GameController.current.ui.ChangeCursor(-1);
                return;
            }

            if(hit.collider.GetComponent<ThoughtsSeen>())
            {
                TargetThoughtTimer += Time.deltaTime;

                if(TimeSeenThought <= TargetThoughtTimer)
                {
                    hit.collider.GetComponent<ThoughtsSeen>().SpawnThought();
                    TargetThoughtTimer = 0;
                }
            }
            else
            {
                TargetThoughtTimer = 0;
                if(!((m_PlayerMovement.isInputHold || m_PlayerMovement.isInput2Hold) && gcObject.playerTargetTag != "")) gcObject.playerTargetTag = hit.collider.tag;

                // Debug.Log("[TargetController] Hitpoint : " + hit.point);
                gcObject.playerTargetPosition = hit.point;
                bool leftButton = m_PlayerMovement.isInputDown;
                if(
                    (!m_CVars.CanLook && hit.collider.GetComponent<InteractBase>()) ||
                    (hit.collider.GetComponent<InteractBase>() && Mathf.Abs((hit.transform.position - transform.position).magnitude) < InteractRange)
                )
                {
                    if( gcObject.state != BoxScripts.GameState.INTERACTING &&
                        gcObject.state != BoxScripts.GameState.ENDINTERACTING &&
                        gcObject.state != BoxScripts.GameState.LOOKITEM &&
                        gcObject.state != BoxScripts.GameState.ENDLOOKITEM)
                        {
                            //gcObject.playerTargetTag = hit.collider.tag;
                            switch(hit.collider.tag)
                            {
                                case "BasicInteraction":
                                    GameController.current.ui.ChangeCursor(0);
                                break;
                                case "Picture":
                                    GameController.current.ui.ChangeCursor(3);
                                    //gcObject.ChangeState(BoxScripts.GameState.TARGETINGPICTURE);
                                    // GameController.current.SetCursor();
                                    break;
                                case "Pick":
                                    GameController.current.ui.ChangeCursor(1);
                                    //gcObject.ChangeState(BoxScripts.GameState.TARGETINGPICTURE);
                                    // GameController.current.SetCursor();
                                    break;
                                case "Locked":
                                    // SHOULD CHANGE CURSOR
                                    GameController.current.ui.ChangeCursor(2);
                                    // Debug.Log("Is locked, shouldnt do anything");
                                    break;
                                case "Requirement":
                                    // SHOULD CHANGE CURSOR
                                    GameController.current.ui.ChangeCursor(-1);
                                    // Debug.Log("Is locked, shouldnt do anything");
                                    break;
                                default:
                                    GameController.current.ui.ChangeCursor(-1);
                                    // gcObject.ChangeState(BoxScripts.GameState.TARGETING);
                                    break;
                            }
                        }
                    if(gcObject.state == BoxScripts.GameState.MOVINGPICTURE) return;
                    if(hit.collider.tag == "Item" || gcObject.state != BoxScripts.GameState.LOOKITEM)
                    if(_isPressedCd <= 0 && (leftButton || m_PlayerMovement.isInput2Down) && hit.collider.GetComponent<InteractBase>()) {
                        Debug.Log("[TargetController] Executing on " + hit.collider.name);
                        _isPressedCd = 0.5f;
                        hit.collider.GetComponent<InteractBase>().Execute(leftButton);
                    }
                }
            }
        }
        else
        {
            TargetThoughtTimer = 0;
            if(!((m_PlayerMovement.isInputHold || m_PlayerMovement.isInput2Hold) && gcObject.playerTargetTag != "")) gcObject.playerTargetTag = "";
            // if(gcObject.state == BoxScripts.GameState.TARGETING) gcObject.ChangeState(BoxScripts.GameState.PLAYING);
            GameController.current.ui.ChangeCursor(-1);
        }
    }

    private bool StatesToAvoid()
    {
        return gcObject.state == GameState.MOVINGCAMERA || gcObject.state == BoxScripts.GameState.ENDINTERACTING || gcObject.state == BoxScripts.GameState.ENDLOOKITEM || gcObject.state == GameState.OPENNOTEBOOK || gcObject.state == GameState.CLOSENOTEBOOK;
    }
}