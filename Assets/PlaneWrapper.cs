using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneWrapper : MonoBehaviour {

	//This is the code that will manage the components of the plane so that other scripts can be more simplified. This will be specialised to this specific plane 

	//Landing gear
	public GameObject noseGear;
	public GameObject leftGear;
	public GameObject rightGear;
	public bool noseGearWorking = true;
	public bool leftGearWorking = true;
	public bool rightGearWorking = true;
	public GameObject tailHook;
	public bool tailHookWorking = true;
	//Elevators
	public GameObject leftElevator;
	public GameObject rightElevator;
	public int pitchMultiplier;
	//Rudders, paddles are the control surfaces themselves
	public GameObject leftTail;
	public GameObject rightTail;
	public GameObject leftRudderPaddle;
	public GameObject rightRudderPaddle;
	//Canopy
	public GameObject canopy;
	public GameObject frontSeat;
	public GameObject backSeat;
	//Engine
	public GameObject leftEngine;
	public GameObject rightEngine;

	public int throttleCorrection = 10;
	public GameObject fuselage;
	public int throttleMultiplier;
	public int maxPitchForce;

	public float speed;

	// Use this for initialization
	void Start () {
//		fuselage.GetComponent<Rigidbody> ().centerOfMass = fuselage.transform.Find ("COM").transform.position;
//		fuselage.GetComponent<Rigidbody>().centerOfMass = fuselage.transform.Find ("COM").transform.position - fuselage.transform.position;
	}

	void Update(){
//		fuselage.GetComponent<Rigidbody>().centerOfMass = new Vector3(0,0,-2);\
//		fuselage.GetComponent<Rigidbody>().centerOfMass = fuselage.transform.Find ("COM").transform.position - fuselage.transform.position;
		speed = fuselage.gameObject.GetComponent<Rigidbody> ().velocity.x;
//		Debug.DrawLine(fuselage.transform.position+fuselage.GetComponent<Rigidbody>().centerOfMass, fuselage.transform.position+fuselage.GetComponent<Rigidbody>().velocity);
		Debug.DrawLine(fuselage.transform.position+fuselage.GetComponent<Rigidbody>().centerOfMass, Vector3.zero);
//		Debug.DrawLine(GetComponent<Rigidbody>().centerOfMass, Vector3.zero);
	}

	public void ToggleGear(){
		print ("Toggle Gear");
		noseGear.GetComponent<Animator> ().SetTrigger ("Toggle gear");
		leftGear.GetComponent<Animator> ().SetTrigger ("Toggle gear");
		rightGear.GetComponent<Animator> ().SetTrigger ("Toggle gear");
	}

	public void ToggleCanopy(){
		canopy.GetComponent<Animator> ().SetTrigger ("Toggle canopy");
	}

	public void ToggleHook(){
		tailHook.GetComponent<Animator> ().SetTrigger ("Toggle hook");
	}

	public void RotateElevator(float angle){
		rightElevator.transform.localEulerAngles = new Vector3 (0, angle, 0);
		leftElevator.transform.localEulerAngles = new Vector3 (0, angle, 0);
		float pitchForce = Mathf.Clamp(-angle * pitchMultiplier * speed, -maxPitchForce, maxPitchForce);
		print (pitchForce);
		leftElevator.transform.parent.gameObject.GetComponent<Rigidbody> ().AddRelativeForce (new Vector3(0, 0, pitchForce));
		rightElevator.transform.parent.gameObject.GetComponent<Rigidbody> ().AddRelativeForce (new Vector3(0, 0, pitchForce));
	}

	public void RotateRudder(float angle){
		rightRudderPaddle.transform.localEulerAngles = new Vector3 (rightRudderPaddle.transform.localEulerAngles.x, rightRudderPaddle.transform.localEulerAngles.y, angle);
		leftRudderPaddle.transform.localEulerAngles = new Vector3 (leftRudderPaddle.transform.localEulerAngles.x, leftRudderPaddle.transform.localEulerAngles.y, -angle);
	}

	public void Propel(float throttle){
		fuselage.GetComponent<Rigidbody> ().AddRelativeForce (new Vector3 (throttle * throttleMultiplier, 0, 0));
//		leftElevator.transform.parent.gameObject.GetComponent<Rigidbody> ().AddRelativeForce (new Vector3(throttle*500, 0, 0));
//		rightElevator.transform.parent.gameObject.GetComponent<Rigidbody> ().AddRelativeForce (new Vector3(throttle*500, 0, 0));
//		leftEngine.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(throttle*throttleMultiplier, 0, 0));
//		rightEngine.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(throttle*throttleMultiplier, 0, 0));
	}

	public void Eject(){
		canopy.SetActive (false);
		frontSeat.GetComponent<Eject> ().Fire (5);
		backSeat.GetComponent<Eject> ().Fire (0);
	}
}
