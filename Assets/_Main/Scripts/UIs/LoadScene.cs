using UnityEngine;

public class LoadScene : MonoBehaviour {

    public void LoadSelectLevelScene()
    {
        StartCoroutine(GameManager.Instance.LoadScene("MenuSelectLevel"));
    }

    public void LoadMenuScene()
    {
        StartCoroutine(GameManager.Instance.LoadScene("MenuScene"));
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
