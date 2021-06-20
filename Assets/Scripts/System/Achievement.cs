using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BoxScripts;

public class Achievement : MonoBehaviour {
    public List<Sprite> Icons;
    public List<string> Texts;
    public List<string> LowTexts;

    public List<string> Texts_EN;
    public List<string> LowTexts_EN;

    public Image AchievementIcon;
    public Text UpperText;
    public Text LowerText;

    public float Duration = 3f;
    private bool willClose = false;

    private void OnEnable() {
        Duration = 3f;
        willClose = false;
        GetComponent<Animator>().Rebind();
        GetComponent<Animator>().Update(0f);
    }

    private void Update() {
        Duration -= Time.deltaTime;

        if(!willClose)
        {
            if(Duration <= 0)
            {
                willClose = true;
                Duration = 1f;
                GetComponent<Animator>().SetTrigger("Out");
            }
        }

        if(willClose && Duration <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void ShowAchievement(AchievementType achievement)
    {
        if(!AchievementIcon || !UpperText || !LowerText) return;
        if(Icons.Count < (int) achievement || (int) achievement < 0) return;

        AchievementIcon.sprite = Icons[(int) achievement];
        if(GameController.current.gameCObject.lang == "ES")
        {
            UpperText.text = Texts[(int) achievement];
            LowerText.text = LowTexts[(int) achievement];
        }
        else
        {
            UpperText.text = Texts_EN[(int) achievement];
            LowerText.text = LowTexts_EN[(int) achievement];
        }
        

        gameObject.SetActive(true);
    }

}
