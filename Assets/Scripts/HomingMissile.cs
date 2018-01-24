using UnityEngine;
using System.Collections;

/// Controls homing missiles, extends PayloadScript

public class HomingMissile : PayloadScript{


	Rigidbody body;
	CapsuleCollider collider;
	/// Time at launch, gets reset when the missile is destroyed
	[HideInInspector]
	public float startTime = 0;
	PlaneWrapper plane;
	/// Flag for missile destroyed, true after missile detonation
	bool destroyed = false;
	/// Missile's target
	public Transform target;
	/// Speed at which the missile turns
	public float turnSpeed;

	void Start () {
		collider = GetComponent<CapsuleCollider> ();
		plane = GetComponentInParent<PlaneWrapper> ();
	}

	void FixedUpdate () {
		if (destroyed) {
			//Wait until 1 second after detonation to disable
			if (Time.time-startTime == 1)
				gameObject.SetActive (false);
			transform.Find ("Model").gameObject.SetActive (false);
			//Kill all speed and disable physics/collider
			body.velocity = Vector3.zero;
			body.isKinematic = true;
			collider.enabled = false;
		}
		else if (fired) {
			//If there is still fuel
			if (Time.time-startTime < maxFuel) {
				//If the missile has been in flight for at least 1/2 a second
				if (Time.time-startTime > 0.5) {
					//Add force and rotate twords target
					body.AddForce (transform.forward * accel);
					transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (target.position - transform.position), turnSpeed);
				}
			}

			//If the fuel is expended, destroy the missile
			if (Time.time-startTime > maxFuel) {
				explosion.Detonate ();
				destroyed = true;
				startTime = 0;
			}
			//Annull gravity
			body.AddForce (new Vector3 (-Physics.gravity.x, -Physics.gravity.y, -Physics.gravity.z));
			//Enable collider after 1 second of flight
			if (Time.time-startTime >= 1)	collider.enabled = true;
			//Decelerate if above max speed
			if (transform.InverseTransformDirection (body.velocity).x > maxSpeed) {
				body.AddForce (-transform.right * accel * 2);
			}
				
			Debug.DrawLine (transform.position, target.position);
			Debug.DrawLine (transform.position, transform.position + body.velocity * 10);
		}
	}

	/// Fire the missile
	public override void Fire(){
		gameObject.AddComponent<Rigidbody> ();
		body = gameObject.GetComponent<Rigidbody> ();
		//Seperate from plane
		body.AddForce (-transform.forward * dropForce);
		//Start engine particles
		if(engine != null) engine.GetComponent<ParticleSystem> ().Play ();
		print (plane.speed);
		// Match plane speed
		body.AddForce (transform.right * (maxSpeed/2+plane.speed*2));
		fired = true;
		//Set collision detection mode
		body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		//Set drag
		body.angularDrag = 500;
		body.drag = 2.5f;
		//Set timer
		startTime = Time.time;
        //Reduce ammo
        ammo--;
	}

	void OnCollisionEnter(Collision other){
		explosion.Detonate ();
		destroyed = true;
		print ("HIT");

		startTime = 0;
	}
}
