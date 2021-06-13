using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour {
    public List<Light> thunderLights;
    public bool ExecuteThunder = false;

    [Header("Thunder Parameters")]
    [SerializeField] private float MaxIntensity = 8f;
    [SerializeField] private float LowestMaxIntensity = 6f;
    [SerializeField] private float TimeToMaxIntensity = 0.25f;
    [SerializeField] private float TimeToHalf = 0.1f;
    [SerializeField] private float HoldAtMaxTimer = 0.5f;
    [SerializeField] private float SpeedToRemove = 1f;

    private bool isAnimating = false;

    private void Update() {
        if(ExecuteThunder && !isAnimating)
        {
            ExecuteThunder = false;

            StartCoroutine(AnimThunder());
        }
    }

    IEnumerator AnimThunder()
    {
        isAnimating = true;
        float timer = 0f;
        while(timer < TimeToMaxIntensity)
        {
            timer += Time.deltaTime;
            foreach(Light x in thunderLights)
            {
                x.intensity = Mathf.Lerp(0, MaxIntensity, timer / TimeToMaxIntensity);
                yield return null;
            }
            yield return null;
        }

        timer = 0f;
        while(timer < TimeToHalf)
        {
            timer += Time.deltaTime;
            foreach(Light x in thunderLights)
            {
                x.intensity = Mathf.Lerp(MaxIntensity, LowestMaxIntensity, timer / TimeToHalf);
                yield return null;
            }
            yield return null;
        }

        timer = 0f;
        while(timer < TimeToHalf)
        {
            timer += Time.deltaTime;
            foreach(Light x in thunderLights)
            {
                x.intensity = Mathf.Lerp(LowestMaxIntensity, MaxIntensity, timer / TimeToHalf);
                yield return null;
            }
            yield return null;
        }

        yield return new WaitForSeconds(HoldAtMaxTimer);

        timer = 0f;
        while(timer < 1f)
        {
            timer += Time.deltaTime * SpeedToRemove;
            foreach(Light x in thunderLights)
            {
                x.intensity = Mathf.Lerp(x.intensity, 0, timer);
                yield return null;
            }
            yield return null;
        }
        
        isAnimating = false;
        yield return null;
    }
}