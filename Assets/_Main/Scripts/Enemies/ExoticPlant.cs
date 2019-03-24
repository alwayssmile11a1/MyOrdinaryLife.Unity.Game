using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;

public class ExoticPlant : MonoBehaviour
{
    public GameObject bullet;
    public int numberOfBullet;
    public float bulletSpeed;
    public AudioSource shootAudio;

    private BulletPool bulletPool;
    void Awake()
    {
        bulletPool = BulletPool.GetObjectPool(bullet, 5);
    }

    public void Shoot()
    {
        BulletObject bulletObject = bulletPool.Pop(transform.position + transform.up * 0.8f);
        bulletObject.transform.RotateToDirection(-transform.up);
        bulletObject.rigidbody2D.velocity = bulletSpeed * transform.up;
        shootAudio.Play();
    }
}
