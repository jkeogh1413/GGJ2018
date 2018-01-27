using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transmitter : MonoBehaviour
{
	public Vector3 transPosition;
	public string transType;
	public bool transmissionReceiving;

	void Start()
	{
		transPosition = transform.position;
	}

	public Vector3 getTransPosition()
	{
		return transPosition;
	}
}