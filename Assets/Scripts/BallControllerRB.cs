using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControllerRB : MonoBehaviour {

	private Rigidbody rigidbody;
	private float velocity;

	static float MAX_SPEED = 15.0f;
	static float SPEED = 5.0f;

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody>();
		velocity = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		float hori = Input.GetAxis("Horizontal");
		float vert = Input.GetAxis("Vertical");
		if (hori != 0 || vert != 0) {
			velocity = velocity < MAX_SPEED ? velocity + 0.5f * SPEED : MAX_SPEED;
		} else {
			velocity = 0;
		}
		Vector3 movement = new Vector3(velocity * hori, 0, velocity * vert);
		// movement.Normalize();
		rigidbody.AddForce(movement * Time.deltaTime * SPEED);

		Debug.Log(movement);
	}
}
