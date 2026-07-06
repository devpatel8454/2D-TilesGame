using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;

    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;

    [Obsolete]
    void Awake()
    {
        int numberGameSessions = FindObjectsByType<GameSession>(FindObjectsSortMode.None).Length;
        if (numberGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();
    }

    [Obsolete]
    public void processPlayerDeath()
    {
        if (playerLives > 1)
        {
            tackLife();
        }
        else
        {
            resetGameSession();
        }
    }

    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = score.ToString();
    }

    void tackLife()
    {
        playerLives--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = playerLives.ToString();
    }

    // [Obsolete]
    void resetGameSession()
    {
        // Debug.Log("Resetting Game Session");
        // FindFirstObjectByType<ScenePersist>().ResetScenePersist();
        ScenePersist scenePersist = FindAnyObjectByType<ScenePersist>();
        scenePersist?.ResetScenePersist();
        StartCoroutine(DelayandReset());
    }

    IEnumerator DelayandReset()// add a delay to make sure the scene is loaded before resetting the game session
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

}
