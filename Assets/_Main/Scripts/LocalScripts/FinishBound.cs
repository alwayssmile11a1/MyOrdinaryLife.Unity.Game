using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishBound : MonoBehaviour {


    public string endLevelEffect = "EndLevelEffect";

    private int HashEndLevelEffect;



    private void Awake()
    {
        HashEndLevelEffect = VFXController.StringToHash(endLevelEffect);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EndLevelCanvas endLevelCanvas = GameObject.Find("EndLevelCanvasWrapper").GetComponentInChildren<EndLevelCanvas>(true);
        endLevelCanvas.gameObject.SetActive(true);

        PlayerPlatformerController player = collision.GetComponent<PlayerPlatformerController>();

        if (player != null)
        {
            collision.gameObject.SetActive(false);
            VFXController.Instance.Trigger(HashEndLevelEffect, player.transform.position - Vector3.left, 0, false, null);

            int startCount = endLevelCanvas.GetStarsCount();

            SavedData savedData = new SavedData();

            if (savedData.Load(SceneManager.GetActiveScene().name))
            {
                if (savedData.count < startCount)
                {
                    savedData.count = startCount;
                    savedData.Save(SceneManager.GetActiveScene().name);
                }
            }
            else
            {
                savedData.count = startCount;
                savedData.Save(SceneManager.GetActiveScene().name);
            }
        }
    }

}
