using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Passes a collision event to target part
public class PassCollision : MonoBehaviour {

	/// Part to send collision to
	public Part target;

	/// Passes shoot event to part
	void OnTriggerEnter(Collider other){
		target.Collision (other);
	}

	/// Passes shoot event to part
	public void Shot (int damage){
		target.Shot (damage);
	}
}
