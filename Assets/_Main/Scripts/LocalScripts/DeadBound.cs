using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadBound : MonoBehaviour {

    public string deadEffect = "DeadEffect";

    private PlayerPlatformerController player;


    private int HashDeadEffect;

    private void Awake()
    {
        player = FindObjectOfType<PlayerPlatformerController>();
        HashDeadEffect = VFXController.StringToHash(deadEffect);
    }

    private void Update()
    {
        if(player.transform.position.y < transform.position.y)
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
            StartCoroutine(PlayerDie());


        }


    }


    private IEnumerator PlayerDie()
    {
        TimeManager.ChangeTimeBackToNormal();


        VFXController.Instance.Trigger(HashDeadEffect, transform.position, 0, false, null);

        yield return new WaitForSeconds(2f);


        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

}
