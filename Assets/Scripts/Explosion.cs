using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

	public int diameter;
	SphereCollider collider;
	ParticleSystem particles;
	bool detonated = false;
	int counter = 0;

	// Use this for initialization
	void Start () {
		collider = GetComponent<SphereCollider> ();
		collider.radius = diameter * 10;
		particles = GetComponent<ParticleSystem> ();

		ParticleSystem.MainModule explosionMain = particles.main;
		explosionMain.startSize = new ParticleSystem.MinMaxCurve (diameter / 2, diameter);
	}

	void FixedUpdate(){
		if (detonated)
			counter++;
		if (counter == 50) {
			collider.enabled = false;
		}
	}

	public void Detonate(){
		particles.Play ();
		collider.enabled = true;
	}

	void OnTriggerEnter(Collider other){
		if (other.GetComponent<Part> () != null) {
			print ("Plane");
			other.GetComponent<Part>().Detach ();
			return;
		}
		if (other.GetComponentInParent<Part> () != null) {
			print ("Plane");
			other.GetComponentInParent<Part>().Detach ();
			return;
		}
		if (other.tag != "Invincible")
			other.gameObject.SetActive (false);
	}
}
