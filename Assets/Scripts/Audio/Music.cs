using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    private void Start()
    {
        if(PlayerPrefs.GetString("Music") == "no")
        {
            GetComponent<AudioSource>().enabled = false;
        }
        else
        {
            GetComponent<AudioSource>().enabled = true;
        }
    }

    public void SetMusicEnabled()
    {
        Debug.Log((PlayerPrefs.GetString("Music") == "no"));
        bool musicEnabled = PlayerPrefs.GetString("Music") == "no";
        Debug.Log(musicEnabled);
        GetComponent<AudioSource>().enabled = !musicEnabled;
    }
}
