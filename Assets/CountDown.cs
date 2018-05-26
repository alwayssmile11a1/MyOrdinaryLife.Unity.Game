using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountDown : MonoBehaviour {
    private float m_TimeToCountDown = 3;
    private float currentTime;
    private TextMeshProUGUI textMeshProUGUI;


    private bool m_StartGame = false;



	// Use this for initialization
	void Start () {
        currentTime = 0;
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        TimeManager.SlowdownTime(0, -1);
	}
	
	// Update is called once per frame
	void Update () {
        if (!m_StartGame)
        {
           
            //StartCoroutine(countDown());
            m_TimeToCountDown -= (1f/60);
            Debug.Log(m_TimeToCountDown);
            textMeshProUGUI.text = ((int)m_TimeToCountDown).ToString();
            //TimeManager.ChangeTimeBackToNormal();
            if (m_TimeToCountDown < 0)
            {
                m_StartGame = true;
                TimeManager.ChangeTimeBackToNormal();
                m_TimeToCountDown = 3;
                textMeshProUGUI.gameObject.SetActive(false);
            }
            
        }
	}
   
}
