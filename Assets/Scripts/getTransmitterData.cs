using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getTransmitterData : MonoBehaviour {

	public MicAnalyzer ma;
	public RangeDetector rd;
	public GameObject Laser;
	public GameObject Sat;
	public GameObject Radio;
	public GameObject Wifi;
	public GameObject Bluetooth;

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
		if(_transmitterName != "None") {
			Debug.Log ("name " +_transmitterName);
		}


		if (_transmitterName == "Laser") {
			Laser.GetComponent<AlphaLerp>().canLerp = true;
			Debug.Log("current pitch" + ma.curPitch);
		}
		else if (_transmitterName == "WiFi") {
			Sat.GetComponent<AlphaLerp>().canLerp = true;
			Debug.Log("current pitch" + ma.curPitch);
		}
		else if (_transmitterName == "Radio") {
			Radio.GetComponent<AlphaLerp>().canLerp = true;
			Debug.Log("current pitch" + ma.curPitch);
		}
		else if (_transmitterName == "Satellite") {
			Wifi.GetComponent<AlphaLerp>().canLerp = true;
			Debug.Log("current pitch" + ma.curPitch);
		}
		else if (_transmitterName == "Bluetooth") {
			Bluetooth.GetComponent<AlphaLerp>().canLerp = true;
			Debug.Log("current pitch" + ma.curPitch);
		}



	}
}
