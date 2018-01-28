using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderBehavior : MonoBehaviour {

	public RectTransform handle;
	public RectTransform track;
	public float minValue = 20f;
	public float maxValue = 20000f;

	public float currentValue = 200f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(currentValue);
		handle.localPosition = new Vector3(
			track.localPosition.x + track.rect.width * Mathf.Clamp((currentValue - minValue)/(maxValue - minValue) - 0.5f, -0.5f, 0.5f),
			track.localPosition.y,
			track.localPosition.z
		);
	}
}
