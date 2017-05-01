using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerUp : MonoBehaviour {

	private GameObject player;

	// get reference to player heath script
	private PlayerHealth playerHealth;

	[SerializeField] private int healAmount = 30;

	// to play the sound effect
	private AudioSource audioSource;

	// get the power=Up SFX
	[SerializeField] private AudioClip sfxPowerUp;

	// Use this for initialization
	void Start () {

		player = GameManager.instance.Player;
		playerHealth = player.GetComponent<PlayerHealth>();

		// get the audio source to play the powerup clip
		audioSource = GetComponent<AudioSource>();

		// register the creation of this powerup
		GameManager.instance.RegisterPowerUp();

	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void OnTriggerEnter (Collider other)
	{
		// if the colliding game object is the player game object
		if (other.gameObject == player) {

			// increment the powerup
			playerHealth.PowerUpHealth (healAmount);

			// play the sound effect
			audioSource.PlayOneShot(sfxPowerUp);

			// consume the powerup
			// register the creation of this powerup
			GameManager.instance.UnregisterPowerUp();
			Destroy (gameObject);

		}


	}


}
