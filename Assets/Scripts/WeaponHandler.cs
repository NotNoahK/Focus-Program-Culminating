using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponHandler : MonoBehaviour {
	
	public GameObject gun;
	PylonScript[] pylons;
	public int activePylon = 5;
	public bool armed;
	public GameObject bulletHit;
	public int damage;
	PanelScript panel;

	void Start () {
		panel = GetComponentInChildren<PanelScript> ();
		pylons = GetComponentsInChildren<PylonScript> ();
		Array.Sort (pylons, delegate(PylonScript p1, PylonScript p2) {
			return(p1.ID.CompareTo(p2.ID));
		});

		foreach(PylonScript pylon in pylons){
			print (pylon.ID);
		}

	}
	
	void Update () {
		if (InputManager.getButtonUp (InputManager.Button.TOGGLE_WEAPONS)) {
			armed = !armed;
		}

		if (InputManager.getButtonUp (InputManager.Button.WEAPON_LEFT)) {
			activePylon--;
		}		
		if (InputManager.getButtonUp (InputManager.Button.WEAPON_RIGHT)) {
			activePylon++;
		}
		if (activePylon < 0)
			activePylon = pylons.Length-1;
		if (activePylon >= pylons.Length)
			activePylon = 0;
//		print (activePylon);

//		print(pylons[activePylon].type);

		if (InputManager.getButtonUp (InputManager.Button.FIRE) && armed) {
			pylons [activePylon].Fire ();
		}
		if(InputManager.getButton(InputManager.Button.FIRE_CANNON) && armed){
//			Instantiate(, new Vector3(0, 0, 0), Quaternion.identity);
			RaycastHit hit;
			Physics.Raycast(gun.transform.position, transform.right, out hit, 1000);
			if (hit.transform != null) {
				print (hit.collider.name);
				Debug.DrawLine (Vector3.zero, hit.point);
				Instantiate(bulletHit, hit.point, new Quaternion(0,0,0,0));
				PassCollision hitPart = hit.collider.gameObject.GetComponent<PassCollision> ();
				if(hitPart != null){
					hitPart.Shot (damage);
				}
			}
			Debug.DrawRay(gun.transform.position, transform.right*1000);
		}
		panel.SetWeaponData (pylons [activePylon].ammo, pylons [activePylon].type, armed);
	}
}
