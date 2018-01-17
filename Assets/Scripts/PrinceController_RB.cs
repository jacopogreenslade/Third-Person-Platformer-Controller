using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinceController_RB : MonoBehaviour {

	private Rigidbody rigidbody;
	private float velocity;

	public Transform camera;

	public float characterTurnSpeed = 1.0f;

	public float distanceToGround = 2f;

	public float speed = 10.0f;

	public float extraGravity = 10f;

	public float jumpStrength = 300.0f;

	private float stepRayOffset = 2.845f;

	private Animator animator;

	// THese relate to the stepping up on steps actions
	private bool stepping = false;
	private RaycastHit stepHit;
	private ContactPoint stepCollistionPoint;

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody>();
		animator = GetComponentInChildren<Animator>();
	}

	void Update() {
		updateAnimator();
	}

	RaycastHit checkForStep() {
		RaycastHit hit;
		Physics.Raycast(transform.position + transform.forward + transform.up * 2, Vector3.down, out hit, 5f);
		Debug.DrawRay(transform.position + transform.forward + transform.up * 2, Vector3.down * hit.distance, Color.magenta, 1f);
		return hit;
	}

	void OnCollisionEnter(Collision c) {
		
		// This is stupid. Why should I need to create it like this???
		ContactPoint tempPoint = new ContactPoint();
		foreach (ContactPoint contact in c.contacts) {
            Debug.DrawRay(contact.point, -contact.normal, Color.white, 2f);
			print("Hit wall at this angle: " + Vector3.Angle(transform.forward, -contact.normal));
			if (Vector3.Angle(transform.forward, -contact.normal) < 25) {
				stepping = true;
				tempPoint = contact;
			}
        }

		RaycastHit hit = checkForStep();
		float stepHeight = getStepHeight(hit);
		// print(Mathf.Round(100*(stepRayOffset - hit.distance))/100);

		if (stepHeight < 0.1f) {
			stepping = false;
		}
		
		if (stepping) {
			// set info so that the fixed update can come along and perform the movement
			stepCollistionPoint = tempPoint;
			stepHit = hit;
		}
	}

	float getStepHeight(RaycastHit hit) {
		return Mathf.Round(100*(stepRayOffset - hit.distance))/100;
	}

	/*When we step up on a platform we should turn to face it, as we would normally */
	// TODO: IMPORTANT! Rename stepCollistion and stepHit to make it clearer what they do
	void calculateStepRotation() {
		if (transform.rotation != Quaternion.LookRotation(-stepCollistionPoint.normal))
        	transform.rotation = Quaternion.LookRotation(-stepCollistionPoint.normal);
	}

	/*Calculate the movement necessary to get from current location to on top of the step */
	// TODO: Create a step size that doesn't change so you can change the slerp time param based on height
	Vector3 calculateStepMove() {
		Vector3 stepLandingPosition = new Vector3(stepHit.point.x, stepHit.point.y + distanceToGround, stepHit.point.z);
		transform.position = Vector3.Slerp(transform.position, stepLandingPosition, 0.1f);

		// It seems pointless to recalculate this just for the next method
		return stepLandingPosition;
	}

	bool isSteppingFinished(Vector3 finalPosition) {
		if (Vector3.Distance(transform.position, finalPosition) < 0.05f)
			return true;

		return false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float hori = Input.GetAxis("Horizontal");
		float vert = Input.GetAxis("Vertical");

		if (stepping) {
			// TODO: Check for assigned variables stepHit and StepCollision
			//		 before reassigning them in onCollision
			if (!rigidbody.isKinematic)
				rigidbody.isKinematic = true;
			calculateStepRotation();
			Vector3 finalPosition = calculateStepMove();
			stepping = !isSteppingFinished(finalPosition);
			return;
		}
		// Reset rigidbody
		if (rigidbody.isKinematic)
			rigidbody.isKinematic = false;

		ApplyRotation(hori, vert);
		Vector3 movement = calculateHorizontalMove(hori, vert);

		movement = movement * Time.deltaTime * speed;

		if (Input.GetButtonDown("Jump") && isGrounded()) {
			movement.y = jumpStrength * Time.deltaTime;
		} else {
			movement.y = rigidbody.velocity.y;
			rigidbody.AddForce(Vector3.down * extraGravity);
		}
		rigidbody.velocity = movement;
	}

	void updateAnimator() {
		// Get the character speed from the magnitude
		Vector3 groundSpeed = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
		float animSpeed = groundSpeed.magnitude;

		animator.SetFloat("Speed", animSpeed < 0.5 ? 0 : animSpeed);
		animator.SetFloat("VerticalVelocity", rigidbody.velocity.y);
		animator.SetBool("Grounded", isGrounded());

		animator.SetBool("Stepping", stepping);
		if (stepping){
			animator.SetFloat("StepSize", getStepHeight(stepHit) / 2);
		}
	}

	bool isGrounded() {
		// TODO: when running it may be smart to place the raycast behind the player somewhat
		// Debug.DrawRay(transform.position, Vector3.down, new Color(1, 0, 0), 1f);
		return Physics.Raycast(transform.position, Vector3.down, distanceToGround);
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
		Quaternion rotate = Quaternion.Euler(new Vector3(0, turnAngle, 0));

		if (v != 0 || h != 0) {
			transform.rotation = Quaternion.Lerp(
				transform.rotation,
				rotate,
				characterTurnSpeed);
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
