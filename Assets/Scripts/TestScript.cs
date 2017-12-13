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
	void Update () {
		keyDelay--;
		if(keyDelay < 0){
			if (Input.GetKeyUp (KeyCode.G) || Input.GetAxis ("Jump") > 0.5) {
				planeWrapper.ToggleGear ();
				keyDelay = 50;
			}
			if (Input.GetKeyUp (KeyCode.C) || Input.GetAxis ("Fire3") > 0.5) {
				planeWrapper.ToggleCanopy ();
				keyDelay = 50;
			}

			if (Input.GetKeyUp (KeyCode.H)) {
				planeWrapper.ToggleHook ();
				keyDelay = 50;
			}
		}

		if (Input.GetKeyUp (KeyCode.E) || (Input.GetAxis ("Fire1") > 0.5)) {
			planeWrapper.Eject ();
		}

		planeWrapper.Propel (-Input.GetAxis("Throttle"));

		planeWrapper.Pitch (Input.GetAxis ("Vertical")*-20);
		planeWrapper.Roll (Input.GetAxis ("Horizontal")*20);
	}
}
