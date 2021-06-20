using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTranslate : MonoBehaviour
{
    public string ES = "";
    public string EN = "";

    public GameControllerObject gameControllerObject; 
    private Text text;

    private void Awake() {
        text = GetComponent<Text>();
    }
    
    private void OnEnable() {
        if(gameControllerObject.lang == "ES")
        {
            text.text = ES;
        }
        else
        {
            text.text = EN;
        }
        
    }
}
