using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///Test script used to control homing missile test target
public class WallTestScript : MonoBehaviour {

	public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(speed, 0, 0);
	}
}
