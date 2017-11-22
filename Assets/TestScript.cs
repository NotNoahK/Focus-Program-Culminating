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
		if (Input.GetKeyUp (KeyCode.G) || Input.GetAxis ("Jump") > 0.5) {
			planeWrapper.ToggleGear ();
		}
		if (Input.GetKeyUp (KeyCode.C) || Input.GetAxis ("Fire3") > 0.5) {
			planeWrapper.ToggleCanopy ();
		}

		if (Input.GetKeyUp (KeyCode.H) || Input.GetAxis ("Fire2") > 0.5) {
			planeWrapper.ToggleHook ();
		}

		if (Input.GetKeyUp (KeyCode.E) || (Input.GetAxis ("Fire1") > 0.5 && !Input.GetButton("mouse 0"))) {
			planeWrapper.Eject ();
		}

		planeWrapper.RotateElevator (Input.GetAxis ("Vertical")*-20);
		planeWrapper.Propel (Input.GetAxis ("Horizontal"));
	}
}
