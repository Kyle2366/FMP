using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;
using System;


public class AudioManager : MonoBehaviour
{
    public AudioMixer masterMixer;

    public Slider musicSlide;

    public Slider sfxSlide;

    public Sound[] sounds;
    
    private void Start()
    {
        Load();

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.audioMixerGroup;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }

        Play("BGM");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);

        if( s == null)
        {
            print("Sound " + name + " not found");
            return;
        }
        s.source.Play();
    }
  
   
    public void Load()
    {
        SetMusicVolume(PlayerPrefs.GetFloat("music"));
        SetSFXVolume(PlayerPrefs.GetFloat("sfx"));
    }

    public void SetMusicVolume(float vol)
    {
        masterMixer.SetFloat("music", PlayerPrefs.GetFloat("music"));
        musicSlide.value = vol;
        PlayerPrefs.SetFloat("music",vol);
    }

    public void SetSFXVolume(float vol)
    {
        masterMixer.SetFloat("sfx", PlayerPrefs.GetFloat("sfx"));
        sfxSlide.value = vol;
        PlayerPrefs.SetFloat("sfx", vol);
    }
}
