using UnityEngine;

public class ChangePaperMats : MonoBehaviour {
    public Material A_Mat;
    public Material B_Mat;

    public Texture A_ES;
    public Texture B_ES;
    public Texture A_EN;
    public Texture B_EN;


    public void Awake()
    {
        if(GameController.current.gameCObject.lang == "ES")
        {
            A_Mat.SetTexture("_MainTex", A_ES);
            if(B_ES) B_Mat.SetTexture("_MainTex", B_ES);
            
        }
        else
        {
            A_Mat.SetTexture("_MainTex", A_EN);
            if(B_EN) B_Mat.SetTexture("_MainTex", B_EN);
        }

        Destroy(this);
    }
}