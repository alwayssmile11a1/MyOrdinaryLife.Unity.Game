using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButtonGenerator : MonoBehaviour
{
    public      GameObject      buttonPrefab;
    public      float           buttonSize;
    public      Transform       content;
    public      Sprite          buttonBackgroundSprite;

    // Start is called before the first frame update
    void Awake()
    {
        string[] pathSplit = SceneManager.GetActiveScene().path.Split('/');
        string folderName = pathSplit[pathSplit.Length - 2];
        DirectoryInfo directoryInfo = new DirectoryInfo($"Assets/_Main/_Scenes/{folderName}");
        FileInfo[] arrFiles = directoryInfo.GetFiles("Level*.unity");
        
        string activeScene = SceneManager.GetActiveScene().name;
        char episodeIndex = activeScene[activeScene.Length - 1];

        for (int i = 0; i < arrFiles.Length; i++)
        {
            GameObject button = Instantiate(buttonPrefab, content);
            button.GetComponent<Image>().sprite = buttonBackgroundSprite;
            button.name = $"Level{i + 1}";

            LayoutElement layoutElement = button.AddComponent<LayoutElement>();
            layoutElement.minWidth = layoutElement.minHeight = buttonSize;

            LevelButton levelButton = button.GetComponent<LevelButton>();
            levelButton.text.text = (i + 1).ToString();
            levelButton.SetSceneNameAndLoadStar(episodeIndex, i + 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
