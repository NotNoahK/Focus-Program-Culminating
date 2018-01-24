using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// Controls the instrument panel
public class PanelScript : MonoBehaviour {

	/// Parts on the damage indicator
	/// Not a Part (NaP) default
	public enum Parts{
		NaP, WING_LEFT, WING_RIGHT, ELEVATOR_LEFT, ELEVATOR_RIGHT, TAIL, FUSELAGE, NACELLES
	}

	//Images on the damage indicator
	public Image leftWing;
	public Image rightWing;
	public Image leftElevator;
	public Image rightElevator;
	public Image tails;
	public Image[] nacelles;
	public Image[] fuselage;
	//Texts
	public Text ammoText;
	public Text weaponText;

	/// Set the health of a part on the damage indicator
	/// Part: The part on the damage indicator
	/// Health: Part health
	/// Max health: Maximum part health
	public void SetHealth(Parts part,int health,int maxHealth){
		float healthPercent = -1;
		//If the part is attached get health percent
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
//          Owing to issues with the sprites, nacelles are tied into the fueslage
//			case Parts.NACELLES:
//				SetColour (nacelles, healthPercent);
//				break;
			case Parts.FUSELAGE:
				SetColour (fuselage, healthPercent);
				SetColour (nacelles, healthPercent);
				break;
		}
	}

	/// Sets the colour of a part on the damage indicator
	/// Image: The image to recolour
	/// Health percent: Percentage to use in colour calculation
	void SetColour(Image image, float healthPercent){
		//Set the colour of the part from white (100%) to red (0%)
		image.color =  new Color (1, healthPercent, healthPercent);
		//If the part is detached, make it black
		if (healthPercent < 0)
			image.color = Color.black;

	}
	/// Sets the colour of a group of parts on the damage indicator
	/// Image: The image to recolour
	/// Health percent: Percentage to use in colour calculation
	void SetColour(Image[] images, float healthPercent){
		foreach (Image image in images) {
			//Set the colour of the part from white (100%) to red (0%)
			image.color = new Color (1, healthPercent, healthPercent);
			//Set the colour of the part from white (100%) to red (0%)
			if (healthPercent < 0)
				image.color = Color.black;
		}
	}
	/// Set the weapon text
	/// Ammo: Weapon ammo
	/// Weapon type: The type of weapon
	/// Armed: Weapon system armed status
	public void SetWeaponData(int ammo,PayloadType weaponType, bool armed){
		//If the weapons are armed, display data
		if (armed) {
			ammoText.text = ammo + "";
			weaponText.text = weaponType.ToString ();
		} 
		//Otherwise, set the text to DISARMED
		else {
			ammoText.text = "";
			weaponText.text = "DISARMED";
		}
	}
}
                                                                                       