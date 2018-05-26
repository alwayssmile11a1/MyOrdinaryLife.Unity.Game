using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelCanvas : MonoBehaviour {

    public GameObject[] stars;

    private int m_StarCount = 0;



    private void OnEnable()
    {
        for (int i = 0; i < m_StarCount; i++)
        {
            stars[i].gameObject.SetActive(true);
        }
    }



    public void AddOneStar()
    {
        m_StarCount++;
    }


}
