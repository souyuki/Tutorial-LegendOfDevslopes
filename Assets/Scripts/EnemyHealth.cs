using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour {

	// Amount of starting health for the enemy
	[SerializeField] private int startingHealth = 20;

	// the curent health of the enemy
	private int currentHealth;

	// timer to track how often they can be hit
	[SerializeField] private float timeSinceLastHit = 0.5f;

	// track how long for the body to remain until it falls through floor
	[SerializeField] private float disappearSpeed = 2f;

	// get access for sound effects
	private AudioSource audioSource;

	// hit sound for enemy
	[SerializeField] private AudioClip enemyHitSFX;

	// track time elapsed
	private float timer;

	// access to animator to play animations
	private Animator animator;

	// get access to the nav mesh agent
	private NavMeshAgent nav;

	// is the enemy still alive
	private bool isAlive;

	// access to rigidbody component
	private Rigidbody rigidBody;

	// access to capsule collider component
	private CapsuleCollider capsuleCollider;

	// has the enemy disappeared
	private bool disappearEnemy = false;


	// GETTER for isAlive
	public bool IsAlive {
		get {
			return isAlive;
		}
		set {
			isAlive = value;
		}
	}


	// Use this for initialization
	void Start () {

		// get access to all the components
		rigidBody 		= GetComponent<Rigidbody>();
		capsuleCollider = GetComponent<CapsuleCollider>();
		nav 			= GetComponent<NavMeshAgent>();
		animator 		= GetComponent<Animator>();
		audioSource 	= GetComponent<AudioSource>();

		// initialize isAlive to TRUE
		isAlive = true;

		// set current health to starting health
		currentHealth = startingHealth;


	}
	
	// Update is called once per frame
	void Update ()
	{

		// track time elapsed
		timer += Time.deltaTime;

		// if the enemy is dead and needs to fall through floor
		if (disappearEnemy) {

			// move it downward at a speed over each frame.
			transform.Translate(-Vector3.up * disappearSpeed * Time.deltaTime);

		}

	}


	// Check for incoming hits from Player
	void OnTriggerEnter (Collider other)
	{

		// has enough time passed since last hit and Game isnt over
		if (timer >= timeSinceLastHit && !GameManager.instance.IsGameOver) {

			// check to make sure the collider is a player weapon
			if (other.tag == "PlayerWeapon") {

				// apply the hit
				TakeHit();

				// reset the timer
				timer = 0;
			}

		}


	}

	// Apply and check the damage
	private void TakeHit ()
	{

		// if the enemy is still alive, play animations and sounds
		if (currentHealth > 0) {

			// play the hit sound
			audioSource.PlayOneShot (enemyHitSFX);

			// play the Hurt animation
			animator.Play ("Hurt");

			// apply the damage
			currentHealth -= 10;

			Debug.Log(gameObject.name + " taking damage from player");

		}

		// are they dead?
		if (currentHealth <= 0) {

			// enemy is dead!
			isAlive = false;

			// kill the neemy
			KillEnemy();


		}


	}

	// Enemy is dead so let's kill it
	private void KillEnemy()
	{
		// disable some stuff

		// turn off collider
		capsuleCollider.enabled = false;

		// turn off nav agent
		nav.enabled = false;

		// allow body to move via script and not physics
		rigidBody.isKinematic = true;

		// play death animation
		animator.SetTrigger("EnemyDie");

		// trap door exit
		StartCoroutine( RemoveEnemy() ) ;
	}

	// Leave body for amoment ebfore pushing it through floor
	IEnumerator RemoveEnemy ()
	{
		// wait for 4 seconds then start sinking
		yield return new WaitForSeconds(4f);

		// should we be sinking the enemy?
		disappearEnemy = true;

		// wait to sink
		yield return new WaitForSeconds(2f);

		// destroy the enemy game object when its below the floor
		Destroy(gameObject);

	}



}
