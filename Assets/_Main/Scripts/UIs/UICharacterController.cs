using BTAI;
using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharacterController : MonoBehaviour
{
    public Transform LimitLeft;
    public Transform LimitRight;


    private PlayerPlatformerController m_PlayerPlatformerController2D;
    private Animator m_Animator;
    private SpriteRenderer m_SpriteRenderer;
    private float m_TargetXPosition;

    //Behavior Tree
    private Root m_Ai = BT.Root();

    private int m_HashGroundedPara = Animator.StringToHash("Grounded");
    private int m_HashRunPara = Animator.StringToHash("Run");
    private int m_HashJumpPara = Animator.StringToHash("Jump");
    private int m_HashAttack1Para = Animator.StringToHash("Attack1");
    private int m_HashAttack2Para = Animator.StringToHash("Attack2");
    private int m_HashAttack3Para = Animator.StringToHash("Attack3");
    private int m_HashKickPara = Animator.StringToHash("Kick");
    private int m_HashPunchPara = Animator.StringToHash("Punch");
    private int m_HashBowAttackPara = Animator.StringToHash("BowAttack");
    private int m_HashRunSlidePara = Animator.StringToHash("Slide");

    private void Awake()
    {
        m_PlayerPlatformerController2D = GetComponent<PlayerPlatformerController>();
        m_Animator = GetComponent<Animator>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        transform.position = new Vector3(Random.Range(LimitLeft.position.x, LimitRight.position.x), transform.position.y, transform.position.z);

        #region
        //Behaviour tree
        //m_Ai.OpenBranch(
        //    BT.If(() => { return m_Damageable.CurrentHealth >= m_Damageable.startingHealth / 2; }).OpenBranch(

        //         BT.RandomSequence(new int[] { 3, 2, 4 }, 2).OpenBranch(
        //            //Laser follow attack
        //            BT.Sequence().OpenBranch(
        //                BT.Call(() => laserAttackType = 1),
        //                BT.Call(() => m_LineRenderer.colorGradient = laserFollowAttackGradient),
        //                BT.Call(StartLaserAttack),
        //                BT.Wait(1.5f),
        //                BT.Call(() => m_LaserFollowComponent.speed = 1f),
        //                BT.Call(() => m_LineRenderer.enabled = false),
        //                BT.Repeat(15).OpenBranch(
        //                    BT.Call(LaserFollowAttack),
        //                    BT.Wait(0.1f)
        //                ),
        //                BT.Call(() => m_LaserFollowComponent.speed = 2f),
        //                BT.Call(EndLaserAttack),
        //                BT.Wait(1f)
        //            ),
        //            //Cicle laser Attack
        //            BT.Sequence().OpenBranch(
        //                BT.Call(() => laserAttackType = Random.Range(2, 4)),
        //                BT.Call(() => m_LineRenderer.colorGradient = circleLaserAttackGradient),
        //                BT.Call(StartLaserAttack),
        //                BT.WaitUntil(() => Mathf.Abs(m_LaserSweptAngle) >= 360),
        //                BT.Call(() => m_LineRenderer.enabled = false),
        //                BT.Repeat(19).OpenBranch(
        //                    BT.Call(CircleLaserAttack),
        //                    BT.Wait(0.1f)
        //                ),
        //                BT.Call(EndLaserAttack),
        //                BT.Wait(1f)
        //            ),
        //            //Dash
        //            BT.Sequence().OpenBranch(
        //                BT.Call(() => tornadoEffect.Play()),
        //                BT.Wait(1.5f),
        //                BT.Call(DashToLower, 3f, false),
        //                BT.WaitUntil(MoveCheck),
        //                BT.Wait(0.5f),
        //                BT.Call(StartDashing),
        //                BT.Call(DashToLower, 10f, true),
        //                BT.Call(RotateTowardsFuturePosition),
        //                BT.WaitUntil(MoveCheck),
        //                BT.Call(EndDashing),
        //                BT.Wait(0.5f),
        //                BT.Call(FlipSpriteBasedOnSide),
        //                BT.Call(DashToUpper, 3f, false),
        //                BT.WaitUntil(MoveCheck),
        //                BT.Call(() => tornadoEffect.Stop()),
        //                BT.Wait(1.5f)
        //            )

        //        )
        //    ),

        //    BT.If(() => { return m_Damageable.CurrentHealth < m_Damageable.startingHealth / 2; }).OpenBranch(
        //        //Change form animation
        //        BT.If(() => { return !m_FormChanged; }).OpenBranch(
        //            BT.Call(ChangeForm),
        //            BT.WaitForAnimatorState(m_Animator, "ShardKnightAfterTransform"),
        //            BT.Wait(1f)
        //         ),

        //        BT.RandomSequence(new int[] { 3, 3, 2, 2 }, 2).OpenBranch(
        //            //Dash
        //            BT.Sequence().OpenBranch(
        //                BT.Call(() => tornadoEffect.Play()),
        //                BT.Wait(1.5f),
        //                BT.Call(StartDashing),
        //                BT.Call(DashToLower, 10f, true),
        //                BT.Call(RotateTowardsFuturePosition),
        //                BT.WaitUntil(MoveCheck),
        //                BT.Wait(0.2f),
        //                BT.Call(FlipSpriteBasedOnSide),
        //                BT.Call(DashToLower, 12f, true),
        //                BT.Call(RotateTowardsFuturePosition),
        //                BT.WaitUntil(MoveCheck),
        //                BT.Wait(0.2f),
        //                BT.Call(FlipSpriteBasedOnSide),
        //                BT.Call(DashToUpper, 10f, true),
        //                BT.Call(RotateTowardsFuturePosition),
        //                BT.WaitUntil(MoveCheck),
        //                BT.Call(EndDashing),
        //                BT.Call(FlipSpriteBasedOnSide),
        //                BT.Call(() => tornadoEffect.Stop()),
        //                BT.Wait(1.5f)

        //            ),
        //            //Exploding Attack
        //            BT.Sequence().OpenBranch(
        //                BT.Call(() => m_Animator.SetTrigger(m_HashExplodingAttackPara)),
        //                BT.Repeat(5).OpenBranch(
        //                    BT.Call(SpawnConcentratingAttack),
        //                    BT.Wait(0.5f),
        //                    BT.Call(EnableConcentratingAttackDamager),
        //                    BT.Wait(0.8f)
        //                ),
        //                BT.Call(() => m_Animator.SetTrigger(m_HashEndAnimationPara)),
        //                BT.Wait(2f)
        //            ),
        //            //Cicle laser Attack
        //            BT.Sequence().OpenBranch(
        //                BT.Call(() => laserAttackType = Random.Range(2, 4)),
        //                BT.Call(() => m_LineRenderer.colorGradient = circleLaserAttackGradient),
        //                BT.Call(StartLaserAttack),
        //                BT.WaitUntil(() => Mathf.Abs(m_LaserSweptAngle) >= 360),
        //                BT.Call(() => m_LineRenderer.enabled = false),
        //                BT.Repeat(19).OpenBranch(
        //                    BT.Call(CircleLaserAttack),
        //                    BT.Wait(0.1f)
        //                ),
        //                BT.Call(EndLaserAttack),
        //                BT.Wait(1f)
        //            ),
        //            //Meteor shower
        //            BT.Sequence().OpenBranch(
        //                BT.Call(() => m_Animator.SetTrigger(m_HashMeteorShowerAttackPara)),
        //                BT.Wait(0.5f),
        //                BT.Call(() => concentratingStateEffect.Play()),
        //                BT.Wait(1f),
        //                BT.Repeat(30).OpenBranch(
        //                    BT.Call(MeteorShowerAttack),
        //                    BT.Wait(0.2f)
        //                ),
        //                BT.Call(() => m_Animator.SetTrigger(m_HashEndAnimationPara)),
        //                BT.Call(() => concentratingStateEffect.Stop()),
        //                BT.Wait(1f)
        //            )
        //         ),

        //        BT.Call(OrientToTarget)
        //      )


        //);
        #endregion

        m_Ai.OpenBranch(
            BT.Sequence().OpenBranch(
                BT.RandomSequence(new int[] { 3, 1, 1, 1, 1, 1, 1 }).OpenBranch(
                     //Move to random position
                     BT.Sequence().OpenBranch(
                        BT.Call(SetRandomXPosition),
                        BT.WaitUntil(() => Mathf.Abs(m_TargetXPosition - transform.position.x) <= 0.2f),
                        BT.Call(() => m_PlayerPlatformerController2D.speed = 0f),
                        BT.Wait(1.5f)
                     ),
                     //Attack1
                     BT.Sequence().OpenBranch(
                        BT.Call(() => m_Animator.SetTrigger(m_HashAttack1Para)),
                        BT.Wait(1.5f)
                     ),
                     //Attack2
                     BT.Sequence().OpenBranch(
                        BT.Call(() => m_Animator.SetTrigger(m_HashAttack2Para)),
                        BT.Wait(1.5f)
                     ),
                     //Attack3
                     BT.Sequence().OpenBranch(
                        BT.Call(() => m_Animator.SetTrigger(m_HashAttack3Para)),
                        BT.Wait(1.5f)
                     ),
                     //Kick
                     BT.Sequence().OpenBranch(
                        BT.Call(() => m_Animator.SetTrigger(m_HashKickPara)),
                        BT.Wait(1.5f)
                     ),
                     //Punch
                     BT.Sequence().OpenBranch(
                        BT.Call(() => m_Animator.SetTrigger(m_HashPunchPara)),
                        BT.Wait(2.5f)
                     ),
                     //BowAttack
                     BT.Sequence().OpenBranch(
                        BT.Call(() => m_Animator.SetTrigger(m_HashBowAttackPara)),
                        BT.Wait(1.5f)
                     )
                ),
                BT.Wait(2)
          )
        );

    }

    private void Update()
    {
        m_Ai.Tick();
    }

    private void SetRandomXPosition()
    {
        m_TargetXPosition = Random.Range(LimitLeft.position.x, LimitRight.position.x);
        m_PlayerPlatformerController2D.speed = 3.5f;
        m_SpriteRenderer.flipX = m_TargetXPosition > transform.position.x ? false : true;
    }

}
