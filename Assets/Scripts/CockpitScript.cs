using System.Collections;
using UnityEngine;


public class CockpitScript : MonoBehaviour{
	PlaneWrapper plane;
	Rigidbody body;

	Vector3 level;

	public GameObject hud;

	public float multiplier;

	void Start (){
		plane = transform.GetComponentInParent<PlaneWrapper> ();
		body = plane.gameObject.GetComponent<Rigidbody> ();
		Calibrate ();
	}

	void Update (){
		//71 notches on thing
//		print(plane.gameObject.transform.localEulerAngles.y+"\t"+level.y+"\t"+Mathf.DeltaAngle(plane.gameObject.transform.localEulerAngles.y, level.y ));
		hud.transform.localPosition = new Vector3(0, Mathf.DeltaAngle(plane.gameObject.transform.localEulerAngles.y, level.y)*-5.785f+525,0);
	}

	void Calibrate(){
		level = plane.gameObject.transform.localEulerAngles;
		print (level);
	}
}

