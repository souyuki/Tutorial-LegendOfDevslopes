using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;


public class EnemyMove : MonoBehaviour {

	[SerializeField] private Transform player;

	private NavMeshAgent nav;
	private Animator animator;

	// get access to the enemy health
	private EnemyHealth enemyHealth;

	// TODO - the stopping distance from player
	[SerializeField] float stopDistanceFromTarget;

	void Awake()
	{

		Assert.IsNotNull(player);

	}


	// Use this for initialization
	void Start () {

		// access to cmoponents
		animator 	= GetComponent<Animator>();
		nav 		= GetComponent<NavMeshAgent>();
		enemyHealth = GetComponent<EnemyHealth>();

				
	}
	
	// Update is called once per frame
	void Update ()
	{
	 
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
}
