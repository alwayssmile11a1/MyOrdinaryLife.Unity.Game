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

    private void LoadStar()
    {
        SavedData savedData = new SavedData();

        if (savedData.Load(m_SceneName))
        {
            for (int i = 0; i < savedData.count; i++)
            {

                stars[i].color = starActiveColor;
            }

            UIManager.Instance.AddToCurrentTotalScore(savedData.count);

        }
        UIManager.Instance.AddToTotalStars(stars.Length);
    }

    private void SetSceneName(char episodeIndex, int levelIndex)
    {
        m_SceneName = $"Level{episodeIndex}-{levelIndex}";
    }

    public void SetSceneNameAndLoadStar(char episodeIndex, int levelIndex)
    {
        SetSceneName(episodeIndex, levelIndex);

        LoadStar();
    }

    public void LoadScene()
    {
        StartCoroutine(GameManager.Instance.LoadLevel(m_SceneName));
    }

}
