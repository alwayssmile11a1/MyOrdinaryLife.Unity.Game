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

    private int m_StarsCount = 0;


    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
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

    public void RestartLevel()
    {
        ResetGameState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        ResetGameState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadLevel(string name)
    {
        if (Application.CanStreamedLevelBeLoaded(name))
        {
            ResetGameState();
            SceneManager.LoadScene(name);
        }
    }

    public void ResetGameState()
    {
        m_StarsCount = 0;
        TimeManager.ChangeTimeBackToNormal();
        UIManager.Instance.ResetUIs();
    }

}
