using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

    public void LoadSelectLevelScene()
    {
        string activeScene = SceneManager.GetActiveScene().name;
        StartCoroutine(GameManager.Instance.LoadScene($"MenuSelectLevel{activeScene[5]}"));
    }

    public void LoadMenuScene()
    {
        StartCoroutine(GameManager.Instance.LoadScene("MenuScene"));
    }

    public void LoadSelectEpisodeScene()
    {
        StartCoroutine(GameManager.Instance.LoadScene("MenuSelectEpisode"));
    }

    public void RestartScene()
    {
        StartCoroutine(GameManager.Instance.RestartLevel());
    }

    public void LoadNextScene()
    {
        StartCoroutine(GameManager.Instance.LoadNextLevel());
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void PauseGame()
    {
        TimeManager.SlowdownTime(0, -1);
    }

    public void ResumeGame()
    {
        TimeManager.ChangeTimeBackToNormal();
    }
    

}
