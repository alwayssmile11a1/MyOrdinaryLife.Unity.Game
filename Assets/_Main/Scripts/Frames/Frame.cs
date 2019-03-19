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
    public Rect startRect;
    [HideInInspector]
    public Animator animator;


    private readonly int HashActive = Animator.StringToHash("Active");
    private readonly int HashHoverOn = Animator.StringToHash("HoverOn");
    private readonly int HashDisabled = Animator.StringToHash("Disabled");
    private readonly int HashCharacterOn = Animator.StringToHash("CharacterOn");

    private RectTransform rectTransfrom;
    private PlayerPlatformerController m_Player;
    private List<Collider2D> m_Colliders = new List<Collider2D>();
    private SortingGroup m_ObjectsSortingGroup;
    private Frame m_PreviousBeingHoverOnFrame;
    private Camera m_MainCamera;
    private Vector3 m_TargetMovePosition;
    private Vector3 offsetFromMouse;
    private Transform graphics;
    private Vector3 m_OriginalScale;
    private Vector3 m_MouseDownScale;
    private bool m_NeedToMove;

    public bool Disabled { get; private set; }
    public bool CharacterOn { get; private set; }
    public bool IsBeingDragged { get; private set; }
    public bool graphis { get; private set; }

    private void Awake()
    {
        startPosition = transform.position;
        animator = GetComponent<Animator>();
        rectTransfrom = GetComponent<RectTransform>();
        startRect = rectTransfrom.rect;
        startRect.center = startPosition;
        m_MainCamera = Camera.main;
        graphics = transform.Find("Graphics");
        m_OriginalScale = graphics.localScale;
        m_MouseDownScale = new Vector3(m_OriginalScale.x + 1, m_OriginalScale.y + 1, 1);

        //Collect all colliders except CompositeCollider2D
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] is CompositeCollider2D) continue;
            m_Colliders.Add(colliders[i]);
        }

        m_ObjectsSortingGroup = GetComponentInChildren<SortingGroup>();

        m_Player = FindObjectOfType<PlayerPlatformerController>();


    }

    private void Start()
    {
        if (FrameContainsPosition(m_Player.transform.position))
        {
            SetCharacterOn(true);
        }
    }

    private void Update()
    {
        if(m_NeedToMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_TargetMovePosition, 3f);
            if(Mathf.Approximately((transform.position - m_TargetMovePosition).sqrMagnitude, 0f))
            {
                m_NeedToMove = false;
            }
        }
    }

    private void OnMouseDown()
    {
        if (Disabled || CharacterOn) return;

        graphics.localScale = m_MouseDownScale;
    }

    private void OnMouseUp()
    {
        ResetScale();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        if (Disabled || CharacterOn) return;

        if(FrameContainsPosition(m_Player.transform.position))
        {
            SetCharacterOn(true);
            IsBeingDragged = false;
            return;
        }

        IsBeingDragged = true;

        startPosition = transform.position;

        Vector3 touchedPosition = GetTouchedPosition();
        Vector3 position = m_MainCamera.ScreenToWorldPoint(touchedPosition);
        offsetFromMouse = position - transform.position;

        EnableDragEssentials();
    }


    public void OnDrag(PointerEventData eventData)
    {

        if (!IsBeingDragged) return;

        Vector3 touchedPosition = GetTouchedPosition();

        Vector3 position = m_MainCamera.ScreenToWorldPoint(touchedPosition);
        transform.position = new Vector3(position.x - offsetFromMouse.x, position.y - offsetFromMouse.y, transform.position.z);

        //if (FrameCollection.Instance.PreviousBeingHoverOnFrame != null)
        //{
        //    FrameCollection.Instance.PreviousBeingHoverOnFrame.animator.SetBool(HashHoverOn, false);
        //    //FrameCollection.Instance.MoveFrameTo(FrameCollection.Instance.PreviousBeingHoverOnFrame, FrameCollection.Instance.PreviousBeingHoverOnFrame.startPosition);
        //}

        //Check to see which frame is currently being hovered on
        Frame frame;
        if (m_PreviousBeingHoverOnFrame ==null && FrameCollection.Instance.FrameContainsPosition(this, position, out frame))
        {
            if (!frame.Disabled && !frame.CharacterOn)
            {
                m_PreviousBeingHoverOnFrame = frame;
                //frame.animator.SetBool(HashHoverOn, true);
                frame.TemporaryMoveTo(this.startPosition);
            }
        }
        else
        {
            //Check whether mouse position leaved PreviousbeingHoverOn's rect
            if(m_PreviousBeingHoverOnFrame != null && !m_PreviousBeingHoverOnFrame.startRect.Contains(position))
            {
                //m_PreviousBeingHoverOnFrame.animator.SetBool(HashHoverOn, false);
                m_PreviousBeingHoverOnFrame.TemporaryMoveTo(m_PreviousBeingHoverOnFrame.startPosition);
                //m_PreviousBeingHoverOnFrame.transform.position = m_PreviousBeingHoverOnFrame.startPosition;
                m_PreviousBeingHoverOnFrame = null;
            }
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {

        if (!IsBeingDragged) return;

        if (m_PreviousBeingHoverOnFrame!=null)
        {
            FrameCollection.Instance.SwitchBetween(this, m_PreviousBeingHoverOnFrame);
            m_PreviousBeingHoverOnFrame = null;
        }
        else
        {
            transform.position = startPosition;
        }

        DisableDragEssentials();

        IsBeingDragged = false;

    }

    public void TemporaryMoveTo(Vector3 position)
    {
        m_TargetMovePosition = position;
        m_NeedToMove = true;
    }

    private void EnableDragEssentials()
    {
        animator.SetBool(HashActive, true);

        m_ObjectsSortingGroup.sortingOrder = 15;

        //Avoid player jump on being-dragged colliders
        SetCollidersActive(false);

        TimeManager.SlowdownTime(0f, -1f);
    }

    private void DisableDragEssentials()
    {
        animator.SetBool(HashActive, false);

        m_ObjectsSortingGroup.sortingOrder = 2;

        SetCollidersActive(true);

        TimeManager.ChangeTimeBackToNormal();
    }

    public bool FrameContainsPosition(Vector3 position)
    {
        //return RectTransformUtility.RectangleContainsScreenPoint(rectTransfrom, screenPosition, m_MainCamera);
        return startRect.Contains(position);
    }

    public void ResetPosition(Vector3 position)
    {
        transform.position = position;
        startPosition = position;
        startRect.center = position;
    }

    private void SetCollidersActive(bool actived)
    {
        for (int i = 0; i < m_Colliders.Count; i++)
        {
            m_Colliders[i].enabled = actived;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        PlayerPlatformerController player = collision.GetComponent<PlayerPlatformerController>();

        if (player != null)
        {
            Frame nextFrame = FrameCollection.Instance.GetNextFrame(this);

            if(nextFrame!=null)
            {
                nextFrame.SetCharacterOn(true);
                
                //Calculate new player position
                float x = nextFrame.transform.position.x - nextFrame.GetComponentInChildren<SpriteRenderer>().bounds.extents.x;
                float y = nextFrame.transform.position.y + (player.transform.position.y - transform.position.y);
                //Vector3 newPosition = new Vector3(nextFrame.transform.position.x - nextFrame.GetComponentInChildren<SpriteRenderer>().bounds.extents.x,
                //     nextFrame.transform.position.y + (player.transform.position.y - transform.position.y), 0);

                player.transform.position = new Vector3(x, y, player.transform.position.z);

                ////Since OnTriggerExit can be a little bit late, we can lose a bit of player's movement progress when it's set to the new position. 
                ////Therefore, we have to make up that lost. 
                //player.IncrementVerticalMovement(player.jumpSpeed * 0.1f);

                //Avoid the situation in which player just enters the frame and immediately get out of that frame for some reason. We don't want that to happen
                StartCoroutine(DisableFrameColliderTemporarily(nextFrame));

                nextFrame.ResetScale();
                this.Disable();
            }

        }

    }

    private IEnumerator DisableFrameColliderTemporarily(Frame frame)
    {
        frame.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(1f);
        frame.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void Disable()
    {
        animator.SetBool(HashCharacterOn, false);
        animator.SetTrigger(HashDisabled);
        m_ObjectsSortingGroup.sortingOrder = 2;
        IsBeingDragged = false;
        Disabled = true;
    }

    public void ResetScale()
    {
        if (graphics.localScale != m_OriginalScale)
            graphics.localScale = m_OriginalScale;
    }

    public void SetCharacterOn(bool characterOn)
    {
        animator.SetBool(HashCharacterOn, characterOn);

        if (characterOn && IsBeingDragged)
        {
            transform.position = startPosition;

            //if (FrameCollection.Instance.PreviousBeingHoverOnFrame != null)
            //{
            //    FrameCollection.Instance.PreviousBeingHoverOnFrame.animator.SetBool(HashHoverOn, false);
            //}

            DisableDragEssentials();

        }

        IsBeingDragged = !characterOn;

        CharacterOn = characterOn;
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
