using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassCollision : MonoBehaviour {

	public Part target;

	void Start(){
		print("Start " + GetComponent<Collider>());
	}

	void OnTriggerEnter(Collider other){
		print ("Collision");
		target.Collision (other);
	}

	public void Shot (int damage){
		target.Shot (damage);
	}
}
