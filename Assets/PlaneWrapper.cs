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
	//Rudders, paddles are the control surfaces themselves
	public GameObject leftTail;
	public GameObject rightTail;
	public GameObject leftRudderPaddle;
	public GameObject rightRudderPaddle;
	//Ailerons
	public GameObject leftAileron;
	public GameObject rightAileron;
	//Canopy
	public GameObject canopy;
	public GameObject frontSeat;
	public GameObject backSeat;
	//Engine
	public GameObject leftEngine;
	public GameObject rightEngine;

	public GameObject fuselage;
	public GameObject noseCouterWeight;

	public GameObject leftWing;
	public GameObject rightWing;
	public GameObject col;

	public float vSpeed;

	Rigidbody body;

	public float speed;



	public float rollMultiplier;
	public float maxRollForce;
	public float pitchMultiplier;
	public float maxPitchForce;
	public float noseWeight;
	public float maxLiftForce;
	public float liftMultiplier;
	public float throttleMultiplier;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody> ();
	}

	void Update(){
		vSpeed = body.velocity.y;
		speed = transform.InverseTransformDirection(body.velocity).x;
		if (speed < 1)
			speed = 0;
		body.AddForceAtPosition (-transform.forward * noseWeight, noseCouterWeight.transform.position);
		Lift ();
		Glide ();
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

	public void Pitch(float angle){
		rightElevator.transform.localEulerAngles = new Vector3 (0, angle, 0);
		leftElevator.transform.localEulerAngles = new Vector3 (0, angle, 0);
		float pitchForce = Mathf.Clamp(-angle * pitchMultiplier * speed, -maxPitchForce, maxPitchForce);
		body.AddForceAtPosition (transform.forward*pitchForce, rightElevator.transform.position);
		body.AddForceAtPosition (transform.forward*pitchForce, leftElevator.transform.position);


		body.AddForceAtPosition (-transform.forward*pitchForce, noseCouterWeight.transform.position);		
		Debug.DrawLine (noseCouterWeight.transform.position, noseCouterWeight.transform.position+transform.forward*pitchForce*10);

		Debug.DrawLine (rightElevator.transform.position, rightElevator.transform.position+transform.forward*pitchForce*10);
		Debug.DrawLine (leftElevator.transform.position, leftElevator.transform.position+transform.forward*pitchForce*10);
	}

	public void Yaw(float angle){
		rightRudderPaddle.transform.localEulerAngles = new Vector3 (rightRudderPaddle.transform.localEulerAngles.x, rightRudderPaddle.transform.localEulerAngles.y, angle);
		leftRudderPaddle.transform.localEulerAngles = new Vector3 (leftRudderPaddle.transform.localEulerAngles.x, leftRudderPaddle.transform.localEulerAngles.y, -angle);
	}

	public void Roll(float angle){
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
		float liftForce = Mathf.Clamp(speed*liftMultiplier, 0, maxLiftForce);
		print (liftForce);
		body.AddForceAtPosition (liftForce*transform.forward, leftAileron.transform.position);
		body.AddForceAtPosition (liftForce*transform.forward, rightAileron.transform.position);
		body.AddForceAtPosition (-maxLiftForce*1.5f*transform.forward, col.transform.position);
		Debug.DrawLine (leftAileron.transform.position, leftAileron.transform.position + liftForce * transform.forward*10);
		Debug.DrawLine (rightAileron.transform.position, rightAileron.transform.position+liftForce*transform.forward*10);
		Debug.DrawLine (col.transform.position, col.transform.position-maxLiftForce*1.5f*transform.forward*10);
	}

	void Glide(){
		float glideForce = Mathf.Clamp(-vSpeed, -300, 200);
		body.AddForceAtPosition (glideForce*transform.right, leftAileron.transform.position);
		body.AddForceAtPosition (glideForce*transform.right, rightAileron.transform.position);
		Debug.DrawLine (leftAileron.transform.position, leftAileron.transform.position + glideForce * transform.right*10);
		Debug.DrawLine (rightAileron.transform.position, rightAileron.transform.position+glideForce*transform.right*10);

	}
}
