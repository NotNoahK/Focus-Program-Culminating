using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour{
	
	public bool working = true;
	public bool attached = true;
	public int maxHealth = 500;
//	[HideInInspector]
	public int health;
	Collider collider;
	PlaneWrapper plane;
	public PanelScript.Parts type;

	void Start (){
		GetCollider ();
		plane = GetComponentInParent<PlaneWrapper> ();
		if (collider != null) {
			collider.gameObject.AddComponent <PassCollision>();
			collider.gameObject.GetComponent <PassCollision> ().target = this;
		}
	} 

	void Update(){
		if (health <= 0&&attached) {
			Detach ();
		}
		if (type != null && plane != null) {
			plane.panel.SetHealth (type, health, maxHealth);
		}
	}

	void GetCollider (){
		if (GetComponent<Collider> () == null) {
			if (transform.GetChild (0).GetComponent<Collider> () == null) collider = null;
			else collider = transform.GetChild (0).GetComponent<Collider> ();
		}
		else collider = GetComponent<Collider> ();
	}

	public void Collision(Collider other){
		if (gameObject.name == "Fuselage") {
			plane.Explode ();
		}
		Detach ();
	}

	public void Detach(){
		gameObject.AddComponent<Rigidbody> ();
		working = false;
		attached = false;
		if (plane != null) {
			GetComponent<Rigidbody> ().velocity = plane.GetComponent<Rigidbody> ().velocity;
		} else {
			GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(0,10),Random.Range(0,10),Random.Range(0,10)));
		}	
		collider.enabled = true;
		health = -1;
	}

	public void Shot(int damage){
		health -= damage;
	}
}

