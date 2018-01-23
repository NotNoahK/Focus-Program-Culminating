using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///Connection between plane and payloads
public class PylonScript : MonoBehaviour {

    ///Type of payload on pylon
	public PayloadType type;
	PayloadScript payload;
    ///Payload ammo
	public int ammo;
    ///Pylon ID (0 on left)
	public int ID = -1;

	void Start () {
        //Get the payload
		payload = transform.parent.GetComponentInChildren<PayloadScript> ();
        //If there is no payload, create new Empty payload
        if (payload == null) 
			payload = new Empty ();

		print (payload.type);
        //Set type
		type = payload.type;
        //If the ID isn't set, print
		if (ID == -1) {
			print ("ID not set on"+name);
		}
	}

	public void Update(){
        //Update ammo
		ammo = payload.ammo;
	}

    ///Fires the attached payload
	public void Fire(){
		payload.Fire ();
	}

}

///Replaces normal PayloadScript on empty pylon to prevent errors
class Empty : PayloadScript{

    ///Replaces normal PayloadScript on empty pylon to prevent errors
    public Empty(){
        type = PayloadType.EMPTY;
        ammo = 0;
    }

    ///Override the normal fire method because there is nothing to fire
	override public void Fire(){
	}
}
