using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gimbal : MonoBehaviour {

	Vector3  rot;
	Vector3 scale;
	public GameObject camera;

	// Use this for initialization
	void Start () {
		rot = transform.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
		rot.y = camera.transform.eulerAngles.y;
		transform.eulerAngles = rot;// new Vector3 (0, transform.eulerAngles.y,  transform.eulerAngles.z);
		transform.position = camera.transform.position;
	}
}
