using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoxScripts;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    private TextPosition textPos;
    [SerializeField] private Vector2 Margin = new Vector2(96, 96);

    [Header("Thoughts")]
    [SerializeField] private GameObject ThoughtsContainer;
    [SerializeField] private Text ThoughtsText;
    [SerializeField] private Vector2 ThoughtsLeftTopAnchor = new Vector2(116, 68);
    [SerializeField] private Vector2 ThoughtsRightBottomAnchor = new Vector2(116, 78);

    [Header("Bubble")]
    [SerializeField] private GameObject BubbleContainer;
    [SerializeField] private Text BubbleText;

    [Header("Achievement")]

    [SerializeField] private Achievement achievement;


    private List<int> ThoughtsInQueue;
    private List<int> AchievementsInQueue;

    private void Awake() {
        ThoughtsInQueue = new List<int> ();
        AchievementsInQueue = new List<int>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ThoughtsInQueue.Count > 0 && !ThoughtsContainer.activeSelf)
        {
            SpawnThought(ThoughtsInQueue[0]);
            ThoughtsInQueue.Remove(ThoughtsInQueue[0]);
        }

        if(AchievementsInQueue.Count > 0 && !achievement.gameObject.activeSelf)
        {
            SpawnAchievement((AchievementType) AchievementsInQueue[0]);
            AchievementsInQueue.Remove(AchievementsInQueue[0]);
        }
    }

    public void SpawnThought(int dialogueID)
    {
        if(!ThoughtsContainer.activeSelf){
            
            Dialogue tx = GameController.current.database.GetDialogue(dialogueID);
            RectTransform thoughtTransform = ThoughtsContainer.GetComponent<RectTransform>();
            Vector2 anchorSettings = Vector2.one * 0.5f;

            Debug.Log("Spawning this text : " + tx);
            ThoughtsText.text = tx.dialogueText;

            if(tx.isAnchoredAtTop) anchorSettings = Vector2.one;
            else anchorSettings = Vector2.right;
            if(tx.isAnchoredAtCenter) anchorSettings.x = 0.5f;

            thoughtTransform.sizeDelta = new Vector2(tx.size.x, tx.size.y);
            thoughtTransform.pivot = anchorSettings;
            thoughtTransform.anchorMin = anchorSettings;
            thoughtTransform.anchorMax = anchorSettings;
            thoughtTransform.anchoredPosition = new Vector3(tx.anchoredPosition.x, tx.anchoredPosition.y, 0);

            ThoughtsContainer.GetComponent<MessageBox>().SetLifetime(tx.lifeTime);

            ThoughtsContainer.SetActive(true);
        } else ThoughtsInQueue.Add(dialogueID);
    }

    public bool IsThoughtInQueue (int ThoughtId)
    {
        if(ThoughtsInQueue.Contains(ThoughtId)) return true;
        return false;
    }


    public void SpawnAchievement(AchievementType a)
    {
        if(!achievement.gameObject.activeSelf)
        {
            achievement.ShowAchievement(a);

        } else AchievementsInQueue.Add((int) a);
    }

    private float CalculateLength(Text textComponent, string message)
    {
        float totalLength = 0;
        Font myFont = textComponent.font;
        CharacterInfo characterInfo = new CharacterInfo();
        char[] arr = message.ToCharArray();

        foreach(char c in arr)
        {
            myFont.GetCharacterInfo(c, out characterInfo, textComponent.fontSize);  
            Debug.Log(totalLength + " + " + characterInfo.advance);
            totalLength += characterInfo.advance + 24f;
        }
        return totalLength;
    }
}
