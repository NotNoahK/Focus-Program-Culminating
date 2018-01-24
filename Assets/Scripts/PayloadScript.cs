using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Types of payloads that can be attached to wings
public enum PayloadType{
    //NOTE: Fuel payloads are not funtional, they are here for expansion purposes
	MISSILE,BOMB,FUEL,HOMING_MISSILE,EMPTY
}


/// Controls payloads that can be attached to wings, superclass for homing missile
public class PayloadScript : MonoBehaviour {

    /// The source of particle effects for missiles
	public GameObject engine;
	Rigidbody body;
    /// Force that seperates payload from wing
	public float dropForce;
    /// Payload fired flag, true when payload has been fired
	public bool fired = false;
    /// Acceleration per frame of detached payload
	public int accel;
    /// Payload type
	public PayloadType type;
	CapsuleCollider collider;
    /// Time in frames after firing, reset when payload is destroyed
	[HideInInspector]
	public int lifetime = 0;
    /// Maximum speed of the payload once fired
	public int maxSpeed;
	PlaneWrapper plane;
    /// Explosion particles and damage
	public Explosion explosion;
    /// Flag for payload destruction, true when payload has been destroyed
	bool destroyed = false;
	/// How long the missile is propelled for
	public int maxFuel;
	public int ammo = 1;


	void Start () {
		collider = GetComponent<CapsuleCollider> ();
		plane = GetComponentInParent<PlaneWrapper> ();
	}

	void FixedUpdate () {
		if (destroyed) {
			lifetime++;
            //Disable after 50 frames (~1 second)
			if (lifetime == 50)
				gameObject.SetActive (false);
			transform.Find ("Model").gameObject.SetActive (false);
            //Kill all speed and disable physics/collider
            body.velocity = Vector3.zero;
			body.isKinematic = true;
			collider.enabled = false;
		}
		else if (fired) {
			lifetime++;
            //If there is still fuel, propel the payload
			if (lifetime < maxFuel) {
				body.AddForce (transform.right * accel);
			}
            //When the fuel runs out disable the engine and fall
            if (lifetime > maxFuel){
                if (engine != null) engine.GetComponent<ParticleSystem>().Stop();
                //Turn towards velocity vector
                transform.Rotate(new Vector3(0, 0, (Mathf.DeltaAngle(Mathf.Rad2Deg * Mathf.Atan2(transform.right.y * 10, transform.right.x * 10), Mathf.Rad2Deg * Mathf.Atan2(body.velocity.y, body.velocity.x))) / 30), Space.World);
            }
            //Annull gravity for missiles
			if (type == PayloadType.MISSILE) {
				body.AddForce (new Vector3 (-Physics.gravity.x, -Physics.gravity.y, -Physics.gravity.z));
			}
            //Enable collider after firing
			if (lifetime == 15)	collider.enabled = true;
            //Decelerate if above max speed
            if (transform.InverseTransformDirection (body.velocity).x > maxSpeed) {
				body.AddForce (-transform.right * accel * 2);
			}

			Debug.DrawLine (transform.position, transform.position + transform.right * 10);
			Debug.DrawLine (transform.position, transform.position + body.velocity * 10);
		}
	}

    /// Fire the payload
	public virtual void Fire(){
		gameObject.AddComponent<Rigidbody> ();
		body = gameObject.GetComponent<Rigidbody> ();
        //Seperate from plane
        body.AddForce (-transform.forward * dropForce);
        //Start engine particles
        if (engine != null) engine.GetComponent<ParticleSystem> ().Play ();
        // Match plane speed
        body.AddForce (transform.right * (maxSpeed/2+plane.speed*2));
		fired = true;
        //Set collision detection mode
        body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        //Reduce ammo
		ammo--;
	}

	void OnCollisionEnter(Collision other){
		explosion.Detonate ();
		destroyed = true;
		print ("HIT");

		lifetime = 0;
	}
}
