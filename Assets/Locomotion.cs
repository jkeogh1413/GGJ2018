﻿using System.Collections;
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

	private float rotationSpeed = 0.5f;

	// Use this for initialization
	void Start () {
		path = GameObject.Find(SplineObjectName);
		PosArray = path.GetComponent<SplinePathing> ().PosArray;

		transform.SetPositionAndRotation (PosArray [0], transform.rotation);
//		currentIndex += 1;
	}

	// Update is called once per frame
	void Update () {
		float step = speed * Time.deltaTime;
		float rotationStep = rotationSpeed * Time.deltaTime;
		targetPos = PosArray[currentIndex];

		transform.position = Vector3.MoveTowards (transform.position, targetPos, step);

		Vector3 newDir = Vector3.RotateTowards(transform.forward, targetPos - transform.position, rotationStep, 0f);
		Debug.DrawRay(transform.position, newDir, Color.red);
		transform.rotation = Quaternion.LookRotation(newDir);

		if (transform.position == targetPos && PosArray.Count - 1 > currentIndex) {
			currentIndex += 1;
		}
	}
}