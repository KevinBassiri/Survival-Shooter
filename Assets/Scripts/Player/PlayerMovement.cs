using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 6f; //Controls how fast player is, f says it's a floating point variable

	Vector3 movement;	//Stores the direction of the player's movement
	Animator anim; //Reference to the animator component
	Rigidbody playerRigidbody; //Reference to the rigidbody component
	int floorMask; // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
	float camRayLength = 100f; //The length of the ray from the camera into the scene

	void Awake()
	{
		floorMask = LayerMask.GetMask ("Floor"); //Create a layer mask for the floor layer
		anim = GetComponent<Animator> (); //Reference to the animator
		playerRigidbody = GetComponent <Rigidbody> (); //Refrence to the player's rigidbody
	}
	void FixedUpdate() //FixedUpdate run with physics, normal Update runs with rendering
	{
		float h = Input.GetAxisRaw ("Horizontal"); //Raw axis has a value of -1, 0, or 1 to snap to full speed upon moving
		float v = Input.GetAxisRaw ("Vertical"); 

		Move (h, v); //Each of these are called every physics state, since they are in FixedUpdate
		Turning ();
		Animating (h, v);
	}
	void Move(float h, float v)
	{
		movement.Set (h, 0f, v); //Set the movement vector based on the axis input
		movement = movement.normalized * speed * Time.deltaTime;//Normalize the movement vector and make it proportional to speed per second
		playerRigidbody.MovePosition (transform.position + movement); //Move the player to its current position plus the movement
	}
	void Turning()
	{
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition); //Creates a ray from the mouse cursor on screen, in the direction of the camera
		RaycastHit floorHit; //RaycastHit variable stores information about what was hit by the ray
		if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask)) //Perform the raycast and if it hits something on the floor layer...
		{
			Vector3 playerToMouse = floorHit.point - transform.position; //Create a vector from the player to the point on the floor the raycast from the mouse hit
			playerToMouse.y = 0f; //Ensure the vector is on the floor by making the y coordinate 0

			Quaternion newRotation = Quaternion.LookRotation(playerToMouse); //Create a quaternion(rotation) based on looking down on the vector from the player to the mouse
			playerRigidbody.MoveRotation(newRotation); //Set the player's rotation to this new rotation
		}
	}
	void Animating(float h, float v)
	{
		bool walking = h != 0f || v != 0f; //If either of the horizontal or vertical buttons are pressed then walking is true
		anim.SetBool ("IsWalking", walking); //Tell the animator whether or not the player is walking
	}
}
