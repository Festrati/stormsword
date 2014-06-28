﻿using UnityEngine;
using System.Collections;

/* moveScript - Moves the current game object */

public class MoveScript : MonoBehaviour {

	// Speed of object
	public Vector2 speed = new Vector2(10, 10);

	// Direction of object
	public Vector2 direction = new Vector2(-1, 0);

	// Actual movement
	private Vector2 movement;
	
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// Calculate movement direction
		movement = new Vector2(
			speed.x * direction.x,
			speed.y * direction.y);
	}

	void FixedUpdate() {
		// Apply the movement to the rigidbody
		rigidbody2D.velocity = movement;
	}
}