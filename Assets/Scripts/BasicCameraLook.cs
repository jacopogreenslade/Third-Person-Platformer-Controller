using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCameraLook : MonoBehaviour {
	public Transform targetObj;
	public Transform cameraCenter;
	public float lookSpeed = 3.0f;
	public float sensitivity = 10f;

	private Vector3 lookVector;
	private Quaternion lookRotation;

	private float mouseX;
	private float mouseY;

	// Use this for initialization
	void Start () {
		mouseX = 0;
		mouseY = 0;
	}
	
	// Update is called once per frame
	void Update () {
		mouseX += Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;
		mouseY += Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;

		mouseY = Mathf.Clamp(mouseY, 0f, 180f);

		// This function does the same
		transform.LookAt(cameraCenter.position);
		// Rotate with mouse
		cameraCenter.localRotation = Quaternion.Euler(-mouseY, mouseX, 0);
		// Match player position
		cameraCenter.position = new Vector3(targetObj.position.x, targetObj.position.y + 1f, targetObj.position.z);
	}
}
