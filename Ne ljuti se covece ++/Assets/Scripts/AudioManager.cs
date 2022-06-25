using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip backgroundAudioClip;
    public AudioClip diceRollAudioClip;
    public AudioClip inHouseAudioClip;
    public AudioClip gameoverAudioClip;
    public AudioClip eatAudioClip;
    public AudioClip buttonPressedAudioClip;

    public static AudioSource backgroundAudioSource;
    public static AudioSource diceRollAudioSource;
    public static AudioSource inHouseAudioSource;
    public static AudioSource gameoverAudioSource;
    public static AudioSource eatAudioSource;
    public static AudioSource buttonPressedAudioSource;

    internal float allVolumeMultiplier = 1.0f;
    internal float backgroundVolume = 1.0f;
    internal float effectsVolume = 1.0f;

    private AudioSource AddAudio(AudioClip clip, bool playOnAwake, bool loop, float volume)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.playOnAwake = playOnAwake;
        audioSource.loop = loop;
        audioSource.volume = volume;
        return audioSource;
    }

    void Awake()
    {
        instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        allVolumeMultiplier = PlayerPrefs.GetFloat("allSoundsVolume", 1.0f);
        backgroundVolume = PlayerPrefs.GetFloat("backgroundVolume", 1.0f);
        effectsVolume = PlayerPrefs.GetFloat("effectsVolume", 1.0f);

        diceRollAudioSource = AddAudio(diceRollAudioClip, false, false, effectsVolume * allVolumeMultiplier);
        inHouseAudioSource = AddAudio(inHouseAudioClip, false, false, effectsVolume * allVolumeMultiplier);
        gameoverAudioSource = AddAudio(gameoverAudioClip, false, false, effectsVolume * allVolumeMultiplier);
        eatAudioSource = AddAudio(eatAudioClip, false, false, effectsVolume * allVolumeMultiplier);
        buttonPressedAudioSource = AddAudio(buttonPressedAudioClip, false, false, effectsVolume * allVolumeMultiplier);

        if (backgroundAudioSource == null || !backgroundAudioSource.isPlaying)
        {
            backgroundAudioSource = AddAudio(backgroundAudioClip, false, true, backgroundVolume * allVolumeMultiplier);
            backgroundAudioSource.Play();
        }
    }

    #region PlaySounds
    public void PlayDiceRollSound()
    {
        diceRollAudioSource.Play();
    }

    public void PlayInHouseSound()
    {
        inHouseAudioSource.Play();
    }

    public void PlayEatingSound()
    {
        eatAudioSource.Play();
    }

    public void PlayGameoverSound()
    {
        gameoverAudioSource.Play();
    }

    public void PlayButtonPressedSound()
    {
        buttonPressedAudioSource.Play();
    }

    #endregion

    #region SetVolume
    public void SetAllSoundsVolume(float multiplier)
    {
        allVolumeMultiplier = multiplier;
        //SetBackgroundVolume(backgroundAudioSource.volume);
        //SetEffectsVolume(diceRollAudioSource.volume);
    }

    public void SetBackgroundVolume(float volume)
    {
        backgroundVolume = volume;
        backgroundAudioSource.volume = volume * allVolumeMultiplier;
    }

    public void SetEffectsVolume(float volume)
    {
        effectsVolume = volume;
        diceRollAudioSource.volume = volume * allVolumeMultiplier;
        inHouseAudioSource.volume = volume * allVolumeMultiplier;
        gameoverAudioSource.volume = volume * allVolumeMultiplier;
        eatAudioSource.volume = volume * allVolumeMultiplier;
        buttonPressedAudioSource.volume = volume * allVolumeMultiplier;
    }
    #endregion
}
