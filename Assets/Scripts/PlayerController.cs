using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	// hero move speed
	[SerializeField] private float moveSpeed = 10.0f;

	//
	private CharacterController characterController;


	// Use this for initialization
	void Start () {

		// get reference to the character controller component
		characterController = GetComponent<CharacterController>();


		
	}
	
	// Update is called once per frame
	void Update () {

		// move direction is in which the buttons are pressed
		Vector3 moveDirection = new Vector3( Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical") );

		// use built-in function to move it.
		characterController.SimpleMove(moveDirection * moveSpeed);

	}
}
