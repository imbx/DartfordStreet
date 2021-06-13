using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoxScripts;
using UnityEngine.UI;

public class Archive : MonoBehaviour
{
    
    [Header("Archive pages")]
    public List<DiaryPage> Pages;
    private List<int> ArchivePositions;
    private int forcedPage = -1;
    private int currentPage;
    [HideInInspector] public bool wantToForcePage = false;

    [Header("Nodes")]
    public float VerticalSpan = 54f;
    public GameObject nodePrefab;
    public GameObject pointPrefab;
    public Sprite EmptyCircle;
    public Sprite FilledCircle;
    [SerializeField] private List<GameObject> nodes;
    private float totalHeight = 0f;
    public GameObject NodesContainer;
    public Text ActionText;

    public PrimaryController controller;

    private float flipTimer = 0f;

    [Header("Sounds")]
    
    [FMODUnity.EventRef]
    public string openSound = "event:/";
    [FMODUnity.EventRef]
    public string exitSound = "event:/";
    [FMODUnity.EventRef]
    public string papelSound = "event:/";


    
    private void Awake() {
        ArchivePositions = new List<int>();
        nodes = new List<GameObject>();
    }

    void OnEnable()
    {
        SearchPages();
    }

    private void SearchPages()
    {
        for(int i = 0 ; i < Pages.Count; i++)
        {
            DiaryPage diaryPage = Pages[i];
            diaryPage.SetActive(false);
            // Debug.Log("[DiaryController] Page REQID : " + diaryPage.ReqID + " has state " + GameController.current.database.GetProgressionState(diaryPage.ReqID));
            if(GameController.current.database.GetProgressionState(diaryPage.ReqID)) {
                AddToArchive(i);
            }
        }
        SetNodes();
        if(forcedPage != -1) SetNewPage(forcedPage);
        else SetNewPage(currentPage);
    }

    private void AddToArchive (int newId)
    {
        if(ArchivePositions.Contains(newId)) return;
        ArchivePositions.Add(newId);
        GameController.current.music.playMusic(papelSound);
    }
    public void SetNewPage(int pageId)
    {
        if(pageId < ArchivePositions.Count)
        {
            Pages[GetPageFromListPost(currentPage)].SetActive(false);
            Pages[GetPageFromListPost(pageId)].SetActive(true);

            ActionText.text = Pages[GetPageFromListPost(pageId)].isDoubleFaced ? "Pulsa [Tab] para salir   [Click derecho] para voltear" : "Pulsa [Tab] para salir";
            
            SelectCircle(currentPage, pageId);
            currentPage = pageId;
        }
        else
        {
            Pages[GetPageFromListPost(ArchivePositions.Count - 1)].SetActive(false);
            currentPage = ArchivePositions.Count - 1;
        }
        forcedPage = -1;
    }

    public void ForcePage(int pageReq)
    {
        int pageCounter = 0;

        foreach(int i in ArchivePositions)
        {
            if(Pages[i].ReqID == pageReq) {
                currentPage = pageCounter;
                return;
            }
            pageCounter++;
        }
    }

    private int GetPageFromListPost(int listPos)
    {
        int pageId = 0;
        Debug.Log("[DiaryController] Want to change to page " + listPos);
        pageId = ArchivePositions[listPos];
        return pageId;
    }

    public void OnClickNext()
    {
        Debug.Log("[DiaryController] Next");
        int page = currentPage;
        if(page < ArchivePositions.Count)
            page++;
        else return;
        SetNewPage(page);
    }

    public void OnClickPrev()
    {
        Debug.Log("[DiaryController] Prev");
        int page = currentPage;
        if(page > 0)
            page--;
        else return;
        SetNewPage(page);
    }

    private void Update() {
        if(flipTimer > 0) flipTimer -= Time.deltaTime;
        if(controller.isInput2Down && Pages[GetPageFromListPost(currentPage)].isDoubleFaced && flipTimer <= 0)
        {
            flipTimer = 0.75f;
            Pages[GetPageFromListPost(currentPage)].Flip();
        }
    }

    private void DestroyNodes()
    {   if(nodes != null) foreach(GameObject diaryObject in nodes) Destroy(diaryObject);
        nodes = new List<GameObject>();
        NodesContainer.GetComponent<RectTransform>().anchoredPosition = new Vector2(96f, 0);
    }

    private void SetNodes()
    {
        int nodesLeft = Mathf.Abs(ArchivePositions.Count - nodes.Count);
        Debug.Log("[Archive] Nodes left : " + nodesLeft);
        for(int i = 0; i < nodesLeft; i++)
        {
            Debug.Log("[Archive] Creating node");
            GameObject goParent = Instantiate(nodePrefab);
            goParent.transform.SetParent(NodesContainer.transform);
            goParent.transform.localScale = Vector3.one;
            GameObject point = Instantiate(pointPrefab);
            point.transform.SetParent(goParent.transform);
            point.transform.localScale = Vector3.one;
            point.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            goParent.GetComponent<ArchivePoint>().DotImage = point.GetComponent<Image>();
            Debug.Log("[Archive] Setting node data");
            goParent.GetComponent<ArchivePoint>().SetData(nodes.Count, Pages[ArchivePositions[nodes.Count]].Title, this);
            goParent.GetComponent<Button>().targetGraphic = goParent.GetComponent<ArchivePoint>().DotImage;
            
            goParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-(VerticalSpan + totalHeight));
            totalHeight += VerticalSpan;
            Debug.Log("[Archive] Adding node");
            nodes.Add(goParent);
        }
        NodesContainer.GetComponent<RectTransform>().anchoredPosition = new Vector2(96f, (totalHeight * 0.5f));
    }
    public void SelectCircle(int prevPoint, int newPoint)
    {
        nodes[prevPoint].GetComponent<ArchivePoint>().DotImage.sprite = EmptyCircle;
        nodes[newPoint].GetComponent<ArchivePoint>().DotImage.sprite = FilledCircle;
    }
}
