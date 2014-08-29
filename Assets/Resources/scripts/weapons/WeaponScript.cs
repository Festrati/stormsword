using UnityEngine;
using System.Collections;

/* WeaponScript - Basic weapon class (Ranged or Melee) */

public enum WeaponType {
	Melee,
	Ranged
}

public class WeaponScript : MonoBehaviour {

	// Projectile prefab for shooting
	public Transform shotPrefab;
	
	// Weapon stats
	[Tooltip("Cooldown between attacks")]
	public float shootingRate = 0.25f;

	[Tooltip("Damage a weapon does per attack")]
	public float damage = 1;

	[Tooltip("Radius the weapon affects upon impact")]
	public float radius = 5;

	[Tooltip("Amount to knock the character back upon impact")]
	public float knockback = 500f;

	[Tooltip("Maximum amount of time until the character can no longer link attacks")]
	private float comboCooldown = 0f;

	[Tooltip("Amount of time until character can no longer transition to second attacking state")]
	public float firstComboCooldown = 2.0f;

	[Tooltip("Amount of time until character can no longer transition to final attacking state")]
	public float secondComboCooldown = 1.5f;


	[Tooltip("Is this a Melee or Ranged weapon?")]
	public WeaponType weaponType = "Melee";



	public string ownerType = "Player";	// Is a player or an enemy carrying the weapon?

	// Components
	private MoveScript parentMoveScript;
	private MoveScript shotMoveScript;
	private ItemSlotScript mainHandSlot;

	private Vector2 aiPosition;
	private Vector2 playerPosition;
	private Vector2 newDirection;
	private float playerDistance;
	private int attackCounter;


	private GameObject player;

	// Use this for initialization
	void Start () {
		mainHandSlot = transform.parent.GetComponent<ItemSlotScript>();	// Grab the parent mainhand to get any slot-related info
		attackCounter = 0;
		player = GameObject.FindWithTag("Player");
	}

	void FixedUpdate () {
				if(comboCooldown > 0) {
					comboCooldown -= Time.deltaTime;
					Debug.Log ("fixedupdate Combo timer is : " + comboCooldown);
				}
				if(comboCooldown <= 0) {
					comboCooldown = 0;
				}
		}


	/* Attack - Shot triggered by another script */
	public void Attack() {
		if(CanAttack) {
			mainHandSlot.Cooldown(shootingRate);
			// Character attacked, trigger cooldown
 			switch(weaponType) {
			case WeaponType.Melee: 
				// Handle melee weapon code here

				// Only players have combos for now
				if(ownerType == "Player")
				{
					//use attackCounter to decide which state to enter
					if(attackCounter == 0) {
						LightAttack();
					}

					else if(attackCounter == 1) {
						MediumAttack();
					}

					else {
						Debug.Log ("whatd you do to attack counter?");
						attackCounter = 0;
					}
				}

				// Melee attack is attached to parent (character)

				break;

				case WeaponType.Ranged: 
					// Handle ranged weapon code here
					
					// Is the player still alive?
					if(player != null)
					{

						//Shoot directly at player location
						aiPosition = new Vector2(transform.position.x , transform.position.y);
						playerPosition = new Vector2(player.transform.position.x, player.transform.position.y);
						newDirection = (playerPosition - aiPosition);


						playerDistance = Vector3.Distance(player.transform.position, aiPosition);

						// Player is within shot distance
						if(playerDistance <=5f)
						{

							// Create a new shot
							var shotTransform = Instantiate(shotPrefab) as Transform;


							parentMoveScript = transform.parent.parent.GetComponent<MoveScript>();

							// Shot transform should be a child of the item slot (and thus the Character)
							shotTransform.transform.parent = transform;	

							if(parentMoveScript)
							{

								// Spawn projectile under character
								shotTransform.position = transform.position;

								// Figure out what direction character is facing
								// Vector3 newDirection = new Vector3(parentMoveScript.facing.x, parentMoveScript.facing.y, 0);

								//Shoot directly at player location
								aiPosition = new Vector2(transform.position.x , transform.position.y);
								playerPosition = new Vector2(player.transform.position.x, player.transform.position.y);
								newDirection = (playerPosition - aiPosition);

								// Make sure the bullet isn't going way too fast
								newDirection = newDirection.normalized;

								// Get the shot's move script to adjust its direction
								shotMoveScript = shotTransform.GetComponent<MoveScript>();
						
								if(shotMoveScript) {
									shotMoveScript.direction = newDirection;
								}
						
								// Fire the actual projectile
								ProjectileScript projectile = shotTransform.gameObject.GetComponent<ProjectileScript>();

								if(projectile) {
									// Determine what type of player shot this
									projectile.ownerType = gameObject.transform.parent.parent.tag;
								}
							}
						}
					}
				break;	

			}


		}
	}

	// Is the weapon ready to create a new projectile?
	public bool CanAttack {
		get {
			return(mainHandSlot.attackCooldown <= 0f);
		
		}
	}

	void OnTriggerEnter2D(Collider2D defenderCollider) {

		HealthScript defenderHealth = defenderCollider.GetComponent<HealthScript>();

		// Check if the object I'm colliding with can be damaged
		if(defenderHealth != null) {

			GameObject defender = defenderHealth.gameObject;

			if((ownerType == "Player" && defender.tag == "Enemy")
			   ||
			   (ownerType == "Enemy" && defender.tag == "Player")){

				// Attacks should knock the defender back away from attacker
				Knockback (transform, defender, knockback);

				// Calculate armor reduction
				float totalDamage = damage - defenderHealth.armor;

				// Attacks should always do 1 dmg, even if they are very weak
				if(totalDamage <= 0) {
					totalDamage = 1;
				}

				defenderHealth.Damage(totalDamage);		// Apply damage to the defender
			}
		}
	}

	/* Knockback - Knocks a unit (defender) away from an attacker's position (attackerTransform) by amount */
	public void Knockback(Transform attackerTransform, GameObject defender, float amount) {
		Vector2 direction = (defender.transform.position - attackerTransform.position).normalized;

		var defenderMoveScript = defender.GetComponent<MoveScript>();
		defenderMoveScript.Push(direction, amount);
	}

	/* 
	CombinationAttack - FSM
	Each state sets the countdown timer for transitioning
	attackCounter is used to decide which state is called
	*/ 
	void LightAttack()	{
		Debug.Log ("entering attack state 1");
		attackCounter++;
		// Set a time limit to link the next attack
		comboCooldown = firstComboCooldown;
	}

	
	// Second state - mid level attack, can transition to heavy attack or idle state
	void MediumAttack(){
		// Timer has not hit zero, able to transition to this state
		if(comboCooldown > 0){
			Debug.Log ("entering attack state 2");
			attackCounter++;
			// Set a time limit to link the next attack
			comboCooldown = secondComboCooldown;
		}

		// Timer hit zero, going back to idle state
		else{
		Debug.Log ("didn't attack in time... combo broken");
		attackCounter = 0;
		}
	
	}

	// Third state - final heavy attack, transitions back to idle state
	void HeavyAttack(){

		// Timer has not hit zero, able to transition to this state
		if (comboCooldown > 0) {
			Debug.Log ("entering final attack state");
		
		}

		else{
				Debug.Log ("didn't attack in time, final combo broken");
		}
		// Going back to idle state either way
		attackCounter = 0;
	}

}