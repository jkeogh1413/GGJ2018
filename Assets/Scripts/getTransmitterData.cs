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


	public bool _pitchReached = false;
	private float _riAlpha;
	private string _transmitterName;
    private List<GameObject> _list_o_jects;
	// Use this for initialization
	void Start ()
    {
        _list_o_jects = new List<GameObject>();
        _list_o_jects.Add(Laser);
        _list_o_jects.Add(Sat);
        _list_o_jects.Add(Radio);
        _list_o_jects.Add(Wifi);
        _list_o_jects.Add(Bluetooth);
    }

    void Update()
	{

		_transmitterName = rd.getTransmitterFromPitch(ma.curPitch).name;
		if(_transmitterName != "None") {
			Debug.Log ("name " +_transmitterName);
		}

        foreach(GameObject go in _list_o_jects)
        {
            go.GetComponent<AlphaLerp>().canLerp = false;
        }

        CheckTransmitterName(_transmitterName);
	}

    void CheckTransmitterName(string tn)
    {
        if (tn == "Laser")
        {
            Laser.GetComponent<AlphaLerp>().canLerp = true;
            Debug.Log("current pitch" + ma.curPitch);
        }
        else if (tn == "WiFi")
        {
            Sat.GetComponent<AlphaLerp>().canLerp = true;
            Debug.Log("current pitch" + ma.curPitch);
        }
        else if (tn == "Radio")
        {
            Radio.GetComponent<AlphaLerp>().canLerp = true;
            Debug.Log("current pitch" + ma.curPitch);
        }
        else if (tn == "Satellite")
        {
            Wifi.GetComponent<AlphaLerp>().canLerp = true;
            Debug.Log("current pitch" + ma.curPitch);
        }
        else if (tn == "Bluetooth")
        {
            Bluetooth.GetComponent<AlphaLerp>().canLerp = true;
            Debug.Log("current pitch" + ma.curPitch);
        }
    }
}
