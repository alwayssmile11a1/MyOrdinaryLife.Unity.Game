using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlash : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody2D m_Rigidbody2D;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        m_Rigidbody2D.velocity = new Vector2(speed, 0);
    }


    public void OnHitEnemy(Damager damager, Damageable damageable)
    {
        Enemy enemy = damageable.GetComponent<Enemy>();
        if (enemy)
        {
            enemy.OnHit();
        }
    }


}
