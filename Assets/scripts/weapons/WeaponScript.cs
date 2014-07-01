﻿using UnityEngine;
using System.Collections;

/* WeaponScript - Basic weapon class (Ranged or Melee) */

public class WeaponScript : MonoBehaviour {

	// Projectile prefab for shooting
	public Transform shotPrefab;

	// Weapon stats
	public float shootingRate = 0.25f; // Cooldown between attacks
	public float damage = 1;	// Damage a weapon does per attack
	public float radius = 5; 	// Radius the weapon affects upon impact

	// Remaining cooldown for shot
	private float shotCooldown;

	// Use this for initialization
	void Start () {
		shotCooldown = 0f;	// Object has not yet shot
	}

	void Update () {
		if(shotCooldown > 0) {
			shotCooldown -= Time.deltaTime;
		}
	}

	/* Attack - Shot triggered by another script */
	public void Attack(bool isEnemy) {
		if(CanAttack) {
			// User attacked, trigger cooldown
			shotCooldown = shootingRate;

			// Create a new shot
			var shotTransform = Instantiate(shotPrefab) as Transform;

			// Grab the position of the parent object (transform)
			shotTransform.position = transform.position;

			// Find a Shot
			ProjectileScript shot = shotTransform.gameObject.GetComponent<ProjectileScript>();

			// Shot exists
			if(shot != null) {
				shot.isEnemyShot = isEnemy;
			}

			MoveScript move = shotTransform.gameObject.GetComponent<MoveScript>();

			if(move != null) {
				move.direction = this.transform.right;	// Set direction to the 'front' of the sprite
			}

		}
	}

	// Is the weapon ready to create a new projectile?
	public bool CanAttack {
		get {
			return(shotCooldown <= 0f);
		}
	}
}
