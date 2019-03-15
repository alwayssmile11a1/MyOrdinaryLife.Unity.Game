using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadBound : MonoBehaviour {

    public string deadEffect = "DeadEffect";
    public RandomAudioPlayer deadAudio;

    private PlayerPlatformerController player;


    private int HashDeadEffect;

    private void Awake()
    {
        player = FindObjectOfType<PlayerPlatformerController>();
        HashDeadEffect = VFXController.StringToHash(deadEffect);
    }

    private void Update()
    {
        if(player.transform.position.y - transform.position.y < -0.5f)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        PlayerPlatformerController player = collision.GetComponent<PlayerPlatformerController>();
        
        //check to see if it's player and the player collided from above
        if(player!=null && player.transform.position.y > transform.position.y)
        {
            player.gameObject.SetActive(false);

            TimeManager.ChangeTimeBackToNormal();

            VFXController.Instance.Trigger(HashDeadEffect, player.transform.position + Vector3.up * 0.6f, 0, false, null);

            deadAudio.PlayRandomSound();

            StartCoroutine(GameManager.Instance.RestartLevel());

        }


    }

}
