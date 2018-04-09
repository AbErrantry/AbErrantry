using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundOptions : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("masterVol", volume);
        audioMixer.SetFloat("musicVol", volume);
        audioMixer.SetFloat("sfxVol", volume);
        audioMixer.SetFloat("dialogueVol", volume);
    }
}
