using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountDown : MonoBehaviour {
    private float m_TimeToCountDown = 3;
    private float currentTime;
    private TextMeshProUGUI textMeshProUGUI;
	// Use this for initialization
	void Start () {
        currentTime = 0;
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!LoadScene.m_StartGame)
        {
            Debug.Log(LoadScene.m_StartGame);
            //StartCoroutine(countDown());
            m_TimeToCountDown -= (1f/60);
            Debug.Log(m_TimeToCountDown);
            textMeshProUGUI.text = ((int)m_TimeToCountDown).ToString();
            //TimeManager.ChangeTimeBackToNormal();
            if (m_TimeToCountDown < 0)
            {
                LoadScene.m_StartGame = true;
                Debug.Log(LoadScene.m_StartGame);
                TimeManager.ChangeTimeBackToNormal();
                m_TimeToCountDown = 3;
                textMeshProUGUI.gameObject.SetActive(false);
            }
            
        }
	}
    private IEnumerator countDown()
    {
        while (m_TimeToCountDown > 0)
        {
            m_TimeToCountDown -= Time.unscaledDeltaTime;
            Debug.Log(m_TimeToCountDown);
            textMeshProUGUI.text = (m_TimeToCountDown).ToString();
            //TimeManager.ChangeTimeBackToNormal();
            if (currentTime >= m_TimeToCountDown)
            {
                LoadScene.m_StartGame = true;
            }
            yield return null;
        }
    }
}
