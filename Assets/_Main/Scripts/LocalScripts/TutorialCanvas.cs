using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCanvas : MonoBehaviour {

    public GameObject pointer;

    private bool m_Ended = false;


    private void Update()
    {

        if (m_Ended) return;


        for (int i = 0; i < FrameCollection.Instance.FrameCount; i++)
        {
            if(FrameCollection.Instance.GetFrame(i).IsBeingDragged)
            {
                Ended();
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_Ended) return;

        if (collision.GetComponent<PlayerPlatformerController>() != null)
        {
            pointer.SetActive(true);
            TimeManager.SlowdownTime(0, -1);
        }
    }


    public void Ended()
    {
        m_Ended = true;
        pointer.SetActive(false);
        gameObject.SetActive(false);
    }

}
