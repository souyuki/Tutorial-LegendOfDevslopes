using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CameraFollow : MonoBehaviour {

	// expose hero object to targat the camera.
	[SerializeField] private Transform target;

	// camera smoothing when following player
	[SerializeField] float smoothing = 5f;

	// store the offset of the camera from the player
	private Vector3 offset;

	void Awake()
	{
		// enforce player has been wired up to camera
		Assert.IsNotNull(target);

	}


	// Use this for initialization
	void Start () {

		// calculate the offset between camera and player
		offset = transform.position - target.position;
		
	}
	
	// Update is called once per frame
	void Update () {

		// determine where camera should be
		Vector3 followCameraPosition = target.position + offset;

		// move the camera to the correct position via lerp, over the amount of time specified.
		transform.position = Vector3.Lerp(transform.position, followCameraPosition, smoothing * Time.deltaTime);

		

	}
}
