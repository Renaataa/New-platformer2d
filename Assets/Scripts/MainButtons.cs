using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class MainButtons : MonoBehaviour
{
    public GameObject panelShop;
    public GameObject panelPause;
    public GameObject panelSettings;
    public GameObject panelMain;
    public GameObject panelLevel;
    public GameObject ButtonPlay;
    public GameObject ButtonNewGame;
    DBConnection dbConnection;

    void Start()
    {
        if(SceneManager.GetActiveScene().name == "Menu")
        {
            SwitchToMainPanel();
        }
    }
    
    public void PauseOn(){
        AudioBox.instance.AudioPlay(AudioName.Click);
        panelPause.SetActive(true);
        Time.timeScale = 0;
    }

    public void PauseOff(){
        AudioBox.instance.AudioPlay(AudioName.Click);
        panelPause.SetActive(false);
        Time.timeScale = 1;
    }

    public void ShopOn(){
        AudioBox.instance.AudioPlay(AudioName.Click);
        panelShop.SetActive(true);
    }

    public void ShopOff(){
        AudioBox.instance.AudioPlay(AudioName.Click);
        panelShop.SetActive(false);
    }

    public void ResetScene(){
        AudioBox.instance.AudioPlay(AudioName.Click);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void ResetLevel(){
        AudioBox.instance.AudioPlay(AudioName.Click);
        SceneManager.LoadScene(Application.loadedLevel);
        Time.timeScale = 1;
    }
    public void ResetGame(){
        AudioBox.instance.AudioPlay(AudioName.Click);
        PlayerPrefs.DeleteAll();
    }

    public void Menu(){
        AudioBox.instance.AudioPlay(AudioName.Click);
        SceneManager.LoadScene("Menu");
    }

    public void MenuAfterWin()
    {
        UpdateProgressInDB();
        Menu();
    }

    public void Play(){
        Debug.Log("Play: " + LoginPanel.loggedPlayerProgress.level);
        AudioBox.instance.AudioPlay(AudioName.Click);
        SceneManager.LoadScene(LoginPanel.loggedPlayerProgress.level.ToString());
        Time.timeScale = 1;
    }

    public void PlayNextLevel(){
            UpdateProgressInDB();
            //Debug.Log("Play: " + LoginPanel.loggedPlayerProgress.level);
            int currPlayedLevel = int.Parse(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene((currPlayedLevel+1).ToString());
            Time.timeScale = 1;
    }

    void UpdateProgressInDB()
    {
        int currSavedLevel = LoginPanel.loggedPlayerProgress.level;
        if(currSavedLevel == 11) return;
        dbConnection = new DBConnection();
        CharacterAnimation characterAnimation = GameObject.Find("Player").GetComponent<CharacterAnimation>();
        bool shouldIncrementLevel = false;
        Debug.Log("currSavedLevel " + currSavedLevel);
        try
        {
            int currPlayedLevel = int.Parse(SceneManager.GetActiveScene().name);
            Debug.Log("currPlayedLevel " + currPlayedLevel);
            if(currPlayedLevel == currSavedLevel && currPlayedLevel <11)
            {
                shouldIncrementLevel = true;
            }
        }
        catch(Exception ex)
        {
            shouldIncrementLevel = false;
        }

        Debug.Log("should increment level: " + shouldIncrementLevel);
        
        if(shouldIncrementLevel)
        {
            LoginPanel.loggedPlayerProgress.level++;
        }
        
        if(currSavedLevel <11)
        {
            LoginPanel.loggedPlayerProgress.health = characterAnimation.health;
            LoginPanel.loggedPlayerProgress.energy = characterAnimation.energy;
            LoginPanel.loggedPlayerProgress.coins = characterAnimation.coin;
            LoginPanel.loggedPlayerProgress.boost.health = characterAnimation.countHealth;
            LoginPanel.loggedPlayerProgress.boost.jump = characterAnimation.countJump;
            LoginPanel.loggedPlayerProgress.boost.speed = characterAnimation.countSpeed;
            LoginPanel.loggedPlayerProgress.boost.protect = characterAnimation.countProtect;
        }
        
        dbConnection.UpdatePlayerProgressInDB();
    }


    public void Level(){
        AudioBox.instance.AudioPlay(AudioName.Click);
        //SceneManager.LoadScene("Level");
          panelMain.SetActive(false);
          panelLevel.SetActive(true);
    }

    public void LevelOff(){
        AudioBox.instance.AudioPlay(AudioName.Click);
          panelLevel.SetActive(false);
          SwitchToMainPanel();
    }

    public void Exit(){
        AudioBox.instance.AudioPlay(AudioName.Click);
        Application.Quit();
    }

    public void OnMusic(){
        AudioBox.instance.AudioPlay(AudioName.Click);

        if(PlayerPrefs.GetString("Music") != "no"){
            PlayerPrefs.SetString("Music", "no");
            GameObject.Find("TextMusic").GetComponent<TextMeshProUGUI>().text = "Music: off";
        }
        else{
            PlayerPrefs.SetString("Music", "yes");
            GameObject.Find("TextMusic").GetComponent<TextMeshProUGUI>().text = "Music: on";
        }
        //SceneManager.LoadScene(Application.loadedLevel);
    }

    public void OnSound(){
        AudioBox.instance.AudioPlay(AudioName.Click);

        if(PlayerPrefs.GetString("Sound") != "no"){
            PlayerPrefs.SetString("Sound", "no");
            GameObject.Find("TextSound").GetComponent<TextMeshProUGUI>().text = "Sound: off";
        }
        else{
            PlayerPrefs.SetString("Sound", "yes");
            GameObject.Find("TextSound").GetComponent<TextMeshProUGUI>().text = "Sound: on";
        }
        //SceneManager.LoadScene(Application.loadedLevel);
    }

    public void OnSettings(){
        panelMain.SetActive(false);
        AudioBox.instance.AudioPlay(AudioName.Click);
        panelSettings.SetActive(true);

        if(PlayerPrefs.GetString("Music") == "no"){
            GameObject.Find("TextMusic").GetComponent<TextMeshProUGUI>().text = "Music: off";
        }
        else{
            GameObject.Find("TextMusic").GetComponent<TextMeshProUGUI>().text = "Music: on";
        }

        if(PlayerPrefs.GetString("Sound") == "no"){
            GameObject.Find("TextSound").GetComponent<TextMeshProUGUI>().text = "Sound: off";
        }
        else{
            GameObject.Find("TextSound").GetComponent<TextMeshProUGUI>().text = "Sound: on";
        }
    }

    public void OffSettings(){
        AudioBox.instance.AudioPlay(AudioName.Click);
        panelSettings.SetActive(false);
        panelMain.SetActive(true);
    }

    public void SwitchToMainPanel()
    {
        if(LoginPanel.isLogged) 
        {
            panelMain.SetActive(true);
            ButtonPlay.SetActive(LoginPanel.loggedPlayerProgress.level<11);
            ButtonNewGame.SetActive(LoginPanel.loggedPlayerProgress.level<11);
        }       
    }

    public void SwitchOffMainPanel()
    {
        panelMain.SetActive(false);
    }
}
