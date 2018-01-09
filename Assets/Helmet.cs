using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Helmet : MonoBehaviour {

	Vector3  rot;
	Vector3 scale;
	public GameObject camera;
	public GameObject gimbal;
	public Text altText;
	public Text speedText;
	public GameObject canvas;
	PlaneWrapper plane;

	// Use this for initialization
	void Start () {
		rot = transform.eulerAngles;

		plane = GetComponentInParent<PlaneWrapper> ();
	}
	
	// Update is called once per frame
	void Update () {
		rot.y = camera.transform.eulerAngles.y;
		gimbal.transform.eulerAngles = rot;// new Vector3 (0, transform.eulerAngles.y,  transform.eulerAngles.z);

		canvas.transform.eulerAngles = new Vector3(camera.transform.eulerAngles.x, rot.y, rot.z);

		speedText.text = plane.speed.ToString().Split('.')[0];

	}
}
