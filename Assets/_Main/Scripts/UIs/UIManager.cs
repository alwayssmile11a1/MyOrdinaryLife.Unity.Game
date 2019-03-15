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

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }


    public void ResetUIs()
    {
        endLevelCanvas.SetActive(false);
        pauseGameCanvas.SetActive(true);
        pauseGamePanel.SetActive(false);
        m_TotalStarsCount = 0;
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

    public void AddToTotalStars(int count)
    {
        m_TotalStarsCount += count;
    }

    public int GetTotalStars()
    {
        return m_TotalStarsCount;
    }

}
