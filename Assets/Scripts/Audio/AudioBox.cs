using System.Collections.Generic;
using UnityEngine;

public class AudioBox : MonoBehaviour
{
    public List<AudioPair> clips;
    public AudioSource source;
    
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
}
