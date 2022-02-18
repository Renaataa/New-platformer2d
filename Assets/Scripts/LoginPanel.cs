using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
    public GameObject panelLogin;
    public GameObject mainCont;
    public GameObject loginCont;
    public GameObject registerCont;

    public InputField loginCont_userLogin;
    public InputField loginCont_userPass;
    public InputField registerCont_userLogin;
    public InputField registerCont_userPass;
    public InputField registerCont_confirmPass;
    
    DBConnection dbConnection;

    public static bool isLogged = false;
    public static string USER_LOGIN;
    public static Color USER_AVATAR_COLOR;
    public static PlayerProgress loggedPlayerProgress;

    public GameObject loginErrorMsg;
    public GameObject loginErrorMsgEmptyFields;
    public GameObject registerErrorMsg_loginUsed;
    public GameObject registerErrorMsg_confirm;
    public GameObject registerErrorMsgEmptyFields;

    public GameObject playerDetails;
    public GameObject playerDetailsName;
    public GameObject playerDetailsAvatar;
    public GameObject playerDetailsAvatarFirstLetter;

    public GameObject backBtn;
    public GameObject loginBtn;
    public GameObject registerBtn;
    public GameObject logoffBtn;

    public GameObject pleaseLoginInfo;

    void Start()
    {
        dbConnection = new DBConnection();
        dbConnection.InitDatabaseConnection();
        if(!isLogged) 
        {
            OnLogginPanel();
        }
    }

    public void OnLogginPanel()
    {
        panelLogin.SetActive(true);
        SwitchToMainCont();
        SwitchOffLoginCont();
        SwitchOffRegisterCont();
    }

    public void OffLogginPanel()
    {
        panelLogin.SetActive(false);
    }

    void SwitchOnMainCont()
    {
        mainCont.SetActive(true);
        if(isLogged)
        {
            backBtn.SetActive(true);
            logoffBtn.SetActive(true);
            loginBtn.SetActive(false);
            registerBtn.SetActive(false);
            pleaseLoginInfo.SetActive(false);
        }
        else
        {
            backBtn.SetActive(false);
            logoffBtn.SetActive(false);
            loginBtn.SetActive(true);
            registerBtn.SetActive(true);
            pleaseLoginInfo.SetActive(true);
        }
    }

     void SwitchOffMainCont()
    {
        mainCont.SetActive(false);
    }

    void SwitchOffLoginCont()
    {
        ClearLoginContInputs();
        loginCont.SetActive(false);
    }

    void ClearLoginContInputs()
    {
        ClearInput(loginCont_userLogin);
        ClearInput(loginCont_userPass);
    }

    void ClearInput(InputField input)
    {
        input.Select();
        input.text = "";
    }

    void SwitchOffRegisterCont()
    {
        ClearRegisterContInputs();
        registerCont.SetActive(false);
    }

    void ClearRegisterContInputs()
    {
        ClearInput(registerCont_userLogin);
        ClearInput(registerCont_userPass);
        ClearInput(registerCont_confirmPass);
    }

    public void SwitchToLoginCont()
    {
        loginErrorMsg.SetActive(false);
        loginErrorMsgEmptyFields.SetActive(false);
        SwitchOffMainCont();
        SwitchOffRegisterCont();
        loginCont.SetActive(true);
    }

    public void SwitchToRegisterCont()
    {
        SwitchOffMainCont();
        SwitchOffLoginCont();
        registerCont.SetActive(true);
        registerErrorMsg_confirm.SetActive(false);
        registerErrorMsg_loginUsed.SetActive(false);
        registerErrorMsgEmptyFields.SetActive(false);
    }

    public void SwitchToMainCont()
    {
        SwitchOffLoginCont();
        SwitchOffRegisterCont();
        SwitchOnMainCont();

        if(isLogged)
        {
            playerDetails.SetActive(true);
            playerDetailsName.GetComponent<Text>().text = USER_LOGIN;
            playerDetailsAvatarFirstLetter.GetComponent<Text>().text = USER_LOGIN.Substring(0, 1).ToUpper();
        }
        else
        {
            playerDetails.SetActive(false);
        }
    }

    public void LoginUser()
    {
        loginErrorMsg.SetActive(false);
        loginErrorMsgEmptyFields.SetActive(false);
        string login = loginCont_userLogin.text;
        string pass = loginCont_userPass.text;

        if(string.IsNullOrEmpty(login) || string.IsNullOrEmpty(pass))
        {
            loginErrorMsgEmptyFields.SetActive(true);
            return;
        }

        bool isCorrect = dbConnection.LoginUser(login , pass);
        if(isCorrect)
        {
            isLogged = true;
            USER_LOGIN = login;
            PlayerProgress loggedPlayerProgress  = dbConnection.GetPlayerProgress(); 
            LoginPanel.loggedPlayerProgress = loggedPlayerProgress;
            SwitchToMainCont();
        }
        else
        {
            loginErrorMsg.SetActive(true);
        }
    }

    public void RegisterUser()
    {
        registerErrorMsg_confirm.SetActive(false);
        registerErrorMsg_loginUsed.SetActive(false);
        registerErrorMsgEmptyFields.SetActive(false);
        string login = registerCont_userLogin.text;
        string pass = registerCont_userPass.text;
        string confirm = registerCont_confirmPass.text;

        if(string.IsNullOrEmpty(login) || string.IsNullOrEmpty(pass) || string.IsNullOrEmpty(confirm))
        {
            registerErrorMsgEmptyFields.SetActive(true);
            return;
        }

        if(!string.Equals(pass,confirm))
        {
            registerErrorMsg_confirm.SetActive(true);
            return;
        }

        if(dbConnection.CheckIfLoginUsed(login))
        {
            registerErrorMsg_loginUsed.SetActive(true);
            return;
        }

        //var avatarColor = AvatarCircleColor();
        dbConnection.RegisterUser(login , pass);
        isLogged = true;
        USER_LOGIN = login;
        PlayerProgress loggedPlayerProgress  = dbConnection.GetPlayerProgress(); 
        LoginPanel.loggedPlayerProgress = loggedPlayerProgress;
        SwitchToMainCont();
    }

   // Color AvatarCircleColor()
   // {
        //Random rnd = new Random();
        //Color randomColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
        //image.GetComponent<Image>().color = new Color32(255,255,225,100);
       /// return randomColor;
   // }

    public void Logout()
    {
        isLogged = false;
        USER_LOGIN = "";
        SwitchToMainCont();
    }

    public void PlayBtnClickSound()
    {
        GameObject.Find("AudioBox").GetComponent<AudioBox>().AudioPlay(GameObject.Find("AudioBox").GetComponent<AudioBox>().click);
    }
}
    