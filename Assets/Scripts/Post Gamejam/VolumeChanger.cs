using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeChanger : MonoBehaviour
{
    public AudioMixer mixerRef;
    public AudioClip pressSound;

    private AudioSource audioRef;
    private Animator animatorRef;
    private float volumePercent = 100;

    private void Start()
    {
        audioRef = GetComponent<AudioSource>();
        animatorRef = GetComponent<Animator>();
        if (PlayerPrefs.GetFloat("volume") != 0)
        {
            volumePercent = PlayerPrefs.GetFloat("volume");
            ChangeVolume();
        }
    }

    private void OnMouseDown()
    {

        if (volumePercent == 50)
        {
            volumePercent = 0.001f;
        }
        else if (volumePercent < 10)
        {
            volumePercent = 100;
        }
        else
        {
            volumePercent -= 50;
        }
        animatorRef.SetTrigger("Press");
        ChangeVolume();
        audioRef.PlayOneShot(pressSound);
    }

    private void ChangeVolume()
    {
        animatorRef.SetFloat("Volume", volumePercent);
        PlayerPrefs.SetFloat("volume", volumePercent);
        PlayerPrefs.Save();
        mixerRef.SetFloat("MasterVolume", Mathf.Log10(volumePercent / 100) * 20);
    }
}
