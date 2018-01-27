﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// laser, wifi, radio, satellite, bluetooth

public class RangeDetector : MonoBehaviour {

	public float hertzBuffer = 20f;
	public List<TransmitterInfo> transmitters;
	public TransmitterInfo untrackedTransmitter;

	void Start() {
		transmitters = new List<TransmitterInfo> ();
		transmitters.Add (
			new TransmitterInfo () {
				name = "Laser",
				startFreq = 20f,
				endFreq = 300f
			}
		);
		transmitters.Add (
			new TransmitterInfo () {
				name = "WiFi",
				startFreq = 300f,
				endFreq = 700f
			}
		);
		transmitters.Add (
			new TransmitterInfo () {
				name = "Radio",
				startFreq = 700f,
				endFreq = 2000f
			}
		);
		transmitters.Add (
			new TransmitterInfo () {
				name = "Satellite",
				startFreq = 2000f,
				endFreq = 7000f
			}
		);
		transmitters.Add (
			new TransmitterInfo () {
				name = "Bluetooth",
				startFreq = 7000f,
				endFreq = 10000f
			}
		);

		untrackedTransmitter = new TransmitterInfo () {
			name = "None",
			startFreq = 0f,
			endFreq = 0f
		};
	}


	public TransmitterInfo getTransmitterFromPitch(float pitch) {
		foreach (TransmitterInfo transmitter in transmitters) {
			if (pitch >= transmitter.startFreq && pitch <= transmitter.endFreq) {
				return transmitter;
			}
		}
		return untrackedTransmitter;
	}

	public bool inTargetRange(ExampleAudioInfo exampleInfo, float pitch) {
		if (pitch >= (exampleInfo.targetFrequency - hertzBuffer) && pitch <= (exampleInfo.targetFrequency + hertzBuffer)) {
			return true;
		}
		return false;
	}
}