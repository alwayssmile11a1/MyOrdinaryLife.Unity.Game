using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;
using File = UnityEngine.Windows.File;

public class LevelEditor : EditorWindow
{
    bool editExistingLevel = false;
    bool deleteScene = false;
    float buttonWidth = 200;
    float buttonHeight = 25;
    float dropdownWidth = 200;
    float dropdownHeight = 25;
    float labelWidth = 80;
    string openScene = "1";
    int space = 10;
    Vector2 scrollPosition = Vector2.zero;
    static Texture2D screenCapture;

    string test;
    int popupIndex = 0;
    int folderIndex = 0;
    int newSceneIndex;

    LevelEditorSO levelEditorSO;

    [MenuItem("Tool/Level Editor")]
    static void OpenLevelEditor()
    {
        LevelEditor levelEditor = GetWindow<LevelEditor>();
        EditorSceneManager.sceneOpened += SceneOpened;
        screenCapture = AssetDatabase.LoadAssetAtPath($"Assets/_Main/Editor/ScreenShots/SkyLand/Level1.png", typeof(Texture2D)) as Texture2D;
    }

    private static void SceneOpened(Scene scene, OpenSceneMode mode)
    {
        string[] strSplit = EditorSceneManager.GetActiveScene().path.Split('/');
        string sceneName = strSplit[strSplit.Length - 1].Split('.')[0];
        string folder = strSplit[strSplit.Length - 2];
        string path = $"Assets/_Main/Editor/ScreenShots/{folder}/{sceneName}.png";
        if (File.Exists(path))
        {
            screenCapture = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;
        }
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

        GUI.enabled = !editExistingLevel;
        if (GUILayout.Button("Create new level", GUILayout.Height(buttonHeight), GUILayout.Width(buttonWidth)))
        {
            newSceneIndex = FindAppropriateIndex();

            if (EditorUtility.DisplayDialog("Create new scene", "Do you want to create new scene? Unsaved changes in this scene will be discarded.", "Yes", "No"))
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
        folderIndex = EditorGUILayout.Popup(folderIndex, levelEditorSO.sceneFolderName, GUILayout.Height(dropdownHeight), GUILayout.Width(dropdownWidth));
        GUILayout.EndHorizontal();

        GUI.enabled = true;

        #region Edit existing level
        // Toggle (checkbox)
        //editExistingLevel = EditorGUILayout.Toggle("Edit existing level", editExistingLevel, GUILayout.Width(buttonWidth));


        #region Textfield && Arrows
        EditorGUILayout.BeginHorizontal();
        GUIStyle gUIStyleButton = new GUIStyle(GUI.skin.button);
        gUIStyleButton.fontSize = 20;

        // Left arrow
        if (GUILayout.Button("↤", gUIStyleButton, GUILayout.Height(buttonHeight), GUILayout.Width(50)))
        {
            int newNumber = (int.Parse(openScene) - 1);
            if (newNumber >= 1)
            {
                openScene = newNumber.ToString();
                ShowYesNoPopup();
            }
        }

        // Textfield
        GUIStyle gUIStyle = new GUIStyle(GUI.skin.textField);
        gUIStyle.alignment = TextAnchor.MiddleLeft;
        openScene = EditorGUILayout.TextField(openScene, gUIStyle, GUILayout.Height(buttonHeight), GUILayout.Width(100));

        // Right arrow
        if (GUILayout.Button("↦", gUIStyleButton, GUILayout.Height(buttonHeight), GUILayout.Width(50)))
        {
            int newNumber = (int.Parse(openScene) + 1);
            openScene = newNumber.ToString();
            ShowYesNoPopup();
        }
        EditorGUILayout.EndHorizontal();
        #endregion

        GUILayout.Space(space / 2);
        if (GUILayout.Button($"Open Level {openScene}", GUILayout.Height(buttonHeight), GUILayout.Width(buttonWidth)))
        {
            if (int.TryParse(openScene, out int i))
            {
                ShowYesNoPopup();
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
                if (EditorUtility.DisplayDialog($"Delete Level{openScene} scene", $"Do you want to delete {sceneName} scene?", "Yes", "No"))
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

        GUI.enabled = true;
        GUILayout.Space(space / 2);
        GUILayout.Label(screenCapture, GUILayout.Width(Screen.width - 10), GUILayout.Height(300));
        GUILayout.EndScrollView();
        #endregion end ScrollView
    }

    private void ShowYesNoPopup()
    {
        if (EditorSceneManager.GetActiveScene().path.Equals($"Assets/_Main/_Scenes/{levelEditorSO.sceneFolderName[folderIndex]}/Level{folderIndex + 1}-{openScene}")) return;
        if (EditorUtility.DisplayDialog($"Open Level{openScene} scene", $"Do you want to open Level{openScene} scene? Unsaved changes in this scene will be discarded.", "Yes", "No"))
        {
            try
            {
                EditorSceneManager.OpenScene($"Assets/_Main/_Scenes/{levelEditorSO.sceneFolderName[folderIndex]}/Level{folderIndex + 1}-{openScene}.unity");
            }
            catch(Exception e)
            {
                EditorUtility.DisplayDialog("File not found", $"Level{openScene} does not exist in folder Assets/_Main/_Scenes/{levelEditorSO.sceneFolderName[folderIndex]}/", "OK");
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