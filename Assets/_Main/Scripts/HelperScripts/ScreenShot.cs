using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEngine.Windows;
#endif
public class ScreenShot : MonoBehaviour
{
#if UNITY_EDITOR
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
        //consider using Path.Combine for multiple platforms support (maybe)
        //consider putting screenshorts folder outsite Assets directory for build time reduction (maybe) and avoid AssetDatabase importing 
        //example: string screenshotsDirectory = $"../{Application.dataPath}/Screenshots/";
        //consider using AssetDatabase for importing, creating or moving data around inside Assets folder (maybe not a good idea in runtime but worth a try)
        string screenshotsDirectory = $"{Application.dataPath}/_Main/Editor/Screenshots/";
        string levelDirectory = $"{screenshotsDirectory}{folder}";
        if (!Directory.Exists(screenshotsDirectory))
        {
            Directory.CreateDirectory(screenshotsDirectory);
        }
        if (!Directory.Exists(levelDirectory))
        {
            Directory.CreateDirectory(levelDirectory);
            StartCoroutine(DoScreenShot(sceneName, levelDirectory));
        }
        else
        {
            StartCoroutine(DoScreenShot(sceneName, levelDirectory));
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
#endif
}
