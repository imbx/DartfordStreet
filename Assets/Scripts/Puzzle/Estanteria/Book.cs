using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Book : InteractBase {

    [Header("Book")]
    public int Row = 0;
    public int BookID;
    public bool isImportant = false;
    // public bool strictOrder = false;
    
    public BookColor bookColor = BookColor.None;
    public PrimaryController controller;
    [SerializeField] private UnityEvent<Book, Book> Action;

    private Vector3 startPosition;

    private Vector3 startMousePos;
    private bool isMoving = false;

    [FMODUnity.EventRef]
    public string itemSound = "event:/cogerObject2d";

    private void OnEnable() {
        startPosition = transform.position;
    }

    public override void Execute(bool isLeftAction = true) {

        if(controller.isInputHold || controller.isInput2Hold) return;
        if(!isInteractingThis)
        {
            startMousePos = Camera.main.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));
            startPosition = transform.position;
            isInteractingThis = true;
            GetComponent<BoxCollider>().enabled = false;
            GameController.current.music.playMusic(itemSound);
        }
    }

    void Update()
    {
        if(isInteractingThis && !isMoving)
        {
            isMoving = controller.isInputHold || controller.isInput2Hold;
        }
        else if(isInteractingThis && isMoving)
        {
            if(!controller.isInputHold)
            {
                isMoving = false;
                isInteractingThis = false;
                RaycastHit hit;
                Vector3 m_ROrigin = gameControllerObject.camera.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));
                Vector3 direction = gameControllerObject.camera.transform.forward;
                Debug.Log("[Book] End moving");

                Ray r = gameControllerObject.camera.ScreenPointToRay((Vector3)controller.Mouse);
                if(Physics.Raycast(
                    r, out hit, 5f,
                    LayerMask.GetMask("Focus")))
                {
                    if(hit.collider.GetComponent<Book>())
                    {
                        if(hit.collider.GetComponent<Book>().Row == Row)
                        {
                            Debug.Log("[Book] Found placement");
                            Book book = hit.collider.GetComponent<Book>();
                            Vector3 temporalPos = startPosition;

                            UpdatePosition(book.transform.position);
                            book.UpdatePosition(temporalPos);
                            Action.Invoke(this, book);
                        } else ResetPosition();
                        
                        
                    } else ResetPosition();
                } else  ResetPosition();
                GetComponent<BoxCollider>().enabled = true;
            }
        }

        if(isMoving)
        {
            isMoving = controller.isInputHold;
            Ray r = gameControllerObject.camera.ScreenPointToRay((Vector3)controller.Mouse);
            if(Physics.Raycast(
                r, out var hit, 5f))
            {
                transform.position = new Vector3(
                    hit.point.x,
                    startPosition.y,
                    startPosition.z + 0.025f
                );
            }
        }
    }

    public void ResetPosition()
    {
        transform.position = startPosition;
    }

    public void UpdatePosition(Vector3 newPosition)
    {
        transform.position = newPosition;
        startPosition = transform.position;
    }
}

public enum BookColor 
{
    Red,
    Blue,
    Green,
    Yellow,
    Brown,
    None
}