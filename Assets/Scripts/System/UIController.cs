using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public Image Cursor;
    public GameObject Pointer;
    public GameObject Notebook;
    public DiaryController notebookController;
    public Archive archive;
    public GameObject RadioText;
    public GameObject SafeboxText;
    public List<GameObject> TargetUIObjects;
    public List<Sprite> Interactions;

    public GameObject InteractText;

    public void SetNotebookActive(bool active = true) {
        archive.gameObject.SetActive(active);
        // Notebook.SetActive(active);
        //Notebook.GetComponent<Notebook>().enabled = active;
    }

    public void HideUI()
    {
        Debug.Log("Hiding UI");
    }

    public void ChangeCursor(int id)
    {
        if(id < 0) 
        {
            DisableCursor();
            return;
        }
        
        Pointer.SetActive(false);
        Cursor.gameObject.SetActive(true);
        Cursor.sprite = Interactions[id];
    }

    public void DisableCursor(bool disablePointer = false)
    {
        Cursor.gameObject.SetActive(false);
        Pointer.SetActive(!disablePointer);
    }

    public void TargetUI(string targetTag)
    {
        string tempString = GameController.current.database.GetInteraction(targetTag);

        if(tempString != " ")
        {
            InteractText.SetActive(true);
            InteractText.GetComponent<Text>().text = tempString;
        } else InteractText.SetActive(false);
        
        /*if(targetTag == "Safebox")
        {
            SafeboxText.SetActive(true);
        } else SafeboxText.SetActive(false);*/
        
        /*foreach(GameObject gObject in TargetUIObjects)
        {
            if(gObject.activeInHierarchy && gObject.name != targetTag) gObject.SetActive(false);
            else if(gObject.name == targetTag) gObject.SetActive(true);
        }*/
    }

    public void ForceDiaryPage(int reqId)
    {
        archive.ForcePage(reqId);
        // notebookController.forcedReqId = reqId;
        // notebookController.wantToForcePage = true;
    }
}