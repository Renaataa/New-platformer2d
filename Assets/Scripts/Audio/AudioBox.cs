using System.Collections.Generic;
using Code.BaseSystems.Settings;
using UnityEngine;
using UnityEngine.Audio;

public class AudioBox : MonoBehaviour
{
    public List<AudioPair> clips;
    public AudioSource source;
    public AudioMixer mixer;
    public static AudioBox instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance.gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetupSounds();
    }

    public void AudioPlay(AudioName audioName)
    {
        if (SettingsManager.Instance.Sound)
        {
            AudioPair pair = clips.Find(p => p.name == audioName);
            if (pair != null)
            {
                source.PlayOneShot(pair.clip);
            }
        }
    }

    public void MusicEnable(bool enable)
    {
        mixer.SetFloat("MusicVolume", enable ? 0f : -80f);
    }

    public void SoundsEnable(bool enable)
    {
        mixer.SetFloat("SoundsVolume", enable ? 0f : -80f);
    }

    private void SetupSounds()
    {
        MusicEnable(SettingsManager.Instance.Music);
        SoundsEnable(SettingsManager.Instance.Sound);
    }
}
