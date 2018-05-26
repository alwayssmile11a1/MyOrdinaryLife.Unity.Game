using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBound : MonoBehaviour {

    private PlayerPlatformerController player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerPlatformerController>();
    }

    private void Update()
    {
        if(player.transform.position.y < transform.position.y)
        {
            gameObject.SetActive(false);
        }
    }



}
