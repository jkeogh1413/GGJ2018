using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaLerp : MonoBehaviour {

	public RawImage raw_image;
	public MicAnalyzer ma;
	public GameObject Laser;
	public GameObject Sat;
	public GameObject Radio;
	public GameObject Wifi;
	public GameObject Bluetooth;

	public bool canLerp = false;
	public bool _pitchReached = false;
	private float _riAlpha;

	void Start()
	{
		_riAlpha = raw_image.color.a;
	}

	void Update()
	{
		if (canLerp)
		{
			Color c = raw_image.color;
			_riAlpha = Mathf.PingPong(Time.time * Random.Range(0.1f, 2f), 1f);
			c.a = _riAlpha;

			raw_image.color = c;
		}
			
	}



	public void AlphaUp()
	{
		Color c;
		c = GetComponent<RawImage>().color;
		_riAlpha = 1f;
		c.a = _riAlpha;
		raw_image.color = c;
	}

}