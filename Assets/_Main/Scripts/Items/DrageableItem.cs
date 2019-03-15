using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class DrageableItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [HideInInspector]
    public Vector3 startPosition;


    private int m_HashActivePara = Animator.StringToHash("Active");
    private Animator m_Animator;
    private Collider2D m_Collider2D;
    private float m_OriginalGravity;

    private bool m_CanDrag = true;


    // Use this for initialization
    void Awake()    
    {
        startPosition = transform.position;
        m_Animator = GetComponent<Animator>();
        m_Collider2D = GetComponent<Collider2D>();
        m_Collider2D.enabled = false;
    }



    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!m_CanDrag) return;


        startPosition = transform.position;

        EnableDragEssentials();
    }

    public void OnDrag(PointerEventData eventData)
    {

        if (!m_CanDrag) return;

        Vector3 touchedPosition = GetTouchedPosition();

        Vector3 position = Camera.main.ScreenToWorldPoint(touchedPosition);
        transform.position = new Vector3(position.x, position.y, transform.position.z);


    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!m_CanDrag) return;
            
        Frame frame;

        if (FrameCollection.Instance.FrameContainsPosition(null, GetTouchedPosition(), out frame) && !frame.Disabled)
        {
            m_Collider2D.enabled = true;

        }
        else
        {
            transform.position = startPosition;
        }

        DisableDragEssitials();

    }

    private void EnableDragEssentials()
    {
        if(m_Animator!=null)
        {
            m_Animator.SetBool(m_HashActivePara, true);
        }

        TimeManager.SlowdownTime(0.05f, -1f);
    }


    private void DisableDragEssitials()
    {
        if (m_Animator != null)
        {
            m_Animator.SetBool(m_HashActivePara, false);
        }

        TimeManager.ChangeTimeBackToNormal();
    }

    private Vector3 GetTouchedPosition()
    {

#if UNITY_EDITOR

        return Input.mousePosition;

#else  
        return Input.GetTouch(0).position;
#endif

    }

}
