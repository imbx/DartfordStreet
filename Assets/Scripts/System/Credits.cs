using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour {
    private void Start() {
        StartCoroutine(ExitCredits());
    }

    IEnumerator ExitCredits ()
    {
        yield return new WaitForSeconds(10f);

        SceneManager.LoadScene("01_MainMenu");
    }
}