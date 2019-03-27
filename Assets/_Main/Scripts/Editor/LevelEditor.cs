#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;
using File = UnityEngine.Windows.File;

public class LevelEditor : EditorWindow
{
    bool                    deleteScene         = false;
    float                   buttonWidth         = 200;
    float                   buttonHeight        = 25;
    float                   dropdownWidth       = 200;
    float                   dropdownHeight      = 25;
    float                   labelWidth          = 80;
    string                  textFieldNumber     = "1";
    int                     space               = 10;
    int                     popupIndex          = 0;
    int                     folderIndex         = 0;
    Vector2                 scrollPosition      = Vector2.zero;

    int                     newSceneIndex;
    static Texture2D        imageOfLevel;
    bool                    imageExist;
    LevelEditorSO           levelEditorSO;

    /// <summary>
    /// not using any more
    /// </summary>
    static bool startTimeCount;
    float timeCount = 0;
    EditorWindow gameView;
    ////////////////////////////////
    
    static LevelEditor()
    {
        //  EditorSceneManager.sceneOpened += SceneOpened;
    }

    [MenuItem("Tool/Level Editor %#O")]
    static void OpenLevelEditor()
    {
        LevelEditor levelEditor = GetWindow<LevelEditor>();
        imageOfLevel = new Texture2D(Screen.width - 25, 300);
    }

    private void OnEnable()
    {
        levelEditorSO = AssetDatabase.LoadAssetAtPath("Assets/_Main/Editor/SceneNameScriptableObject.asset", typeof(LevelEditorSO)) as LevelEditorSO;
    }

