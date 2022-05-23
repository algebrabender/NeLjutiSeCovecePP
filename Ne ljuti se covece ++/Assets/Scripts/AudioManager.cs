using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip backgroundAudioClip;
    public AudioClip diceRollAudioClip;
    public AudioClip inHouseAudioClip;
    public AudioClip winAudioClip;
    public AudioClip eatAudioClip;
    public AudioClip buttonPressedAudioClip;
    public AudioClip turnAudioClip;

    public static AudioSource backgroundAudioSource;
    public static AudioSource diceRollAudioSource;
    public static AudioSource inHouseAudioSource;
    public static AudioSource winAudioSource;
    public static AudioSource eatAudioSource;
    public static AudioSource buttonPressedAudioSource;
    public static AudioSource turnAudioSource;

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
        backgroundAudioSource = AddAudio(backgroundAudioClip, false, true, 1.0f);
        diceRollAudioSource = AddAudio(diceRollAudioClip, false, false, 1.0f);
        inHouseAudioSource = AddAudio(inHouseAudioClip, false, false, 1.0f);
        winAudioSource = AddAudio(winAudioClip, false, false, 1.0f);
        eatAudioSource = AddAudio(eatAudioClip, false, false, 1.0f);
        buttonPressedAudioSource = AddAudio(buttonPressedAudioClip, false, false, 1.0f);
        turnAudioSource = AddAudio(turnAudioClip, false, false, 1.0f);

        //backgroundAudioSource.Play();
    }

    #region PlaySounds

    internal void PlayDiceRollSound()
    {
        diceRollAudioSource.PlayDelayed(0.5f);
    }

    public void PlayButtonPressedSound()
    {
        buttonPressedAudioSource.Play();
    }

    #endregion
}
