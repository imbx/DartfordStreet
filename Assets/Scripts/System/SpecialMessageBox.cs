using UnityEngine;

public class SpecialMessageBox : MonoBehaviour {
    float destroyTime = 1f;
    private bool willClose = false;
    private void OnEnable() {
        
        willClose = false;
        destroyTime = 1f;
        GetComponent<Animator>().Rebind();
        GetComponent<Animator>().Update(0f);
    }

    public void OpenBox()
    {
        gameObject.SetActive(true);
    }

    public void DestroyItem()
    {
        willClose = true;
        GetComponent<Animator>().SetTrigger("TriggerAnim");
    }

    private void Update() {
        if(willClose)
        {
            destroyTime -= Time.deltaTime;
        }

        if(willClose && destroyTime <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}