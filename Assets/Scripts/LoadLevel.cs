using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class LoadLevel : MonoBehaviour, IPointerDownHandler
{
    bool loaded = false;
    
    private void Start()
    {
        Debug.Log(LoginPanel.loggedPlayerProgress.level);
        if(LoginPanel.loggedPlayerProgress.level >= Convert.ToInt32(gameObject.name)){
            gameObject.GetComponentInChildren<Image>().color =  new Color(0.651f, 0.549f, 0.345f);
            loaded = true;
        }
    }
    
    public void OnPointerDown(PointerEventData e)
    {
        AudioBox.instance.AudioPlay(AudioName.Click);
        if(loaded)
        {
            SceneManager.LoadScene(Convert.ToInt32(gameObject.name).ToString());
            Time.timeScale = 1;
        }
    }
}
