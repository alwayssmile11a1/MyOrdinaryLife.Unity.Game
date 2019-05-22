using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TotalLife : MonoBehaviour
{
    public Text text;
    void Start()
    {
        //if (UIManager.Instance.GetPlayerLife() == null)
        //{
        //    SceneManager.LoadScene(1);
        //}
        text.text = $"x{UIManager.Instance.GetPlayerLife().ToString()}\r\n";
    }
    
}
