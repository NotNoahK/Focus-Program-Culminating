using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelScript : MonoBehaviour {


	public int testHealth = 500;
	public int maxTestHealth = 500;
	public Parts testPart;

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

	public void SetHealth(Parts part,int health,int maxHealth){
		float healthPercent = (float) health / (float) maxHealth;
		switch(part){
			case Parts.WING_LEFT:
				SetColour (leftWing, healthPercent);
				return;
			case Parts.WING_RIGHT:
				SetColour (rightWing, healthPercent);
				return;
			case Parts.ELEVATOR_LEFT:
				SetColour (leftElevator, healthPercent);
				return;
			case Parts.ELEVATOR_RIGHT:
				SetColour (rightElevator, healthPercent);
				return;
			case Parts.TAIL:
				SetColour (tails, healthPercent);
				return;
			case Parts.NACELLES:
				SetColour (nacelles, healthPercent);
				return;
			case Parts.FUSELAGE:
				SetColour (fuselage, healthPercent);
				return;
		}
	}

	void SetColour(Image image, float healthPercent){
		image.color =  new Color (1, healthPercent, healthPercent);


	}

	void SetColour(Image[] images, float healthPercent){
		foreach (Image image in images) {
			image.color = new Color (1, healthPercent, healthPercent);
		}
	}

	void Update(){
		SetHealth (testPart, testHealth, maxTestHealth);
	}
}
                                                                                       