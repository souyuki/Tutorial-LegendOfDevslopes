using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour {

	// get the player to target
	private Transform player;

	private NavMeshAgent nav;
	private Animator animator;

	// get access to the enemy health
	private EnemyHealth enemyHealth;

	// TODO - the stopping distance from player
	//[SerializeField] float stopDistanceFromTarget;
	public float stopDistanceFromTarget = 10f;


	// Use this for initialization
	void Start () {

		// access to cmoponents
		animator 	= GetComponent<Animator>();
		nav 		= GetComponent<NavMeshAgent>();
		enemyHealth = GetComponent<EnemyHealth>();

		// give the enemy the player object
		player = GameManager.instance.Player.transform;

		//nav.stoppingDistance = stopDistanceFromTarget;
	}
	
	// Update is called once per frame
	void Update ()
	{

		// get distance between enemy and player
		//float currentDistanceFromTarget = Vector3.Distance(transform.position, player.position);

	 
		// check that game is not over and enemy is alive
		if (!GameManager.instance.IsGameOver && enemyHealth.IsAlive) {

			// set pathfinding for the enemies to go after the player
			nav.SetDestination (player.position);

		} else if ( (!GameManager.instance.IsGameOver || GameManager.instance.IsGameOver) && !enemyHealth.IsAlive) {
			// enemy is dead, regardless of game state
			nav.enabled = false;

		} else {

			// game is over, so have the animation turn to idle
			nav.enabled = false;
			animator.Play("Idle");

		}

	}

	// for debugging and understading distances
	void OnDrawGizmos ()
	{

//		// Stop Distance
//		Gizmos.color = Color.blue;
//		Gizmos.DrawWireSphere(transform.position, stopDistanceFromTarget);


	}

}
