using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transmitter : MonoBehaviour
{
	public Vector3 transPosition;
	public string transType = "Default";
	public bool withinView = false;
	public bool transmitting = false;

	void Start()
	{
		transPosition = transform.position;
	}

	void FixedUpdate () {
		Debug.Log (string.Format ("Transmitter {0} - Within View: {1} ; Transmitting: {2}", transType, withinView.ToString (), transmitting.ToString ()));
	}

	public Vector3 getTransPosition()
	{
		return transPosition;
	}

	public void resetState() {
		withinView = false;
		transmitting = false;
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "GazeDetector") {
			withinView = true;
		}
	}

	void OnTriggerExit(Collider col) {
		if (col.gameObject.tag == "GazeDetector") {
			withinView = false;
		}
	}
}