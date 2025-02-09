using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    
    public static AudioManager instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        { 
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.mute = s.mute;
            s.source.playOnAwake = s.playOnAwake;
            s.source.priority = s.priority;
        }
    }

    private void Start()
    {
        Play("Theme");
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            // Debug.LogWarning("Sound: " + name + " not found!");

            return;
        }

        s.source.Play();
    }
    
    public void ToggleMasterAudio()
    {
        foreach (Sound s in sounds)
        {
            s.source.mute = !s.source.mute;
        }
    }

    public void ToggleMusic()
    {
        sounds[0].source.mute = !sounds[0].source.mute;
    }

    public void ToggleSFX()
    {
        sounds[1].source.mute = !sounds[1].source.mute;
    }

    public void ToggleMusicVolume(float volume)
    {
        sounds[0].source.volume = volume;
    }

    public void ToggleSFXVolume(float volume)
    {
        sounds[1].source.volume = volume;
    }

}
