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

    // Use this for initialization
    void Awake()
    {
        startPosition = transform.position;
        m_Animator = GetComponent<Animator>();
    }



    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;

        m_Animator.SetBool(m_HashActivePara, true);

    }

    public void OnDrag(PointerEventData eventData)
    {

        Vector3 touchedPosition = GetTouchedPosition();

        Vector3 position = Camera.main.ScreenToWorldPoint(touchedPosition);
        transform.position = new Vector3(position.x, position.y, transform.position.z);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Frame frame;
        if (FrameCollection.Instance.FrameContainsPosition(null, GetTouchedPosition(), out frame) && !frame.Disabled)
        {
            //Active Item
            m_Animator.SetBool(m_HashActivePara, false);
        }
        else
        {
            transform.position = startPosition;
        }


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
