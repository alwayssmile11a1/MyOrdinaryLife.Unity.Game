using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Frame : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{





    [HideInInspector]
    public Vector3 startPosition;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]

    private readonly int HashActive = Animator.StringToHash("Active");
    private readonly int HashHoverOn = Animator.StringToHash("HoverOn");
    private RectTransform rectTransfrom;
    private bool m_CanDrag;
    private AlessiaController m_Player;
    private Collider2D[] m_Colliders;
    private SortingGroup m_ObjectsSortingGroup;



    public void Awake()
    {
        startPosition = transform.position;
        animator = GetComponent<Animator>();
        rectTransfrom = GetComponent<RectTransform>();

        m_Colliders = GetComponentsInChildren<Collider2D>();

        m_ObjectsSortingGroup = GetComponentInChildren<SortingGroup>();

        m_Player = FindObjectOfType<AlessiaController>();


        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
       
        if(FrameContainsPosition(Camera.main.WorldToScreenPoint(m_Player.transform.position)))
        {
            m_CanDrag = false;
            return;
        }

        m_CanDrag = true;

        startPosition = transform.position;
        animator.SetBool(HashActive, true);

        m_ObjectsSortingGroup.sortingOrder = 15;


        //Avoid player jump on being-dragged colliders
        SetCollidersActive(false);

        TimeManager.SlowdownTime(0.05f, -1f);
    }

    public void OnDrag(PointerEventData eventData)
    {

        if (!m_CanDrag) return;

        Vector3 touchedPosition = GetTouchedPosition();

        Vector3 position = Camera.main.ScreenToWorldPoint(touchedPosition);
        transform.position = (Vector2)position;

        if (FrameCollection.Instance.PreviousBeingHoverOnFrame != null)
        {
            FrameCollection.Instance.PreviousBeingHoverOnFrame.animator.SetBool(HashHoverOn, false);
        }

        Frame frame;
        if (FrameCollection.Instance.FrameContainsPosition(this, touchedPosition, out frame) &&
            !frame.FrameContainsPosition(Camera.main.WorldToScreenPoint(m_Player.transform.position)))
        {
            frame.animator.SetBool(HashHoverOn, true);
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {

        if (!m_CanDrag) return;



        if (FrameCollection.Instance.PreviousBeingHoverOnFrame != null)
        {
            FrameCollection.Instance.PreviousBeingHoverOnFrame.animator.SetBool(HashHoverOn, false);
        }

        Frame frame;
        if (FrameCollection.Instance.FrameContainsPosition(this, GetTouchedPosition(), out frame) && 
            !frame.FrameContainsPosition(Camera.main.WorldToScreenPoint( m_Player.transform.position)))
        {
            FrameCollection.Instance.SwitchBetween(this, frame);
        }
        else
        {
            transform.position = startPosition;
        }

        animator.SetBool(HashActive, false);

        m_ObjectsSortingGroup.sortingOrder = 2;

        SetCollidersActive(true);

        TimeManager.ChangeTimeBackToNormal();
    }


    public bool FrameContainsPosition(Vector3 screenPosition)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(rectTransfrom, screenPosition, Camera.main);
    }


    public void SetCollidersActive(bool actived)
    {
        for (int i = 0; i < m_Colliders.Length; i++)
        {
            m_Colliders[i].enabled = actived;
        }
    }



    private Vector3 GetTouchedPosition()
    {

#if UNITY_EDITOR

        return  Input.mousePosition;

#else  
        return Input.GetTouch(0).position;
#endif


    }

}
