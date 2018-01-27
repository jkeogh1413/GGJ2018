using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getTransmitterData : MonoBehaviour {

	public MicAnalyzer ma;
	public RangeDetector rd;
	public GameObject Laser;

	private bool _canLerp = false;
	public bool _pitchReached = false;
	private float _riAlpha;
	private string _transmitterName;

	// Use this for initialization
	void Start () {
		
	}
	
	void Update()
	{

		_transmitterName = rd.getTransmitterFromPitch(ma.curPitch).name;
		Debug.Log ("name " +_transmitterName);

		if (_transmitterName == "Laser") {
			Laser.GetComponent<AlphaLerp>().canLerp = true;
			Debug.Log("current pitch" + ma.curPitch);
		}



	}
}
