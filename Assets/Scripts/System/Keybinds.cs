using UnityEngine.InputSystem;
using UnityEngine;

public class Keybinds : MonoBehaviour {

    public PrimaryController controller;
    InputAction movementAction;
    InputAction lookAction;
    bool        mouseRightButtonPressed;

    private float isHoldCounter = 0f;
    private float isHold2Counter = 0f;

    private float inputThreshold = 0.25f;
    
    void Start() {
        var map = new InputActionMap("DartfordStController");
        lookAction = map.AddAction("look", binding: "<Mouse>/delta");
        lookAction.AddBinding("<Gamepad>/rightStick").WithProcessor("scaleVector2(x=15, y=15)");
        movementAction = map.AddAction("move", binding: "<Gamepad>/leftStick");
        movementAction.AddCompositeBinding("Dpad")
            .With("Up", "<Keyboard>/w")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/s")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/a")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/d")
            .With("Right", "<Keyboard>/rightArrow");

        movementAction.Enable();
        lookAction.Enable();
    }


    void Update () {
        Vector2 vecMov = new Vector2(movementAction.ReadValue<Vector2>().y, movementAction.ReadValue<Vector2>().x);
        controller.Axis = vecMov;
        controller.CameraAxis = lookAction.ReadValue<Vector2>();
        controller.Mouse = mousePosition();
        inputThreshold -= Time.deltaTime;

        controller.isTabPressed = isTabPressed();
        controller.isLanternPressed = isLanternPressed();
        controller.isEscapePressed = isEscapePressed();
        controller.isInputPressed = isActionPressed();
        controller.isInput2Pressed = isAction2Pressed();
        controller.isInputDown = controller.isInputHold ? false : isActionPressed();
        controller.isInput2Down = controller.isInput2Hold ? false : isAction2Pressed();

        if(isActionPressed()) isHoldCounter += Time.deltaTime;
        else isHoldCounter = 0;

        if(isActionPressed() && isHoldCounter > 0.2f){
            controller.isInputHold = true;
        } else controller.isInputHold = false;

        if(isAction2Pressed()) isHold2Counter += Time.deltaTime;
        else isHold2Counter = 0;

        if(isAction2Pressed() && isHold2Counter > 0.2f){
            controller.isInput2Hold = true;
        } else controller.isInput2Hold = false;

        

        /*if(inputThreshold <= 0f)
        {
            inputThreshold = 0.25f;
            
        }
        else
        {
            controller.isEscapePressed = false;
            controller.isInputPressed = false;
            controller.isInput2Pressed = false;
        }*/
        
    }

    bool isEscapePressed() {
        return Keyboard.current != null ? Keyboard.current.escapeKey.isPressed : false; 
    }

    bool isTabPressed() {
        return Keyboard.current != null ? Keyboard.current.tabKey.isPressed : false; 
    }

    bool isActionPressed () {
        return Mouse.current != null ? Mouse.current.leftButton.isPressed : false; 
    }

    bool isAction2Pressed () {
        return Mouse.current != null ? Mouse.current.rightButton.isPressed : false; 
    }

    bool isLanternPressed () {
        return Keyboard.current != null ? Keyboard.current.qKey.isPressed : false; 
    }

    Vector2 mousePosition() {
        return Mouse.current.position.ReadValue();
    }
}