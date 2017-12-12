using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : MonoBehaviour {

	public GameObject engine;
	Rigidbody body;
	public float dropForce;
	public bool fired = false;
	public int accel;
	public string type;
	CapsuleCollider collider;
	[HideInInspector]
	public int lifetime = 0;
	public int maxSpeed;
	PlaneWrapper plane;

	// Use this for initialization
	void Start () {
		collider = GetComponent<CapsuleCollider> ();
		plane = GetComponentInParent<PlaneWrapper> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.Alpha1))
			Fire();
		if (fired) {
			lifetime++;
			body.AddForce (transform.right * accel);
			body.AddForce (new Vector3 (-Physics.gravity.x, -Physics.gravity.y, -Physics.gravity.z));
			if (lifetime == 15)	collider.enabled = true;
//			print (transform.InverseTransformDirection (body.velocity).x);
			if (transform.InverseTransformDirection (body.velocity).x > maxSpeed) {
				body.AddForce (-transform.right * accel * 2);
			}
		}
	}

	void Fire(){
		gameObject.AddComponent<Rigidbody> ();
		body = gameObject.GetComponent<Rigidbody> ();
		body.AddForce (-transform.forward * dropForce);
		engine.GetComponent<ParticleSystem> ().Play ();
		print (plane.speed);
		body.AddForce (transform.right * (maxSpeed/2+plane.speed*2));
		fired = true;
	}
}
