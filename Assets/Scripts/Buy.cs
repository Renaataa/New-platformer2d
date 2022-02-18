using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class Buy : MonoBehaviour
{   
    public void BuyBoost(){
        GameObject.Find("AudioBox").GetComponent<AudioBox>().AudioPlay(GameObject.Find("AudioBox").GetComponent<AudioBox>().click);
        if(name.Contains("Health") == true && GameObject.Find("Player").GetComponent<CharacterAnimation>().coin >= 15){
            GameObject.Find("Player").GetComponent<CharacterAnimation>().coin -= 15;
            GameObject.Find("Player").GetComponent<CharacterAnimation>().panelBoost.SetActive(true);
            GameObject.Find("Player").GetComponent<CharacterAnimation>().healthBoost.SetActive(true);
            GameObject.Find("Player").GetComponent<CharacterAnimation>().countHealth++;
            GameObject.Find("HealthBoostText").GetComponent<TextMeshProUGUI>().text = GameObject.Find("Player").GetComponent<CharacterAnimation>().countHealth.ToString();
            GameObject.Find("AudioBox").GetComponent<AudioBox>().AudioPlay(GameObject.Find("AudioBox").GetComponent<AudioBox>().pickUpBootle);
        }
        else if(name.Contains("Jump") == true && GameObject.Find("Player").GetComponent<CharacterAnimation>().coin >= 15){
            GameObject.Find("Player").GetComponent<CharacterAnimation>().coin -= 15;
            GameObject.Find("Player").GetComponent<CharacterAnimation>().panelBoost.SetActive(true);
            GameObject.Find("Player").GetComponent<CharacterAnimation>().jumpBoost.SetActive(true);
            GameObject.Find("Player").GetComponent<CharacterAnimation>().countJump++;
            GameObject.Find("JumpBoostText").GetComponent<TextMeshProUGUI>().text = GameObject.Find("Player").GetComponent<CharacterAnimation>().countJump.ToString();
            GameObject.Find("AudioBox").GetComponent<AudioBox>().AudioPlay(GameObject.Find("AudioBox").GetComponent<AudioBox>().pickUpBootle);
        }
        else if(name.Contains("Speed") == true && GameObject.Find("Player").GetComponent<CharacterAnimation>().coin >= 15){
            GameObject.Find("Player").GetComponent<CharacterAnimation>().coin -= 15;
            GameObject.Find("Player").GetComponent<CharacterAnimation>().panelBoost.SetActive(true);
            GameObject.Find("Player").GetComponent<CharacterAnimation>().speedBoost.SetActive(true);
            GameObject.Find("Player").GetComponent<CharacterAnimation>().countSpeed++;
            GameObject.Find("SpeedBoostText").GetComponent<TextMeshProUGUI>().text = GameObject.Find("Player").GetComponent<CharacterAnimation>().countSpeed.ToString();
            GameObject.Find("AudioBox").GetComponent<AudioBox>().AudioPlay(GameObject.Find("AudioBox").GetComponent<AudioBox>().pickUpBootle);
        }
        else if(name.Contains("Protect") == true && GameObject.Find("Player").GetComponent<CharacterAnimation>().coin >= 15){
            GameObject.Find("Player").GetComponent<CharacterAnimation>().coin -= 15;
            GameObject.Find("Player").GetComponent<CharacterAnimation>().panelBoost.SetActive(true);
            GameObject.Find("Player").GetComponent<CharacterAnimation>().protectBoost.SetActive(true);
            GameObject.Find("Player").GetComponent<CharacterAnimation>().countProtect++;
            GameObject.Find("ProtectBoostText").GetComponent<TextMeshProUGUI>().text = GameObject.Find("Player").GetComponent<CharacterAnimation>().countProtect.ToString();
            GameObject.Find("AudioBox").GetComponent<AudioBox>().AudioPlay(GameObject.Find("AudioBox").GetComponent<AudioBox>().pickUpBootle);
        }
        
        GameObject.Find("Shop").SetActive(false);
        GameObject.Find("PanelPause").SetActive(false);
        Time.timeScale = 1;
    }
}
