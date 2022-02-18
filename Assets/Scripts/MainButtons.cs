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
        GameObject.Find("AudioBox").GetComponent<AudioBox>().AudioPlay(GameObject.Find("AudioBox").GetComponent<AudioBox>().click);
        panelPause.SetActive(true);
        Time.timeScale = 0;
    }

    public void PauseOff(){
        GameObject.Find("AudioBox").GetComponent<AudioBox>().AudioPlay(GameObject.Find("AudioBox").GetComponent<AudioBox>().click);
        panelPause.SetActive(false);
        Time.timeScale = 1;
    }

    public void ShopOn(){
        GameObject.Find("AudioBox").GetComponent<AudioBox>().AudioPlay(GameObject.Find("AudioBox").GetComponent<AudioBox>().click);
        panelShop.SetActive(true);
    }

    public void ShopOff(){
        GameObject.Find("AudioBox").GetComponent<AudioBox>().AudioPlay(GameObject.Find("AudioBox").GetComponent<AudioBox>().click);
        panelShop.SetActive(false);
    }

    public void ResetScene(){
        GameObject.Find("AudioBox").GetComponent<AudioBox>().AudioPlay(GameObject.Find("AudioBox").GetComponent<AudioBox>().click);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void ResetLevel(){
        GameObject.Find("AudioBox").GetComponent<AudioBox>().AudioPlay(GameObject.Find("AudioBox").GetComponent<AudioBox>().click);
        SceneManager.LoadScene(Application.loadedLevel);
        Time.timeScale = 1;
    }
    public void ResetGame(){
        GameObject.Find("AudioBox").GetComponent<AudioBox>().AudioPlay(GameObject.Find("AudioBox").GetComponent<AudioBox>().click);
        PlayerPrefs.DeleteAll();
    }

    public void Menu(){
        GameObject.Find("AudioBox").GetComponent<AudioBox>().AudioPlay(GameObject.Find("AudioBox").GetComponent<AudioBox>().click);
        SceneManager.LoadScene("Menu");
    }

    public void MenuAfterWin()
    {
        UpdateProgressInDB();
        Menu();
    }

    public void Play(){
        Debug.Log("Play: " + LoginPanel.loggedPlayerProgress.level);
        GameObject.Find("AudioBox").GetComponent<AudioBox>().AudioPlay(GameObject.Find("AudioBox").GetComponent<AudioBox>().click);
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
        GameObject.Find("AudioBox").GetComponent<AudioBox>().AudioPlay(GameObject.Find("AudioBox").GetComponent<AudioBox>().click);
        //SceneManager.LoadScene("Level");
          panelMain.SetActive(false);
          panelLevel.SetActive(true);
    }

    public void LevelOff(){
        GameObject.Find("AudioBox").GetComponent<AudioBox>().AudioPlay(GameObject.Find("AudioBox").GetComponent<AudioBox>().click);
          panelLevel.SetActive(false);
          SwitchToMainPanel();
    }

    public void Exit(){
        GameObject.Find("AudioBox").GetComponent<AudioBox>().AudioPlay(GameObject.Find("AudioBox").GetComponent<AudioBox>().click);
        Application.Quit();
    }

    public void OnMusic(){
        GameObject.Find("AudioBox").GetComponent<AudioBox>().AudioPlay(GameObject.Find("AudioBox").GetComponent<AudioBox>().click);

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
        GameObject.Find("AudioBox").GetComponent<AudioBox>().AudioPlay(GameObject.Find("AudioBox").GetComponent<AudioBox>().click);

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
        GameObject.Find("AudioBox").GetComponent<AudioBox>().AudioPlay(GameObject.Find("AudioBox").GetComponent<AudioBox>().click);
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
        GameObject.Find("AudioBox").GetComponent<AudioBox>().AudioPlay(GameObject.Find("AudioBox").GetComponent<AudioBox>().click);
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
