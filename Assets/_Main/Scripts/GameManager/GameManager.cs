using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    #region Singleton
    public static GameManager Instance
    {
        get
        {
            if (s_Instance != null)
                return s_Instance;

            s_Instance = FindObjectOfType<GameManager>();

            if (s_Instance != null)
                return s_Instance;

            GameManager gameManagerPrefab = Resources.Load<GameManager>("GameManager");
            s_Instance = Instantiate(gameManagerPrefab);

            return s_Instance;
        }
    }

    protected static GameManager s_Instance;
    #endregion


    public float fadeDuration = 0.5f;

    private int m_StarsCount = 0;
    private IngameFadeCanvas m_IngameFadeCanvas;
    private WaitForSeconds m_FadeDurationSeconds;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        m_FadeDurationSeconds = new WaitForSeconds(fadeDuration);
    }

    public void AddOneStar()
    {
        m_StarsCount++;
    }

    public int GetStarsCount()
    {
        return m_StarsCount;
    }
    
    public void FinishLevel()
    {
        UIManager.Instance.FinishLevel();

        //save progress
        int startCount = m_StarsCount;
        SavedData savedData = new SavedData();
        if (savedData.Load(SceneManager.GetActiveScene().name))
        {
            if (savedData.count < startCount)
            {
                savedData.count = startCount;
                savedData.Save(SceneManager.GetActiveScene().name);
            }
        }
        else
        {
            savedData.count = startCount;
            savedData.Save(SceneManager.GetActiveScene().name);
        }

        m_StarsCount = 0;
    }

    public IEnumerator RestartLevel()
    {
        FadeSceneOut();
        yield return m_FadeDurationSeconds;
        ResetGameState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public IEnumerator LoadNextLevel()
    {
        FadeSceneOut();
        yield return m_FadeDurationSeconds;
        if (Application.CanStreamedLevelBeLoaded(SceneManager.GetActiveScene().buildIndex + 1))
        {
            ResetGameState();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    //Used to load level
    public IEnumerator LoadLevel(string name)
    {
        FadeSceneOut();
        yield return m_FadeDurationSeconds;
        if (Application.CanStreamedLevelBeLoaded(name))
        {
            ResetGameState();
            SceneManager.LoadScene(name);
        }
    }

    //Used to load other scenes
    public IEnumerator LoadScene(string name)
    {
        FadeSceneOut();
        yield return m_FadeDurationSeconds;
        ResetGameState();
        UIManager.Instance.TurnOff();
        SceneManager.LoadScene(name);
    }

    private void ResetGameState()
    {
        m_StarsCount = 0;
        UIManager.Instance.ResetUIs();
    }

    private void FadeSceneOut()
    {
        //Change time back to normal so coroutine can work normally
        TimeManager.ChangeTimeBackToNormal();
        if (m_IngameFadeCanvas != null)
            m_IngameFadeCanvas.FadeSceneOut();

    }

    public void RegisterIngameFadeCanvas(IngameFadeCanvas ingameFadeCanvas)
    {
        m_IngameFadeCanvas = ingameFadeCanvas;
    }
    
}
