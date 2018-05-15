using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Frame : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [HideInInspector]
    public Vector3 startPosition;
    [HideInInspector]
    public Animator animator;

    private readonly int HashActive = Animator.StringToHash("Active");
    private readonly int HashHoverOn = Animator.StringToHash("HoverOn");
    private RectTransform rectTransfrom;

    private bool m_CanDrag;

    private AlessiaController m_Player;

    public void Awake()
    {
        startPosition = transform.position;
        animator = GetComponent<Animator>();
        rectTransfrom = GetComponent<RectTransform>();

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

        TimeManager.SlowdownTime(0.05f, -1f);

        startPosition = transform.position;
        animator.SetBool(HashActive, true);



    }

    public void OnDrag(PointerEventData eventData)
    {

        if (!m_CanDrag) return;

        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.position = (Vector2)position;

        Frame frame;

        if (FrameCollection.Instance.PreviousBeingHoverOnFrame != null)
        {
            FrameCollection.Instance.PreviousBeingHoverOnFrame.animator.SetBool(HashHoverOn, false);
        }

        if (FrameCollection.Instance.FrameContainsPosition(this, Input.mousePosition, out frame))
        {
            frame.animator.SetBool(HashHoverOn, true);
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {

        if (!m_CanDrag) return;

        Frame frame;

        if (FrameCollection.Instance.PreviousBeingHoverOnFrame != null)
        {
            FrameCollection.Instance.PreviousBeingHoverOnFrame.animator.SetBool(HashHoverOn, false);
        }

        if (FrameCollection.Instance.FrameContainsPosition(this, Input.mousePosition, out frame))
        {
            FrameCollection.Instance.SwitchBetween(this, frame);
        }
        else
        {
            transform.position = startPosition;
        }

        animator.SetBool(HashActive, false);

        TimeManager.ChangeTimeBackToNormal();
    }


    private bool FrameContainsPosition(Vector3 screenPosition)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(rectTransfrom, screenPosition, Camera.main);
    }

}
