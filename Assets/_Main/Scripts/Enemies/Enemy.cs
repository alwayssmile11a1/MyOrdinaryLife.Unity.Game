using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Tooltip("If the sprite face left on the spritesheet, enable this. Otherwise, leave disabled")]
    public bool spriteFaceLeft = false;

    [Header("Scanning settings")]
    [Tooltip("The angle of the forward of the view cone. 0 is forward of the sprite, 90 is up, 180 behind etc.")]
    [Range(0.0f, 360.0f)]
    public float viewDirection = 0.0f;
    [Range(0.0f, 360.0f)]
    public float viewFov = 180f;
    public float viewDistance = 2f;

    [Header("Melee Attack Data")]
    public Damager meleeDamager;

    [Header("Animation")]
    public string meleeAttackTransitionName = "MeleeAttack";
    public string deathTransitionName = "Dead";
    public string hitTransitionName = "Hit";

    [Header("Audio")]
    public RandomAudioPlayer meleeAttackAudio;
    public RandomAudioPlayer dieAudio;
    public RandomAudioPlayer hitAudio;

    [Header("Misc")]
    public string hitEffectName = "HitEffect";
    public string deadEffectName = "DeadEffect";

    protected PlayerPlatformerController m_Player;

    //as we flip the sprite instead of rotating/scaling the object, this give the forward vector according to the sprite orientation
    protected Vector2 m_SpriteForward;
    protected SpriteRenderer m_SpriteRenderer;
    protected Animator m_Animator;
    protected bool m_IsAttacking = false;
    protected bool m_Dead = false;

    protected int m_HashDeadEffect;
    protected int m_HashHitEffect;

    public int HashMeleeAttackPara { get; private set; }
    public int HashDeathPara { get; private set; }
    public int HashHitPara { get; private set; }


    protected void Awake()
    {
        HashMeleeAttackPara = Animator.StringToHash(meleeAttackTransitionName);
        HashDeathPara = Animator.StringToHash(deathTransitionName);
        HashHitPara = Animator.StringToHash(hitTransitionName);

        m_HashDeadEffect = VFXController.StringToHash(deadEffectName);
        m_HashHitEffect = VFXController.StringToHash(hitEffectName);
        
        m_Animator = GetComponent<Animator>();
        if (m_Animator == null)
        {
            m_Animator = GetComponentInChildren<Animator>();
        }

        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        if (m_SpriteRenderer == null)
        {
            m_SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        m_SpriteForward = spriteFaceLeft ? Vector2.left : Vector2.right;
        if (m_SpriteRenderer.flipX) m_SpriteForward = -m_SpriteForward;
    }

    protected void Start()
    {
        m_Player = GameManager.Instance.GetPlayer();
    }

    protected void Update()
    {
        if(!m_IsAttacking && !m_Player.IsDead() && IsPlayerVisible())
        {
            OnSpotPlayer();
        }
    }

    protected virtual void OnSpotPlayer()
    {
        PerformMeleeAttack();
    }

    public virtual void OnHit()
    {
        gameObject.SetActive(false);
    }

    public bool IsPlayerVisible()
    {
        Vector3 dir = m_Player.transform.position - transform.position;

        if (dir.sqrMagnitude > viewDistance * viewDistance)
        {
            return false;
        }

        Vector3 testForward = Quaternion.Euler(0, 0, spriteFaceLeft ? Mathf.Sign(m_SpriteForward.x) * -viewDirection : Mathf.Sign(m_SpriteForward.x) * viewDirection) * m_SpriteForward;

        float angle = Vector3.Angle(testForward, dir);

        if (angle > viewFov * 0.5f)
        {

            return false;
        }

        return true;
    }

    public void PerformMeleeAttack()
    {
        m_Animator.SetTrigger(HashMeleeAttackPara);

        if (meleeAttackAudio != null)
            meleeAttackAudio.PlayRandomSound();

        m_IsAttacking = true;
    }

    //This is called when the damager get enabled (so the enemy can damage the player). Likely be called by the animation throught animation event (see the attack animation of the Chomper)
    public void StartMeleeAttack()
    {
        if (meleeDamager != null)
        {
            meleeDamager.gameObject.SetActive(true);
            meleeDamager.EnableDamage();
        }
    }

    public void EndMeleeAttack()
    {
        if (meleeDamager != null)
        {
            meleeDamager.gameObject.SetActive(false);
            meleeDamager.DisableDamage();
        }

        m_IsAttacking = false;
    }

    public void Die(Damager damager, Damageable damageable)
    {
        //Vector2 throwVector = new Vector2(0, 2.0f);
        //Vector2 damagerToThis = damager.transform.position - transform.position;

        //throwVector.x = Mathf.Sign(damagerToThis.x) * -4.0f;
        //SetMoveVector(throwVector);
        if (HashDeathPara != 0)
        {
            m_Animator.SetTrigger(HashDeathPara);
        }

        PlayDieAudio();

        m_Dead = true;

        gameObject.SetActive(false);
        
        if (m_HashDeadEffect != 0)
        {
            VFXController.Instance.Trigger(m_HashDeadEffect, transform.position, 0, m_SpriteForward.x > 0 ? false : true, null);
        }
    }

    public void Hit(Damager damager, Damageable damageable)
    {
        if (HashHitPara != 0)
        {
            m_Animator.SetTrigger(HashHitPara);
        }

        if (m_HashHitEffect != 0)
        {
            VFXController.Instance.Trigger(m_HashHitEffect, transform.position, 0, m_SpriteForward.x > 0 ? false : true, null);
        }

        PlayHitAudio();
    }

    public void PlayMeleeAttackAudio()
    {
        if (meleeAttackAudio != null)
            meleeAttackAudio.PlayRandomSound();
    }

    public void PlayDieAudio()
    {
        if (dieAudio != null)
            dieAudio.PlayRandomSound();
    }

    public void PlayHitAudio()
    {
        if (hitAudio != null)
            hitAudio.PlayRandomSound();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        //draw the cone of view
        Vector3 forward = spriteFaceLeft ? Vector2.left : Vector2.right;
        forward = Quaternion.Euler(0, 0, spriteFaceLeft ? -viewDirection : viewDirection) * forward;


        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        if (spriteRenderer.flipX) forward.x = -forward.x;

        //Vector3 endpoint = transform.position + (Quaternion.Euler(0, 0, viewFov * 0.5f) * forward);
        UnityEditor.Handles.color = new Color(0, 1.0f, 0, 0.2f);
        UnityEditor.Handles.DrawSolidArc(transform.position, -Vector3.forward, (Quaternion.Euler(0, 0, viewFov * 0.5f) * forward).normalized, viewFov, viewDistance);

    }
#endif

}
