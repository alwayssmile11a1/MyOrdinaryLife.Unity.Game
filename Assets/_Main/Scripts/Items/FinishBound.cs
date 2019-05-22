using Gamekit2D;
using UnityEngine;

public class FinishBound : MonoBehaviour {


    public string endLevelEffect = "EndLevelEffect";
    private int m_HashEndLevelEffect;
    private RandomAudioPlayer m_Audio;


    private void Awake()
    {
        m_HashEndLevelEffect = VFXController.StringToHash(endLevelEffect);
        m_Audio = GetComponent<RandomAudioPlayer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerPlatformerController player = collision.GetComponent<PlayerPlatformerController>();

        if (player != null)
        {
            collision.gameObject.SetActive(false);
            VFXController.Instance.Trigger(m_HashEndLevelEffect, player.transform.position - Vector3.left, 0, false, null);
            m_Audio.PlayRandomSound();
            GameManager.Instance.FinishLevel();
            UIManager.Instance.UpdatePlayerLife(true);
        }
    }

}
