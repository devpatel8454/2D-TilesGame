using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public void OnRestartGame()
    {
        Debug.Log(SceneManager.GetActiveScene().name);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
