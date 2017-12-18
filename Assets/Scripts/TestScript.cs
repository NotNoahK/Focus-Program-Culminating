using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {

	public GameObject planeModel;
	public PlaneWrapper planeWrapper;
	int keyDelay = 10;

	// Use this for initialization
	void Start () {
		planeWrapper = planeModel.GetComponent<PlaneWrapper> ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		keyDelay--;
		if(keyDelay < 0){
			if (Input.GetAxis("Gear")>0.1) {
				planeWrapper.ToggleGear ();
				keyDelay = 50;
			}
			if (Input.GetKeyUp (KeyCode.C)) {
				planeWrapper.ToggleCanopy ();
				keyDelay = 50;
			}

			if (Input.GetKeyUp (KeyCode.H)) {
				planeWrapper.ToggleHook ();
				keyDelay = 50;
			}
		}

		if (Input.GetKeyUp (KeyCode.E)) {
			planeWrapper.Eject ();
		}

		planeWrapper.Propel ((-Input.GetAxis("Throttle")-1)/2);

		planeWrapper.Pitch (Input.GetAxis ("Pitch")*-20);
		planeWrapper.Roll (Input.GetAxis ("Roll")*20);
		planeWrapper.Yaw (Input.GetAxis ("Yaw") * 20);

		InputManager.getAxis (InputManager.Axis.PITCH);
	}
}
