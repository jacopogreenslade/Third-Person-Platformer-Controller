using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Align the character rotation with the forward vector of the camera
 */
public class BasicTPC : MonoBehaviour {

	public Transform camera;
	public float speed = 10.0f;
	private Vector3 moveVector;
	private Quaternion rotate;

	public Animator animator;

	private CharacterController controller;

	protected Vector3 velocity;
	public float gravityModifier = 1.0f;

	// Use this for initialization
	void Start () {
		moveVector = new Vector3();
		rotate = new Quaternion();
		animator = GetComponentInChildren<Animator>();
		controller = GetComponent<CharacterController>();
	}

	void FixedUpdate () {
		// Calculate gravity
		velocity = Physics.gravity * gravityModifier * Time.deltaTime;
		Vector3 deltaPosition = velocity * Time.deltaTime;

		// Only use the y for gravity
		Vector3 move = Vector3.up * deltaPosition.y;
	}
	
	// Update is called once per frame
	void Update () {
		float vert = Input.GetAxis("Vertical");
		float hori = Input.GetAxis("Horizontal");

		ApplyRotation(hori, vert);
		Vector3 horiMove = calculateHorizontalMove(hori, vert);
		
		if (!controller.isGrounded){
			controller.Move(Physics.gravity * Time.deltaTime);
		} else {
			if (!new Vector3().Equals(horiMove))
			controller.Move(horiMove * Time.deltaTime * speed);
		}
		updateAnimator();
	}

	void updateAnimator() {
		// Get the character speed from the magnitude
		float animSpeed = controller.velocity.magnitude;
		animator.SetFloat("Speed", animSpeed < 0.5 ? 0 : animSpeed);
		Debug.Log(animSpeed);
	}

	void ApplyRotation(float h, float v) {
		// The starting angle is always the camera's y angle
		float turnAngle = camera.rotation.eulerAngles.y;
		// Get the angle in radians, convert it, then make sure it's positive
		turnAngle += Mathf.Atan2(h, v) * Mathf.Rad2Deg;
		// Reset angle if negative
		if (turnAngle < 0) {
			turnAngle += 360;
		}
		// Get the rotation quaternion from the turn angle
		rotate = Quaternion.Euler(new Vector3(0, turnAngle, 0));
		if (v != 0 || h != 0) {
			transform.rotation = Quaternion.Slerp(
				transform.rotation,
				rotate,
				0.1f);
		}
	}

	Vector3 calculateHorizontalMove(float h, float v) {
		// Clamp input to be usefull
		float processedSpeed = Mathf.Clamp(Mathf.Abs(v) + Mathf.Abs(h), 0, 1f);

		// Transform the local move vector to world space
		Vector3 adjustedMove = transform.TransformDirection(new Vector3(0, 0, processedSpeed));

		return adjustedMove;
	}
}
