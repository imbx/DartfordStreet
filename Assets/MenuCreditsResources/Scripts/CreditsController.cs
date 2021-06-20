using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsController : MonoBehaviour
{
    public Camera mainCamera;
    public GameControllerObject gObject;
    public List<Transform> targetPoints;
    public Animator cameraAnimator;
    public Animator newspaper;

    public CreditScene credits;

    private bool isPlayingAnimation = false;
    public float firstTimer = 2f;
    public float timer = 12f;
    private bool firstTimerDone = false;

    private bool secondTimerDone = false;

    public Image FadeImage;


    // Start is called before the first frame update
    void OnEnable()
    {
        mainCamera.transform.position = gObject.hasGoodEnd ? targetPoints[0].position : targetPoints[1].position;
        mainCamera.transform.rotation = gObject.hasGoodEnd ? targetPoints[0].rotation : targetPoints[1].rotation;
        if(gObject.hasGoodEnd)
        {
            cameraAnimator.SetBool("isGood", gObject.hasGoodEnd);
            cameraAnimator.SetTrigger("playAnim");
        }
        else
        {
            newspaper.SetTrigger("playAnim");
        }
        // GetComponent<Animation>().clip = gObject.hasGoodEnd ? GoodEndAnimations[0] : BadEndAnimations[0];
        // GetComponent<Animation>().Play();

        isPlayingAnimation = true;
    }

    // Update is called once per frame
    void Update()
    {
        firstTimer -= Time.deltaTime;

        if(firstTimer <= 0f && !firstTimerDone)
        {
            firstTimerDone = true;
            credits.AnimateCredits();
            if(! gObject.hasGoodEnd) cameraAnimator.SetTrigger("playAnim");
        }

        if(firstTimerDone && !secondTimerDone)
        {
            timer -= Time.deltaTime;
        }

        if(timer <= 0f && !secondTimerDone)
        {
            cameraAnimator.SetTrigger("playAnim");
            secondTimerDone = true;
            timer = 1f;
        }

        if(secondTimerDone)
        {
            timer -= Time.deltaTime * credits.speedToGo * 2f;
        }

        if(timer <= 0 && secondTimerDone)
        {
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeIn()
    {
        float timer = 0;

        while(timer <= 1f)
        {
            timer += Time.deltaTime * 2f;
            FadeImage.color = new Color(0, 0, 0, Mathf.Lerp(0f, 1f, timer));

            yield return null;
        }
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadScene("01_MainMenu");
        Destroy(this);

        yield return null;
    }
}
