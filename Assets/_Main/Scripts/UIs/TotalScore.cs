using UnityEngine;
using UnityEngine.UI;

public class TotalScore : MonoBehaviour {


    public Text text;
    public bool shortForm = false;

    void Start () {
        if (shortForm)
        {
            text.text = UIManager.Instance.GetCurrentTotalScore().ToString();
        }
        else
        {
            text.text = $"{UIManager.Instance.GetCurrentTotalScore()}/{UIManager.Instance.GetTotalStars()}";
        }
	}
	
}
