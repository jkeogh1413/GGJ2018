using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeTracker : MonoBehaviour {
	GameObject[] trackedObjects;
	public float gazeDistance = 50f;
	public float gazeAngle = 45f;

	void Start () {
		refreshDroneList ();
	}

	void refreshDroneList() {
		trackedObjects = GameObject.FindGameObjectsWithTag ("Transmitter");
	}

	void FixedUpdate () {
		detectGaze ();
	}

	void detectGaze() {
		foreach (GameObject go in trackedObjects) {
			float distance = Vector3.Distance (transform.position, go.transform.position);
			if (distance < gazeDistance) {
				//Debug.Log (string.Format ("Object {0} is at gaze distance of {1} under threshold of {2}", go.name, distance, gazeDistance));

				float angle = Vector3.Angle (transform.forward, go.transform.position);
				if (angle <= gazeAngle) {
					//Debug.Log (string.Format ("Object {0} is at gaze angle of {1} under threshold of {2}", go.name, angle, gazeAngle));
					// Tell the thing I'm looking at that it is being looked at
				}
			}
		}
	}
				
}
