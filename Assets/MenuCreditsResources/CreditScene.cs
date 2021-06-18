using UnityEngine;

public class CreditScene : MonoBehaviour {

    public RectTransform targetUIText;

    public float speedToGo = 0.001f;

    public Vector2 startendY = Vector2.zero;

    public bool playAnimation = false;
    private float timer = 0f;

    void OnEnable()
    {
        timer = 0f;
        targetUIText.localPosition = new Vector3(targetUIText.localPosition.x, startendY.x,targetUIText.localPosition.z);
        playAnimation = true;
    }

    private void Update() {
        if(playAnimation)
        {
            timer += Time.deltaTime * speedToGo;
            targetUIText.localPosition = new Vector3(targetUIText.localPosition.x, Mathf.Lerp(startendY.x, startendY.y, timer),targetUIText.localPosition.z);
        }
    }
    
}