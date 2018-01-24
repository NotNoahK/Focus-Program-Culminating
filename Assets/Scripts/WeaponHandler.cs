using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

///Controls weapons systems
public class WeaponHandler : MonoBehaviour {
	
    ///The gameobject where the gun is raycast from
	public GameObject gun;
    ///The pylons
	PylonScript[] pylons;
    ///Currently selected pylon
	public int activePylon = 5;
    ///Armed flag, when true weapons can be fired
	public bool armed;
    //BulletHit prefab for bullet particles
	public GameObject bulletHit;
    ///The damage done by bullets
	public int damage;
    ///Instrument panel
	PanelScript panel;

	void Start () {
		panel = GetComponentInChildren<PanelScript> ();
        //Get all pylons
		pylons = GetComponentsInChildren<PylonScript> ();
        //Sort pylons based on ID (From left to right)
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

        //Cycle through pylons
		if (InputManager.getButtonUp (InputManager.Button.WEAPON_LEFT)) {
			activePylon--;
		}		
		if (InputManager.getButtonUp (InputManager.Button.WEAPON_RIGHT)) {
			activePylon++;
		}
        //Loop around
		if (activePylon < 0)
			activePylon = pylons.Length-1;
		if (activePylon >= pylons.Length)
			activePylon = 0;

        //Fire pylon payload
		if (InputManager.getButtonUp (InputManager.Button.FIRE) && armed) {
			pylons [activePylon].Fire ();
		}
        //Fire guns
		if(InputManager.getButton(InputManager.Button.FIRE_CANNON) && armed){
            //Create raycast
			RaycastHit hit;
			Physics.Raycast(gun.transform.position, transform.right, out hit, 1000);
            //If the bullet hits
			if (hit.transform != null) {
				print (hit.collider.name);
                //Create hit particles
				Instantiate(bulletHit, hit.point, new Quaternion(0,0,0,0));
                //If there is a passCollision script attached, call Shot()
				PassCollision hitPart = hit.collider.gameObject.GetComponent<PassCollision> ();
				if(hitPart != null){
					hitPart.Shot (damage);
				}
			}
			Debug.DrawRay(gun.transform.position, transform.right*1000);
		}
        ///Set Instrument panel data
		panel.SetWeaponData (pylons [activePylon].ammo, pylons [activePylon].type, armed);
	}
}
