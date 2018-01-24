using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Script that handles ejection sequence and seats post-ejection
public class Eject : MonoBehaviour {


	Rigidbody body;
	/// Fired flag, true if ejection has been started
	bool fired = false;
	/// Timer value at start of ejection sequence
	int maxTimer = 30;
	/// Timer to schedule ejection sequence events
	int timer;

	void Start(){
		timer = maxTimer;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (fired) {
			timer--;
			//Add rigidbody and inital force
			if (timer == maxTimer) {
				gameObject.AddComponent<Rigidbody> ();
				body = GetComponent<Rigidbody> ();
				body.AddRelativeForce (new Vector3 (0, 0, 50));
			}
			if (timer < maxTimer) {
				//Add continued force
				if (timer > 0)
					body.AddForce (transform.forward*(Mathf.Abs (timer - maxTimer)+10)*50);
				//Wait until canopy is cleared to enable collider
				if (timer == maxTimer - 50)
					GetComponent<MeshCollider> ().enabled = true;
			}
			//Set drag for descent
			if (timer == 0) {
				body.drag = 0.5f;
				body.angularDrag = 0.5f;
			}
		}

	}

	/// Start ejection sequence
	/// fireDelay: delay before ejection sequence starts
	public void Fire(int fireDelay){
		fired = true;
		timer += fireDelay +1;
	}
}
