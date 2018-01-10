using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponHandler : MonoBehaviour {

	PylonScript[] pylons;
	public int activePylon = 5;
	public bool armed;

	void Start () {
		pylons = GetComponentsInChildren<PylonScript> ();
		Array.Sort (pylons, delegate(PylonScript p1, PylonScript p2) {
			return(p1.ID.CompareTo(p2.ID));
		});

		foreach(PylonScript pylon in pylons){
			print (pylon.ID);
		}
	}
	
	void Update () {
		
	}
}
