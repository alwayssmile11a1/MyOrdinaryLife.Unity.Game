using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameCollection : MonoBehaviour {

    
    public static FrameCollection Instance
    {
        get
        {
            if (m_Instance != null) return m_Instance;

            m_Instance = FindObjectOfType<FrameCollection>();

            if(m_Instance ==null)
            {
                GameObject obj = new GameObject("FrameCollection");

                m_Instance = Instantiate(obj).AddComponent<FrameCollection>();
            }

            return m_Instance;
        }

    }

    public Frame PreviousBeingHoverOnFrame
    {
        get; private set;

    }

    private static FrameCollection m_Instance;

    private Frame[] frames;

	// Use this for initialization
	private void Awake () {
		
        if(Instance != this)
        {
            Destroy(gameObject);
        }


        frames = FindObjectsOfType<Frame>();

	}
	
	public bool FrameContainsPosition(Frame ignoredFrame, Vector3 screenPosition, out Frame frame)
    {
        for (int i = 0; i < frames.Length; i++)
        {
            if (frames[i] == ignoredFrame) continue;

            RectTransform rect = frames[i].GetComponent<RectTransform>();

            if (RectTransformUtility.RectangleContainsScreenPoint(rect, screenPosition, Camera.main ))
            {
                frame = frames[i];
                PreviousBeingHoverOnFrame = frames[i];
                return true;
            }
            
        }

        frame = null;

        return false;

    }

    public void SwitchBetween(Frame frame1, Frame frame2)
    {
        StartCoroutine(InternalSwitchBetween(frame1,frame2));

    }


    private IEnumerator InternalSwitchBetween(Frame frame1, Frame frame2)
    {

        frame1.transform.position = frame2.transform.position;

        while (!Mathf.Approximately((frame2.transform.position - frame1.startPosition).sqrMagnitude, 0f) )
        {
            frame2.transform.position = Vector3.MoveTowards(frame2.transform.position, frame1.startPosition, 3f);

            

            yield return null;
        }




    }

}
