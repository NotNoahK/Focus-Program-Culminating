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
	//Ailerons
	public GameObject leftAileron;
	public GameObject rightAileron;
	public int rollMultiplier;
	public int maxRollForce;
	//Canopy
	public GameObject canopy;
	public GameObject frontSeat;
	public GameObject backSeat;
	//Engine
	public GameObject leftEngine;
	public GameObject rightEngine;

	public int throttleCorrection = 10;
	public GameObject fuselage;
	public GameObject noseCouterWeight;
	public float noseWeight;
	public int throttleMultiplier;
	public int maxPitchForce;

	public GameObject leftWing;
	public GameObject rightWing;
	public GameObject col;

	Rigidbody body;


	public float speed;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody> ();
	}

	void Update(){
		speed = body.velocity.x;
		body.AddForceAtPosition (-transform.forward * noseWeight, noseCouterWeight.transform.position);
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
		body.AddForceAtPosition (transform.forward*pitchForce, rightElevator.transform.position);
		body.AddForceAtPosition (transform.forward, leftElevator.transform.position);

		Debug.DrawLine (rightElevator.transform.position, rightElevator.transform.position+transform.forward*pitchForce*10);
		Debug.DrawLine (leftElevator.transform.position, leftElevator.transform.position+transform.forward*pitchForce*10);
	}

	public void RotateRudder(float angle){
		rightRudderPaddle.transform.localEulerAngles = new Vector3 (rightRudderPaddle.transform.localEulerAngles.x, rightRudderPaddle.transform.localEulerAngles.y, angle);
		leftRudderPaddle.transform.localEulerAngles = new Vector3 (leftRudderPaddle.transform.localEulerAngles.x, leftRudderPaddle.transform.localEulerAngles.y, -angle);
	}

	public void RollAileron(float angle){
		leftAileron.transform.localEulerAngles = new Vector3 (0, -angle, 0);
		rightAileron.transform.localEulerAngles = new Vector3 (0, angle, 0);
		float rollForce = Mathf.Clamp(-angle * rollMultiplier * speed, -maxRollForce, maxRollForce);
//		print(rollForce);
//		rollForce = 0;
		print(transform.up);
		body.AddForceAtPosition (-transform.forward*rollForce, leftAileron.transform.position);
		body.AddForceAtPosition (transform.forward*rollForce, rightAileron.transform.position);
		Debug.DrawLine (rightAileron.transform.position, rightAileron.transform.position+transform.forward*rollForce*10);
		Debug.DrawLine (leftAileron.transform.position, leftAileron.transform.position-transform.forward*rollForce*10);
	}

	public void Propel(float throttle){
		float throttleForce = throttle * throttleMultiplier;
		body.AddForceAtPosition (-transform.right * throttleForce, leftEngine.transform.position);
		body.AddForceAtPosition (-transform.right * throttleForce, rightEngine.transform.position);
//		print(rightEngine.transform.position);
		Debug.DrawLine (rightEngine.transform.position, rightEngine.transform.position+(-transform.right * throttleForce)*1);
		Debug.DrawLine (leftEngine.transform.position, leftEngine.transform.position+(-transform.right * throttleForce)*1);
	}

	public void Eject(){
		canopy.AddComponent<Rigidbody> ();
		canopy.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0,1000, 10000));
		frontSeat.GetComponent<Eject> ().Fire (15);
		backSeat.GetComponent<Eject> ().Fire (10);
	}

	void Lift(){
		float liftForce = transform.forward*5;
		body.AddForceAtPosition (liftForce, leftWing.transform.position);
		body.AddForceAtPosition (liftForce, rightWing.transform.position);
		body.AddForceAtPosition (-liftForce*2, col.transform.position);
	}
}
