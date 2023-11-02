using UnityEngine.Audio;
using UnityEngine;


// Altered From https://www.youtube.com/watch?v=6OT43pvUyfY
[System.Serializable]
public class Sounds
{
    public string soundName;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume;
    [Range(0f, 1f)] public float pitch;
    [HideInInspector] public AudioSource source;
    public bool enable;
    public bool loop;
}
