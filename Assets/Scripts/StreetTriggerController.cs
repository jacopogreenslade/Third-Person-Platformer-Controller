using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetTriggerController : MonoBehaviour {

	public bool isRed = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter(Collider other) {
		if (isRed) {
			// Feedback
			Debug.Log("You Failed!");
		}
	}
}
