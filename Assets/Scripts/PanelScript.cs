using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelScript : MonoBehaviour {


	public enum Parts{
		WING_LEFT, WING_RIGHT, ELEVATOR_LEFT, ELEVATOR_RIGHT, TAIL, FUSELAGE, NACELLES
	}

	public Image leftWing;
	public Image rightWing;
	public Image leftElevator;
	public Image rightElevator;
	public Image tails;
	public Image[] nacelles;
	public Image[] fuselage;
	public Text ammoText;
	public Text weaponText;

	public void SetHealth(Parts part,int health,int maxHealth){
		float healthPercent = -1;
		if(health > 0) healthPercent = (float) health / (float) maxHealth;
		switch(part){
			case Parts.WING_LEFT:
				SetColour (leftWing, healthPercent);
				break;
			case Parts.WING_RIGHT:
				SetColour (rightWing, healthPercent);
				break;
			case Parts.ELEVATOR_LEFT:
				SetColour (leftElevator, healthPercent);
				break;
			case Parts.ELEVATOR_RIGHT:
				SetColour (rightElevator, healthPercent);
				break;
			case Parts.TAIL:
				SetColour (tails, healthPercent);
				break;
//			case Parts.NACELLES:
//				SetColour (nacelles, healthPercent);
//				break;
			case Parts.FUSELAGE:
				SetColour (fuselage, healthPercent);
				SetColour (nacelles, healthPercent);
				break;
		}
	}

	void SetColour(Image image, float healthPercent){
		image.color =  new Color (1, healthPercent, healthPercent);

		if (healthPercent < 0)
			image.color = Color.black;

	}

	void SetColour(Image[] images, float healthPercent){
		foreach (Image image in images) {
			image.color = new Color (1, healthPercent, healthPercent);
			if (healthPercent < 0)
				image.color = Color.black;
		}
	}
	public void SetWeaponData(int ammo,PayloadType weaponType, bool armed){
		if (armed) {
			ammoText.text = ammo + "";
			weaponText.text = weaponType.ToString ();
		} else {
			ammoText.text = "";
			weaponText.text = "DISARMED";
		}
	}

	void Update(){
	}
}
                                                                                       