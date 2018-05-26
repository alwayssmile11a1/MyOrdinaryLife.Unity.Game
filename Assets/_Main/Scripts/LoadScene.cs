using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour {
    ScreenFader screenFader;
    public enum LoadType
    {
        ButtonText,
        NextScene,
        Restart,
        LevelSelect,
        Menu,
        PauseGame
    }
    public LoadType loadType;
    private void Awake()
    {
        screenFader = ScreenFader.Instance;
    }
    public void LoadNewScene()
    {
        screenFader.StartFadeSceneOut();
        
        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
        yield return new WaitForSeconds(screenFader.fadeDuration);
        screenFader.StartFadeSceneIn();
        switch (loadType)
        {
            case LoadType.ButtonText: LoadTextScene(); break;
            case LoadType.NextScene: LoadNextScene(); break;
            case LoadType.Restart: RestartScene(); break;
            case LoadType.LevelSelect: LoadSelectLevelScene(); break;
            case LoadType.Menu: LoadMenuScene(); break;
        }
    }
    private void LoadTextScene()
    {
        string sceneName = "Level " + gameObject.GetComponentInChildren<TextMeshProUGUI>().text.Trim();
        SceneManager.LoadSceneAsync(sceneName);
    }
    private void LoadSelectLevelScene()
    {
        SceneManager.LoadScene("MenuSelectLevel");
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartSceneNoDelay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMenuSceneNoDelay()
    {
        SceneManager.LoadScene("MenuScene");
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    private void LoadMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }
    public void ExitGame()
    {
        Application.Quit();
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
