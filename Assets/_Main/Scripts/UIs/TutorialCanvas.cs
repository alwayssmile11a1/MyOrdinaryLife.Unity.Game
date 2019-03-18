using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCanvas : MonoBehaviour {

    private void Awake()
    {
        FindObjectOfType<PlayerPlatformerController>().DontStartDelay();
    }


    public void ResumeGame()
    {
        StartCoroutine(FindObjectOfType<PlayerPlatformerController>().StartDelay());
    }

}
