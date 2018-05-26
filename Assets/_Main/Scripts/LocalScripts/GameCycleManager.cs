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


    public void FinishLevel(Collider2D collision)
    {
        collision.gameObject.SetActive(false);

        //Show dialog panel or something here

    }


}
