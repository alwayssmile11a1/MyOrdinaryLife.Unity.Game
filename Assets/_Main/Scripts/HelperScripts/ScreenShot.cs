#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class ScreenShot : MonoBehaviour
{
    // Add this to ignore screen fader
    private int frameToCapture = 30;
    private int frameCount = 0;
    private bool Captured = false;

    private void Update()
    {
        if (!Captured)
        {
            frameCount++;
            if (frameCount == frameToCapture)
            {
                StartCapturing();
                Captured = true;
                frameCount = 0;
            }
        }
    }

    private void StartCapturing()
    {
        string[] strSplit = SceneManager.GetActiveScene().path.Split('/');
        string sceneName = strSplit[strSplit.Length - 1].Split('.')[0];
        string folder = strSplit[strSplit.Length - 2];
        string directory = $"{Application.dataPath}/_Main/Editor/Screenshots/{folder}";
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
            StartCoroutine(DoScreenShot(sceneName, directory));
        }
        else
        {
            StartCoroutine(DoScreenShot(sceneName, directory));
        }
    }

    private IEnumerator DoScreenShot(string sceneName, string directory)
    {
        yield return new WaitForEndOfFrame();
        Texture2D texture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        texture2D.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
        texture2D.Apply();
        byte[] writeBytes = texture2D.EncodeToPNG();
        if (!File.Exists($"{directory}/{sceneName}.png"))
        {
            File.WriteAllBytes($"{directory}/{sceneName}.png", writeBytes);
        }
        else
        {
            byte[] readBytes = File.ReadAllBytes($"{directory}/{sceneName}.png");
            if (!readBytes.Equals(writeBytes))
            {
                File.WriteAllBytes($"{directory}/{sceneName}.png", writeBytes);
            }
        }
    }

}
#endif