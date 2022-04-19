using Code.BaseSystems.Settings;
using UnityEngine;

public class Music : MonoBehaviour
{
    private void Start()
    {
        SetMusicEnabled();
    }

    public void SetMusicEnabled()
    {
        bool musicEnabled = SettingsManager.Instance.Music;
        GetComponent<AudioSource>().enabled = musicEnabled;
    }
}
