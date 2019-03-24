using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadBound : MonoBehaviour {

    public RandomAudioPlayer deadAudio;

    private PlayerPlatformerController player;

    private void Start()
    {
        player = GameManager.Instance.GetPlayer();
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
            player.Die();
            deadAudio.PlayRandomSound();
        }
    }

}
