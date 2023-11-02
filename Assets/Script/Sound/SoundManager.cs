using UnityEngine;
using UnityEngine.Audio;
using System;

// Altered from https://www.youtube.com/watch?v=6OT43pvUyfY
public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Sounds[] sounds;
    void Awake()
    {
        foreach (Sounds s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.enabled = s.enable;
            s.source.loop = s.loop;
        }
    }

    // Update is called once per frame
    public void Play(string soundName) {
        Sounds s = Array.Find(sounds, sound => sound.soundName == soundName);
        s.source.Play();
    }

    public void Enable(string soundName, bool enable) {
        Sounds s = Array.Find(sounds, sound => sound.soundName == soundName);
        s.source.enabled = enable;
    }
}
