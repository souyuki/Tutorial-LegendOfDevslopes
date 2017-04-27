using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

	// Starting health of the player
	[SerializeField] private int startingHealth = 100;

	// Time allowed between taking hits
	[SerializeField] private float timeSinceLasthit = 2f;

	// track time between hits
	private float timer = 0f;

	// to be able to kill all movement when player is dead
	private CharacterController characterController;

	// Access to play the death animation
	private Animator animator;

	// Current health of player
	[SerializeField] private int currentHealth;

	// get the player's audio source
	private AudioSource audioSource;

	// when the player is hit by the enemy
	[SerializeField] private AudioClip playerHitSFX;


	// Use this for initialization
	void Start () {

		// get access to the animator
		animator = GetComponent<Animator>();

		// get access to character controller 
		characterController = GetComponent<CharacterController>();

		// set current health
		currentHealth = startingHealth;

		// get the player's audio source
		audioSource = GetComponent<AudioSource>();

	}
	
	// Update is called once per frame
	void Update () {

		// increment time elapsed
		timer += Time.deltaTime;
				
	}


	void OnTriggerEnter (Collider other)
	{
		// did we collide with a weapon and it's been long enough?
		if (timer > timeSinceLasthit && !GameManager.instance.IsGameOver) {

			// if it was a weapon
			if (other.tag == "Weapon") {

				// apply the hit to the player
				TakeHit();


				// reset the time elapsed counter
				timer = 0f;

			}

		}


	}


	private void TakeHit ()
	{
		if (currentHealth > 0) {

			// process the hit against hit points
			GameManager.instance.PlayerHit (currentHealth);

			// play the player being hit animation
			animator.Play ("Hurt");

			// play the sound effect
			audioSource.PlayOneShot(playerHitSFX);

			// TODO - give enemies independent damage
			// Apply Damage
			currentHealth -= 10;

		}

		// if player is at or below zero, player is dead
		if (currentHealth <= 0) {

			// Kill the player
			KillPlayer();
		}

	}

	// Handle player death
	private void KillPlayer ()
	{

		// pass the current health 
		GameManager.instance.PlayerHit(currentHealth);

		// play the Death animation
		animator.SetTrigger("HeroDie");

		// Disable controls to prevent movement
		characterController.enabled = false;

	}
}
