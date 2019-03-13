using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEditor : EditorWindow
{
    bool editExistingLevel = false;
    float buttonWidth = 200;
    float buttonHeight = 25;
    string openScene = string.Empty;
    int space = 10;

    string[] options = new string[] { "1x3", "2x2", "2x3" };
    int popupIndex = 0;

    // xóa sau
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
            // xóa sau
            newSceneIndex = EditorSceneManager.sceneCountInBuildSettings - 1;

            if (EditorUtility.DisplayDialog("Create new scene", "Do you want to create new scene? Unsaved changes in this scene will be discarded.", "Yes", "No"))
            {
                // Create new scene
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

                // Add new scene to build settings
                AddNewSceneToBuildSetting();

                // Add prefabs to new scene
                AddPrefabsToNewScene();

                // Add background to new scene
                AddBackgroundToNewScene();

                // Save scene after adding everything
                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), $"Assets/_Main/_Scenes/Level{newSceneIndex}.unity");
            }
        }

        // Popup (it's a dropdown)
        popupIndex = EditorGUILayout.Popup(popupIndex, options, GUILayout.Height(buttonHeight), GUILayout.Width(buttonWidth));

        GUI.enabled = true;

        // Toggle (checkbox)
        editExistingLevel = EditorGUILayout.Toggle("Edit existing level", editExistingLevel);

        GUI.enabled = editExistingLevel;

        // Textfield
        openScene = EditorGUILayout.TextField(openScene, GUILayout.Height(buttonHeight), GUILayout.Width(buttonWidth));

        if (GUILayout.Button("Open", GUILayout.Height(buttonHeight), GUILayout.Width(buttonWidth)))
        {
            if (EditorUtility.DisplayDialog($"Open Level{openScene} scene", $"Do you want to open Level{openScene} scene? Unsaved changes in this scene will be discarded.", "Yes", "No"))
            {
                EditorSceneManager.OpenScene($"Assets/_Main/_Scenes/Level{openScene}.unity");
            }
        }

        //if (GUILayout.Button("Delete", GUILayout.Height(buttonHeight), GUILayout.Width(buttonWidth)))
        //{
        //    if (EditorUtility.DisplayDialog($"Delete Level{openScene} scene", $"Do you want to delete Level{openScene} scene?", "Yes", "No"))
        //    {
        //        Scene scene = SceneManager.GetSceneByName($"Level{editExistingLevel}");
        //    }
        //}

        GUI.enabled = true;

        //if (GUILayout.Button("Save changes in current scene", GUILayout.Height(buttonHeight), GUILayout.Width(buttonWidth)))
        //{
        //    Debug.LogError("Chưa làm cái save current scene !!!");
        //    Debug.LogError("Chưa làm cái save current scene !!!");
        //    Debug.LogError("Chưa làm cái save current scene !!!");
        //    Debug.LogError("Chưa làm cái save current scene !!!");
        //}
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

    private static void AddNewSceneToBuildSetting()
    {
        EditorBuildSettingsScene[] originalSettingScenes = EditorBuildSettings.scenes;
        EditorBuildSettingsScene sceneToAdd = new EditorBuildSettingsScene($"Assets/_Main/_Scenes/Level{EditorSceneManager.sceneCountInBuildSettings - 1}.unity", true);
        EditorBuildSettingsScene[] newSettings = new EditorBuildSettingsScene[originalSettingScenes.Length + 1];
        System.Array.Copy(originalSettingScenes, newSettings, originalSettingScenes.Length);
        newSettings[newSettings.Length - 1] = sceneToAdd;
        EditorBuildSettings.scenes = newSettings;
    }
}
