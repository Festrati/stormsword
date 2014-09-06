﻿using UnityEngine;
using System.Collections;

/* EnemyScript - Generic Enemy Behavior */

public class EnemyMovement
{
	public string enemyWander = "Wander";
	public string enemyStalk = "Stalker";
	public string enemyCharger = "Charge";
}

public class EnemyScript : MonoBehaviour {

	private CharacterScript characterScript;

	private CommandStackScript commands;
	
	public EnemyMovement enemyMovement;

	public string characterType = "Enemy";

	// String to define AI movement

	void Start() {
		characterScript = GetComponent<CharacterScript>();

		commands = new CommandStackScript();
	}

	void Update() {
		// Continuously attack until dead
		characterScript.Attack();

		commands.Execute();
	}
}
