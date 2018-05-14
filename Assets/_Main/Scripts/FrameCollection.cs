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


    private static FrameCollection m_Instance;

    private Frame[] frames;
    private 

	// Use this for initialization
	void Awake () {
		
        if(Instance != this)
        {
            Destroy(gameObject);
        }


        frames = FindObjectsOfType<Frame>();

	}
	
	public bool FrameContainsPosition(Frame ignoreFrame, Vector3 position, out Frame frame)
    {

        for (int i = 0; i < frames.Length; i++)
        {
            if (frames[i] == ignoreFrame) continue;

            RectTransform rect = frames[i].GetComponent<RectTransform>();

            if (RectTransformUtility.RectangleContainsScreenPoint(rect, position,Camera.main ))
            {
                frame = frames[i];
                return true;
            }
            
        }

        frame = null;

        return false;

    }

    public void SwitchBetween(Frame frame1, Frame frame2)
    {
        frame1.transform.position = frame2.transform.position;

        frame2.transform.position = frame1.startPosition;


    }


}
