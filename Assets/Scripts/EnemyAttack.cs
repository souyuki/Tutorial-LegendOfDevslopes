using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

	// attack distance
	[SerializeField] private float attackRange = 3f;

	// The amount of time between attacks
	[SerializeField] private float timeBetweenAttacks = 1f;

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

		// Start the recursive enemy attack coroutine
		StartCoroutine( Attack() );

	}
	
	// Update is called once per frame
	void Update ()
	{

		// calculate distance between enemy and the player
		if ( Vector3.Distance (transform.position, player.transform.position) < attackRange ) {
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

}
