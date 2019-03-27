using BTAI;
using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingSkull : MonoBehaviour
{
    public float delayTime = 1f;
    public float attackSpeed = 5f;
    public Transform[] positionsToMove;
    public SpriteRenderer glowCircle;
    public string deadEffect;
    public RandomAudioPlayer hitAudio;
    public RandomAudioPlayer deadAudio;

    //Behavior Tree
    private Root m_Ai = BT.Root();
    private int m_NewPositionIndex = -1;
    private Vector2 m_PlayerPosition;
    private PlayerPlatformerController m_Player;
    private Rigidbody2D m_Rigidbody2D;
    private Animator m_Animator;
    private SpriteRenderer m_SpriteRenderer;
    private bool m_GotHit;
    private int m_Health = 3;
    private int m_HashDeadEffect;

    private int m_HashHidePara = Animator.StringToHash("Hide");
    private int m_HashAppearPara = Animator.StringToHash("Appear");
    private int m_HashAttackPara = Animator.StringToHash("Attack");
    private int m_HashHurtPara = Animator.StringToHash("Hurt");
    private int m_HashDiePara = Animator.StringToHash("Die");

    private void Awake()
    {
        m_Ai.OpenBranch(
            BT.Sequence().OpenBranch(
                BT.Wait(2f),
                BT.Call(SetNewPosition),
                BT.Wait(1f),
                BT.Call(MoveToNewPosition),
                BT.Wait(1.5f),
                BT.Call(SetAttackPlayer),
                BT.Wait(1f),
                BT.WaitUntil(AttackPlayer),
                BT.Wait(0.5f),
                BT.Call(() => m_Rigidbody2D.velocity = Vector2.zero)
            )
        );

        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpriteRenderer.enabled = false;
        glowCircle.enabled = false;
        m_HashDeadEffect = VFXController.StringToHash(deadEffect);
    }

    private void Start()
    {
        m_Player = GameManager.Instance.GetPlayer();
    }

    private void Update()
    {
        if(m_Health == 0)
        {
            m_Animator.SetTrigger(m_HashDiePara);
            m_Health = -1;
        }

        if (delayTime <= 0)
        {
            if (m_Health > 0)
            {
                m_Ai.Tick();
            }
        }
        else
        {
            delayTime -= Time.deltaTime;
            if(delayTime<=0)
            {
                m_Animator.SetTrigger(m_HashAppearPara);
                m_SpriteRenderer.enabled = true;
                glowCircle.enabled = true;
            }
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
        VFXController.Instance.Trigger(m_HashDeadEffect, transform.position, 0, false, null);
        deadAudio.PlayRandomSound();
    }

    private void SetNewPosition()
    {
        m_NewPositionIndex++;
        m_Animator.SetTrigger(m_HashHidePara);
    }

    public void MoveToNewPosition()
    {
        if (m_NewPositionIndex == positionsToMove.Length) return;
        transform.position = positionsToMove[m_NewPositionIndex].position;
        m_Animator.SetTrigger(m_HashAppearPara);
    }

    private void SetAttackPlayer()
    {
        m_PlayerPosition = m_Player.transform.position;
        m_Animator.SetBool(m_HashAttackPara, true);
    }

    private bool AttackPlayer()
    {
        if (m_Rigidbody2D.position == m_PlayerPosition || m_GotHit)
        {
            m_GotHit = false;
            m_Animator.SetBool(m_HashAttackPara, false);
            return true;
        }
        else
        {
            m_Rigidbody2D.MovePosition(Vector3.MoveTowards(m_Rigidbody2D.position, m_PlayerPosition, attackSpeed * Time.deltaTime));
            return false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerPlatformerController player = collision.collider.GetComponent<PlayerPlatformerController>();
        if (!player)
        {
            if (collision.contacts[0].normalImpulse > 3f)
            {
                Hurt(collision.contacts[0].normal.normalized * 4f);
            }
        }

    }

    public void Hurt(Vector2 forceDirection)
    {
        CameraShaker.Shake(0.6f, 0.2f);

        m_Rigidbody2D.AddForce(forceDirection * 4f, ForceMode2D.Impulse);

        m_Animator.SetTrigger(m_HashHurtPara);

        m_GotHit = true;

        hitAudio.PlayRandomSound();

        m_Health--;

        Debug.Log(m_Health);
    }

    public void Hurt()
    {
        Hurt(Vector2.right * 2);
    }

}
