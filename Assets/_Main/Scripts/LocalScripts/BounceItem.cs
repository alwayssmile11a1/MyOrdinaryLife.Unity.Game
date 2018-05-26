using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceItem : MonoBehaviour {

    public float jumpSpeed = 15;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerPlatformerController player = collision.GetComponent<PlayerPlatformerController>();


        if(player!=null)
        {
            
            player.Jump(jumpSpeed);
            

        }

    }



    


}
