using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaLerp : MonoBehaviour {

	public MicAnalyzer ma;

	public bool canLerp = false;
	public bool _pitchReached = false;
	private float _riAlpha;
    private RawImage _raw_image;

    void Start()
	{
        _raw_image = GetComponent<RawImage>();
        _riAlpha = _raw_image.color.a;
	}

	void Update()
	{
		if (canLerp)
		{
			Color c = _raw_image.color;
			_riAlpha = Mathf.PingPong(Time.time * Random.Range(0.1f, 2f), 1f);
			c.a = _riAlpha;

			_raw_image.color = c;
		}
			
	}


	public void AlphaUp()
	{
		Color c;
		c = GetComponent<RawImage>().color;
		_riAlpha = 1f;
		c.a = _riAlpha;
		_raw_image.color = c;
	}

}