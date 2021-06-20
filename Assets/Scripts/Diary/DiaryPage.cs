using UnityEngine;
using BoxScripts;
using System.Collections.Generic;
using UnityEngine.UI;

public class DiaryPage : MonoBehaviour {
    public int ReqID = 0;
    public string Title = " ";
    public string Title_EN = " ";
    public NotebookType pageType;
    
    public bool isDoubleFaced = false;
    public Sprite CaraA;
    public Sprite CaraB;

    public Sprite A_EN;
    public Sprite B_EN;
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
        if(GameController.current.gameCObject.lang == "ES")
            GetComponent<Image>().sprite = isShowingFaceA ? CaraA : CaraB;
        else GetComponent<Image>().sprite = isShowingFaceA ? A_EN : B_EN;
    }
    public void Flip()
    {
        isShowingFaceA = !isShowingFaceA;
        if(GameController.current.gameCObject.lang == "ES")
            GetComponent<Image>().sprite = isShowingFaceA ? CaraA : CaraB;
        else GetComponent<Image>().sprite = isShowingFaceA ? A_EN : B_EN;
    }

    
}