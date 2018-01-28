using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulldozerBehaviour : MonoBehaviour {
	Vector3 lastPosition;
	Vector3 deltaPosition;
	bool dozerColliding = false;

	void OnTriggerEnter (Collider col) {
		dozerColliding = true;
	}

	void OnTriggerExit (Collider col) {
		dozerColliding = false;
	}

	void OnTriggerStay (Collider col) {
		if (col.gameObject.tag == "Moveable") {
			deltaPosition = transform.position - lastPosition;
//			col.transform.SetPositionAndRotation (col.transform.position + deltaPosition, col.transform.rotation);
			col.GetComponent<Rigidbody> ().AddForce (deltaPosition * 2500);
			lastPosition = transform.position;
		}
	}

	// Use this for initialization
	void Start () {
		lastPosition = transform.position;
	}

	// Update is called once per frame
	void Update () {
		if (!dozerColliding) {
			lastPosition = transform.position;
		}
	}
}
