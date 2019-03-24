using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [HideInInspector] public DeadBound deadBound;
    [SerializeField] private Bullet bullet;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerPlatformerController player = collision.gameObject.GetComponent<PlayerPlatformerController>();
        if (player != null)
        {
            deadBound.OnPlayerDie(player);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<ParticleSystem>() != null)
        {
            bullet.ReturnToPool();
        }
    }
}
