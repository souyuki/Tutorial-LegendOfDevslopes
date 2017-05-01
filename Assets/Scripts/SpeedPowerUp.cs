using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : MonoBehaviour {


	private GameObject player;
	private PlayerController playerController;


	// Use this for initialization
	void Start () {

		// get access to player
		player = GameManager.instance.Player;

		// get the players player controller
		playerController = player.GetComponent<PlayerController>();

		// register the existence of this powerup
		GameManager.instance.RegisterPowerUp();

	}



	// collecting the speed power-up
	void OnTriggerEnter (Collider other)
	{
		// was it the player that collided with it>
		if (other.gameObject == player) {

			// apply the power-up effects
			playerController.SpeedPowerUp();

			// consume the power up
			Destroy(gameObject);

			// unregister it
			GameManager.instance.UnregisterPowerUp();

		}

	}
		


}
