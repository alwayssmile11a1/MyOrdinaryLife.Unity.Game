using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {

    public Color starActiveColor;
    public Text text;

    private  List<Image> stars = new List<Image>();

    private void Awake()
    {

        GetComponentsInChildren<Image>(stars);
        stars.RemoveAt(0);

        SavedData savedData = new SavedData();

        if(savedData.Load(gameObject.name))
        {
            for (int i = 0; i < savedData.count; i++)
            {
              
                stars[i].color = starActiveColor;
            }

            UIManager.Instance.AddToTotalStars(savedData.count);

        }

    }

    public void LoadScene()
    {
        string activeScene = SceneManager.GetActiveScene().name;
        string sceneName = $"Level{activeScene[activeScene.Length-1]}-{text.text}";

        StartCoroutine(GameManager.Instance.LoadLevel(sceneName));
    }

}
