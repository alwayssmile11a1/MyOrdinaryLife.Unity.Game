using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class LevelEditor : EditorWindow
{
    bool editExistingLevel = false;
    float buttonWidth = 200;
    float buttonHeight = 25;
    string openScene = string.Empty;
    int space = 10;

    string[] options = new string[] { "1x3", "2x2", "2x3" };
    int popupIndex = 0;
    
    int newSceneIndex;

    [MenuItem("Tool/Level Editor")]
    static void OpenLevelEditor()
    {
        GetWindow<LevelEditor>();
    }

    private void OnGUI()
    {
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
                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), $"Assets/_Main/_Scenes/Levels/Level{newSceneIndex}.unity");
            }
        }

        GUILayout.Space(space);
        // Popup (it's a dropdown)
        popupIndex = EditorGUILayout.Popup(popupIndex, options, GUILayout.Height(buttonHeight), GUILayout.Width(buttonWidth));

        GUI.enabled = true;

        // Toggle (checkbox)
        editExistingLevel = EditorGUILayout.Toggle("Edit existing level", editExistingLevel, GUILayout.Width(buttonWidth));

        GUI.enabled = editExistingLevel;

        #region Textfield && Arrows
        EditorGUILayout.BeginHorizontal();
        GUIStyle gUIStyleButton = new GUIStyle(GUI.skin.button);
        gUIStyleButton.fontSize = 20;
        // Left arrow
        if (GUILayout.Button("↤", gUIStyleButton, GUILayout.Height(buttonHeight), GUILayout.Width(50)))
        {
            int newNumber = (int.Parse(openScene) - 1);
            if(newNumber >= 1)
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

        GUILayout.Space(space / 2);
        if (GUILayout.Button("Delete", GUILayout.Height(buttonHeight), GUILayout.Width(buttonWidth)))
        {
            string filePath = $"Assets/_Main/_Scenes/Levels/Level{openScene}.unity";
            if (File.Exists(filePath))
            {
                if (EditorUtility.DisplayDialog($"Delete Level{openScene} scene", $"Do you want to delete Level{openScene} scene?", "Yes", "No"))
                {
                    File.Delete(filePath);
                    #if UNITY_EDITOR
                        AssetDatabase.Refresh();
                    #endif

                    EditorBuildSettingsScene[] originalSettingScenes = EditorBuildSettings.scenes;
                    EditorBuildSettingsScene sceneToRemove = new EditorBuildSettingsScene($"Assets/_Main/_Scenes/Levels/Level{openScene}.unity", true);
                    EditorBuildSettingsScene[] newSettings = new EditorBuildSettingsScene[originalSettingScenes.Length - 1];
                    for (int i = 0, j = 0; i < originalSettingScenes.Length; i++)
                    {
                        if(originalSettingScenes[i].path != sceneToRemove.path)
                        {
                            newSettings[j++] = originalSettingScenes[i];
                        }
                    }
                    EditorBuildSettings.scenes = newSettings;

                    if (EditorSceneManager.GetActiveScene().name == $"Level{openScene}")
                    {
                        EditorSceneManager.OpenScene($"Assets/_Main/_Scenes/Levels/Level{int.Parse(openScene) - 1}.unity");
                    }

                    EditorUtility.DisplayDialog("Message", $"Level{openScene} has been deleted from project folder and build settings", "OK");
                }
            }
        }
    }

    private void ShowYesNoPopup()
    {
        if (EditorSceneManager.GetActiveScene().name.Equals($"Level{openScene}")) return;
        if (EditorUtility.DisplayDialog($"Open Level{openScene} scene", $"Do you want to open Level{openScene} scene? Unsaved changes in this scene will be discarded.", "Yes", "No"))
        {
            EditorSceneManager.OpenScene($"Assets/_Main/_Scenes/Levels/Level{openScene}.unity");
        }
    }

    private static void AddBackgroundToNewScene()
    {
        GameObject environment = new GameObject("Environment", typeof(Parallax));
        GameObject backgroundGradient = new GameObject("Background_Gradient", typeof(SpriteRenderer));
        backgroundGradient.transform.SetParent(environment.transform);
        backgroundGradient.GetComponent<SpriteRenderer>().sortingOrder = -100;
        backgroundGradient.transform.localScale = new Vector3(10000, 10, 1);
        Texture2D texture2D = AssetDatabase.LoadAssetAtPath($"Assets/_Main/Art/Background/Background_Gradient.png", typeof(Texture2D)) as Texture2D;
        backgroundGradient.GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
    }

    private void AddPrefabsToNewScene()
    {
        GameObject sharedSceneObject = AssetDatabase.LoadAssetAtPath($"Assets/_Main/Prefabs/SharedSceneObject.prefab", typeof(GameObject)) as GameObject;
        GameObject gameManager = AssetDatabase.LoadAssetAtPath($"Assets/_Main/Prefabs/GameManager/GameManager.prefab", typeof(GameObject)) as GameObject;
        GameObject mainCharacter = AssetDatabase.LoadAssetAtPath($"Assets/_Main/Prefabs/MainCharacter/MainCharacter.prefab", typeof(GameObject)) as GameObject;
        GameObject layoutCanvas = AssetDatabase.LoadAssetAtPath($"Assets/_Main/Prefabs/Frames/LayoutCanvas{options[popupIndex]}.prefab", typeof(GameObject)) as GameObject;
        PrefabUtility.InstantiatePrefab(sharedSceneObject);
        PrefabUtility.InstantiatePrefab(gameManager);
        PrefabUtility.InstantiatePrefab(layoutCanvas);
        PrefabUtility.InstantiatePrefab(mainCharacter);
    }

    private static void AddNewSceneToBuildSetting(int newIndex)
    {
        EditorBuildSettingsScene[] originalSettingScenes = EditorBuildSettings.scenes;
        EditorBuildSettingsScene sceneToAdd = new EditorBuildSettingsScene($"Assets/_Main/_Scenes/Levels/Level{newIndex}.unity", true);
        EditorBuildSettingsScene[] newSettings = new EditorBuildSettingsScene[originalSettingScenes.Length + 1];
        System.Array.Copy(originalSettingScenes, newSettings, originalSettingScenes.Length);
        newSettings[newSettings.Length - 1] = sceneToAdd;
        EditorBuildSettings.scenes = newSettings;
    }

    private int FindAppropriateIndex()
    {
        int j = 1;
        List<int> listIndex = new List<int>();
        for (int i = 2; i < EditorBuildSettings.scenes.Length; i++)
        {
            string[] strSplit = EditorBuildSettings.scenes[i].path.Split('/');
            string sceneIndex = strSplit[strSplit.Length - 1].Split('.')[0].Substring(5);
            listIndex.Add(int.Parse(sceneIndex));
        }
        listIndex.Sort();
        for(int i = 0; i < listIndex.Count; i++, j++)
        {
            if(listIndex[i] == j)
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
