using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaLerp : MonoBehaviour {

    public RawImage raw_image;

    private bool _canLerp;
    public bool _pitchReached = false;
    private float _riAlpha;

    void Start()
    {
        _riAlpha = raw_image.color.a;
        _canLerp = true;
    }

    void Update()
    {
        if (_canLerp)
        {
            Color c = raw_image.color;

            _riAlpha = Mathf.PingPong(Time.time * Random.Range(0.1f, 2f), 1f);
            c.a = _riAlpha;

            raw_image.color = c;

            if (_pitchReached)
            {
                AlphaUp();
            }
        }
    }




    public void AlphaUp()
    {
        Color c = raw_image.color;

        _riAlpha = 1f;
        c.a = _riAlpha;

        raw_image.color = c;
    }
    
}
