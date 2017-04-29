using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class PlayerHealth : MonoBehaviour {

	// Starting health of the player
	[SerializeField] private int startingHealth = 100;

	// Time allowed between taking hits
	[SerializeField] private float timeSinceLasthit = 2f;

	// Wire up the health slider bar
	[SerializeField] private Slider healthSlider;

	// 
	private ParticleSystem blood;


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

	// 
	void Awake()
	{
		// make sure we have a health slider
		Assert.IsNotNull(healthSlider);	

	}


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

		// get the blood particle effect
		blood = GetComponentInChildren<ParticleSystem>();


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

			// play the blood particle fx
			blood.Play();


			// play the sound effect
			audioSource.PlayOneShot(playerHitSFX);

			// TODO - give enemies independent damage
			// Apply Damage
			currentHealth -= 10;

			healthSlider.value = currentHealth;

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
