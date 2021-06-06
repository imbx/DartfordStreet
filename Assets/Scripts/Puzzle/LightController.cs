using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LightController : MonoBehaviour {
    public int Id = -1;
    public List<int> RequirementIds;
    public List<GameObject> lights;
    public UnityEvent otherActions;
    private List<float> lightsIntensity;

    public float IntensitySpeed = 4f;

    private bool isAnimating = false;

    private bool hasExecutedLights = false;
    private void Awake() {
        GameController.current.database.AddProgressionID(Id);
    }

    public void SwitchLights(bool isOn = true, bool saveIntensity = true)
    {
        if(hasExecutedLights) return;
        hasExecutedLights = true;
        GameController.current.database.EditProgression(Id);
        if(lights.Count <= 0) return;
        if(saveIntensity) lightsIntensity = new List<float>();
        lights.ForEach(l => {
            l.SetActive(isOn);
            if(saveIntensity) lightsIntensity.Add(l.GetComponent<Light>().intensity);
        });
        otherActions.Invoke();
    }

    public void LightController_AnimLights()
    {
        if(isAnimating) return;
        StartCoroutine(AnimLights());
    }

    IEnumerator AnimLights()
    {
        yield return new WaitForSeconds(1f);
        isAnimating = true;
        SwitchLights(false, true);
        lights.ForEach(l => {
            l.GetComponent<Light>().intensity = 0;
        });
        SwitchLights(true, false);

        yield return new WaitForSeconds(0.35f);

        // SwitchLights(true, false);

        float timer = 0;

        while(timer < 1f)
        {
            timer += Time.deltaTime * IntensitySpeed;
            for(int i = 0; i < lights.Count; i++)
            {
                lights[i].GetComponent<Light>().intensity = Mathf.Lerp(0, lightsIntensity[i], timer);
                yield return null;
            }
            yield return null;
        }
        isAnimating = false;
        yield return null;
    }
    
    private void Update() {
        bool generalCondition = true;

        if(RequirementIds.Count > 0 && !hasExecutedLights)
        {
            // Debug.Log("[LightController] Checking Lights");
            foreach(int i in RequirementIds)
            {
                // Debug.Log("[LightController] Requirement " + i + " is " + GameController.current.database.GetProgressionState(i));
                if(!GameController.current.database.GetProgressionState(i)) generalCondition = false;
                // Debug.Log("[LightController] General condition is " + generalCondition);
            }
            if(generalCondition)
            {
                Debug.Log("[LightController] Switching Lights " + generalCondition);
                SwitchLights();
            }
        }
        
    }
}