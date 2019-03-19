using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {
    public Transform destination;
    public ParticleSystem burst;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //shouldn't use compare tag for perfomance purpose
        //if (collision.tag.Equals("Player"))
        if (collision.GetComponent<PlayerPlatformerController>() != null)
        {
            destination.GetComponent<BoxCollider2D>().enabled = false;
            collision.transform.position = destination.position;
            audioSource.Play();
            burst.Play();
        }
    }
}
