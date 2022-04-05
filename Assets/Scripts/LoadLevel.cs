using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;

public class LoadLevel : MonoBehaviour, IPointerDownHandler
{
    public int levelToLoad;
    public Color inactiveColor;
    public Color activeColor;
    public Image backgroundImage;
    public TextMeshProUGUI levelText;
    
    bool loaded = false;

    private void Start()
    {
        LoginPanel.OnLoginUser += SetLevel;
        levelText.text = levelToLoad.ToString();
        SetLevel();
    }

    private void SetLevel()
    {
        loaded = LoginPanel.loggedPlayerProgress.level >= levelToLoad;
        backgroundImage.color = loaded ? activeColor : inactiveColor;
        
    }
    
    public void OnPointerDown(PointerEventData e)
    {
        AudioBox.instance.AudioPlay(AudioName.Click);
        if(loaded)
        {
            SceneManager.LoadScene(levelToLoad.ToString());
            Time.timeScale = 1;
        }
    }

    private void OnDestroy()
    {
        LoginPanel.OnLoginUser -= SetLevel;
    }
}