    private void OnGUI()
    {
        #region ScrollView
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        GUILayout.Space(space);
        
        if (GUILayout.Button("Create new level", GUILayout.Height(buttonHeight), GUILayout.Width(buttonWidth)))
        {
            newSceneIndex = FindAppropriateIndex();

            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                // Create new scene
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

                // Add new scene to build settings
                AddNewSceneToBuildSetting(newSceneIndex);

                // Add prefabs to new scene
                AddPrefabsToNewScene();

                // Add background to new scene
                AddBackgroundToNewScene();

                // Save scene after adding everything
                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), $"Assets/_Main/_Scenes/{levelEditorSO.sceneFolderName[folderIndex]}/Level{folderIndex + 1}-{newSceneIndex}.unity");
            }
            return;
        }

        GUILayout.Space(space);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Frame type", GUILayout.Width(labelWidth));
        // Popup (it's a dropdown)
        popupIndex = EditorGUILayout.Popup(popupIndex, levelEditorSO.frameType, GUILayout.Height(dropdownHeight), GUILayout.Width(dropdownWidth));
        GUILayout.EndHorizontal();

        GUILayout.Space(space);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Scene Folder", GUILayout.Width(labelWidth));
        GUI.SetNextControlName("dropdownFolderName");
        folderIndex = EditorGUILayout.Popup(folderIndex, levelEditorSO.sceneFolderName, GUILayout.Height(dropdownHeight), GUILayout.Width(dropdownWidth));
        GUILayout.EndHorizontal();

        GUI.enabled = true;

        #region Edit existing level


        #region Textfield && Arrows
        EditorGUILayout.BeginHorizontal();
        GUIStyle gUIStyleButton = new GUIStyle(GUI.skin.button);
        gUIStyleButton.fontSize = 20;

        // Left arrow
        if (GUILayout.Button("↤", gUIStyleButton, GUILayout.Height(buttonHeight), GUILayout.Width(50)))
        {
            int newNumber = (int.Parse(textFieldNumber) - 1);
            if (newNumber >= 1)
            {
                textFieldNumber = newNumber.ToString();
                //ShowYesNoPopup();
                ChangeImage();
                GUI.FocusControl("dropdownFolderName");
            }
        }

        // Textfield
        GUIStyle gUIStyle = new GUIStyle(GUI.skin.textField);
        gUIStyle.alignment = TextAnchor.MiddleLeft;
        textFieldNumber = EditorGUILayout.TextField(textFieldNumber, gUIStyle, GUILayout.Height(buttonHeight), GUILayout.Width(100));

        // Right arrow
        if (GUILayout.Button("↦", gUIStyleButton, GUILayout.Height(buttonHeight), GUILayout.Width(50)))
        {
            int newNumber = (int.Parse(textFieldNumber) + 1);
            textFieldNumber = newNumber.ToString();
            //ShowYesNoPopup();
            ChangeImage();
            GUI.FocusControl("dropdownFolderName");
        }
        EditorGUILayout.EndHorizontal();
        #endregion
        
        GUILayout.Space(space / 2);

        
        if (GUILayout.Button($"Open Level {textFieldNumber}", GUILayout.Height(buttonHeight), GUILayout.Width(buttonWidth)))
        {
            if (int.TryParse(textFieldNumber, out int i))
            {
                ShowUnSavePopup();
                ChangeImage();
            }
        }
        deleteScene = EditorGUILayout.BeginToggleGroup("Delete current scene", deleteScene);
        GUILayout.Space(space / 2);
        if (GUILayout.Button("Delete", GUILayout.Height(buttonHeight), GUILayout.Width(buttonWidth)))
        {
            string sceneName = EditorSceneManager.GetActiveScene().name;
            string filePath = $"Assets/_Main/_Scenes/{levelEditorSO.sceneFolderName[folderIndex]}/{sceneName}.unity";
            if (File.Exists(filePath))
            {
                if (EditorUtility.DisplayDialog($"Delete Level{textFieldNumber} scene", $"Do you want to delete {sceneName} scene?", "Yes", "No"))
                {
                    File.Delete(filePath);
#if UNITY_EDITOR
                    AssetDatabase.Refresh();
#endif

                    EditorBuildSettingsScene[] originalSettingScenes = EditorBuildSettings.scenes;
                    EditorBuildSettingsScene sceneToRemove = new EditorBuildSettingsScene($"Assets/_Main/_Scenes/{levelEditorSO.sceneFolderName[folderIndex]}/{sceneName}.unity", true);
                    EditorBuildSettingsScene[] newSettings = new EditorBuildSettingsScene[originalSettingScenes.Length - 1];
                    for (int i = 0, j = 0; i < originalSettingScenes.Length; i++)
                    {
                        if (originalSettingScenes[i].path != sceneToRemove.path)
                        {
                            newSettings[j++] = originalSettingScenes[i];
                        }
                    }
                    EditorBuildSettings.scenes = newSettings;

                    EditorSceneManager.OpenScene(EditorBuildSettings.scenes[EditorBuildSettings.scenes.Length - 1].path);

                    EditorUtility.DisplayDialog("Message", $"{sceneName} has been deleted from project folder and build settings", "OK");
                }
            }
        }
        EditorGUILayout.EndToggleGroup();
        #endregion

        #region Intro, Menu, Menu Select Scene
        int minus = 30;
        GUILayout.Space(space);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button($"Open {levelEditorSO.sceneFolderName[folderIndex]} Select Level", GUILayout.Width(buttonWidth - minus), GUILayout.Height(buttonHeight)))
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                try
                {
                    EditorSceneManager.OpenScene($"Assets/_Main/_Scenes/{levelEditorSO.sceneFolderName[folderIndex]}/MenuSelectLevel{folderIndex + 1}.unity");
                }
                catch (Exception)
                {
                    EditorUtility.DisplayDialog("File not found", $"Can not find MenuSelectLevel{folderIndex + 1} in folder Assets/_Main/_Scenes/{levelEditorSO.sceneFolderName[folderIndex]}", "OK");
                }
            }
        }
        
        if (GUILayout.Button("Open Intro Scene", GUILayout.Width(buttonWidth -minus), GUILayout.Height(buttonHeight)))
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                try
                {
                    EditorSceneManager.OpenScene("Assets/_Main/_Scenes/IntroScene.unity");
                }
                catch (Exception)
                {
                    EditorUtility.DisplayDialog("File not found", "Can not find IntroScene in folder Assets/_Main/_Scenes/", "OK");
                }
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(space);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Open Menu Scene", GUILayout.Width(buttonWidth -minus), GUILayout.Height(buttonHeight)))
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                try
                {
                    EditorSceneManager.OpenScene("Assets/_Main/_Scenes/MenuScene.unity");
                }
                catch (Exception)
                {
                    EditorUtility.DisplayDialog("File not found", "Can not find MenuScene in folder Assets/_Main/_Scenes/", "OK");
                }
            }
        }
        
        if (GUILayout.Button("Open Menu Select Episode", GUILayout.Width(buttonWidth - minus), GUILayout.Height(buttonHeight)))
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                try
                {
                    EditorSceneManager.OpenScene("Assets/_Main/_Scenes/MenuSelectEpisode.unity");
                }
                catch (Exception)
                {
                    EditorUtility.DisplayDialog("File not found", "Can not find MenuSelectEpisode in folder Assets/_Main/_Scenes/", "OK");
                }
            }
        }
        GUILayout.EndHorizontal();
        #endregion

        #region Scene Image
        GUI.enabled = true;
        GUILayout.Space(space / 2);
        if (!imageExist)
        {
            GUILayout.Label("Image does not exists in folder");
        }
        else
        {
            GUILayout.Label(imageOfLevel, GUILayout.Width(Screen.width - 25));
        }
        #endregion

        GUILayout.EndScrollView();
        #endregion end ScrollView
    }

    #region Capture a specific position on screen
    private void CaptureGameView()
    {
        if (gameView == null)
        {
            var assembly = typeof(EditorWindow).Assembly;
            var type = assembly.GetType("UnityEditor.GameView");
            gameView = EditorWindow.GetWindow(type);
        }

        // Get screen position and sizes
        var vec2Position = new Vector2(gameView.position.position.x, gameView.position.position.y + 40);
        var sizeX = gameView.position.width;
        var sizeY = gameView.position.height - 20;

        // Take Screenshot at given position sizes
        var colors = InternalEditorUtility.ReadScreenPixel(vec2Position, (int)sizeX, (int)sizeY);

        imageOfLevel = new Texture2D((int)sizeX, (int)sizeY);
        imageOfLevel.SetPixels(colors);
        imageOfLevel.Apply();

        timeCount = 0;
        startTimeCount = false;

        Repaint();
    }
    #endregion

    private void ChangeImage()
    {
        string imagePath = $"Assets/_Main/Editor/ScreenShots/{levelEditorSO.sceneFolderName[folderIndex]}/Level{folderIndex + 1}-{textFieldNumber}.png";
        if (File.Exists(imagePath))
        {
            imageOfLevel = new Texture2D(Screen.width - 25, 300);
            imageOfLevel.LoadImage(File.ReadAllBytes(imagePath));
            imageExist = true;
        }
        else
        {
            imageExist = false;
        }
    }

    private void ShowUnSavePopup()
    {
        if (EditorSceneManager.GetActiveScene().path.Equals($"Assets/_Main/_Scenes/{levelEditorSO.sceneFolderName[folderIndex]}/Level{folderIndex + 1}-{textFieldNumber}")) return;

        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            try
            {
                EditorSceneManager.OpenScene($"Assets/_Main/_Scenes/{levelEditorSO.sceneFolderName[folderIndex]}/Level{folderIndex + 1}-{textFieldNumber}.unity");
            }
            catch (Exception)
            {
                EditorUtility.DisplayDialog("File not found", $"Level{textFieldNumber} does not exist in folder Assets/_Main/_Scenes/{levelEditorSO.sceneFolderName[folderIndex]}/", "OK");
            }
        }
    }

    private static void AddBackgroundToNewScene()
    {
        GameObject environment = new GameObject("Environment", typeof(Parallax));
        GameObject backgroundImage = new GameObject("Background_Gradient", typeof(SpriteRenderer));
        backgroundImage.transform.SetParent(environment.transform);
        backgroundImage.GetComponent<SpriteRenderer>().sortingOrder = -100;
        backgroundImage.transform.localScale = new Vector3(10000, 10, 1);
        Texture2D texture2D = AssetDatabase.LoadAssetAtPath($"Assets/_Main/Art/Background/Background_Gradient.png", typeof(Texture2D)) as Texture2D;
        backgroundImage.GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
    }

    private void AddPrefabsToNewScene()
    {
        //GameObject sharedSceneObject = AssetDatabase.LoadAssetAtPath($"Assets/_Main/Prefabs/SharedSceneObject.prefab", typeof(GameObject)) as GameObject;
        //GameObject mainCharacter = AssetDatabase.LoadAssetAtPath($"Assets/_Main/Prefabs/MainCharacter/MainCharacter.prefab", typeof(GameObject)) as GameObject;
        //GameObject gameManager = AssetDatabase.LoadAssetAtPath($"Assets/_Main/Resources/GameManager.prefab", typeof(GameObject)) as GameObject;
        //GameObject uiManager = AssetDatabase.LoadAssetAtPath($"Assets/_Main/Resources/UIManager.prefab", typeof(GameObject)) as GameObject;
        //PrefabUtility.InstantiatePrefab(sharedSceneObject);
        //PrefabUtility.InstantiatePrefab(gameManager);
        //PrefabUtility.InstantiatePrefab(uiManager);
        //PrefabUtility.InstantiatePrefab(mainCharacter);

        foreach (GameObject prefab in levelEditorSO.prefabs)
        {
            PrefabUtility.InstantiatePrefab(prefab);
        }

        GameObject layoutCanvas = AssetDatabase.LoadAssetAtPath($"Assets/_Main/Prefabs/Frames/LayoutCanvas{levelEditorSO.frameType[popupIndex]}.prefab", typeof(GameObject)) as GameObject;
        PrefabUtility.InstantiatePrefab(layoutCanvas);
    }

    private void AddNewSceneToBuildSetting(int newIndex)
    {
        EditorBuildSettingsScene[] originalSettingScenes = EditorBuildSettings.scenes;
        EditorBuildSettingsScene sceneToAdd = new EditorBuildSettingsScene($"Assets/_Main/_Scenes/{levelEditorSO.sceneFolderName[folderIndex]}/Level{folderIndex + 1}-{newIndex}.unity", true);
        EditorBuildSettingsScene[] newSettings = new EditorBuildSettingsScene[originalSettingScenes.Length + 1];
        System.Array.Copy(originalSettingScenes, newSettings, originalSettingScenes.Length);
        newSettings[newSettings.Length - 1] = sceneToAdd;
        EditorBuildSettings.scenes = newSettings;
    }

    private int FindAppropriateIndex()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo($"Assets/_Main/_Scenes/{levelEditorSO.sceneFolderName[folderIndex]}");
        FileInfo[] arrFiles = directoryInfo.GetFiles("Level*.unity");
        int j = 1;
        List<int> listIndex = new List<int>();

        for (int i = 0; i < arrFiles.Length; i++)
        {
            string name = arrFiles[i].Name;
            listIndex.Add(int.Parse(arrFiles[i].Name.Split('.')[0].Split('-')[1]));
        }
        listIndex.Sort();
        for (int i = 0; i < listIndex.Count; i++, j++)
        {
            if (listIndex[i] == j)
            {
                continue;
            }
            else
            {
                return j;
            }
        }
        return j;
    }
}

#endif