using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float speed = 1f;

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.position += Vector3.right * speed * Time.deltaTime;
    }
}
