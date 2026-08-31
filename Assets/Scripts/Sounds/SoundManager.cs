using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }

    public AudioSource soundEffect;
    public AudioSource soundMusic;

    public SoundType[] Sounds;

    public bool IsMute = false;
    public float MasterVolume = 1f;
    public float MusicVolume = 1f;
    public float EffectVolume = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        IsMute = PlayerPrefs.GetInt("IsMute", 0) == 1;
        MasterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        EffectVolume = PlayerPrefs.GetFloat("EffectVolume", 1f);

        SetMasterVolume(MasterVolume);
        SetMusicVolume(MusicVolume);
        SetEffectVolume(EffectVolume);
        Mute(IsMute);

        PlayMusic(global::Sounds.Music);
    }

    public void SetMasterVolume(float volume)
    {
        MasterVolume = volume;
        PlayerPrefs.SetFloat("MasterVolume", MasterVolume);
        SetMusicVolume(MusicVolume);
        SetEffectVolume(EffectVolume);
        PlayerPrefs.Save();
    }

    public void SetEffectVolume(float volume)
    {
        EffectVolume = volume;
        PlayerPrefs.SetFloat("EffectVolume", EffectVolume);
        soundEffect.volume = IsMute ? 0.0f : EffectVolume * MasterVolume;
        PlayerPrefs.Save();
    }

    public void Mute(bool status)
    {
        IsMute = status;

        if (IsMute)
        {
            soundMusic.volume = 0f;
            soundEffect.volume = 0f;
        }
        else
        {
            soundMusic.volume = MusicVolume * MasterVolume;
            soundEffect.volume = EffectVolume * MasterVolume;

            if(!soundMusic.isPlaying)
            {
                RestartMusic();
            }
        }

        PlayerPrefs.SetInt("IsMute", IsMute ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float volume)
    {
        MusicVolume = volume;
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
        soundMusic.volume = IsMute ? 0.0f : MusicVolume * MasterVolume;
        PlayerPrefs.Save();
    }

    public void PlayMusic(Sounds sound)
    {
        if (IsMute)
            return;

        AudioClip clip = getSoundClip(sound);
        if (clip != null)
        {
            soundMusic.clip = clip;
            soundMusic.loop = true;
            soundMusic.Play();
        }
        else
        {
            Debug.LogError("Clip not found for sound type: " + sound);
        }
    }

    public void Play(Sounds sound)
    {
        if (IsMute)
            return;

        AudioClip clip = getSoundClip(sound);
        if (clip != null)
        {
            soundEffect.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("Clip not found for sound type: " + sound);
        }
    }


    private AudioClip getSoundClip(Sounds sound)
    {
        SoundType item = Array.Find(Sounds, i => i.soundType == sound);
        if (item != null)
            return item.soundClip;
        return null;
    }

    public void StopAllSounds()
    {
        soundEffect.Stop();
        soundMusic.Stop();
        soundMusic.clip = null;
    }

    public void RestartMusic()
    {
        PlayMusic(global::Sounds.Music);
    }
}

[Serializable]
public class SoundType
{
    public Sounds soundType;
    public AudioClip soundClip;
}

public enum Sounds
{
    ButtonClick,
    Music,
    PlayerMove,
    PlayerDeath,
    PlayerDamage,
    EnemyDeath,
    KeyPickUp,
    LevelUp
}