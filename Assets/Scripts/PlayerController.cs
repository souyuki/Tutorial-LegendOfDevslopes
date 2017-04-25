using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	// hero move speed
	[SerializeField] private float moveSpeed = 10.0f;

	[SerializeField] private LayerMask layerMask;


	[SerializeField] private float turnSpeedSmoothing = 10f;



	//
	private CharacterController characterController;

	// our current look target;  init at zero
	private Vector3 currentLookTarget = Vector3.zero;

	private Animator animator;

	// Use this for initialization
	void Start () {

		// get reference to the character controller component
		characterController = GetComponent<CharacterController>();

		// set reference to animator controller
		animator = GetComponent<Animator>();
		
	}
	
	// Update is called once per frame
	void Update ()
	{

		// move direction is in which the buttons are pressed
		Vector3 moveDirection = new Vector3 (Input.GetAxis ("Horizontal"), 0f, Input.GetAxis ("Vertical"));

		// use built-in function to move it.
		characterController.SimpleMove (moveDirection * moveSpeed);


		// detect and trigger walk animation
		if (moveDirection == Vector3.zero) {
			animator.SetBool ("IsWalking", false);
		} else {
			animator.SetBool ("IsWalking", true);
		}

		// LEFT MOUSE BUTTON - double chop attack
		if(Input.GetMouseButtonDown(0) ){
			animator.Play("DoubleChop");
		}

		// RIGHT MOUSE BUTTON - spin attack
		if(Input.GetMouseButtonDown(1) ){
			animator.Play("Whirlwind");
		}



	}

	// runs after all physics 
	void FixedUpdate ()
	{
		// create variable for raycast
		RaycastHit hit;

		// send ray from camera.main to user mouse position
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		// Visualize the raycast; send ray from origin point, in its direction for 500 units, set it to color blue
		Debug.DrawRay (ray.origin, ray.direction * 500, Color.blue);

		// make the Ray, store the hits, ray distance of 500, pass layer mask we are tryig to hit, then ignore any other physics triggers it might cause
		if (Physics.Raycast (ray, out hit, 500, layerMask, QueryTriggerInteraction.Ignore)) {

			// if we arent already looking at where the mouse is pointed, then look there
			if (hit.point != currentLookTarget) {
				// set look to new location
				currentLookTarget = hit.point;
			}

			// get the mouse position in the X,Z planes, no vertical movement.
			Vector3 targetPosition = new Vector3 (hit.point.x, transform.position.y, hit.point.z);

			// Calculate the rotation for the hero to look at the new targetPosition
			Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);


			// actually rotate the hero
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * turnSpeedSmoothing);
		}

	}


}
