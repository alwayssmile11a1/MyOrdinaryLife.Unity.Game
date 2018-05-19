using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Gamekit;

public class TriggerEnterEventTrigger : MonoBehaviour {

    public LayerMask targetLayer;

    [System.Serializable]
    public class TriggerEnterEvent : UnityEvent<Collider2D>
    {

    }


    public TriggerEnterEvent OnTriggerEnterEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetLayer.Contains(collision.gameObject))
        {
            OnTriggerEnterEvent.Invoke(collision);
        }
    }




}
