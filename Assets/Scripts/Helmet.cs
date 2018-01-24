using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// Class to handle heads up display
public class Helmet : MonoBehaviour {

	/// Target rotation
	Vector3 rot;
	public GameObject camera;
	/// Gameobject moved to control HUD lines
	public GameObject gimbal;
	/// Altitude text
	public Text altText;
	/// Speed text
	public Text speedText;
	public GameObject canvas;
	PlaneWrapper plane;

	void Start () {
		//Calibrate target rotation
		rot = transform.eulerAngles;

		plane = GetComponentInParent<PlaneWrapper> ();
	}
	
	void Update () {
		//Y axis follows camera
		rot.y = camera.transform.eulerAngles.y;
		gimbal.transform.eulerAngles = rot;

		//Have canvas follow camera on 2 axes
		canvas.transform.eulerAngles = new Vector3(camera.transform.eulerAngles.x, rot.y, rot.z);

		//Set speed and altitude texts
		speedText.text = plane.speed.ToString().Split('.')[0];
		altText.text = plane.altitude.ToString().Split('.')[0];

	}
}
