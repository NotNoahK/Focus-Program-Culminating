using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PylonScript : MonoBehaviour {

	public PayloadType type;
	PayloadScript payload;
	public int ammo;

	// Use this for initialization
	void Start () {
		payload = transform.parent.GetComponentInChildren<PayloadScript> ();
		payload = new Empty ();
		type = payload.type;
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
}
