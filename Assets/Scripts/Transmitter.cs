using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transmitter : MonoBehaviour
{
	public Vector3 transPosition;
	public string transType = "Default";
	public bool withinView = false;
	public bool transmitting = false;

	public RangeDetector rangeDetector;
	public MicAnalyzer micAnalyzer;

	void Start()
	{
		transPosition = transform.position;
		rangeDetector = GameObject.Find ("RangeDetector").GetComponent<RangeDetector> ();
		micAnalyzer = GameObject.Find ("MicController").GetComponent<MicAnalyzer> ();
	}

	void FixedUpdate () {
		if (withinView) {
			// Spin antenna
			foreach (Transform child in transform) {
				if (child.name.Contains ("transmitter.antenna")) {
					child.Rotate (Vector3.up * Time.fixedDeltaTime * 200f, Space.World);
				}
			}

			// Play the note
			if (!transform.Find ("EmitSound").GetComponent<AudioSource> ().isPlaying) {
				transform.Find ("EmitSound").GetComponent<AudioSource> ().Play ();
			}

			// Look for matching pitch
			if (micAnalyzer.curDb < micAnalyzer.DbThresh) {
				transmitting = false;
				transform.parent.GetComponent<Locomotion> ().Stop ();
				return;
			}

			float pitch = micAnalyzer.curPitch;
			TransmitterInfo matchingTransmitter = rangeDetector.getTransmitterFromPitch (pitch);
			if (matchingTransmitter.name == transType) {
				//Debug.Log (string.Format ("Transmitter {0} - Within View: {1} ; Transmitting: {2}", transType, withinView.ToString (), transmitting.ToString ()));
				transmitting = true;

				// JASPER:  Here is where we do the thing
				transform.parent.GetComponent<Locomotion> ().Move ();
			} else {
				transmitting = false;
				transform.parent.GetComponent<Locomotion> ().Stop ();
			}
		} else {
			transmitting = false;
			transform.parent.GetComponent<Locomotion> ().Stop ();
		}
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