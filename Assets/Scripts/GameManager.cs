using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	// create singleton variable
	public static GameManager instance = null;

	[SerializeField] private GameObject player;


	// the spawn locations for the power ups
	[SerializeField] private GameObject[] powerUpSpawnPoints;
	[SerializeField] private int maxPowerUps = 3;
	private float powerUpSpawnTimeDelay = 5f; 
	private float currentPowerUpSpawnTimer = 0f;
	private int currentPowerUpCount = 0;
	private GameObject newPowerUp;
	[SerializeField] private GameObject healthPowerUp;
	[SerializeField] private GameObject speedPowerUp;


	// store all enemy spawn points
	[SerializeField] private GameObject[] spawnPoints;

	// Enemies to spawn
	[SerializeField] private GameObject enemyTanker;
	[SerializeField] private GameObject enemySoldier;
	[SerializeField] private GameObject enemyRanger;
	[SerializeField] private GameObject arrow;

	// which game level we are on text
	[SerializeField] private Text levelTextValue;

	// which current game level we are on value
	private int currentLevel;

	// enemy spawn rate on waves
	private float generatedSpawnTime = 1f;

	// elapsed time tracker between spawn
	private float currentSpawnTime = 0f;

	// create list of enemies to keep track of
	private List<EnemyHealth> spawnedEnemies = new List<EnemyHealth>();

	// create list of enemies that were killed
	private List<EnemyHealth> killedEnemies = new List<EnemyHealth>();


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

	// GETTER for player
	public GameObject Arrow {
		get {
			return arrow;
		}
	}


	public int CurrentPowerUpCount {
		get {
			return currentPowerUpCount;
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

		// set current level to 1
		currentLevel = 1;

		// start spawning enemies
		StartCoroutine( SpawnEnemy () );

		// start spawning powerups
		StartCoroutine( SpawnPowerUp () );

	}
	
	// Update is called once per frame
	void Update () {

		// keep track of time between spawn
		currentSpawnTime += Time.deltaTime;

		// keep track of power-up spawn time
		currentPowerUpSpawnTimer += Time.deltaTime;

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

	// store the spawned enemy
	public void RegisterEnemy (EnemyHealth enemy)
	{
		// add to list
		spawnedEnemies.Add(enemy);

	}

	// store the killed enemy
	public void KilledEnemy(EnemyHealth enemy)
	{
		// add to list
		killedEnemies.Add(enemy);

	}


	public void RegisterPowerUp ()
	{
		currentPowerUpCount++;
	}

	public void UnregisterPowerUp ()
	{
		currentPowerUpCount--;
	}


	IEnumerator SpawnEnemy ()
	{

		// check spawn time > current time
		if (currentSpawnTime > generatedSpawnTime) {

				// reset spawn time
				currentSpawnTime = 0f;

			// check enemy count
			if (spawnedEnemies.Count < currentLevel) {


				Debug.Log ("GameManager SpawnEnemy() :: Spawning an enemy!");

				// spawn enemy
				// 0 = solider, 1 = ranger, 2 = tanker
				GameObject enemyToSpawn = null;

				int rnd = Random.Range (0, 3);

				switch (rnd) {

				case 0:
					enemyToSpawn = enemySoldier;
					break;

				case 1:
					enemyToSpawn = enemyRanger;
					break;

				case 2:
					enemyToSpawn = enemyTanker;
					break;

				default:
					Debug.Log ("No Enemy Type Found in GameManager SpawnEnemy(). ERROR!");
					break;

				}


				// which spawn point is it at
				int spawnRND = Random.Range (0, spawnPoints.Length);
				Transform assignedSpawnPoint = spawnPoints[spawnRND].transform;

				// spawn it
				GameObject enemyCombatant = Instantiate (enemyToSpawn, assignedSpawnPoint) as GameObject;

				// move it to it
				enemyCombatant.transform.position = assignedSpawnPoint.position;


			} 

			if (killedEnemies.Count == currentLevel) {

				// clear both lists
				spawnedEnemies.Clear();
				killedEnemies.Clear();

				yield return new WaitForSeconds (3f);

				// set new level
				currentLevel += 1;
				levelTextValue.text = "Level " + currentLevel;

				Debug.Log("SpawnEnemy() :: Level Complete, new level: " + currentLevel);

			}

		}

		yield return null; 
		StartCoroutine( SpawnEnemy () );

	} // end SpawnEnemy()



	IEnumerator SpawnPowerUp ()
	{
		// if enough time has passed
		if (currentPowerUpSpawnTimer > powerUpSpawnTimeDelay) {

			currentPowerUpSpawnTimer = 0;

			// if max power-ups is not met
			if (CurrentPowerUpCount < maxPowerUps) {

				Debug.Log ("SpawnPowerUp() called");

				// which power-up
				int powerUpPick = Random.Range (0, 2);

				if (powerUpPick == 0) {
					newPowerUp = Instantiate (healthPowerUp) as GameObject;
				} else if (powerUpPick == 1) {
					newPowerUp = Instantiate (speedPowerUp) as GameObject;
				}

				// move it to spawn location
				int rnd = Random.Range(0,powerUpSpawnPoints.Length);
				newPowerUp.transform.position = powerUpSpawnPoints[rnd].transform.position;

			}

		}

		// return and be recursive
		yield return null; 
		StartCoroutine( SpawnPowerUp () );


	}


}
