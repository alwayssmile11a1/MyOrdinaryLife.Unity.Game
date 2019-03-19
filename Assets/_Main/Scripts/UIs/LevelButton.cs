using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {

    public Color starActiveColor;
    public Text text;
    public Image[] stars;

    private string m_SceneName;

    private void Awake()
    {
        string activeScene = SceneManager.GetActiveScene().name;
        m_SceneName = $"Level{activeScene[activeScene.Length - 1]}-{text.text}";

        SavedData savedData = new SavedData();

        if(savedData.Load(m_SceneName))
        {
            for (int i = 0; i < savedData.count; i++)
            {
              
                stars[i].color = starActiveColor;
            }

            UIManager.Instance.AddToCurrentTotalScore(savedData.count);

        }
        UIManager.Instance.AddToTotalStars(stars.Length);
    }

    public void LoadScene()
    {
        StartCoroutine(GameManager.Instance.LoadLevel(m_SceneName));
    }

}
