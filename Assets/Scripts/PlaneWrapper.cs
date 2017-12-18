using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneWrapper : MonoBehaviour {

	//This is the code that will manage the components of the plane so that other scripts can be more simplified. This will be specialised to this specific plane

	//Landing gear
	public Part noseGear;
	public Part leftGear;
	public Part rightGear;
	public GameObject tailHook;
	//Elevators
	public Part leftElevator;
	public Part rightElevator;
	//Rudders, paddles are the control surfaces themselves
	public GameObject leftTail;
	public GameObject rightTail;
	public Part leftRudderPaddle;
	public Part rightRudderPaddle;
	//Ailerons
	public Part leftAileron;
	public Part rightAileron;
	//Canopy
	public GameObject canopy;
	public GameObject frontSeat;
	public GameObject backSeat;
	//Engine
	public Part leftEngine;
	public Part rightEngine;

	public Part fuselage;
	public GameObject noseCouterWeight;

	public Part leftWing;
	public Part rightWing;
	public GameObject col;


	Rigidbody body;

	public float speed;
	public float vSpeed;

	float throttle;


	public float rollMultiplier;
	public float maxRollForce;
	public float pitchMultiplier;
	public float maxPitchForce;
	public float yawMultiplier;
	public float maxYawForce;
	public float noseWeight;
	public float maxLiftForce;
	public float liftMultiplier;
	public float throttleMultiplier;
	public float dragForce;
	public float stallSpeed;
	public float stallMultiplier;


	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody> ();
	}

	void FixedUpdate(){
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
		if(noseGear.working)
			noseGear.GetComponent<Animator> ().SetTrigger ("Toggle gear");
		if(leftGear.working)
			leftGear.GetComponent<Animator> ().SetTrigger ("Toggle gear");
		if(rightGear.working)
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
		if (rightElevator.working) {
			rightElevator.transform.localEulerAngles = new Vector3 (0, angle, 0);
			body.AddForceAtPosition (transform.forward * pitchForce, rightElevator.transform.position);
			Debug.DrawLine (rightElevator.transform.position, rightElevator.transform.position+transform.forward*pitchForce*10);
		}
		//Left Elevator
		if (leftElevator.working) {
			leftElevator.transform.localEulerAngles = new Vector3 (0, angle, 0);
			body.AddForceAtPosition (transform.forward * pitchForce, leftElevator.transform.position);
			Debug.DrawLine (leftElevator.transform.position, leftElevator.transform.position+transform.forward*pitchForce*10);
		}
		//Nose Cone Conterbalance
		if (leftElevator.working) {
			body.AddForceAtPosition (-transform.forward * pitchForce/2, noseCouterWeight.transform.position);
			Debug.DrawLine (noseCouterWeight.transform.position, noseCouterWeight.transform.position + transform.forward * pitchForce * 5);
		}
		if (rightElevator.working) {
			body.AddForceAtPosition (-transform.forward * pitchForce/2, noseCouterWeight.transform.position);
			Debug.DrawLine (noseCouterWeight.transform.position, noseCouterWeight.transform.position + transform.forward * pitchForce * 5);
		}
	}

	public void Yaw(float angle){
		//Calculate force
		float yawForce = Mathf.Clamp(-angle * yawMultiplier * speed, -maxYawForce, maxYawForce);

		//Left Rudder
		if (leftRudderPaddle.working) {
			leftRudderPaddle.transform.localEulerAngles = new Vector3 (leftRudderPaddle.transform.localEulerAngles.x, leftRudderPaddle.transform.localEulerAngles.y, -angle);

			body.AddForceAtPosition (-transform.up * yawForce, leftRudderPaddle.transform.position);
			Debug.DrawLine (leftRudderPaddle.transform.position, leftRudderPaddle.transform.position+transform.up*yawForce*10);

			body.AddForceAtPosition (-transform.up * -yawForce, noseCouterWeight.transform.position);
			Debug.DrawLine (noseCouterWeight.transform.position, noseCouterWeight.transform.position+transform.up*-yawForce*10);
		}
		//Right Rudder
		if (rightRudderPaddle.working) {
			rightRudderPaddle.transform.localEulerAngles = new Vector3 (rightRudderPaddle.transform.localEulerAngles.x, rightRudderPaddle.transform.localEulerAngles.y, angle);

			body.AddForceAtPosition (transform.up * yawForce, rightRudderPaddle.transform.position);
			Debug.DrawLine (rightRudderPaddle.transform.position, rightRudderPaddle.transform.position+transform.up*yawForce*10);

			body.AddForceAtPosition (-transform.up * -yawForce, noseCouterWeight.transform.position);
			Debug.DrawLine (noseCouterWeight.transform.position, noseCouterWeight.transform.position+transform.up*-yawForce*10);
		}

	}

	public void Roll(float angle){
		//Calculate Force
		float rollForce = Mathf.Clamp(-angle * rollMultiplier * speed, -maxRollForce, maxRollForce);

		//Left Aileron
		if (leftAileron.working) {
			leftAileron.transform.localEulerAngles = new Vector3 (0, -angle, 0);
			body.AddForceAtPosition (-transform.forward * rollForce, leftAileron.transform.position);
			Debug.DrawLine (leftAileron.transform.position, leftAileron.transform.position-transform.forward*rollForce*10);
		}
		//Right Aileron
		if (rightAileron.working) {
			rightAileron.transform.localEulerAngles = new Vector3 (0, angle, 0);
			body.AddForceAtPosition (transform.forward * rollForce, rightAileron.transform.position);
			Debug.DrawLine (rightAileron.transform.position, rightAileron.transform.position+transform.forward*rollForce*10);
		}
	}

	public void Propel(float throttle){
		//Calculate Force
		float throttleForce = throttle * throttleMultiplier;

		//Left Engine
		if (leftEngine.working) {
			body.AddForceAtPosition (-transform.right * throttleForce, leftEngine.transform.position);
			Debug.DrawLine (leftEngine.transform.position, leftEngine.transform.position + (-transform.right * throttleForce) * 1);
			ParticleSystem.MainModule main = leftEngine.GetComponent<ParticleSystem> ().main;
			main.startLifetime = Mathf.Abs (throttle);
//			print(throttle);
			leftEngine.transform.Find ("Point light").gameObject.GetComponent<Light>().intensity = Mathf.Abs (throttle)*250;
		} else {
			ParticleSystem.MainModule main = leftEngine.GetComponent<ParticleSystem> ().main;
			main.startLifetime = Mathf.Abs (0);
			leftEngine.transform.Find ("Point light").gameObject.GetComponent<Light>().intensity = 0;
		}
		//Right Engine
		if (rightEngine.working) {
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
		if (leftWing.working) {
			body.AddForceAtPosition (liftForce*transform.forward, leftAileron.transform.position);
			Debug.DrawLine (leftAileron.transform.position, leftAileron.transform.position + liftForce * transform.forward*10);
		}
		//Right Wing
		if (rightWing.working) {
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
		if (leftWing.working) {
			body.AddForceAtPosition (glideForce*transform.right, leftAileron.transform.position);
			Debug.DrawLine (leftAileron.transform.position, leftAileron.transform.position + glideForce * transform.right*10);
		}
		//Right Wing
		if (rightWing.working) {
			body.AddForceAtPosition (glideForce*transform.right, rightAileron.transform.position);
			Debug.DrawLine (rightAileron.transform.position, rightAileron.transform.position+glideForce*transform.right*10);
		}

		//Spin nose down code

		//Calculate delta angle
		Vector3 stallForce = new Vector3 (0, 0, (Mathf.DeltaAngle (Mathf.Rad2Deg * Mathf.Atan2 (transform.right.y * 10, transform.right.x * 10), Mathf.Rad2Deg * Mathf.Atan2 (body.velocity.y, body.velocity.x))));

		//Calculate multiplier based on curve https://www.desmos.com/calculator/qcdupodqv1
		//For equasion to work, stallspeed MUST be 100 or less
		float curve = 0;
		if(speed < stallSpeed){
			body.AddTorque(stallForce*stallMultiplier);
//			curve = -9.07351f * Mathf.Pow (stallSpeed-speed, 0.17202f) + 19.971f;
//			if (curve < 0)	curve = 0;
		}


		Debug.DrawLine (transform.position, transform.position + transform.right * 10);
		Debug.DrawLine (transform.position, transform.position + body.velocity * 10);
	}
}
