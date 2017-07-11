using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public List<AudioClip> Sounds = new List<AudioClip>();
    public AudioSource soundSource;
    public List<AudioClip> Music = new List<AudioClip>();
    public AudioSource musicSource;

    private bool mute;
    public bool Mute
    {
        get
        {
            return mute;
        }
        set
        {
            mute = value;
            if (mute)
            {
                soundSource.volume = 0f;
                musicSource.volume = 0f;
            }
            else
            {
                soundSource.volume = 1f;
                musicSource.volume = 1f;
            }
        }
    }

    void Awake()
    {
        instance = this;
    }

    public void PlaySound(string soundName)
    {
        soundSource.PlayOneShot(Sounds.Find(x => x.name == soundName));
    }

    public void PlayMusic(string musicName)
    {
        musicSource.clip = Music.Find(x => x.name == musicName);
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

}
