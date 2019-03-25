﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerPlatformerController : MonoBehaviour
{
    public float startDelayTime = 1f;
    public float speed = 5f;
    public float jumpSpeed = 8.5f;
    public float jumpAbortSpeedReduction = 20f;
    public float gravity = 15f;

    [Header("Obstacles Check")]
    public float obstacleForwardDistance = 1.5f;
    public float gulfForwardDistance = 0.1f;

    [Header("Audio")]
    public string footStepAudioPlayer;
    public string hitAudioPlayer;
    public string deadAudioPlayer;
    public string appearingAudioPlayer;

    [Header("Effect")]
    public string appearingEffect = "AppearingEffect";
    public string deadEffect = "DeadEffect";

    [Header("Misc")]
    public GameObject slashPrefab;
    public GameObject shadow;

    private CharacterController2D m_CharacterController2D;
    private Collider2D m_Collider2D;
    private Animator m_Animator;
    private SpriteRenderer m_SpriteRenderer;
    private Vector2 m_MoveVector;
    private PlatformEffector2D m_PlatformEffector2D;
    private float m_OriginalSpeed;

    public readonly int m_HashGroundedPara = Animator.StringToHash("Grounded");
    public readonly int m_HashRunPara = Animator.StringToHash("Run");
    public readonly int m_HashRunFastPara = Animator.StringToHash("RunFast");
    public readonly int m_HashJumpPara = Animator.StringToHash("Jump");
    public readonly int m_HashHurtPara = Animator.StringToHash("Hurt");
    public readonly int m_HashAttack3Para = Animator.StringToHash("Attack3");
    public readonly int m_HashJumpAttack3Para = Animator.StringToHash("JumpAttack3");
    public readonly int m_HashDeadPara = Animator.StringToHash("Dead");

    private BulletPool m_SlashPool;

    private int m_HashDeadEffect;
    private int m_HashAppearingEffect;

    private int m_HashFootStepAudioPlayer;
    private int m_HashDeadAudioPlayer;
    private int m_HashHitAudioPlayer;
    private int m_HashAppearingAudioPlayer;

    private bool m_CanAct = false;
    private bool m_DontStartDelay = false;
    private bool m_CanJump = true;
    private bool m_CanRun = true;
    private bool m_IsDead = false;
    private Vector2 m_DamagedVector = Vector2.zero;

    private const float k_GroundedStickingVelocityMultiplier = 3f;    // This is to help the character stick to vertically moving platforms.

    private void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Animator = GetComponent<Animator>();
        m_CharacterController2D = GetComponent<CharacterController2D>();
        m_Collider2D = GetComponent<Collider2D>();

        m_HashDeadEffect = VFXController.StringToHash(deadEffect);
        m_HashAppearingEffect = VFXController.StringToHash(appearingEffect);

        m_HashFootStepAudioPlayer = AudioPlayerController.StringToHash(footStepAudioPlayer);
        m_HashHitAudioPlayer = AudioPlayerController.StringToHash(hitAudioPlayer);
        m_HashDeadAudioPlayer = AudioPlayerController.StringToHash(deadAudioPlayer);
        m_HashAppearingAudioPlayer = AudioPlayerController.StringToHash(appearingAudioPlayer);

        m_SlashPool = BulletPool.GetObjectPool(slashPrefab, 1);

        m_SpriteRenderer.enabled = false;
        shadow.SetActive(false);

        GameManager.Instance.RegisterPlayer(this);
    }

    private void Start()
    {
        if (!m_DontStartDelay)
        {
            StartCoroutine(StartDelay());
        }
    }

    public IEnumerator StartDelay()
    {
        m_CanAct = false;
        yield return new WaitForSeconds(0.3f);
        VFXController.Instance.Trigger(m_HashAppearingEffect, transform.position, 0, false, null);
        AudioPlayerController.Instance.Trigger(m_HashAppearingAudioPlayer);
        yield return new WaitForSeconds(1f);
        m_SpriteRenderer.enabled = true;
        shadow.SetActive(true);
        yield return new WaitForSeconds(startDelayTime);
        m_CanAct = true;
    }

    public void DontStartDelay()
    {
        m_DontStartDelay = true;
    }

    public bool CanAct()
    {
        return m_CanAct;
    }

    private void FixedUpdate()
    {
        if (m_CanAct)
        {
            TakeAction();
        }

        if (m_CanRun)
        {
            Face();
        }

        Animate();

    }

    private void TakeAction()
    {
        if (CheckForObstacle(obstacleForwardDistance, gulfForwardDistance))
        {
            if (m_CanJump)
            {
                Jump();
            }
        }


        //Reduce jump speed
        JumpAbortReduction();

        //Vertical movement
        if (!m_CharacterController2D.IsGrounded)
        {
            AirborneVerticalMovement();
        }
        else
        {
            GroundedVerticalMovement();
        }

        if (m_CanRun)
        {
            SetHorizontalMovement(m_SpriteRenderer.flipX ? -speed : speed);
        }
        else
        {
            SetHorizontalMovement(0);
        }

        if(m_DamagedVector != Vector2.zero)
        {
            SetMoveVector(m_DamagedVector);
            m_DamagedVector.y = 0;
        }

        //Move
        Move();

    }


    private void Face()
    {
        if (!m_SpriteRenderer.flipX && m_MoveVector.x < 0)
        {
            m_SpriteRenderer.flipX = true;

        }

        if (m_SpriteRenderer.flipX && m_MoveVector.x > 0)
        {
            m_SpriteRenderer.flipX = false;
        }

    }

    private void Animate()
    {
        m_Animator.SetBool(m_HashGroundedPara, m_CharacterController2D.IsGrounded);
        m_Animator.SetFloat(m_HashRunPara, Mathf.Abs(m_MoveVector.x));
        m_Animator.SetFloat(m_HashJumpPara, m_MoveVector.y);
    }

    private void Move()
    {
        m_CharacterController2D.Move(m_MoveVector * Time.fixedDeltaTime);
    }

    public void Jump()
    {
        if (m_CharacterController2D.IsGrounded)
        {
            SetVerticalMovement(jumpSpeed);
            m_CanRun = true;
        }
    }

    public void Jump(float jump)
    {

        SetVerticalMovement(jump);

    }

    public void SetMoveVector(Vector2 newMoveVector)
    {
        m_MoveVector = newMoveVector;
    }

    public Vector2 GetMoveVector()
    {
        return m_MoveVector;
    }

    public void SetHorizontalMovement(float newHorizontalMovement)
    {
        m_MoveVector.x = newHorizontalMovement;
    }

    public void SetVerticalMovement(float newVerticalMovement)
    {
        m_MoveVector.y = newVerticalMovement;
    }

    public void IncrementMovement(Vector2 additionalMovement)
    {
        m_MoveVector += additionalMovement;
    }

    public void IncrementHorizontalMovement(float additionalHorizontalMovement)
    {
        m_MoveVector.x += additionalHorizontalMovement;
    }

    public void IncrementVerticalMovement(float additionalVerticalMovement)
    {
        m_MoveVector.y += additionalVerticalMovement;
    }

    public void JumpAbortReduction()
    {
        if (m_MoveVector.y > 0.0f)
        {
            m_MoveVector.y -= jumpAbortSpeedReduction * Time.deltaTime;
        }
    }

    public void GroundedVerticalMovement()
    {
        m_MoveVector.y -= gravity * Time.deltaTime;

        if (m_MoveVector.y < -gravity * Time.deltaTime * k_GroundedStickingVelocityMultiplier)
        {
            m_MoveVector.y = -gravity * Time.deltaTime * k_GroundedStickingVelocityMultiplier;
        }
    }

    public void AirborneVerticalMovement()
    {
        if (Mathf.Approximately(m_MoveVector.y, 0f) || m_CharacterController2D.IsCeilinged && m_MoveVector.y > 0f)
        {
            m_MoveVector.y = 0f;
        }
        m_MoveVector.y -= gravity * Time.deltaTime;
    }

    public bool CheckForObstacle(float obstacleForwardDistance, float gulfForwardDistance)
    {


        ////we circle cast with a size sligly smaller than the collider height. That avoid to collide with very small bump on the ground
        //if (Physics2D.CircleCast(m_Collider2D.bounds.center, m_Collider2D.bounds.extents.y - 0.2f,
        //                                                            m_SpriteRenderer.flipX ? Vector2.left : Vector2.right,
        //                                                            obstacleForwardDistance, m_CharacterController2D.groundedLayerMask.value))
        //{
        //    return true;
        //}
        #if UNITY_EDITOR
        Debug.DrawRay(m_Collider2D.bounds.center + Vector3.down * 0.2f, m_SpriteRenderer.flipX ? Vector2.left : Vector2.right * obstacleForwardDistance, Color.red);
        #endif
        if (Physics2D.Raycast(m_Collider2D.bounds.center + Vector3.down * 0.2f, m_SpriteRenderer.flipX ? Vector2.left : Vector2.right, obstacleForwardDistance, m_CharacterController2D.groundedLayerMask.value))
        {
            return true;
        }

        Vector3 castingPosition = (Vector2)(m_Collider2D.bounds.center) + (m_SpriteRenderer.flipX ? Vector2.left : Vector2.right) * (m_Collider2D.bounds.extents.x + gulfForwardDistance);
        //if (!Physics2D.CircleCast(castingPosition, 0.1f, Vector2.down, m_Collider2D.bounds.extents.y + 0.2f, m_CharacterController2D.groundedLayerMask.value))
        //{
        //    return true;
        //}
        #if UNITY_EDITOR
        Debug.DrawRay(castingPosition, Vector2.down * (m_Collider2D.bounds.extents.y + 0.2f), Color.red);
        #endif
        if (!Physics2D.Raycast(castingPosition, Vector2.down, m_Collider2D.bounds.extents.y + 0.2f, m_CharacterController2D.groundedLayerMask.value))
        {
            return true;
        }
        return false;
    }

    public void PlayFootStepAudioPlayer()
    {
        AudioPlayerController.Instance.Trigger(m_HashFootStepAudioPlayer);
    }

    public void PlayDeadAudioPlayer()
    {
        AudioPlayerController.Instance.Trigger(m_HashDeadAudioPlayer);
    }

    public void PlayHitAudioPlayer()
    {
        AudioPlayerController.Instance.Trigger(m_HashHitAudioPlayer);
    }



    public bool Attack3()
    {
        if(m_CharacterController2D.IsGrounded)
        {
            m_Animator.SetTrigger(m_HashAttack3Para);
            m_CanRun = false;
            //StartCoroutine(InternalEndAttack(0.7f));

            return true;
        }
        return false;
        //else
        //{
        //    m_Animator.SetTrigger(m_HashJumpAttack3Para);
        //}
    }

    public void EndAttack()
    {
        m_CanRun = true;
        //StartCoroutine(InternalEndAttack());
    }

    //private IEnumerator InternalEndAttack(float time)
    //{
    //    yield return new WaitForSeconds(time);
    //    m_CanRun = true;
    //}

    public void Die()
    {
        if (!m_IsDead)
        {
            m_IsDead = true;
            m_CanRun = false;
            m_CanJump = false;

            m_DamagedVector = new Vector2(-4f, 30f);

            m_Animator.SetTrigger(m_HashDeadPara);

            TimeManager.ChangeTimeBackToNormal();

            CameraShaker.Shake(0.3f, 0.3f);

            GameManager.Instance.StartCoroutine(GameManager.Instance.RestartLevelWithDelay(2f));

            PlayHitAudioPlayer();
        }
    }

    public void PlayDeadEffect()
    {
        m_DamagedVector = Vector2.zero;
        VFXController.Instance.Trigger(m_HashDeadEffect, transform.position - Vector3.one * 0.5f, 0, false, null);
        PlayDeadAudioPlayer();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public bool IsDead()
    {
        return m_IsDead;
    }

    public void InstantiateSlashPrefab()
    {
        m_SlashPool.Pop(transform.position + Vector3.right * 0.5f);
    }

}
