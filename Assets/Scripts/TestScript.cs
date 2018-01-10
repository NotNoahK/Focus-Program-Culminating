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
			if (InputManager.getButtonUp(InputManager.Button.GEAR)) {
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

		if (InputManager.getButton(InputManager.Button.EJECT)) {
			planeWrapper.Eject ();
		}

		planeWrapper.Propel (InputManager.getAxis(InputManager.Axis.THROTTLE));

		planeWrapper.Pitch (InputManager.getAxis(InputManager.Axis.PITCH));
		planeWrapper.Roll (InputManager.getAxis(InputManager.Axis.ROLL));
		planeWrapper.Yaw (InputManager.getAxis(InputManager.Axis.YAW));
	}
}
