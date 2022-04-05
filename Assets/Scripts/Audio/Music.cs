using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    private void Start()
    {
        SetMusicEnabled();
    }

    public void SetMusicEnabled()
    {
        bool musicDisabled = PlayerPrefs.GetString("Music") == "no";
        GetComponent<AudioSource>().enabled = !musicDisabled;
        Debug.Log("Music enabled : " + !musicDisabled);
    }
}
