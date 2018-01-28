using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Locomotion : MonoBehaviour {

	GameObject path;
	List<Vector3> PosArray;
	Vector3 targetPos;

	public int currentIndex = 0;
	public string SplineObjectName;
	public float speed = 10f;
	bool loop;
	public bool moveForward = false;

	private float rotationSpeed = 0.5f;

	// Use this for initialization
	void Start () {
		path = GameObject.Find(SplineObjectName);
		PosArray = path.GetComponent<SplinePathing> ().PosArray;
		loop = path.GetComponent<SplinePathing> ().isLooping;
		transform.SetPositionAndRotation (PosArray [0], transform.rotation);
		transform.LookAt (PosArray [1]);
	}

	// Update is called once per frame
	void Update () {
		float step = speed * Time.deltaTime;
		float rotationStep = rotationSpeed * Time.deltaTime;
		targetPos = PosArray[currentIndex];

		// Vector3.MoveTowards must be called every frame, else it only moves a little bit
		transform.position = Vector3.MoveTowards (transform.position, targetPos, step);
		//Vector3 newDir = Vector3.RotateTowards(transform.forward, targetPos - transform.position, rotationStep, 0f);
		//transform.rotation = Quaternion.LookRotation(newDir);

		transform.LookAt (targetPos);

		// Unless move forward is active the locomotive will not iterate through the positions and thus not move
		if (moveForward && transform.position == targetPos && PosArray.Count - 1 > currentIndex) {
			currentIndex += 1;
			if (loop && PosArray.Count -1 == currentIndex) {
				currentIndex = 0;
			}
		}
	}

	public void Reverse (int indexes) {
		currentIndex = currentIndex - indexes;
	}

	public void Move () {
		moveForward = true;
	}

	public void Stop () {
		moveForward = false;
	}
}