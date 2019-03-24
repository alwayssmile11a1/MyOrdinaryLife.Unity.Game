using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEffectScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<CollectedBehaviour>() != null)
        {
            Debug.Log("Hit gem");
        }
    }
}
