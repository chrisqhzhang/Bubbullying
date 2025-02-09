using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    
    public AudioClip clip;
    
    public bool mute;
    
    public bool playOnAwake;
    
    public bool loop;
    
    [Range(0, 256)]
    public int priority;
    
    [Range(0f, 1f)]
    public float volume;
    
    [Range(0.1f, 3.0f)]
    public float pitch;
    
    [HideInInspector]
    public AudioSource source;
}
