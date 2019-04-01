using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EpisodeButton : MonoBehaviour
{
    public Text episodeNumberText;
    public Text starsCountText;
    public int unlockNumber = 0;

    private void Awake()
    {
        SavedData savedData = new SavedData();

        if (savedData.Load("Episode" + episodeNumberText.text))
        {
            UIManager.Instance.AddToCurrentTotalScore(savedData.count);
            starsCountText.text = $"* {savedData.count}";
        }
        else
        {
            starsCountText.text = $"* {0}";
        }
        GetComponent<Button>().interactable = UIManager.Instance.GetCurrentTotalScore() >= unlockNumber ? true : false;
    }

    public void LoadScene()
    {
        //It would be more convenient to instatiate LevelButtons at run time depending on which episode users choose, but it might affect runtime performance 
        //Even though making one MenuSelectLevel for each episode could be repetitive and tedious, for performance purpose, we do it anyway.
        string sceneName = "MenuSelectLevel" + episodeNumberText.text;
        StartCoroutine(GameManager.Instance.LoadScene(sceneName));
    }

}
