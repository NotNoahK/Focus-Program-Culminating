using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

	public int diameter;
	SphereCollider collider;
	ParticleSystem particles;

	// Use this for initialization
	void Start () {
		collider = GetComponent<SphereCollider> ();
		collider.radius = diameter * 10;
		particles = GetComponent<ParticleSystem> ();

		ParticleSystem.MainModule explosionMain = particles.main;
		explosionMain.startSize = new ParticleSystem.MinMaxCurve (diameter / 2, diameter);
	}
	
	public void Detonate(){
		particles.Play ();
		collider.enabled = true;
	}

	void OnTriggerEnter(Collider other){
		if (other.tag != "Invincible")
			other.gameObject.SetActive (false);
	}
}
