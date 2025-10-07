using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//Re-used from game jam. Luc Rancourt's scripts.

public class AudioManager : Singleton<AudioManager>
{
    // Variables
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup musicMixer;
    [SerializeField] private AudioMixerGroup sfxMixer;

    [SerializeField] private SFX defaultMusic;
    private AudioSource _musicSource;

    private List<AudioSource> _soundEffectSources = new List<AudioSource>();

    // Functions
    private void Start()
    {
        LoadVolume();
        if(defaultMusic != null)
            PlayMusic(defaultMusic);
    }

    #region Volume
    private void LoadVolume()          
    {
        SetMixerVolume(AudioMixerKeys.MasterVolumeKey, PlayerPrefs.GetFloat(AudioMixerKeys.MasterVolumeKey));   //Player prefs to be changed in a settings menu
        SetMixerVolume(AudioMixerKeys.MusicVolumeKey, PlayerPrefs.GetFloat(AudioMixerKeys.MusicVolumeKey));
        SetMixerVolume(AudioMixerKeys.SFXVolumeKey, PlayerPrefs.GetFloat(AudioMixerKeys.SFXVolumeKey));
    }

    public void SetMixerVolume(string key, float volume)
    {
        audioMixer.SetFloat(key, Mathf.Log10(volume) * 20);
    }
    #endregion

    #region Plays
    private void PlayMusic(SFX music)
    {
        _musicSource = gameObject.AddComponent<AudioSource>();
        _musicSource.outputAudioMixerGroup = musicMixer;

        SetupSource(ref _musicSource, music);

        _musicSource.loop = true;
        _musicSource.Play();
    }

    public void PlaySound(SFX sound)
    {
        AudioSource soundSource = GetAvailableSFXSource();

        SetupSource(ref soundSource, sound);
        soundSource.loop = false;
        soundSource.Play();
    }

    public AudioSource PlayLoopingSound(SFX sound)
    {
        AudioSource soundSource = GetAvailableSFXSource();
        SetupSource(ref soundSource, sound);
        soundSource.loop = true;
        soundSource.Play();
        return soundSource;
    }

    public void PlayRandomSound(List<SFX> listOfSFX)
    {
        PlaySound(listOfSFX[Random.Range(0, listOfSFX.Count - 1)]);
    }
    #endregion

    private void SetupSource(ref AudioSource source, SFX sfx)
    {
        source.clip = sfx.Clip;
        source.volume = sfx.Volume;
        source.pitch = sfx.Pitch;
    }

    private AudioSource GetAvailableSFXSource()
    {
        foreach (AudioSource soundEffectSource in _soundEffectSources)
        {
            if (!soundEffectSource.isPlaying)
            {
                return soundEffectSource;
            }
        }

        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        newAudioSource.outputAudioMixerGroup = sfxMixer;
        _soundEffectSources.Add(newAudioSource);
        return newAudioSource;
    }
}
