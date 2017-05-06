using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {


	// for the prefabs
	[SerializeField] GameObject hero;
	[SerializeField] GameObject tanker;
	[SerializeField] GameObject soldier;
	[SerializeField] GameObject ranger;

	// get the animators
	private Animator heroAnim;
	private Animator tankerAnim;
	private Animator soldierAnim;
	private Animator rangerAnim;

	// the two buttons
	[SerializeField] private Button battleButton;
	[SerializeField] private Button quitButton;




	// Use this for initialization
	void Start () {

		// get all the animator references
		heroAnim = hero.GetComponent<Animator>();
		tankerAnim = tanker.GetComponent<Animator>();
		soldierAnim = soldier.GetComponent<Animator>();
		rangerAnim = ranger.GetComponent<Animator>();


		// start the animations
		StartCoroutine( ShowCase() );
								
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// loop through the attack animations for each of the characters
	IEnumerator ShowCase()
	{

		// delay 1 sec
		yield return new WaitForSeconds(1f);

		// hero animates
		heroAnim.Play("Whirlwind");

		// delay 1 sec
		yield return new WaitForSeconds(1f);

		tankerAnim.Play("Attack");

		// delay 1 sec
		yield return new WaitForSeconds(1f);

		rangerAnim.Play("Attack");

		// delay 1 sec
		yield return new WaitForSeconds(1f);

		soldierAnim.Play("Attack");

		// delay 1 sec
		yield return new WaitForSeconds(1f);

		StartCoroutine( ShowCase() );

	}

	// Battle button functionality
	public void Battle ()
	{
		SceneManager.LoadScene("Level");

	}

	// Quit button functionality
	public void QuitGame ()
	{
		Application.Quit();

	}




}
