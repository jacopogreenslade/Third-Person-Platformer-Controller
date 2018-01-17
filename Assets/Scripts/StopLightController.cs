using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopLightController : MonoBehaviour {

	public Material green;

	public Material red;

	private Material actualMaterial;

	private float counter;

	public StreetTriggerController controller;

	public bool isRed;

	// Use this for initialization
	void Start () {
		counter = 0f;

		actualMaterial = GetComponent<Renderer>().material;
		
		isRed = false;
	}
	
	// Update is called once per frame
	void Update () {
		switchLight();
		
		controller.isRed = isRed;
		// Debug.Log("isRed" + isRed);
	}

	void switchLight() {
		counter += Time.deltaTime;
		// Debug.Log("counter" + counter);

		if(counter >= 5) {
			counter = 0.0f;
			isRed = !isRed;
			actualMaterial.SetColor("_Color", isRed ? Color.red : Color.green);
		}
	}
}
