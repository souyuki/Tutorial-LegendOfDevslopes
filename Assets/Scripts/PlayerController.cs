using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	// hero move speed
	[SerializeField] private float moveSpeed = 6.0f;

	// move bonus for speed power-up
	private float speedPowerUpIncreaseAmount = 6.0f;

	// speed boost particle effect
	private GameObject fireTrail;
	private ParticleSystem fireParticleSystem;

	[SerializeField] private LayerMask layerMask;

	[SerializeField] private float turnSpeedSmoothing = 10f;

	// get the colliders for the hero swords
	private BoxCollider[] heroSwordColliders;

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

		// look for the sword colliders
		heroSwordColliders = GetComponentsInChildren<BoxCollider>();

		// get the firetrail object
		fireTrail = GameObject.FindWithTag("Fire") as GameObject;

		// turn it off at the start
		fireTrail.SetActive(false);

	}
	
	// Update is called once per frame
	void Update ()
	{

		if (!GameManager.instance.IsGameOver) {

			PlayerMove ();

		}

	}

	// runs after all physics 
	void FixedUpdate ()
	{

		if (!GameManager.instance.IsGameOver) {

			PlayerLookAt ();

		}

	} // Fixed Update


	private void PlayerMove ()
	{
		// move direction is in which the buttons are pressed
		Vector3 moveDirection = new Vector3 (Input.GetAxis ("Horizontal"), 0f, Input.GetAxis ("Vertical"));
		// use built-in function to move it.
		characterController.SimpleMove (moveDirection * moveSpeed);
		// detect and trigger walk animation
		if (moveDirection == Vector3.zero) {
			animator.SetBool ("IsWalking", false);
		}
		else {
			animator.SetBool ("IsWalking", true);
		}
		// LEFT MOUSE BUTTON - double chop attack
		if (Input.GetMouseButtonDown (0)) {
			animator.Play ("DoubleChop");
		}
		// RIGHT MOUSE BUTTON - spin attack
		if (Input.GetMouseButtonDown (1)) {
			animator.Play ("Whirlwind");
		}
	}


	private void PlayerLookAt ()
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
			Quaternion rotation = Quaternion.LookRotation (targetPosition - transform.position);
			// actually rotate the hero
			transform.rotation = Quaternion.Lerp (transform.rotation, rotation, Time.deltaTime * turnSpeedSmoothing);
		}
	}


	// Enable the Weapon Box Collider - animation event
	public void HeroAttackBegin ()
	{

		// enable weapon box colliders
		foreach (BoxCollider weapon in heroSwordColliders) {

			// enable
			weapon.enabled = true;
		}

	}

	// Enable the Weapon Box Collider - animation event
	public void HeroAttackEnd ()
	{
		// disable weapon box colliders
		foreach (BoxCollider weapon in heroSwordColliders) {

			// disable
			weapon.enabled = false;
		}

	}

	// pickup of the speed power-up
	public void SpeedPowerUp ()
	{
		// call the coroutine
		StartCoroutine( FireTrailRoutine() );

	}

	// Coroutine for the speed power-up
	IEnumerator FireTrailRoutine ()
	{

		// add bonus move speed
		moveSpeed += speedPowerUpIncreaseAmount;

		// turn on wildfire particle
		fireTrail.SetActive(true);

		// give it 10 seconds
		yield return new WaitForSeconds(10f);

		// disable the speed
		moveSpeed -= speedPowerUpIncreaseAmount;

		// get access to particule system
		fireParticleSystem = fireTrail.GetComponent<ParticleSystem>();

		// get the emitter
		var em = fireParticleSystem.emission;

		// turn off the emitter prior to killing the power-up
		em.enabled = false;

		// give it a 3 seconds fade out
		yield return new WaitForSeconds(3f);

		// turn off the particle
		em.enabled = true;
		fireTrail.SetActive(false);


	}

}
