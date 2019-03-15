using UnityEngine;
using UnityEngine.UI;

public class TotalScore : MonoBehaviour {


    public Text text;

    // Use this for initialization
    void Start () {
        text.text = UIManager.Instance.GetTotalStars() + "/" + 36.ToString();
	}
	
}
