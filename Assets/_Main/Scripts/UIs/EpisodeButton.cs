using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EpisodeButton : MonoBehaviour
{
    public Text episodeNumberText;
    public Text starsCountText;

    private void Awake()
    {
        //SavedData savedData = new SavedData();

        //if (savedData.Load(gameObject.name))
        //{
        //    for (int i = 0; i < savedData.count; i++)
        //    {

        //        stars[i].color = starActiveColor;
        //    }

        //    UIManager.Instance.AddToTotalStars(savedData.count);

        //}

    }

    public void LoadScene()
    {
        //It would be more convenient to instatiate LevelButtons at run time depending on which episode users choose, but it might affect runtime performance 
        //Even though making one MenuSelectLevel for each episode could be repetitive and tedious, for performance purpose, we do it anyway.
        string sceneName = "MenuSelectLevel" + episodeNumberText.text;
        StartCoroutine(GameManager.Instance.LoadScene(sceneName));
    }

}
