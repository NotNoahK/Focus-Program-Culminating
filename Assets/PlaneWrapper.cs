using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneWrapper : MonoBehaviour {

	//This is the code that will manage the components of the plane so that other scripts can be more simplified. This will be specialised to this specific plane 

	//Landing gear
	public GameObject noseGear;
	public GameObject leftGear;
	public GameObject rightGear;
	public GameObject tailHook;
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
	public float dragForce;

	public bool noseGearWorking = true;
	public bool leftGearWorking = true;
	public bool rightGearWorking = true;
	public bool leftWingWorking = true;
	public bool rightWingWorking = true;
	public bool leftAileronWorking = true;
	public bool rightAileronWorking = true;
	public bool leftRudderWorking = true;
	public bool rightRudderWorking = true;
	public bool leftElevatorWorking = true;
	public bool rightElevatorWorking = true;
	public bool leftEngineWorking = true;
	public bool rightEngineWorking = true;

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
		body.AddForceAtPosition (-transform.right * dragForce, noseCouterWeight.transform.position);
		Lift ();
		Glide ();
	}

	public void ToggleGear(){
		print ("Toggle Gear");
		if(noseGearWorking)
			noseGear.GetComponent<Animator> ().SetTrigger ("Toggle gear");
		if(leftGearWorking)
			leftGear.GetComponent<Animator> ().SetTrigger ("Toggle gear");
		if(rightGearWorking)
			rightGear.GetComponent<Animator> ().SetTrigger ("Toggle gear");
	}

	public void ToggleCanopy(){
		canopy.GetComponent<Animator> ().SetTrigger ("Toggle canopy");
	}

	public void ToggleHook(){
		tailHook.GetComponent<Animator> ().SetTrigger ("Toggle hook");
	}

	public void Pitch(float angle){
		//Calculate Force
		float pitchForce = Mathf.Clamp(-angle * pitchMultiplier * speed, -maxPitchForce, maxPitchForce);

		//Right Elevator
		if (rightElevatorWorking) {
			rightElevator.transform.localEulerAngles = new Vector3 (0, angle, 0);
			body.AddForceAtPosition (transform.forward * pitchForce, rightElevator.transform.position);
			Debug.DrawLine (rightElevator.transform.position, rightElevator.transform.position+transform.forward*pitchForce*10);
		}
		//Left Elevator
		if (leftElevatorWorking) {
			leftElevator.transform.localEulerAngles = new Vector3 (0, angle, 0);
			body.AddForceAtPosition (transform.forward * pitchForce, leftElevator.transform.position);	
			Debug.DrawLine (leftElevator.transform.position, leftElevator.transform.position+transform.forward*pitchForce*10);
		}
		//Nose Cone Conterbalance
		if (leftElevatorWorking) {
			body.AddForceAtPosition (-transform.forward * pitchForce/2, noseCouterWeight.transform.position);		
			Debug.DrawLine (noseCouterWeight.transform.position, noseCouterWeight.transform.position + transform.forward * pitchForce * 5);
		}
		if (rightElevatorWorking) {
			body.AddForceAtPosition (-transform.forward * pitchForce/2, noseCouterWeight.transform.position);		
			Debug.DrawLine (noseCouterWeight.transform.position, noseCouterWeight.transform.position + transform.forward * pitchForce * 5);
		}
	}

	public void Yaw(float angle){
		//Left Rudder
		if (leftRudderWorking) {	
			leftRudderPaddle.transform.localEulerAngles = new Vector3 (leftRudderPaddle.transform.localEulerAngles.x, leftRudderPaddle.transform.localEulerAngles.y, -angle);
		}
		//Right Rudder
		if (rightRudderWorking) {
			rightRudderPaddle.transform.localEulerAngles = new Vector3 (rightRudderPaddle.transform.localEulerAngles.x, rightRudderPaddle.transform.localEulerAngles.y, angle);
		}
	}

	public void Roll(float angle){
		//Calculate Force
		float rollForce = Mathf.Clamp(-angle * rollMultiplier * speed, -maxRollForce, maxRollForce);

		//Left Aileron
		if (leftAileronWorking) {
			leftAileron.transform.localEulerAngles = new Vector3 (0, -angle, 0);
			body.AddForceAtPosition (-transform.forward * rollForce, leftAileron.transform.position);
			Debug.DrawLine (leftAileron.transform.position, leftAileron.transform.position-transform.forward*rollForce*10);
		}
		//Right Aileron
		if (rightAileronWorking) {
			rightAileron.transform.localEulerAngles = new Vector3 (0, angle, 0);
			body.AddForceAtPosition (transform.forward * rollForce, rightAileron.transform.position);
			Debug.DrawLine (rightAileron.transform.position, rightAileron.transform.position+transform.forward*rollForce*10);
		}
	}

	public void Propel(float throttle){
		//Calculate Force
		float throttleForce = throttle * throttleMultiplier;

		//Left Engine
		if (leftEngineWorking) {
			body.AddForceAtPosition (-transform.right * throttleForce, leftEngine.transform.position);
			Debug.DrawLine (leftEngine.transform.position, leftEngine.transform.position + (-transform.right * throttleForce) * 1);
			ParticleSystem.MainModule main = leftEngine.GetComponent<ParticleSystem> ().main;
			main.startLifetime = Mathf.Abs (throttle);
			print(throttle);
			leftEngine.transform.Find ("Point light").gameObject.GetComponent<Light>().intensity = Mathf.Abs (throttle)*250;
		} else {
			ParticleSystem.MainModule main = leftEngine.GetComponent<ParticleSystem> ().main;
			main.startLifetime = Mathf.Abs (0);
			leftEngine.transform.Find ("Point light").gameObject.GetComponent<Light>().intensity = 0;
		}
		//Right Engine
		if (rightEngineWorking) {
			body.AddForceAtPosition (-transform.right * throttleForce, rightEngine.transform.position);
			Debug.DrawLine (rightEngine.transform.position, rightEngine.transform.position+(-transform.right * throttleForce)*1);
			ParticleSystem.MainModule main = rightEngine.GetComponent<ParticleSystem> ().main;
			main.startLifetime = Mathf.Abs(throttle);
			rightEngine.transform.Find ("Point light").gameObject.GetComponent<Light>().intensity = Mathf.Abs (throttle)*250;
		} else {
			ParticleSystem.MainModule main = rightEngine.GetComponent<ParticleSystem> ().main;
			main.startLifetime = Mathf.Abs (0);
			rightEngine.transform.Find ("Point light").gameObject.GetComponent<Light>().intensity = 0;
		}
	}

	public void Eject(){
		canopy.AddComponent<Rigidbody> ();
		canopy.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0,1000, 10000));
		frontSeat.GetComponent<Eject> ().Fire (15);
		backSeat.GetComponent<Eject> ().Fire (10);
	}

	void Lift(){
		//Calculate Force
		float liftForce = Mathf.Clamp(speed*liftMultiplier, 0, maxLiftForce);
		//Left Wing
		if (leftWingWorking) {
			body.AddForceAtPosition (liftForce*transform.forward, leftAileron.transform.position);
			Debug.DrawLine (leftAileron.transform.position, leftAileron.transform.position + liftForce * transform.forward*10);
		}
		//Right Wing
		if (rightWingWorking) {
			body.AddForceAtPosition (liftForce*transform.forward, rightAileron.transform.position);
			Debug.DrawLine (rightAileron.transform.position, rightAileron.transform.position+liftForce*transform.forward*10);
		}
		//Center Downforce
		body.AddForceAtPosition (-maxLiftForce*1.5f*transform.forward, col.transform.position);
		Debug.DrawLine (col.transform.position, col.transform.position-maxLiftForce*1.5f*transform.forward*10);
	}

	void Glide(){
		//Calculate Force
		float glideForce = Mathf.Clamp(-vSpeed, -300, 200);
		//Remove glide when vertical
		if (transform.localEulerAngles.y > 240 && transform.localEulerAngles.y < 300) {
			glideForce = -Mathf.Abs(glideForce);
		}
		//Left Wing
		if (leftWingWorking) {
			body.AddForceAtPosition (glideForce*transform.right, leftAileron.transform.position);
			Debug.DrawLine (leftAileron.transform.position, leftAileron.transform.position + glideForce * transform.right*10);
		}
		//Right Wing
		if (rightWingWorking) {
			body.AddForceAtPosition (glideForce*transform.right, rightAileron.transform.position);
			Debug.DrawLine (rightAileron.transform.position, rightAileron.transform.position+glideForce*transform.right*10);
		}
	}
}
	
