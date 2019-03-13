using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidOnly : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
#if UNITY_ANDROID
        gameObject.SetActive(true);
#else
        gameObject.SetActive(false);
#endif
    }
}
