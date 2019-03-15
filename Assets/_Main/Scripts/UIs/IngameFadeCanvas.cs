using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameFadeCanvas : MonoBehaviour
{
    private Animator m_Animator;
    private int m_HashFadeOutPara = Animator.StringToHash("FadeOut");

    void Awake()
    {
        m_Animator = GetComponent<Animator>();
        GameManager.Instance.RegisterIngameFadeCanvas(this);
    }

    public void FadeSceneOut()
    {
        m_Animator.SetTrigger(m_HashFadeOutPara);
    }

}
