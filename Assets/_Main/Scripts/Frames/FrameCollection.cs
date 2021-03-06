﻿using System.Collections;
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

    //public Frame PreviousBeingHoverOnFrame
    //{
    //    get; private set;

    //}

    public int FrameCount { get; private set; }

    private static FrameCollection m_Instance;

    private Frame[] m_Frames;

    

	// Use this for initialization
	private void Awake () {
		
        if(Instance != this)
        {
            Destroy(gameObject);
        }

        m_Frames = GetComponentsInChildren<Frame>();

        FrameCount = m_Frames.Length;

        //for (int i = 0; i < m_Frames.Length; i++)
        //{
        //    Debug.Log(m_Frames[i].name);
        //}
	}
	
	public bool FrameContainsPosition(Frame ignoredFrame, Vector3 position, out Frame frame)
    {
        for (int i = 0; i < m_Frames.Length; i++)
        {
            if (m_Frames[i] == ignoredFrame) continue;

            //RectTransform rect = m_Frames[i].GetComponent<RectTransform>();

            //if (RectTransformUtility.RectangleContainsScreenPoint(rect, screenPosition, Camera.main ))
            if(m_Frames[i].startRect.Contains(position))
            {
                frame = m_Frames[i];
                //PreviousBeingHoverOnFrame = m_Frames[i];
                return true;
            }
            
        }

        frame = null;

        return false;

    }

    public void SwitchBetween(Frame frame1, Frame frame2)
    {
        //StartCoroutine(InternalSwitchBetween(frame1,frame2));

        InternalSwitchBetween(frame1, frame2);
    }


    private void InternalSwitchBetween(Frame frame1, Frame frame2)
    {

        //Switch order of two frames
        for (int i = 0; i < m_Frames.Length; i++)
        {
            if(m_Frames[i]==frame1)
            {
                m_Frames[i] = frame2;
            }
            else
            {
                if (m_Frames[i] == frame2)
                {
                    m_Frames[i] = frame1;
                }
            }
        }

        Vector3 starPosition1 = frame1.startPosition;
        Vector3 starPosition2 = frame2.startPosition;

        frame1.ResetPosition(starPosition2);
        frame2.ResetPosition(starPosition1);

        //while (!Mathf.Approximately((frame2.transform.position - frame1.startPosition).sqrMagnitude, 0f) )
        //{
        //    frame2.transform.position = Vector3.MoveTowards(frame2.transform.position, frame1.startPosition, 3f);


        //    yield return null;
        //}

    }

    public Frame GetNextFrame(Frame frame)
    {
        for (int i = 0; i < m_Frames.Length; i++)
        {
            if(m_Frames[i]==frame && i+1 < m_Frames.Length)
            {
                return m_Frames[i+1];
            }
        }


        return null;
    }

    public Frame GetFrame(int index)
    {
        return m_Frames[index];
    }

    //public void MoveFrameTo(Frame frame, Vector3 position)
    //{
    //    StartCoroutine(InternalMoveFrameTo(frame, position));
    //}

    //private IEnumerator InternalMoveFrameTo(Frame frame, Vector3 position)
    //{
    //    while (!Mathf.Approximately((frame.transform.position - position).sqrMagnitude, 0f))
    //    {
    //        frame.transform.position = Vector3.MoveTowards(frame.transform.position, position, 3f);

    //        yield return null;
    //    }
    //}

}
