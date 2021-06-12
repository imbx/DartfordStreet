using UnityEngine;
using BoxScripts;
using System.Collections.Generic;
using UnityEngine.UI;

public class DiaryPage : MonoBehaviour {
    public int ReqID = 0;
    public string Title = " ";
    public NotebookType pageType;
    
    public bool isDoubleFaced = false;
    public Sprite CaraA;
    public Sprite CaraB;
    private bool isShowingFaceA = true;

    public bool isActive { get { return gameObject.activeInHierarchy; } }

    private void Awake() {
        if(GameController.current)
        {
            if(GameController.current.database.ProgressionExists(ReqID)) return;
                GameController.current.database.AddProgressionID(ReqID);
        }
    }
    public void SetActive(bool setActive)
    {
        gameObject.SetActive(setActive);
        GetComponent<Image>().sprite = isShowingFaceA ? CaraA : CaraB;
    }
    public void Flip()
    {
        isShowingFaceA = !isShowingFaceA;
        GetComponent<Image>().sprite = isShowingFaceA ? CaraA : CaraB;
    }

    
}