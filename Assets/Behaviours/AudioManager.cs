using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    LoadoutFactory loadout_factory;
    AudioSource audio_source;

	void Start()
    {
		loadout_factory = GameObject.FindObjectOfType<LoadoutFactory>();
        audio_source = GetComponent<AudioSource>();
	}
	
	public void PlayOneShot(string clip_name)
    {
        audio_source.PlayOneShot(loadout_factory.GetAudioClip(clip_name));
    }

    public void PlayOneShot(AudioClip clip)
    {
        audio_source.PlayOneShot(clip);
    }

}
