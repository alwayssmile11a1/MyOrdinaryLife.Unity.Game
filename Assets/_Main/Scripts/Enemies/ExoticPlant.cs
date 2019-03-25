using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;

public class ExoticPlant : MonoBehaviour
{
    public              GameObject      bullet;
    public              int             numberOfBullet;
    public              float           bulletSpeed;
    public              AudioSource     shootAudio;

    private             BulletPool      bulletPool;
    private             Frame           frame;
    private             Animator        animator;

    private readonly    int             HashAttack = Animator.StringToHash("Attack");

    void Awake()
    {
        bulletPool = BulletPool.GetObjectPool(bullet, 5);
        animator = GetComponent<Animator>();
        frame = transform.parent.GetComponentInParent<Frame>();
        frame.onPlayerExitFrame.AddListener(OnPlayerExitFrame);
    }

    public void Shoot()
    {
        BulletObject bulletObject = bulletPool.Pop(transform.position + transform.up * 0.8f);
        bulletObject.transform.SetParent(this.transform);
        bulletObject.transform.RotateToDirection(-transform.up);
        bulletObject.rigidbody2D.velocity = bulletSpeed * transform.up;
        shootAudio.Play();
    }

    private void OnPlayerExitFrame()
    {
        animator.SetBool(HashAttack, false);
    }
}
