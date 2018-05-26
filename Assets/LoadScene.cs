﻿using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
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
        Menu
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
            case LoadType.Menu: LoadMenuScene(); break;
        }
    }
    private void LoadTextScene()
    {
        SceneManager.LoadSceneAsync(gameObject.GetComponentInChildren<Text>().text);
    }
    private void LoadMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}