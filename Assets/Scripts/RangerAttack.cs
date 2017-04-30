using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerAttack : MonoBehaviour {

	// Minimum attack distance
	[SerializeField] private float attackRangeMin = 0f;

	// Maxmimum attack distance
	[SerializeField] private float attackRangeMax = 10f;

	// The amount of time between attacks
	[SerializeField] private float timeBetweenAttacks = 1f;

	// arrow firign location
	[SerializeField] private Transform fireLocation;

	// Access to the enemy health
	private EnemyHealth enemyHealth;

	// Animator to play the attack animations
	private Animator animator;

	// store the player
	private GameObject player;

	// arrow projectile
	private GameObject arrow;


	// check if player is in attack range
	private bool isPlayerInRange;


	// Use this for initialization
	void Start () {

		// get the animator component
		animator = GetComponent<Animator>();

		// get the player from GameManager
		player = GameManager.instance.Player;

		// get access to the enemy health
		enemyHealth = GetComponent<EnemyHealth>();

		// get access to the arrow object
		arrow = GameManager.instance.Arrow;

		// Start the recursive enemy attack coroutine
		StartCoroutine( Attack() );

	}
	
	// Update is called once per frame
	void Update ()
	{
		// get float distance between enemy and player
		float currentEnemyDistance = Vector3.Distance (transform.position, player.transform.position);

		// calculate distance between enemy and the player and check enemy is alive
		if ( currentEnemyDistance <= attackRangeMax && enemyHealth.IsAlive) {
			// player is in range
			isPlayerInRange = true;

			// set animation trigger to true
			animator.SetBool("PlayerInRange", true);

			// rotate to face the player
			RotateTowards(player.transform);

		} else {
			
			isPlayerInRange = false;

			// set animation trigger to false
			animator.SetBool("PlayerInRange", false);
		}

		//Debug.Log("isPlayerInRange: " + isPlayerInRange);


	}


	// Enemy Attack Process
	IEnumerator Attack ()
	{

		// check if player is in range and the game is not over
		if (isPlayerInRange && !GameManager.instance.IsGameOver) {

			Debug.Log("Player in range, attack()");

			// play the attack animation
			animator.Play("Attack");

			// time between attacks
			yield return new WaitForSeconds(timeBetweenAttacks);

		}

		yield return null;

		StartCoroutine( Attack() );

	}


	// Rotate the Ranger
	private void RotateTowards (Transform playerTransform)
	{
		// generate direction vector
		Vector3 direction = (playerTransform.position - transform.position).normalized;

		// generate rotation 
		Quaternion lookRotation = Quaternion.LookRotation(direction);

		// now turn the player
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
		

	}

	// generate and fire the arrow from the crossbow
	public void FireArrow ()
	{
		//Debug.Log ("Firing arrow");

		// create the arrow
		GameObject newArrow = Instantiate(arrow) as GameObject;

		// position the starting point for the arrow.
		newArrow.transform.position = fireLocation.position;

		// add rotation to point it along with ranger
		newArrow.transform.rotation = transform.rotation;

		// get the rigidbody
		newArrow.GetComponent<Rigidbody>().velocity = transform.forward * 25f;




	}


	// for debugging and understading distances
	void OnDrawGizmos ()
	{

		// draw move sphere
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, attackRangeMax);


	}


}
