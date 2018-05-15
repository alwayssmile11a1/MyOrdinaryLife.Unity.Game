using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCycleManager : MonoBehaviour {


    private PlayerPlatformerController m_Player;

	// Use this for initialization
	void Awake () {

        m_Player = FindObjectOfType<PlayerPlatformerController>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}




}
