using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour {

    ScreenFader screenFader;
    //public enum LoadType
    //{
    //    ButtonText,
    //    NextScene,
    //    Restart,
    //    LevelSelect,
    //    Menu,
    //    PauseGame
    //}
    //public LoadType loadType;
    private void Awake()
    {
        screenFader = ScreenFader.Instance;
    }
    //public void LoadNewScene()
    //{
    //    screenFader.StartFadeSceneOut();
        
    //    StartCoroutine(Load());
    //}

    //IEnumerator Load()
    //{
    //    yield return new WaitForSeconds(screenFader.fadeDuration);

    //    //switch (loadType)
    //    //{
    //    //    case LoadType.ButtonText: LoadTextScene(); break;
    //    //    case LoadType.NextScene: LoadNextScene(); break;
    //    //    case LoadType.Restart: RestartScene(); break;
    //    //    case LoadType.LevelSelect: LoadSelectLevelScene(); break;
    //    //    case LoadType.Menu: LoadMenuScene(); break;
    //    //}

    //    screenFader.FadeSceneInUnscale();

    //}

    public void LoadSelectLevelScene()
    {
        GameManager.Instance.ResetGameState();
        UIManager.Instance.TurnOff();
        SceneManager.LoadScene("MenuSelectLevel");
    }

    public void LoadMenuScene()
    {
        GameManager.Instance.ResetGameState();
        UIManager.Instance.TurnOff();
        SceneManager.LoadScene("MenuScene");
    }

    public void RestartScene()
    {
        GameManager.Instance.RestartLevel();
    }

    public void LoadNextScene()
    {
        GameManager.Instance.LoadNextLevel();
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
