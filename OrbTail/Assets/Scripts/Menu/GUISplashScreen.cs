using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GUISplashScreen : MonoBehaviour
{
    public GameObject radiance;
    public GameObject polimi;
    public GameObject mente_zero;
    
    void Start ()
    {
        iTween.FadeTo(polimi, 0f, 0f);
        iTween.FadeTo(mente_zero, 0f, 0f);

        StartCoroutine("ChangeSecondSplash");
    }
    
    private IEnumerator ChangeSecondSplash()
    {
        yield return new WaitForSeconds(timeOfSplash);

        iTween.FadeTo(radiance, 0f, fadeTime);
        iTween.FadeTo(polimi, 1f, fadeTime);
        iTween.FadeTo(mente_zero, 1f, fadeTime);

        yield return new WaitForSeconds(timeOfSplash + fadeTime);

        SceneManager.LoadScene("MenuMain", LoadSceneMode.Single);
    }

    private float timeOfSplash = 1.5f;
    private float fadeTime = 0.3f;
}
