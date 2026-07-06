using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevel : MonoBehaviour
{
    [SerializeField] float LevelDelay = 1f;
    [SerializeField] AudioClip levelExitSFX;
    bool isLoading = false;

    // [Obsolete]
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isLoading) return;

        if (collision.CompareTag("Player"))
        {
            isLoading = true;
            AudioSource.PlayClipAtPoint(levelExitSFX, transform.position);
            StartCoroutine(LoadNextLevel());
        }
    }


    // [Obsolete]
    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(LevelDelay);
        int currentScenceIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentScenceIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        // Debug.Log("Resetting Game exit");

        ScenePersist scenePersist = FindAnyObjectByType<ScenePersist>();
        scenePersist?.ResetScenePersist();
        yield return null;
        SceneManager.LoadScene(nextSceneIndex);
    }
}
