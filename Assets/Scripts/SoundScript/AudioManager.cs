using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void PlayMusic(string name)
    {
        Sound sounds = Array.Find(musicSounds, x  => x.name == name);

        if (sounds == null)
        {
            Debug.Log("Sound Not Found");
        }

        else
        {
            musicSource.clip = sounds.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name) 
    {
        Sound sounds = Array.Find(sfxSounds, x => x.name == name);

        if (sounds == null)
        {
            Debug.Log("Sound Not Found");
        }
        else 
        {
            sfxSource.PlayOneShot(sounds.clip);
        }
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }
    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }
    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

}
