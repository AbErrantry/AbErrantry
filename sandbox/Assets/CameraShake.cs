using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraShake : MonoBehaviour {

	public bool isShaking;
	public CinemachineVirtualCamera cm;
	public CinemachineBasicMultiChannelPerlin noise;
	public void Start()
	{
		//cm = GetComponent<CinemachineVirtualCamera>();
		noise = cm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		isShaking = false;
	}
	public IEnumerator Shaker(float shakeIntensity, float shakeTiming)
	{
		Noise(1, shakeIntensity);
		yield return new WaitForSeconds(shakeTiming);
		Noise(0, 0);
		isShaking = false;   
	}

	public void Noise(float amplitudeGain, float frequencyGain)
	{
		noise.m_AmplitudeGain = amplitudeGain;
		noise.m_FrequencyGain = frequencyGain;    
	}
}
