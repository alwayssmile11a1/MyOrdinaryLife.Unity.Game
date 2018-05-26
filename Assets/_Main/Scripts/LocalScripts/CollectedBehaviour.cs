using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class CollectedBehaviour : MonoBehaviour
{

    public UnityEvent OnCollect;
    public string collectedEffect = "CollectedEffect";

    private int HashCollectedEffect;


    

    private void Awake()
    {
        HashCollectedEffect = Gamekit2D.VFXController.StringToHash(collectedEffect);

    }



	void OnTriggerEnter2D(Collider2D theCollider)
	{
		if (theCollider.CompareTag ("Player")) {
			Collected ();
		}
	}

	void Collected()
	{
        gameObject.SetActive(false);
        Gamekit2D.VFXController.Instance.Trigger(HashCollectedEffect, transform.position, 0, false, null);
        OnCollect.Invoke();
	}
}
