using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTesting : MonoBehaviour {

	Mesh mesh;

	// Use this for initialization
	void Start () {
		mesh = GetComponent<MeshFilter> ().mesh;
	}
	
	// Update is called once per frame
	void Update () {
		mesh.triangles = new int[0];
	}
}
