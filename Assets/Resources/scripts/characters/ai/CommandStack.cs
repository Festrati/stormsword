using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* CommandStack - A Stack of commands that allow an enemy to figure out what he/she/it should be doing at any given time */
public class CommandStack : MonoBehaviour {

	private Stack<Command> commands;

	// Use this for initialization
	void Start () {
		commands = new Stack<Command>();

		private Command command = new Command();

		commands.Push(command);
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (commands.ToString);
	}
}
