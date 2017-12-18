using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : MonoBehaviour {

	public static InputManager instance;

	public enum Buttons{FIRE,GEAR};
	static string[] joyButtons = {"joystick button 4", "joystick button 5"};
	static KeyCode[] keyButtons = {KeyCode.Alpha1, KeyCode.G};

	public enum Axis{PITCH,ROLL,YAW,THROTTLE};
	static string[] axes = new string[4];

	void Start () {
		axes [0] = new Axis ();
		axes [1] = new Axis ();
		axes [2] = new Axis ();
		axes [3] = new Axis ();
		if (instance == null) {
			instance = this;
		} else {
			Destroy (this);
		}
	}

	public static bool getButton(Buttons button){
		bool result = Input.GetButton (joyButtons [(int)button]) || Input.GetKey (keyButtons [(int)button]);
		print(result);
		return result; 
	}

	public static float getAxis(Axis axis){
		print (Input.GetAxisRaw (axes [(int)axis]));
		return 0;
	}

}

class Axis{
	float deadzone;
	float sensitivity;
	string name;

	public Axis(string name, float deadzone, float sensitivity){
		this.name = name;
		this.deadzone = deadzone;
		this.sensitivity = sensitivity;
	}

	public float Get(){
		float result = 0;
		result = Input.GetAxisRaw (name);
		if (result < deadzone && result > -deadzone) {
			result = 0;	
		}
		return result;
	}
}
