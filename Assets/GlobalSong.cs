using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSong : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<AudioSource> ().PlayDelayed (15f);	
	}
}
