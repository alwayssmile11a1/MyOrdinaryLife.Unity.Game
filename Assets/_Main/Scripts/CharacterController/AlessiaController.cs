using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;

[RequireComponent(typeof(SimpleCharacterController2D))]
public class AlessiaController : MonoBehaviour {

    public float speed = 5f;
    public float jumpTakeOffSpeed = 5f;


    [Header("Audio")]
    public RandomAudioPlayer footStepAudioPlayer;
    public RandomAudioPlayer landAudioPlayer;

    private SimpleCharacterController2D m_CharacterController2D;
    private Vector2 m_Velocity = new Vector2();
    private Collider2D m_Collider2D;
    private Rigidbody2D m_Rigidbody2D;


    private Animator m_Animator;
    private SpriteRenderer m_SpriteRenderer;



    private int m_HashGroundedPara = Animator.StringToHash("Grounded");
    private int m_HashRunPara = Animator.StringToHash("Run");
    private int m_HashHurtPara = Animator.StringToHash("Hurt");
    private int m_HashDashPara = Animator.StringToHash("Dash");


    private void Awake() {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Animator = GetComponent<Animator>();
        m_CharacterController2D = GetComponent<SimpleCharacterController2D>();
        m_Collider2D = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {

        Move();
        Animate();
    }

    public void Jump()
    {
        if (m_CharacterController2D.IsGrounded)
        {
            m_Rigidbody2D.velocity = new Vector3(0, jumpTakeOffSpeed);
        }
    }

    private void Move()
    {
        if (CheckForObstacle(0.5f,0.2f) && m_CharacterController2D.IsGrounded)
        {
            Jump();
        }

        //set velocity 
        m_Velocity.Set((m_SpriteRenderer.flipX ? -1 : 1) * speed, m_Rigidbody2D.velocity.y);

        //Move rigidbody
        m_Rigidbody2D.velocity = m_Velocity;

        

    }

    private void Flip()
    {

        m_SpriteRenderer.flipX = !m_SpriteRenderer.flipX;
        
    }

    private void Animate()
    {
        m_Animator.SetBool(m_HashGroundedPara, m_CharacterController2D.IsGrounded);
        m_Animator.SetFloat(m_HashRunPara, Mathf.Abs(m_Velocity.x));
    }

    public bool CheckForObstacle(float obstacleForwardDistance, float gulfForwardDistance)
    {


        //we circle cast with a size sligly smaller than the collider height. That avoid to collide with very small bump on the ground
        if (Physics2D.CircleCast(m_Collider2D.bounds.center, m_Collider2D.bounds.extents.y - 0.2f,
                                                                    m_SpriteRenderer.flipX ? Vector2.left : Vector2.right,
                                                                    obstacleForwardDistance, m_CharacterController2D.groundedLayerMask.value))
        {
            return true;
        }

        Vector3 castingPosition = (Vector2)(m_Collider2D.bounds.center) +
                             (m_SpriteRenderer.flipX ? Vector2.left : Vector2.right) * (m_Collider2D.bounds.extents.x + gulfForwardDistance);


        if (!Physics2D.CircleCast(castingPosition, 0.1f, Vector2.down, m_Collider2D.bounds.extents.y + 0.2f, m_CharacterController2D.groundedLayerMask.value))
        {
            return true;
        }

        return false;
    }


    public void PlayFootStepAudioPlayer()
    {
        footStepAudioPlayer.PlayRandomSound();
    }

    public void PlayLandAudioPlayer()
    {
        landAudioPlayer.PlayRandomSound();
    }


}
