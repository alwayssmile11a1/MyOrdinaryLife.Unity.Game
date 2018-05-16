using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCycleManager : MonoBehaviour {


    public void ReloadScene()
    {

        TimeManager.ChangeTimeBackToNormal();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }




}
