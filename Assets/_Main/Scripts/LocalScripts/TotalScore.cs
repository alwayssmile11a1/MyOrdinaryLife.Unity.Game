using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TotalScore : MonoBehaviour {


    public int ScoresCount { set; get; }


    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }


    // Use this for initialization
    void Start () {


        text.SetText(ScoresCount + "/" + 36.ToString());

	}
	
	






}
