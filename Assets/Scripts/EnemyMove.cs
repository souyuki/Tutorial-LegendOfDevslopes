using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;


public class EnemyMove : MonoBehaviour {

	[SerializeField] private Transform player;

	private NavMeshAgent nav;
	private Animator animator;

	// TODO - the stopping distance from player
	[SerializeField] float stopDistanceFromTarget;

	void Awake()
	{

		Assert.IsNotNull(player);

	}


	// Use this for initialization
	void Start () {

		animator = GetComponent<Animator>();
		nav = GetComponent<NavMeshAgent>();

				
	}
	
	// Update is called once per frame
	void Update ()
	{

		// check that game is not over
		if (!GameManager.instance.IsGameOver) {

			// set pathfinding for the enemies to go after the player
			nav.SetDestination (player.position);

		} else {

			// game is over, so have the animation turn to idle
			nav.enabled = false;
			animator.Play("Idle");

		}

	}
}
