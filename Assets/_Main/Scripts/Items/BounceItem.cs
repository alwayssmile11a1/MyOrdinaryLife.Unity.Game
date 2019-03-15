using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceItem : MonoBehaviour {

    public string effect = "Effect";

    

    public float jumpSpeed = 15;

    private int HashEffect;


    private void Awake()
    {
        HashEffect = VFXController.StringToHash(effect);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerPlatformerController player = collision.GetComponent<PlayerPlatformerController>();


        if(player!=null)
        {
            
            player.Jump(jumpSpeed);

            transform.parent.gameObject.SetActive(false);

            VFXController.Instance.Trigger(HashEffect, transform.position, 0, false, null);
        }


    }



    


}
