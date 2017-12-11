using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eject : MonoBehaviour {

	Rigidbody body;
	bool fired = false;
	int maxTimer = 30;
	int timer;

	void Start(){
		timer = maxTimer;
	}

	// Update is called once per frame
	void Update () {
		if (fired) {
			timer--;
			if (timer == maxTimer) {
				gameObject.AddComponent<Rigidbody> ();
				body = GetComponent<Rigidbody> ();
				body.AddRelativeForce (new Vector3 (0, 0, 50));
			}
			if (timer < maxTimer) {
				if (timer > 0)
					body.AddForce (transform.forward*(Mathf.Abs (timer - maxTimer)+10)*50);
				//Wait until canopy is cleared
				if (timer == maxTimer - 2)
					GetComponent<MeshCollider> ().enabled = true;
			}
			if (timer == 0) {
				body.drag = 0.5f;
				body.angularDrag = 0.5f;
			}
		}
		
	}

	public void Fire(int fireDelay){
		fired = true;
		timer += fireDelay +1;
	}
}
