using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {


    public float startDelay = 0f;
    public float changeAnimationTimer = 2f;

    private int m_HashHurtPara = Animator.StringToHash("Hurt");
    private int m_HashPunchPara = Animator.StringToHash("Punch");
    private int m_HashKickPara = Animator.StringToHash("Kick");


    private Animator m_Animator;

    private float m_Timer;
    private float m_ChangeAnimationTimer =0.001f;

	// Use this for initialization
	void Awake () {
        m_Animator = GetComponent<Animator>();
        m_Timer = startDelay;
    }



    private void Update()
    {
            
        if(m_Timer >0)
        {
            m_Timer -= Time.deltaTime;
        }
        else
        {

            if(m_ChangeAnimationTimer>0)
            {
                m_ChangeAnimationTimer -= Time.deltaTime;

                if(m_ChangeAnimationTimer<=0)
                {
                    int randomNumber = Random.Range(0, 2);


                    switch(randomNumber)
                    {
                        case 0:
                            {
                                m_Animator.SetTrigger(m_HashPunchPara);


                                break;
                            }

                        case 1:
                            {
                                m_Animator.SetTrigger(m_HashKickPara);

                                break;
                            }



                    }

                    m_ChangeAnimationTimer = changeAnimationTimer;
                }

            }
            


        }



    }









}
