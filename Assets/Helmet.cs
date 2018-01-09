using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helmet : MonoBehaviour {

	Vector3  rot;
	Vector3 scale;
	public GameObject camera;
	public GameObject gimbal;
	public GameObject altText;
	public GameObject speedText;
	public GameObject canvas;

	// Use this for initialization
	void Start () {
		rot = transform.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
		rot.y = camera.transform.eulerAngles.y;
		gimbal.transform.eulerAngles = rot;// new Vector3 (0, transform.eulerAngles.y,  transform.eulerAngles.z);

		canvas.transform.eulerAngles = new Vector3(camera.transform.eulerAngles.x, rot.y, rot.z);

	}
}
