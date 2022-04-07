using System.Collections.Generic;
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
        if (PlayerPrefs.GetString("Sound") != "no")
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
        if (PlayerPrefs.HasKey("Music"))
        {
            MusicEnable(PlayerPrefs.GetString("Music") != "no");
        }

        if (PlayerPrefs.HasKey("Sound"))
        {
            SoundsEnable(PlayerPrefs.GetString("Sound") != "no");
        }
    }
}
