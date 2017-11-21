using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {

	public GameObject planeModel;
	public PlaneWrapper planeWrapper;


	// Use this for initialization
	void Start () {
		planeWrapper = planeModel.GetComponent<PlaneWrapper> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.G)) {
			planeWrapper.ToggleGear ();
		}
		if (Input.GetKeyUp (KeyCode.C)) {
			planeWrapper.ToggleCanopy ();
		}

		if (Input.GetKeyUp (KeyCode.H)) {
			planeWrapper.ToggleHook ();
		}

		if (Input.GetKeyUp (KeyCode.E)) {
			planeWrapper.Eject ();
		}

		planeWrapper.RotateElevator (Input.GetAxis ("Vertical")*-20);
		planeWrapper.RotateRudder (Input.GetAxis ("Horizontal")*20);
	}
}
