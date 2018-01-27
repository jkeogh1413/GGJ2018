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

	public static int numSamples = 1024;
	private float fSample;

	private const float RefValue = 0.1f;
	private const float Threshold = 0.01f;

	float[] samples;
	float[] spectrum;

	AudioSource audioSource;

	void OnEnable () {

		samples = new float[numSamples];
		spectrum = new float[numSamples];

		fSample = AudioSettings.outputSampleRate;
		Debug.Log (fSample);
	}

	public void refreshSamples() {
		audioSource.GetOutputData (samples, 0);
		audioSource.GetSpectrumData (spectrum, 0, FFTWindow.BlackmanHarris);
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
