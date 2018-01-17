using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTextController : MonoBehaviour {

	void OnMouseEnter() {
		Debug.Log("Enter");
	}

	void OnMouseDown() {
		Debug.Log("Clicked");
	}

	void OnMouseExit() {
		Debug.Log("Exit");
	}
}
