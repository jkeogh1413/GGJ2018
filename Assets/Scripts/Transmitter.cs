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
			if (micAnalyzer.curDb < micAnalyzer.DbThresh)
				return;

			float pitch = micAnalyzer.curPitch;
			TransmitterInfo matchingTransmitter = rangeDetector.getTransmitterFromPitch (pitch);
			if (matchingTransmitter.name == transType) {
				Debug.Log (string.Format ("Transmitter {0} - Within View: {1} ; Transmitting: {2}", transType, withinView.ToString (), transmitting.ToString ()));
				transmitting = true;

				// JASPER:  Here is where we do the thing
				//transform.parent.GetComponent<Locomotion> ().move ();
			} else {
				transmitting = false;
			}
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

			// TANNER:  Play sample noise?
		}
	}

	void OnTriggerExit(Collider col) {
		if (col.gameObject.tag == "GazeDetector") {
			withinView = false;
		}
	}
}