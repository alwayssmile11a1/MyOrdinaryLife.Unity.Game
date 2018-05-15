using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject {

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    public LayerMask groundLayerMask;

    private SpriteRenderer m_SpriteRenderer;
    private Animator m_Animator;

    private CapsuleCollider2D m_CapsuleCollider2D;

    private float m_OriginalMaxSpeed;

    // Use this for initialization
    void Awake () 
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer> (); 
        m_Animator = GetComponent<Animator> ();
        m_CapsuleCollider2D = GetComponent<CapsuleCollider2D>();
        m_OriginalMaxSpeed = maxSpeed;
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = m_SpriteRenderer.flipX ? -1f : 1f;

        if (CheckForObstacle(0.5f, 0.2f) && grounded)
        {
            velocity.y = jumpTakeOffSpeed;
        }

        //move.x = Input.GetAxis ("Horizontal");

        //if (Input.GetButtonDown("Jump") && grounded)
        //{
        //    velocity.y = jumpTakeOffSpeed;
        //}
        //else if (Input.GetButtonUp("Jump"))
        //{
        //    if (velocity.y > 0)
        //    {
        //        velocity.y = velocity.y * 0.5f;
        //    }
        //}

        //if(move.x > 0.01f)
        //{
        //    if(m_SpriteRenderer.flipX == true)
        //    {
        //        m_SpriteRenderer.flipX = false;
        //    }
        //} 
        //else if (move.x < -0.01f)
        //{
        //    if(m_SpriteRenderer.flipX == false)
        //    {
        //        m_SpriteRenderer.flipX = true;
        //    }
        //}

        m_Animator.SetBool ("grounded", grounded);
        m_Animator.SetFloat ("velocityX", Mathf.Abs (velocity.x) / maxSpeed);

        targetVelocity = move * maxSpeed;
    }


    public bool CheckForObstacle(float obstacleForwardDistance,float gulfForwardDistance)
    {


        //we circle cast with a size sligly smaller than the collider height. That avoid to collide with very small bump on the ground
        if (Physics2D.CircleCast(m_CapsuleCollider2D.bounds.center, m_CapsuleCollider2D.bounds.extents.y - 0.2f,
                                                                    m_SpriteRenderer.flipX?Vector2.left:Vector2.right,
                                                                    obstacleForwardDistance, groundLayerMask.value))
        {
            return true;
        }
        
        Vector3 castingPosition = (Vector2)(m_CapsuleCollider2D.bounds.center) + 
                             (m_SpriteRenderer.flipX ? Vector2.left : Vector2.right)  * (m_CapsuleCollider2D.bounds.extents.x + gulfForwardDistance);


        if (!Physics2D.CircleCast(castingPosition, 0.1f, Vector2.down, m_CapsuleCollider2D.bounds.extents.y + 0.2f, groundLayerMask.value))
        {
            return true;
        }

        return false;
    }

    public void Flip()
    {
        m_SpriteRenderer.flipX = !m_SpriteRenderer.flipX;

    }


    public void SlowDown(float amount)
    {
        ChangeBackToNormal();

        m_Animator.speed = amount;
        maxSpeed = maxSpeed * amount;


    }

    public void ChangeBackToNormal()
    {
        m_Animator.speed = 1;
        maxSpeed = m_OriginalMaxSpeed;
    }

}