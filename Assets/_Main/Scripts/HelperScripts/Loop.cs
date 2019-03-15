using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loop : MonoBehaviour
{

    public Transform left;
    public Transform right;
    public Transform anchorPosition;
    public Loop targetPosition;

    private void Update()
    {
        if (anchorPosition.position.x + left.localPosition.x * transform.lossyScale.x > transform.position.x + right.localPosition.x * transform.lossyScale.x)
        {

            transform.position = new Vector3(targetPosition.transform.position.x + transform.lossyScale.x * (targetPosition.right.localPosition.x - left.localPosition.x),
                                            transform.position.y, transform.position.z);
        }

        //if (anchorPosition.position.x + right.position.x > transform.position.x - left.position.x)
        //{

        //    transform.position = new Vector3(targetPosition.transform.position.x - targetPosition.left.position.x - right.position.x, transform.position.y, transform.position.z);
        //}
    }

}
