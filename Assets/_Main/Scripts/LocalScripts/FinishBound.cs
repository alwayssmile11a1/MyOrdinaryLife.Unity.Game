using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishBound : MonoBehaviour {


    public string endLevelEffect = "EndLevelEffect";

    public int StarsCount { get; set; }

    private int HashEndLevelEffect;



    private void Awake()
    {
        HashEndLevelEffect = VFXController.StringToHash(endLevelEffect);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject.Find("EndLevelCanvasWrapper").GetComponentInChildren<EndLevelCanvas>(true).gameObject.SetActive(true);

        PlayerPlatformerController player = collision.GetComponent<PlayerPlatformerController>();

        if (player != null)
        {
            collision.gameObject.SetActive(false);
            VFXController.Instance.Trigger(HashEndLevelEffect, player.transform.position - Vector3.left, 0, false, null);
        }

   


        SavedData savedData = new SavedData();

        if(savedData.Load(SceneManager.GetActiveScene().name))
        {
            if (savedData.count < StarsCount)
            {
                savedData.count = StarsCount;
                savedData.Save(SceneManager.GetActiveScene().name);
            }
        }
        else
        {
            savedData.count = StarsCount;
            savedData.Save(SceneManager.GetActiveScene().name);
        }


    }

}
