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


        if(player!=null)
        {
            player.gameObject.SetActive(false);
            StartCoroutine(PlayerDie(player.transform.position + Vector3.up * 0.6f));


        }


    }


    private IEnumerator PlayerDie(Vector3 effectPosition)
    {
        TimeManager.ChangeTimeBackToNormal();


        VFXController.Instance.Trigger(HashDeadEffect, effectPosition, 0, false, null);

        deadAudio.PlayRandomSound();

        yield return new WaitForSeconds(2.5f);


        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

}
