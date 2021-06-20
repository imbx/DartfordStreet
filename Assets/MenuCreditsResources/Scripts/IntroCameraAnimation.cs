using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroCameraAnimation : MonoBehaviour
{

    public Image FadeImage;
    public string LevelName = "FinalScene-Box";
    
    IEnumerator FadeInLoad()
    {

        float timer = 0;

         while(timer <= 1f)
        {
            timer += Time.deltaTime * 2f;
            FadeImage.color = new Color(0, 0, 0, Mathf.Lerp(0f, 1f, timer));

            yield return null;
        }
        yield return new WaitForSeconds(0.15f);
        SceneManager.LoadScene(LevelName);
        yield return null;
    }
    
    public void TriggerFade()
    {
        StartCoroutine(FadeInLoad());
    }
}
