using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager Instance
    {
        get
        {
            if (s_Instance != null)
                return s_Instance;

            s_Instance = FindObjectOfType<UIManager>();

            if (s_Instance != null)
                return s_Instance;

            UIManager gameManagerPrefab = Resources.Load<UIManager>("UIManager");
            s_Instance = Instantiate(gameManagerPrefab);

            return s_Instance;
        }
    }

    protected static UIManager s_Instance;
    #endregion

    public GameObject endLevelCanvas;
    public GameObject pauseGameCanvas;
    public GameObject pauseGamePanel;
    public GameObject[] stars;

    private int m_TotalStarsCount = 0;
    private int m_CurrentTotalScore = 0;
    private int m_PlayerLife;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        if (PlayerPrefs.HasKey("playerLife"))
        {
            m_PlayerLife = PlayerPrefs.GetInt("playerLife");
        }
        else
        {
            PlayerPrefs.SetInt("playerLife", 30);
            m_PlayerLife = 30;
        }
    }


    public void ResetUIs()
    {
        endLevelCanvas.SetActive(false);
        pauseGameCanvas.SetActive(true);
        pauseGamePanel.SetActive(false);
        m_TotalStarsCount = 0;
        m_CurrentTotalScore = 0;
        for (int i = 0; i < 3; i++)
        {
            stars[i].gameObject.SetActive(false);
        }
    }

    public void TurnOff()
    {
        endLevelCanvas.SetActive(false);
        pauseGameCanvas.SetActive(false);
    }

    public void FinishLevel()
    {
        endLevelCanvas.SetActive(true);
        for (int i = 0; i < GameManager.Instance.GetStarsCount(); i++)
        {
            stars[i].gameObject.SetActive(true);
        }
    }

    public void AddToCurrentTotalScore(int count)
    {
        m_CurrentTotalScore += count;
    }

    public int GetCurrentTotalScore()
    {
        return m_CurrentTotalScore;
    }

    public void AddToTotalStars(int count)
    {
        m_TotalStarsCount += count;
    }

    public int GetTotalStars()
    {
        return m_TotalStarsCount;
    }

    public int GetPlayerLife()
    {
        return m_PlayerLife;
    }
    /// <summary>
    /// Update playerLife variable
    /// </summary>
    /// <param name="minus">if true subtract playerLife by 1, if false plus playerLife by 1</param>
    public void UpdatePlayerLife(bool minus)
    {
        m_PlayerLife = minus ? --m_PlayerLife : ++m_PlayerLife;
        PlayerPrefs.SetInt("playerLife", m_PlayerLife);
    }
}
