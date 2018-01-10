using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PylonScript : MonoBehaviour {

	public PayloadType type;
	PayloadScript payload;
	public int ammo;
	public int ID = -1;

	// Use this for initialization
	void Start () {
		payload = transform.parent.GetComponentInChildren<PayloadScript> ();
		print (transform.parent.name);
		if(payload == null) 
			payload = transform.parent.GetComponentInChildren<HomingMissile> ();
		if(payload == null) 
			payload = new Empty ();
		print (payload.type);
		type = payload.type;
		if (ID == -1) {
			print ("ID not set on"+name);
		}
	}

	public void Update(){
		ammo = payload.ammo;
	}

	public void Fire(){
		payload.Fire ();
	}

}

//Used as substitute for empty pylon

class Empty : PayloadScript{
	PayloadType type = PayloadType.EMPTY;
	int ammo = 0;


	public void Fire(){
	}
}
