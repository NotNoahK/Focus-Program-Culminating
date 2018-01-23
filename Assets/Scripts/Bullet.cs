using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Class to handle bullet particles
public class Bullet: MonoBehaviour {

	/// Time of impact
	float startTime;


	void Start () {
		startTime = Time.time;
	}
	
	void Update () {
		//Wait one second for the particles to finish
		if (Time.time - startTime >= 1) {
			//Destroy gameobject to avoid clutter
			Destroy (gameObject);
		}
	}
}
