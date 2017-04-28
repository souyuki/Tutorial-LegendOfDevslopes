using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

	// Minimum attack distance
	[SerializeField] private float attackRangeMin = 0f;

	// Maxmimum attack distance
	[SerializeField] private float attackRangeMax = 3f;

	// The amount of time between attacks
	[SerializeField] private float timeBetweenAttacks = 1f;

	// Access to the enemy health
	private EnemyHealth enemyHealth;

	// Animator to play the attack animations
	private Animator animator;

	// store the player
	private GameObject player;

	// check if player is in attack range
	private bool isPlayerInRange;

	// Store all the weapon colliders (for dual wielders)
	private BoxCollider[] weaponColliders;


	// Use this for initialization
	void Start () {

		// get the animator component
		animator = GetComponent<Animator>();

		// get the weapon colliders buried in the hierarchy
		weaponColliders = GetComponentsInChildren<BoxCollider>();

		// get the player from GameManager
		player = GameManager.instance.Player;

		// get access to the enemy health
		enemyHealth = GetComponent<EnemyHealth>();

		// Start the recursive enemy attack coroutine
		StartCoroutine( Attack() );

	}
	
	// Update is called once per frame
	void Update ()
	{
		// get float distance between enemy and player
		float currentEnemyDistance = Vector3.Distance (transform.position, player.transform.position);

		// calculate distance between enemy and the player and check enemy is alive
		if ( currentEnemyDistance >= attackRangeMin && currentEnemyDistance <= attackRangeMax && enemyHealth.IsAlive) {
			// player is in range
			isPlayerInRange = true;
		} else {
			isPlayerInRange = false;
		}

		//Debug.Log("isPlayerInRange: " + isPlayerInRange);


	}


	// Enemy Attack Process
	IEnumerator Attack ()
	{

		// check if player is in range and the game is not over
		if (isPlayerInRange && !GameManager.instance.IsGameOver) {

			// play the attack animation
			animator.Play("Attack");

			// time between attacks
			yield return new WaitForSeconds(timeBetweenAttacks);

		}

		yield return null;

		StartCoroutine( Attack() );

	}

	// Enable the Weapon Box Collider - animation event
	public void EnemyAttackBegin ()
	{

		// enable weapon box collider
		foreach (BoxCollider weapon in weaponColliders) {

			// enable
			weapon.enabled = true;
		}

	}

	// Disable the Weapon Box Collider - animation event
	public void EnemyAttackEnd ()
	{
		// disable weapon box collider
		foreach (BoxCollider weapon in weaponColliders) {

			// disable
			weapon.enabled = false;
		}

	}

}
