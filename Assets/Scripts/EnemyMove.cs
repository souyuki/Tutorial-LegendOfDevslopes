using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;


public class EnemyMove : MonoBehaviour {

	[SerializeField] private Transform player;

	private NavMeshAgent nav;
	private Animator animator;


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
	void Update () {

		// set pathfinding for the enemies to go after the player
		nav.SetDestination(player.position);

	}
}
