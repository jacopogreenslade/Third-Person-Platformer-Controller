using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControllerRB : MonoBehaviour {

	private Rigidbody rigidbody;
	private float velocity;

	static float MAX_SPEED = 15.0f;
	static float SPEED = 10.0f;

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody>();
		velocity = 0f;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float hori = Input.GetAxis("Horizontal");
		float vert = Input.GetAxis("Vertical");

		// Apply movement directly to velocity
		if (hori != 0 || vert != 0) {
			velocity = velocity < MAX_SPEED ? velocity + 0.5f * SPEED : MAX_SPEED;
		} else {
			return;
		}

		Vector3 movement = new Vector3(hori * velocity, 0, vert * velocity);

		movement = movement * Time.deltaTime * SPEED;

		if (Input.GetButtonDown("Jump") && isGrounded()) {
			movement.y = 300.0f * Time.deltaTime;
		} else {
			movement.y = rigidbody.velocity.y;
		}
		// Apply with add force.
		// rigidbody.AddForce(movement * Time.deltaTime * SPEED);
		rigidbody.velocity = movement;

		Debug.Log("Velocity : " + rigidbody.velocity);
	}

	bool isGrounded() {
		Debug.DrawRay(transform.position, Vector3.down, new Color(1, 0, 0));
		return Physics.Raycast(transform.position, Vector3.down, 10.0f);
	}
}
