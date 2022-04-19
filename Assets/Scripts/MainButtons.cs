using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Collections.Generic;
using Code.BaseSystems.Settings;
using Code.BaseSystems.Translations;
using UnityEngine.Events;

public class MainButtons : MonoBehaviour
{
    public GameObject panelShop;
    public GameObject panelPause;
    public GameObject panelSettings;
    public GameObject panelMain;
    public GameObject panelLevel;
    public GameObject newGamePanel;
    public GameObject helpPanel;
    public HelpPanel helpPanelController;
    public GameObject ButtonPlay;
    public GameObject ButtonNewGame;
    public TMP_Dropdown languagesDropdown;
    public TextMeshProUGUI musicSettingText;
    public TextMeshProUGUI soundSettingText;
    
    DBConnection dbConnection;

    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "Menu")
        {
            SwitchToMainPanel();
        }

        TranslationsManager.Instance.OnLanguageChange += TranslationsManager_OnLanguageChange;
    }

    private void OnDestroy()
    {
        TranslationsManager.Instance.OnLanguageChange -= TranslationsManager_OnLanguageChange;
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

    public void OnNewGame()
    {
        AudioBox.instance.AudioPlay(AudioName.Click);
        panelMain.SetActive(false);
        newGamePanel.SetActive(true);
    }

    public void OnNewGameBack()
    {
        AudioBox.instance.AudioPlay(AudioName.Click);
        panelMain.SetActive(true);
        newGamePanel.SetActive(false);
    }
    
    public void ResetGame(){
        AudioBox.instance.AudioPlay(AudioName.Click);
        PlayerPrefs.DeleteAll();
        
        SettingsManager.Instance.SaveSettings();
        dbConnection = new DBConnection();
        dbConnection.ResetPlayerProgress();
        LoginPanel.loggedPlayerProgress = dbConnection.GetPlayerProgress();

        Play();
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

        if(SettingsManager.Instance.Music)
        {
            SettingsManager.Instance.Music = false;
            AudioBox.instance.MusicEnable(false);
            GameObject.Find("TextMusic").GetComponent<TextMeshProUGUI>().text = TranslationsManager.Instance.GetPhrase("musicOff");
        }
        else
        {
            SettingsManager.Instance.Music = true;
            AudioBox.instance.MusicEnable(true);
            GameObject.Find("TextMusic").GetComponent<TextMeshProUGUI>().text = TranslationsManager.Instance.GetPhrase("musicOn");
        }
        //SceneManager.LoadScene(Application.loadedLevel);
    }

    public void OnSound(){
        AudioBox.instance.AudioPlay(AudioName.Click);

        if(SettingsManager.Instance.Sound)
        {
            SettingsManager.Instance.Sound = false;
            AudioBox.instance.SoundsEnable(false);
            GameObject.Find("TextSound").GetComponent<TextMeshProUGUI>().text = TranslationsManager.Instance.GetPhrase("soundOff");
        }
        else
        {
            SettingsManager.Instance.Sound = true;
            AudioBox.instance.SoundsEnable(true);
            GameObject.Find("TextSound").GetComponent<TextMeshProUGUI>().text = TranslationsManager.Instance.GetPhrase("soundOn");
        }
        //SceneManager.LoadScene(Application.loadedLevel);
    }

    public void OnSettings(){
        panelMain.SetActive(false);
        AudioBox.instance.AudioPlay(AudioName.Click);
        panelSettings.SetActive(true);
        PrepareLanguagesSettings();

        if(!SettingsManager.Instance.Music){
            GameObject.Find("TextMusic").GetComponent<TextMeshProUGUI>().text = TranslationsManager.Instance.GetPhrase("musicOff");
        }
        else{
            GameObject.Find("TextMusic").GetComponent<TextMeshProUGUI>().text = TranslationsManager.Instance.GetPhrase("musicOn");
        }

        if(!SettingsManager.Instance.Sound){
            GameObject.Find("TextSound").GetComponent<TextMeshProUGUI>().text = TranslationsManager.Instance.GetPhrase("soundOff");
        }
        else{
            GameObject.Find("TextSound").GetComponent<TextMeshProUGUI>().text = TranslationsManager.Instance.GetPhrase("soundOn");
        }
    }

    public void OffSettings(){
        SettingsManager.Instance.SaveSettings();
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

    public void OnHelp()
    {
        AudioBox.instance.AudioPlay(AudioName.Click);
        helpPanelController.SetHelpPanel();
        panelMain.SetActive(false);
        helpPanel.SetActive(true);
    }

    public void OnHelpClose()
    {
        AudioBox.instance.AudioPlay(AudioName.Click);
        panelMain.SetActive(true);
        helpPanel.SetActive(false);
    }

    private void PrepareLanguagesSettings()
    {
        List<string> languages = TranslationsManager.Instance.GetLanguagesList();
        languagesDropdown.ClearOptions();
        languagesDropdown.AddOptions(languages);
        languagesDropdown.SetValueWithoutNotify((int)SettingsManager.Instance.Language);
        languagesDropdown.onValueChanged.RemoveAllListeners();
        languagesDropdown.onValueChanged.AddListener(ChangeLanguage);
    }

    private void ChangeLanguage(int dropdownValue)
    {
        SettingsManager.Instance.Language = TranslationsManager.Instance.LanguageFromDropdownValue(dropdownValue).Id;
    }

    private void TranslationsManager_OnLanguageChange()
    {
        bool musicOn = SettingsManager.Instance.Music;
        bool soundOn = SettingsManager.Instance.Sound;
        if (musicSettingText != null && soundSettingText != null)
        {
            musicSettingText.text = TranslationsManager.Instance.GetPhrase(musicOn ? "musicOn" : "musicOff");
            soundSettingText.text = TranslationsManager.Instance.GetPhrase(soundOn ? "soundOn" : "soundOff");
        }
    }
}
