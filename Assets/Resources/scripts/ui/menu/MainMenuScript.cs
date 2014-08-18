using UnityEngine;
using System.Collections;

/* MainMenuScript - Controller for main menu functionality
 	Allows player to start the game (or continue.... one day!) */
public class MainMenuScript : MonoBehaviour {

	public static MainMenuScript instance;

	[Tooltip("Drag the first level (scene) here")]
	public Object firstScene;

	void Awake() {
		instance = this;	// Allows us to access MainMenuScript from anywhere 
							// (which is ok because there will only ever be one of them active at a time)
	}
	// Update is called once per frame
	void Update () {
		
	}

	public void StartGame() {
		Debug.Log ("Loading scene: " + firstScene.name);
		Application.LoadLevel(firstScene.name);
	}

	public void ContinueGame() {
	}
}
