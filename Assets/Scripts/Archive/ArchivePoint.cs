using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ArchivePoint : MonoBehaviour {
    public int destinationPage = 0;
    public Image DotImage;
    public Text nodeText;
    private string nodeTitle = "";
    private Archive nodesParent;

    [FMODUnity.EventRef]
    string pasarPagina = "event:/libreta/paginas";

    public void SetData(int dest, string title, Archive archive)
    {
        destinationPage = dest;
        nodesParent = archive;
        nodeTitle = title;
        nodeText.text = title;
    }

    public void SendPoint()
    {
        Debug.Log("[DiaryPoint] Should send point " + destinationPage);
        nodesParent.SetNewPage(destinationPage);
        GameController.current.music.playMusic(pasarPagina);
    }

}