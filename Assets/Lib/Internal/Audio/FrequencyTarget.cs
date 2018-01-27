using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct TransmitterInfo
{
	public string name;
	public float startFreq;
	public float endFreq;
}
// example of target object
// FrequencyTarget targetA = new FrequencyTarget();
// targetA.startFreq = 430;
// targetA.endFreq = 450;
public struct ExampleAudioInfo
{
	public string name;
	public float targetFrequency;
}