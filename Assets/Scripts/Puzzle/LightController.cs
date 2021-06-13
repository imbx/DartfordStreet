using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LightController : MonoBehaviour {
    public int Id = -1;
    public List<int> RequirementIds;
    public List<GameObject> lights;
    public List<Material> lightMaterials;
    public UnityEvent otherActions;
    private List<float> lightsIntensity;

    public float IntensitySpeed = 4f;

    private bool isAnimating = false;

    private bool hasExecutedLights = false;
    private void Awake() {
        GameController.current.database.AddProgressionID(Id);
    }

    private void Start() {
        foreach(Material mat in lightMaterials)
        {
            mat.SetColor("_EmissionColor", new Vector4(0.9254902f, 0.6588235f, 0.4235294f, 1f) * -0f);
            // mat.EnableKeyword("_EMISSION");
        }
        
        lightsIntensity = new List<float>();

        if(lights.Count > 0)
        {
            lights.ForEach(l => {
                l.SetActive(false);
                lightsIntensity.Add(l.GetComponent<Light>().intensity);
                l.GetComponent<Light>().intensity = 0;
            });
        }
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
            // if(saveIntensity) lightsIntensity.Add(l.GetComponent<Light>().intensity);
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
        isAnimating = true;
        yield return new WaitForSeconds(1f);
        /* SwitchLights(false, true);
        lights.ForEach(l => {
            l.GetComponent<Light>().intensity = 0;
        });
        hasExecutedLights = false;*/
        SwitchLights(true, false);

        yield return new WaitForSeconds(0.35f);
        

        // SwitchLights(true, false);
        foreach(Material mat in lightMaterials)
        {
            Debug.Log("[LightController] Enabling Emision in " + mat.name);
            // mat.SetColor("_EmissionColor", new Color(0.9254902f, 0.6588235f, 0.4235294f, -2f));
            mat.EnableKeyword("_EMISSION");
            yield return null;
        }

        float timer = 0;

        while(timer < 1f)
        {
            timer += Time.deltaTime * IntensitySpeed;
            
            for(int i = 0; i < lights.Count; i++)
            {
                lights[i].GetComponent<Light>().intensity = Mathf.Lerp(0, lightsIntensity[i], timer);
                yield return null;
            }

            foreach(Material mat in lightMaterials)
            {
                // mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", new Vector4(0.9254902f, 0.6588235f, 0.4235294f, 1f) * Mathf.Lerp(0f, 1f, timer));
                yield return null;
            }

            yield return null;
        }
        isAnimating = false;
        yield return null;
    }
    
    private void Update() {
        bool generalCondition = true;

        if(RequirementIds.Count > 0 && !hasExecutedLights && !isAnimating)
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
                LightController_AnimLights();
            }
        }
        
    }
}