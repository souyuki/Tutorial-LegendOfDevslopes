using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	// create singleton variable
	public static GameManager instance = null;

	[SerializeField] GameObject player;

	// keep track if game is over
	private bool isGameOver = false;

	// GETTER for isGameOver
	public bool IsGameOver {
		get {
			return isGameOver;
		}
		set {
			isGameOver = value;
		}
	}

	// GETTER for player
	public GameObject Player {
		get {
			return player;
		}
		set {
			player = value;
		}
	}


	void Awake ()
	{


		// Create singleton
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);

	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Check if player is still alive
	public void PlayerHit (int currentHP)
	{

		if (currentHP > 0) {
			// player is alive
			isGameOver = false;
		} else {
			// player is dead
			isGameOver = true;
		}


	}


}
