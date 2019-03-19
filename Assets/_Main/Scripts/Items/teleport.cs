using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour {
    public Transform destination;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            destination.GetComponent<BoxCollider2D>().enabled = false;
            collision.transform.position = destination.position;
            audioSource.Play();
        }
    }
}
