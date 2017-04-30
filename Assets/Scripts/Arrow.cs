using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

	// destroy the arrows on collision
	void OnCollisionEnter(Collision col) {

		Destroy(gameObject);

	}


}
