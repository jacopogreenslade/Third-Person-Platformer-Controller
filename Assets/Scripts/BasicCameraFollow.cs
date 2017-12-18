using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** When the target obj is in motion the camera must:
 * 	move with it
 *	align it's position with the player's motion direction (or forward direction)
 *	
 */
public class BasicCameraFollow : MonoBehaviour {

	public Transform targetObj;
	public float alignSpeedMov = 3.0f;
	public float alignSpeedStill = 2.0f;
	public float distance = 3f;
	public float cameraHeight = 4f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		float vert = Input.GetAxis("Vertical");
		float hori = Input.GetAxis("Horizontal");
		float alignSpeed = vert > 0 ? alignSpeedMov : alignSpeedStill;

		Vector3 pForRot = targetObj.forward.normalized;

		Vector3 destination = targetObj.position + (pForRot*(-distance));
		destination.y = cameraHeight;

		if (vert != 0 || hori != 0){
			transform.position = Vector3.Slerp(transform.position, destination, alignSpeed);
		}
	}
}
