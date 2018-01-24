using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// The main code to handle plane parts, also used for ground objects with health
public class Part : MonoBehaviour{

	/// Working flag
	public bool working = true;
	/// Attached flag
	public bool attached = true;
	/// Maximum/starting health of the part
	public int maxHealth = 500;
	/// The health of the part
	[HideInInspector]
	public int health;
	Collider collider;
	PlaneWrapper plane;
	/// Part type (For damage indicator)
	public PanelScript.Parts type;

	void Start (){
		GetCollider ();
		plane = GetComponentInParent<PlaneWrapper> ();
		if (collider != null) {
			//Add a PassCollision script pointing at self on collider
			collider.gameObject.AddComponent <PassCollision>();
			collider.gameObject.GetComponent <PassCollision> ().target = this;
		}
	} 

	void Update(){
		if (health <= 0 && attached) {
			Detach ();
		}
		//If it's a part of the plane and is a damage indicator part, set damage indicator
		if (type != PanelScript.Parts.NaP && plane != null) {
			plane.panel.SetHealth (type, health, maxHealth);
		}
	}

	/// Finds the collider responsible for the part
	void GetCollider (){
		//Collider will be on same gameObject or first child
		if (GetComponent<Collider> () == null) {
			if (transform.GetChild (0).GetComponent<Collider> () == null) collider = null;
			else collider = transform.GetChild (0).GetComponent<Collider> ();
		}
		else collider = GetComponent<Collider> ();
	}

	/// Called by passCollision when collision is detected
	public void Collision(Collider other){
		//If this is the fuselage, destroy the plane
		if (gameObject.name == "Fuselage") {
			plane.Explode ();
		}
		Detach ();
	}

	/// Detach the part from the parent rigidbody
	public void Detach(){
		gameObject.AddComponent<Rigidbody> ();
		working = false;
		attached = false;
		//If it's from a plane, inherit the plane's speed
		if (plane != null) {
			GetComponent<Rigidbody> ().velocity = plane.GetComponent<Rigidbody> ().velocity;
		} 
		//Otherwise add a random torque
		else {
			GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(0,10),Random.Range(0,10),Random.Range(0,10)));
		}	
		collider.enabled = true;
		health = -1;
	}

	/// Damage the part
	/// Damage: The amount of health to remove
	public void Shot(int damage){
		health -= damage;
	}
}

