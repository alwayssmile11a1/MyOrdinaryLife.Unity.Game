using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {


    public float startDelay = 0f;
    public float changeAnimationTimer = 2f;

    private int m_HashSkill1Para = Animator.StringToHash("Skill1");
    private int m_HashSkill2Para = Animator.StringToHash("Skill2");
    private int m_HashDisappear = Animator.StringToHash("Disappearing");


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
                                m_Animator.SetTrigger(m_HashSkill1Para);


                                break;
                            }

                        case 1:
                            {
                                m_Animator.SetTrigger(m_HashSkill2Para);

                                break;
                            }



                    }

                    m_ChangeAnimationTimer = changeAnimationTimer;
                }

            }
            


        }



    }


    public void Disappear()
    {

        m_Animator.SetTrigger(m_HashDisappear);

    }



    public void Disable()
    {
        gameObject.SetActive(false);
    }





}
