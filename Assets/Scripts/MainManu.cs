using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManu : MonoBehaviour
{
    [SerializeField] AudioClip mainMenuMusic;
    public void Play()
    {
        AudioSource.PlayClipAtPoint(mainMenuMusic, Camera.main.transform.position);
        StartCoroutine(LoadSceneDelay());
    }

    IEnumerator LoadSceneDelay()
    {
        yield return new WaitForSeconds(1f); 

        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
    
}
