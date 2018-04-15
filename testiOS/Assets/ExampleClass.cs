using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleClass : MonoBehaviour {
	public string function = "Enter Function";

	void OnGUI() {
		GUI.Label (new Rect (230, 150, 245, 40), "GraphAR");
		function = GUI.TextField (new Rect (225, 200, 250, 25), function, 40);
		if (GUI.Button (new Rect (300, 250, 100, 30), "Enter")) {
			Debug.Log (function);
		}
	}
}
