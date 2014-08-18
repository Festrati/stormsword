using UnityEngine;
using System.Collections;

/* AbilityUIScript - places ability UI elements on the screen */
public class AbilityUIScript : MonoBehaviour {

	[Tooltip("Opacity of the ability icon when disabled")]
	public float disabledAlpha = 0.6f;

	[Tooltip("Opacity of the ability icon when available to be cast")]
	public float enabledAlpha = 1f;
	private float currentAlpha = 1f;		// Current Alpha (opacity) value of the image

	// UI Position
	[Tooltip("X coordinate to display the image at")]
	public int x;
	[Tooltip("Y coordinate to display the image at")]
	public int y;
	[Tooltip("Width of the image")]
	public int width;
	[Tooltip("Height of the image"]
	public int height;
	private int positioning_x;
	private int positioning_y;

	internal int abilityIndex = 0;			// What # in the array is this ability?

	// Ability Hotkey
	private GameObject player;				// Player manning the keyboard
	private PlayerScript playerScript;		// Script with player input code to grab key bindings
	private string abilityKey;						// Key used to trigger abilities

	// Ability Icon
	private AbilitySlotScript abilitySlotScript;	// Current ability slot
	private AbilityScript abilityScript;		// Currently equipped ability (to get the icon)
	private Transform ability;
	private Texture2D abilityIcon;	// The image to display for this ability


	// Use this for initialization
	void Start () {
		// Get ability hotkey
		GameObject player = GameObject.FindWithTag("Player");
		playerScript = player.GetComponent<PlayerScript>();
		abilityKey = playerScript.abilityKeys[abilityIndex].ToString();

		// Get ability icon
		abilitySlotScript = GetComponent<AbilitySlotScript>();
		ability = abilitySlotScript.abilityEquipped;		// Currently equipped ability
		abilityScript = ability.gameObject.GetComponent<AbilityScript>();
		abilityIcon = abilityScript.abilityIcon;

		// If there are are multiple abilities equipped, show them next to each other, not on top of each other
		positioning_x = abilityIndex * 40;	
		positioning_y = 0;
	}
	
	void OnGUI() {
		// Draw the ability's image and apply opacity to fade if necessary
		Color tmpColor = GUI.color;		// Placeholder for current color setting
		GUI.color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, currentAlpha);
		GUI.Button(new Rect(x + positioning_x, y + positioning_y, width, height), abilityIcon);	// Draw the actual image
		GUI.Label (new Rect(x-1 + positioning_x, y+15 + positioning_y, width, height), abilityKey);	// Add the key binding
		GUI.color = tmpColor;			// Restore color setting for other GUI elements drawn in the same frame
	}

	/* FadeOut - Called to fade the current UI element to partial opacity
	 	e.g. when it's not available */
	internal void FadeOut() {
		currentAlpha = disabledAlpha;
	}

	/* FadeIn - Called to fade the current UI element to full opacity */
	internal void FadeIn() {
		currentAlpha = enabledAlpha;	// 1f = 'fully visible'
	}
}
