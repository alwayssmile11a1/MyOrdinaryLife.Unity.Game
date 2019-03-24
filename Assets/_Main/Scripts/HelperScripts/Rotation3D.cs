using System.Collections;
using UnityEngine;


public class Rotation3D : MonoBehaviour {
	public float xRotation = 0F;
	public float yRotation = 0F;
	public float zRotation = 0F;

	private void FixedUpdate(){
        this.transform.localEulerAngles += new Vector3(xRotation, yRotation, zRotation) * Time.deltaTime;
	}
}
