using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Script to handle explosion particles and damage
public class Explosion : MonoBehaviour {

	/// Effective diameter of particles and damage, set to 1/5th of collider size
	public int diameter;
	SphereCollider collider;
	ParticleSystem particles;
	/// Detonated flag
	bool detonated = false;
	/// Counter for disabling
	int counter = 0;

	void Start () {
		//Set collider size
		collider = GetComponent<SphereCollider> ();
		collider.radius = diameter * 10;
		//Set explosion size
		particles = GetComponent<ParticleSystem> ();
		ParticleSystem.MainModule explosionMain = particles.main;
		explosionMain.startSize = new ParticleSystem.MinMaxCurve (diameter / 2, diameter);
	}

	void FixedUpdate(){
		if (detonated)
			counter++;
		//Disable collider after 50 frames (~1 second)
		if (counter == 50) {
            collider.enabled = false;
		}
	}

	/// Start detonation
	public void Detonate(){
		particles.Play ();
		collider.enabled = true;
		detonated = true;
	}

	void OnTriggerEnter(Collider other){
		//If its a plane part, detach it
		if (other.GetComponent<Part> () != null) {
			print ("Plane");
			other.GetComponent<Part> ().Detach ();
			return;
		}
		//If its not incinvible and it's not a plane part, disable it
		if (other.tag != "Invincible")
			other.gameObject.SetActive (false);
	}
}
