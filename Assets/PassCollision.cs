using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassCollision : MonoBehaviour {

	public Part target;

	void Start(){
		print("Start" + GetComponent<Collider>());
	}

	void OnCollisionEnter(Collision collision){
		print ("Collision");
		target.OnCollisionEnter (collision);
	}
}
