using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [HideInInspector]
    public      DeadBound       deadBound;
    [SerializeField]
    private     Bullet          bullet;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"collision: {collision.gameObject.name}");
        PlayerPlatformerController player = collision.gameObject.GetComponent<PlayerPlatformerController>();
        if (player != null)
        {
            Debug.Log($"collision: {collision.gameObject.name} hit");
            player.Die();
        }
        bullet.ReturnToPool();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"trigger: {collision.gameObject.name}");
        if (collision.GetComponent<ParticleSystem>() != null)
        {
            bullet.ReturnToPool();
        }
    }
}
