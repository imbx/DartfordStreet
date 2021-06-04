using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour {
    public GameObject CounterGameObject;
    public Text CounterText;
    public BombController bomb;
    private bool isUserPlaying = false;
    private bool isUserInRoom = false;
    private bool hasToEnd = false;
    private float Counter = 60f;

    public UnityEvent Action;

    public Image uiImage;

    private void Start() {
        CounterGameObject.SetActive(false);
        isUserInRoom = false;
        isUserPlaying = false;
        Counter = 60f;
    }

    private void Update() {
        if(isUserInRoom)
        {
            isUserInRoom = false;
            GetComponent<BoxCollider>().enabled = false;
            Action.Invoke();
            StartCoroutine(PreRoomAnimation());
        }
        if(!isUserPlaying) return;

        
        if(bomb.isBombDeactivated) hasToEnd = true;

        if(bomb.isBombExploding || (Counter < 0f)) StartCoroutine(PlayEnd());
        if(hasToEnd && Counter >= 0f) StartCoroutine(PlayGoodEnd());
        // else if(hasToEnd && Counter < 0f) PlayBadEnd;

        Counter -= Time.deltaTime;
        CounterText.text = "Time to explode:\n" + (Mathf.Round(Counter * 10f) / 10f) + " seconds";
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") isUserInRoom = true;
    }

    IEnumerator PreRoomAnimation ()
    {
        CounterGameObject.SetActive(true);




        isUserPlaying = true;
        yield return null;
    }

    IEnumerator PlayEnd()
    {
        isUserPlaying = false;
        yield return new WaitForSeconds(1.5f);

        Color col = Color.white;
        col.a = 0;
        uiImage.gameObject.SetActive(true);

        float timer = 0f;

        while (timer < 1f)
        {
            col.a = Mathf.Lerp(0, 1, timer);
            uiImage.color = col;

            timer += Time.deltaTime * 0.5f;
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);

        timer = 0f;

        while (timer < 1f)
        {
            col = new Color(Mathf.Lerp(1, 0, timer),  Mathf.Lerp(1, 0, timer), Mathf.Lerp(1, 0, timer), 1);
            uiImage.color = col;

            timer += Time.deltaTime * 1.5f;
            yield return null;
        }


        yield return null;

        SceneManager.LoadScene("Credits");
    }

    IEnumerator PlayGoodEnd()
    {
        yield return new WaitForSeconds(1.5f);


    }
}