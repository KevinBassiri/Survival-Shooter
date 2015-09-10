using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform target; //Position the camera will be following
	public float smoothing = 5f; //Camera will be smoother when following player

	Vector3 offset; //Initial offset from target

	void Start() //Sets the initial position away from the target
	{
		offset = transform.position - target.position;//Calculates the inital offset
	}
	void FixedUpdate() //Used to move the camera during every physic effect
	{
		Vector3 targetCamPos = target.position + offset; //Creates a position the camera is aiming for based on the offset of the target
		transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime); // Smoothly interpolate between the camera's current position and it's target position.
	}
}
