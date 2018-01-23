using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Script to interface plane control with game controller script
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
	public Part leftTailPart;
	public Part rightTailPart;
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
    //Fueselage and nose cone
	public Part fuselage;
	public GameObject noseCouterWeight;
    //Wings
	public Part leftWing;
	public Part rightWing;
    /// Center of lift used for downforce
	public GameObject col;
    /// Explosion triggered on crash
	public Explosion explosion;


	Rigidbody body;

    /// Plane's speed
	public float speed;
    /// Plane's vertical speed
	public float vSpeed;
    /// Plane's altitude
	public float altitude;
    /// Current throttle value
	float throttle;

    ///Roll force multiplier
	public float rollMultiplier;
    ///Max roll force
	public float maxRollForce;

    ///Pitch force multiplier
	public float pitchMultiplier;
    ///Max pitch force
	public float maxPitchForce;

    ///Yaw force multiplier
	public float yawMultiplier;
    ///Max yaw force;
	public float maxYawForce;

    ///Nose cone downforce
	public float noseWeight;

    ///Max lift force
	public float maxLiftForce;
    ///Lift force multiplier
	public float liftMultiplier;

    ///Thorttle force multiplier
	public float throttleMultiplier;
    ///Amount of drag
	public float dragForce;

    ///Minimun stall speed
	public float stallSpeed;
    ///Stall force multiplier
	public float stallMultiplier;

	/// Time in seconds for engines to spool up
	public float spoolTime;

    ///Plane destoyed flag, true after crash
	bool destroyed = false;

    ///Time at crash
	float destroyedTime;

    ///Instrument panel
	[HideInInspector]
	public PanelScript panel;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody> ();
		panel = GetComponentInChildren<PanelScript> ();
        //Set part types for damage indicator
		leftWing.type = PanelScript.Parts.WING_LEFT;
		rightWing.type = PanelScript.Parts.WING_RIGHT;
		rightElevator.type = PanelScript.Parts.ELEVATOR_RIGHT;
		leftElevator.type = PanelScript.Parts.ELEVATOR_LEFT;
		fuselage.type = PanelScript.Parts.FUSELAGE;
		rightTailPart.type = PanelScript.Parts.TAIL;
		leftTailPart.type = PanelScript.Parts.TAIL;
	}

	void FixedUpdate(){
        //Get vertical speed
		vSpeed = body.velocity.y;
        //Create raycast for altitude
		LayerMask mask = ~((1<<0)|(1<<1)|(1<<2)|(1<<3)|(1<<4)|(1<<5)|(1<<6)|(1<<7)|(1<<8));
		RaycastHit hit;
		Physics.Raycast(transform.position, Vector3.down, out hit ,1000000, mask);
        //Get altitude
		altitude = hit.distance;
        //Get speed
		speed = transform.InverseTransformDirection(body.velocity).x;
        //Prevent negative speed readouts
		if (speed < 1)
			speed = 0;
        //Add nose downforce
		body.AddForceAtPosition (-transform.forward * noseWeight, noseCouterWeight.transform.position);
        //Add drag
        body.AddForceAtPosition (-transform.right * dragForce, noseCouterWeight.transform.position);
		Lift ();
		Glide ();
        //If the plane has been destroyed for a time, disable camera
		if (Time.time - destroyedTime >= 0.3 && destroyed) {
			Camera[] cameras = GetComponentsInChildren<Camera> ();
			foreach (Camera camera in cameras) {
				camera.enabled = false;
			}
		}
	}

    ///Toggle the landing gear
	public void ToggleGear(){
		print ("Toggle Gear");
		if(noseGear.working)
			noseGear.GetComponent<Animator> ().SetTrigger ("Toggle gear");
		if(leftGear.working)
			leftGear.GetComponent<Animator> ().SetTrigger ("Toggle gear");
		if(rightGear.working)
			rightGear.GetComponent<Animator> ().SetTrigger ("Toggle gear");
	}

    ///Toggle the canopy
	public void ToggleCanopy(){
		canopy.GetComponent<Animator> ().SetTrigger ("Toggle canopy");
	}

    ///Toggle the tailhook
	public void ToggleHook(){
		tailHook.GetComponent<Animator> ().SetTrigger ("Toggle hook");
	}

    ///Apply pitch force to plane
    ///Angle: Angle of elevators
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

    ///Apply yaw force to plane
    ///Angle: Angle of rudders
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

    ///Apply roll force to plane
    ///Angle: Angle of ailerons
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

    ///Propel the plane
    ///Target: Target throttle
	public void Propel(float target){
        //Adjust throttle to meet target
		if (throttle < target) throttle += 1 / (spoolTime * 60);
		if (throttle > target) throttle -= 1 / (spoolTime * 60);

		//Calculate Force
		float throttleForce = throttle * throttleMultiplier;

		//Left Engine
		if (leftEngine.working) {
			body.AddForceAtPosition (-transform.right * throttleForce, leftEngine.transform.position);
			Debug.DrawLine (leftEngine.transform.position, leftEngine.transform.position + (-transform.right * throttleForce) * 1);
            //Set particles
            ParticleSystem.MainModule main = leftEngine.GetComponent<ParticleSystem> ().main;
			main.startLifetime = Mathf.Abs (throttle);
			leftEngine.transform.Find ("Point light").gameObject.GetComponent<Light>().intensity = Mathf.Abs (throttle)*250;
		} else {
            //Set particles
			ParticleSystem.MainModule main = leftEngine.GetComponent<ParticleSystem> ().main;
			main.startLifetime = Mathf.Abs (0);
			leftEngine.transform.Find ("Point light").gameObject.GetComponent<Light>().intensity = 0;
		}
		//Right Engine
		if (rightEngine.working) {
			body.AddForceAtPosition (-transform.right * throttleForce, rightEngine.transform.position);
			Debug.DrawLine (rightEngine.transform.position, rightEngine.transform.position+(-transform.right * throttleForce)*1);
            //Set particles
            ParticleSystem.MainModule main = rightEngine.GetComponent<ParticleSystem> ().main;
			main.startLifetime = Mathf.Abs(throttle);
			rightEngine.transform.Find ("Point light").gameObject.GetComponent<Light>().intensity = Mathf.Abs (throttle)*250;
		} else {
            //Set particles
            ParticleSystem.MainModule main = rightEngine.GetComponent<ParticleSystem> ().main;
			main.startLifetime = Mathf.Abs (0);
			rightEngine.transform.Find ("Point light").gameObject.GetComponent<Light>().intensity = 0;
		}
	}

    ///Starts ejection sequence
	public void Eject(){
        //Disable canopy colliders
		Collider[] col = canopy.GetComponentsInChildren<Collider> ();
		foreach(Collider c in col){
			c.enabled = false;
		}
        //Jettison canopy
		canopy.AddComponent<Rigidbody> ();
		canopy.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0,1000, 10000));
        //Eject the seats with a 5 frame delay between front and back
		frontSeat.GetComponent<Eject> ().Fire (15);
		backSeat.GetComponent<Eject> ().Fire (10);
	}

    ///Apply lift force to the plane
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

    ///Apply glide force to the plane
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
        ///The angle to turn to match velocity vector
		Vector3 stallForce = new Vector3 (0, 0, (Mathf.DeltaAngle (Mathf.Rad2Deg * Mathf.Atan2 (transform.right.y * 10, transform.right.x * 10), Mathf.Rad2Deg * Mathf.Atan2 (body.velocity.y, body.velocity.x))));

        ///Add torque if speed is under stall speed
		if(speed < stallSpeed){
			body.AddTorque(stallForce*stallMultiplier);
		}


		Debug.DrawLine (transform.position, transform.position + transform.right * 10);
		Debug.DrawLine (transform.position, transform.position + body.velocity * 10);
	}

    ///Destroy the plane after a crash
	public void Explode(){
		if (!destroyed) {
			explosion.Detonate ();
			destroyed = true;
			destroyedTime = Time.time;
		}
	}

}
