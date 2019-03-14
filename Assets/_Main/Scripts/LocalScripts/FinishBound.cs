using Gamekit2D;
using UnityEngine;

public class FinishBound : MonoBehaviour {


    public string endLevelEffect = "EndLevelEffect";
    private int HashEndLevelEffect;



    private void Awake()
    {
        HashEndLevelEffect = VFXController.StringToHash(endLevelEffect);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerPlatformerController player = collision.GetComponent<PlayerPlatformerController>();

        if (player != null)
        {
            collision.gameObject.SetActive(false);
            VFXController.Instance.Trigger(HashEndLevelEffect, player.transform.position - Vector3.left, 0, false, null);
            GameManager.Instance.FinishLevel();
        }
    }

}
