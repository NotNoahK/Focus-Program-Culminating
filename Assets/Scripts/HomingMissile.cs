using UnityEngine;
using System.Collections;

public class HomingMissile : PayloadScript{


	public GameObject engine;
	Rigidbody body;
	public float dropForce;
	public bool fired = false;
	public int accel;
	public PayloadType type = PayloadType.HOMING_MISSILE;
	CapsuleCollider collider;
	[HideInInspector]
	public float startTime = 0;
	public int maxSpeed;
	PlaneWrapper plane;
	public Explosion explosion;
	bool destroyed = false;
	/// How long the missile is propelled for
	public int maxFuel;
	public int ammo = 0;

	public Transform target;
	public float turnSpeed;


	// Use this for initialization
	void Start () {
		collider = GetComponent<CapsuleCollider> ();
		plane = GetComponentInParent<PlaneWrapper> ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		if(Input.GetKeyDown(KeyCode.Alpha2))
			Fire();
		if (destroyed) {
			if (Time.time-startTime == 1)
				gameObject.SetActive (false);
			transform.Find ("Model").gameObject.SetActive (false);
			body.velocity = Vector3.zero;
			body.isKinematic = true;
			collider.enabled = false;
		}
		else if (fired) {
			if (Time.time-startTime < maxFuel) {
				if (Time.time-startTime > 0.5) {
					body.AddForce (transform.forward * accel);
					transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (target.position - transform.position), turnSpeed);
				}
			}


			if (Time.time-startTime > maxFuel) {
				explosion.Detonate ();
				destroyed = true;
				startTime = 0;
			}
			body.AddForce (new Vector3 (-Physics.gravity.x, -Physics.gravity.y, -Physics.gravity.z));
			if (Time.time-startTime >= 1)	collider.enabled = true;
			if (transform.InverseTransformDirection (body.velocity).x > maxSpeed) {
				body.AddForce (-transform.right * accel * 2);
			}

			Debug.DrawLine (transform.position, target.position);
			Debug.DrawLine (transform.position, transform.position + body.velocity * 10);
		}
	}

	void Fire(){
		gameObject.AddComponent<Rigidbody> ();
		body = gameObject.GetComponent<Rigidbody> ();
		body.AddForce (-transform.forward * dropForce);
		if(engine != null) engine.GetComponent<ParticleSystem> ().Play ();
		print (plane.speed);
		body.AddForce (transform.right * (maxSpeed/2+plane.speed*2));
		fired = true;
		body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		body.angularDrag = 500;
		body.drag = 2.5f;
		startTime = Time.time;
	}

	void OnCollisionEnter(Collision other){
		explosion.Detonate ();
		destroyed = true;
		print ("HIT");

		startTime = 0;
	}
}
