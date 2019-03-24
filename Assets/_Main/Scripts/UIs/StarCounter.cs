using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCounter : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerPlatformerController>() != null)
        {
            GameManager.Instance.AddOneStar();
        }
    }

}
