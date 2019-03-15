using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroCanvas : MonoBehaviour
{

    public float delayTime = 1f;

    // Start is called before the first frame update
    void Awake()
    {
        Invoke("GoToMenuScene", delayTime);
    }

    void GoToMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }

}
