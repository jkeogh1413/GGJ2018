using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicAnalyzer : MonoBehaviour {

	public float bpm = 0f;
	public float animationDelay;
	private float secondsDelay;

	public float RmsValue;
	public float DbValue;
	public float PitchValue;
	public float DbThresh = -2f;

	public static int numSamples = 1024;
	private float fSample;
	private int recordLength = 300;

	private const float RefValue = 0.1f;
	private const float Threshold = 0.01f;

	private string microphoneName = null;

	public float curPitch = 0f;
	public float curDb = 0f;

	float[] samples;
	float[] spectrum;

	AudioSource audioSource;

	// TEMP
	public SteamVR_TrackedObject trackedObj;
	private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
	private Valve.VR.EVRButtonId trigger = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
	private Valve.VR.EVRButtonId padDown = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;

	public int curPitchIndex = 0;
	private float[] hardPitches = {
		0f,
		100f,
		500f,
		1000f,
		3000f,
		8000f
	};
	void incrementPitchIndex() {
		curPitchIndex = (curPitchIndex + 1) % hardPitches.Length;
	}
	void decrementPitchIndex () {
		int tempIndex = curPitchIndex - 1;
		if (tempIndex < 0) {
			curPitchIndex = hardPitches.Length - 1;
		} else {
			curPitchIndex = tempIndex;
		}
	}
	void Update() {
		if (Input.GetMouseButtonDown (0) || (trackedObj && controller.GetPress(trigger))) {
			incrementPitchIndex ();
		} else if (Input.GetMouseButtonDown (1) || (trackedObj && controller.GetPressDown(padDown))) {
			decrementPitchIndex ();
		}
	}
			
	// END TEMP
		

	void Start ()
	{
		audioSource = GetComponent<AudioSource>();
		samples = new float[numSamples];
		spectrum = new float[numSamples];

		fSample = AudioSettings.outputSampleRate;
		Debug.Log (fSample);

		InvokeRepeating ("refreshMic", 0f, recordLength);
		InvokeRepeating ("getMicAnalysis", 1f, 0.2f);
	}

	void OnDisable() {
		audioSource.Stop ();
		Microphone.End (microphoneName);
	}

	void OnApplicationExit() {
		Microphone.End (microphoneName);
	}
		
	public void refreshMic() {
		Debug.Log ("Refreshing clip with Microphone data");
		audioSource.clip = Microphone.Start(microphoneName, true, recordLength, AudioSettings.outputSampleRate);
		audioSource.Play();
	}

	public void getMicAnalysis() {
		//Debug.Log (string.Format ("{0} {1} {2}", Microphone.IsRecording (microphoneName).ToString (), Microphone.devices [0], Microphone.devices [Microphone.devices.Length - 1]));

		audioSource.GetOutputData (samples, 0);
		audioSource.GetSpectrumData (spectrum, 0, FFTWindow.BlackmanHarris);

		// TEMP
		curPitch = hardPitches[curPitchIndex];
		curDb = 1f;
		//curDb = getDbValue();
		//curPitch = getPitch (0f, 10000f);

		if (DbThresh <= curDb)
		{
			Debug.Log(string.Format("Tracking significant db of {0} and average pitch of {1}", curDb.ToString(), curPitch.ToString()));

		}
	}

	public float getDbValue() {
		// get samples from audio

		// Sum samples ^2
		float sum = 0;
		for (int i = 0; i < numSamples; i++)
		{
			sum += samples [i] * samples [i];
		}

		// Root-mean-square value is the "effective" value of the waveform
		RmsValue = Mathf.Sqrt(sum / numSamples);

		// decibel value clamped to -160dB min
		DbValue = 20 * Mathf.Log10(RmsValue / RefValue);
		if (DbValue < -160) {
			DbValue = -160;
		}

		return DbValue;
	}

	public Dictionary<string, int> getNormalizedRangeIndex(float frequencyRangeStart, float frequencyRangeEnd) {
		Dictionary<string, int> returnData = new Dictionary <string, int> ();
		float frequencyRangeMax = fSample / 2f;

		frequencyRangeStart = Mathf.Clamp (frequencyRangeStart, 20f, frequencyRangeEnd);
		frequencyRangeEnd = Mathf.Clamp (frequencyRangeEnd, frequencyRangeStart, frequencyRangeEnd);

		returnData["normalizedRangeStart"] = Mathf.FloorToInt (frequencyRangeStart * numSamples / frequencyRangeMax);
		returnData["normalizedRangeEnd"] = Mathf.FloorToInt (frequencyRangeEnd * numSamples / frequencyRangeMax);

		return returnData;
	}

	public float getFrequencyFromIndex(int index) {
		return index * ((fSample / 2) / numSamples);
	}

	public float getBandAverage(float frequencyRangeStart, float frequencyRangeEnd) {
		/* Frequency Ranges:
		 * 	Sub-bass: 20Hz - 60Hz
		 *  Bass: 60Hz - 250Hz
		 *  Low Mid: 250Hz - 500Hz
		 *  Mid: 500Hz - 2000Hz
		 *  Upper Mid: 2000Hz - 4000Hz
		 *  Presence: 4000Hz - 6000Hz
		 *  Brilliance: 6000Hz - 20000Hz
		*/
		Dictionary<string, int> normalizedRange = getNormalizedRangeIndex (frequencyRangeStart, frequencyRangeEnd);

		float sum = 0f;
		int includedIndexes = 0;
		for (int i = normalizedRange["normalizedRangeStart"]; i < normalizedRange["normalizedRangeEnd"]; i++) {
			// Should I square?
			//sum += Mathf.Pow(spectrum [i], 2f);
			float currentFrequency = getFrequencyFromIndex(i);

			float lowFilterValue = 24000f;
			float highFilterValue = 0f;

			if (currentFrequency < lowFilterValue && currentFrequency > highFilterValue) {
				sum += spectrum [i];
				includedIndexes++;
			}
		}
		if (includedIndexes > 0)
			return sum / includedIndexes;

		return 0f;
	}

	public float getPitch(float frequencyRangeStart, float frequencyRangeEnd) {
		// get sound spectrum
		Dictionary<string, int> normalizedRange = getNormalizedRangeIndex (frequencyRangeStart, frequencyRangeEnd);

		// max value
		float maxV = 0;
		// index of max value
		var maxN = 0;
		for (int i = normalizedRange["normalizedRangeStart"]; i < normalizedRange["normalizedRangeEnd"]; i++) {
			if (spectrum [i] > maxV && spectrum [i] > Threshold) {
				maxV = spectrum [i];
				maxN = i;
			}
		}

		// Check neighbors to increase accuracy of peak value
		float freqN = maxN;
		if (maxN > 0 && maxN < normalizedRange["normalizedRangeEnd"] - 1)
		{
			var dL = spectrum[maxN - 1] / spectrum[maxN];
			var dR = spectrum[maxN + 1] / spectrum[maxN];
			freqN += 0.5f * (dR * dR - dL * dL);
		}
		PitchValue = freqN * (fSample / 2) / numSamples;

		return PitchValue;
	}
}
