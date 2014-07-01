﻿using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	// Components
	private Animator animator;

	// Speed of the player
	public Vector2 speed = new Vector2(50, 50);

	// Store the player's movement
	private Vector2 movement;

	void Awake() {
		animator = GetComponent<Animator>();

	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// Retrieve axis information from controller
		float inputX = Input.GetAxis("Horizontal");
		float inputY = Input.GetAxis("Vertical");

		// Calculate movement per-direction
		movement = new Vector2(
			speed.x * inputX,
			speed.y * inputY);

		// Shooting!
		bool shoot = Input.GetButtonDown("Fire1");
		shoot |= Input.GetButtonDown("Fire2");

		if(shoot) {
			// User clicked 'fire'
			WeaponScript weapon = GetComponent<WeaponScript>();

			// If player has weapon equipped
			if(weapon != null) {
				weapon.Attack (false);	// False because player is not an enemy
			}
		}

		/* Play movement animation */
		// 0 - Up, 1 - Right, 2 - Down, 3 - Left
		// Player moving right
		if(movement.x > 0) {
			animator.SetInteger("direction", 1);
		}
		else if(movement.x < 0) {
			animator.SetInteger("direction", 3);
		}
		else if(movement.y > 0) {
			animator.SetInteger("direction", 0);
		}
		else if(movement.y < 0) {
			animator.SetInteger("direction", 2);
		}


		// Make sure player cannot leave the camera view
		var dist = (transform.position - Camera.main.transform.position).z;

		var leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;
		var rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;

		var topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).y;
		var bottomBorder = Camera.main.ViewportToWorldPoint (new Vector3(0, 1, dist)).y;

		transform.position = new Vector3(
			Mathf.Clamp (transform.position.x, leftBorder, rightBorder),
			Mathf.Clamp (transform.position.y, topBorder, bottomBorder),
			transform.position.z);

	}

	// Physics calcs
	void FixedUpdate() {
		// Move the game object
		rigidbody2D.velocity = movement;
	}
}
