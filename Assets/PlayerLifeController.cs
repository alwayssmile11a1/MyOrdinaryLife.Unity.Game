using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerLife.Instance.UpdatePlayerLife(true);
    }
}
