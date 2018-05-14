using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Frame : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [HideInInspector]
    public Vector3 startPosition;

    public void Awake()
    {
        startPosition = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        startPosition = transform.position;

    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.position = (Vector2)position;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Frame frame;
        if (FrameCollection.Instance.FrameContainsPosition(this, Input.mousePosition, out frame))
        {
            FrameCollection.Instance.SwitchBetween(this, frame);
        }
        else
        {
            transform.position = startPosition;
        }
    }

}
