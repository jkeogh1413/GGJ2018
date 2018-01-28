using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulldozerBehaviour : MonoBehaviour {
	Vector3 lastPosition;
	Vector3 deltaPosition;

	void OnTriggerStay (Collider col) {
		if (col.gameObject.tag == "Moveable") {
			Debug.Log ("last position" + lastPosition);
			Debug.Log ("transformed position" + transform.position);
			deltaPosition = transform.position - lastPosition;
			Debug.Log ("delta" + deltaPosition);
			col.transform.SetPositionAndRotation (col.transform.position + deltaPosition, col.transform.rotation);
		}
	}

	// Use this for initialization
	void Start () {
		lastPosition = transform.position;
	}

	// Update is called once per frame
	void Update () {
		lastPosition = transform.position;
	}
}
