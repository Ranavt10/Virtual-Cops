using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;


public class RegisterformScript : MonoBehaviour
{
    [Header("Login")]
    public InputField emailLoginField;

    public InputField passwordLoginField;
   // public Text warningLoginText;
  //  public Text confirmLoginText;

    [Header("Register")]
    public InputField usernameRegisterField;
    public InputField emailRegisterField;
    public InputField passwordRegisterField;
    public InputField passwordRegisterVerifyField;
   // public Text warningRegisterText;
    public GameObject regbutton;
    public GameObject loginbutton;
    public GameObject forgetpasswordbutton;
    public GameObject loading;
    public Text infotextlogin;
    public Text infotextRegister;

    public InputField referralInputField;

    public Sprite eyesopen;
    public Sprite eyesclose;
    public Button[] eyebutton;

    private static string registerConcat = "auth/register";

    private static string loginUserConcat = "auth/login";

    private string registerUserapi = APIHolder.getBaseUrl() + registerConcat;/*"https://a978-72-255-51-32.ngrok-free.app/api/auth/register"*/

    private string loginUserapi = APIHolder.getBaseUrl() + loginUserConcat;/*"https://a978-72-255-51-32.ngrok-free.app/api/auth/login";*/

    private void Start()
    {
        regbutton.SetActive(true);

    }

    public void RegisterButton()
    {
        if (usernameRegisterField.text.Length < 0)
        {
            StartCoroutine(waitlogin());
            regbutton.SetActive(false);
            loading.SetActive(true);
            infotextRegister.text = "Please Enter UserName";

            return;
        }
        else if (emailRegisterField.text.Length < 0)
        {
            StartCoroutine(waitlogin());
            regbutton.SetActive(false);
            loading.SetActive(true);
            infotextRegister.text = "Please Enter Email";
            return;
        }
        else if (passwordRegisterField.text.Length < 0)
        {
            StartCoroutine(waitlogin());
            regbutton.SetActive(false);
            loading.SetActive(true);
            infotextRegister.text = "Please Enter Strong Password";
            return;
        }
        else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            StartCoroutine(waitlogin());
            regbutton.SetActive(false);
            loading.SetActive(true);
            infotextRegister.text = "Confirm Password not Matched";
            return;
        }
        else
        {
            StartCoroutine(RegisterUserCallapi(usernameRegisterField.text, emailRegisterField.text, passwordRegisterField.text, referralInputField.text));
            StartCoroutine(waitlogin());
            regbutton.SetActive(false);
            loading.SetActive(true);
        }

  
       

    }

    

    private IEnumerator RegisterUserCallapi(string username, string email, string password, string refferalcode)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("email", email);
        form.AddField("password", password);
        form.AddField("referralCode", refferalcode);

        using (UnityWebRequest www = UnityWebRequest.Post(registerUserapi, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                ApiResponseRegister response = JsonUtility.FromJson<ApiResponseRegister>(www.downloadHandler.text);

                // Using the parsed data
                bool isSuccess = response.success;
                string message = response.message;
                infotextRegister.text = message;
            }
            else
            {
                Debug.Log("Request sent successfully!");
                Debug.Log("Received: " + www.downloadHandler.text);


                // Parsing the response
                ApiResponseRegister response = JsonUtility.FromJson<ApiResponseRegister>(www.downloadHandler.text);

                // Using the parsed data
                bool isSuccess = response.success;
                string message = response.message;
                infotextRegister.text = message;
                Debug.Log("Success: " + isSuccess + ", Message: " + message);

                if (isSuccess == true)
                {
                    PlayerPrefs.SetInt("jj", 1);
                   
                    StartCoroutine(offloadingandloginpenal());
                }
               

            }
        }
    }



    private IEnumerator loginUserCallapi(string email, string password)
    {
        // Creating a wwwForm object to send the parameters
        WWWForm form = new WWWForm();

        form.AddField("email", email);
        form.AddField("password", password);

        // Creating the UnityWebRequest and sending it
        using (UnityWebRequest www = UnityWebRequest.Post(loginUserapi, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                var json = JObject.Parse(www.downloadHandler.text);
                string message = json["message"].ToString();
                infotextlogin.text = message;
            }
            else
            {
                Debug.Log("Request sent successfully!");
                Debug.Log("Received: " + www.downloadHandler.text);

                // Parse the JSON response using Newtonsoft.Json
                var json = JObject.Parse(www.downloadHandler.text);

                // Extracting and using the desired values
                string token = json["token"].ToString();
               
                string message = json["message"].ToString();
                int winLevels = int.Parse(json["winLevels"].ToString());
                float vrc = float.Parse(json["vrc"].ToString());
                string username = json["username"].ToString();

                infotextlogin.text = "Login successfully: " + message;

                // Optionally store the values in PlayerPrefs
                PlayerPrefs.SetString("Username", email);
                PlayerPrefs.SetString("Password", password);  // Be cautious storing passwords!
                PlayerPrefs.SetString("Token", token);
                PlayerPrefs.SetInt("unlocklevel", winLevels);
                PlayerPrefs.SetFloat("Meta", vrc);
                PlayerPrefs.SetInt("Guest", 0);
                PlayerPrefs.SetInt("firsttime", 1);

                PlayerPrefs.SetString("PlayerName", username);

                float gems = float.Parse(json["gems"].ToString());
                string userid = json["userId"].ToString();
                string useremail = json["email"].ToString();
                string walletAddress = json["walletAddress"].ToString();

                Debug.Log("Gems are:" + gems);
                PlayerPrefs.SetString("WalletAddress", walletAddress);
                PlayerPrefs.SetFloat("gems", gems);
                PlayerPrefs.SetString("UserId", userid);
                PlayerPrefs.SetString("userEmail", useremail);

                // Retrieve and store weapons array
                JArray weaponsArray = (JArray)json["weapons"];

                // Iterate through weapons array and store values
                for (int i = 0; i < weaponsArray.Count; i++)
                {
                    // Convert the weapon value to a string
                    string weaponValue = weaponsArray[i].ToString();
                    Debug.Log("Weapon Val is:" + weaponValue);
                    string weaponPrefKey = "Weapon_" + weaponValue; // Customize the prefix as per your need
                    PlayerPrefs.SetInt(weaponPrefKey, 1);
                }

                JArray charactersArray = (JArray)json["characters"];

                for (int i = 0; i < charactersArray.Count; i++)
                {
                    string characterValue = charactersArray[i].ToString();
                    Debug.Log("Character Val is:" + characterValue);
                    string characterPrefKey = "Character_" + characterValue;
                    PlayerPrefs.SetInt(characterPrefKey, 1);
                }

                JArray avatarsArray = (JArray)json["avatars"];

                for (int i = 0; i < avatarsArray.Count; i++)
                {
                    string avatarValue = avatarsArray[i].ToString();
                    Debug.Log("Character Val is:" + avatarValue);
                    string avatarPrefKey = "Avatar_" + avatarValue;
                    PlayerPrefs.SetInt(avatarPrefKey, 1);
                }

                JArray BannersArray = (JArray)json["banners"];

                for (int i = 0; i < BannersArray.Count; i++)
                {
                    string BannerValue = BannersArray[i].ToString();
                    Debug.Log("Character Val is:" + BannerValue);
                    string BannerPrefKey = "Banner_" + BannerValue;
                    PlayerPrefs.SetInt(BannerPrefKey, 1);
                }

                retunvrclevel();
                SceneFading.Instance.FadeOutAndLoadScene(StaticValue.SCENE_MENU);
            }
        }
    }

    private IEnumerator loginUserCallapiFromRegister(string email, string password)
    {
        // Creating a wwwForm object to send the parameters
        WWWForm form = new WWWForm();

        form.AddField("email", email);
        form.AddField("password", password);

        // Creating the UnityWebRequest and sending it
        using (UnityWebRequest www = UnityWebRequest.Post(loginUserapi, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                var json = JObject.Parse(www.downloadHandler.text);
                string message = json["message"].ToString();
                infotextlogin.text = message;
            }
            else
            {
                Debug.Log("Request sent successfully!");
                Debug.Log("Received: " + www.downloadHandler.text);

                // Parse the JSON response using Newtonsoft.Json
                var json = JObject.Parse(www.downloadHandler.text);

                // Extracting and using the desired values
                string token = json["token"].ToString();

                string message = json["message"].ToString();
                int winLevels = int.Parse(json["winLevels"].ToString());
                float vrc = float.Parse(json["vrc"].ToString());
                string username = json["username"].ToString();

                infotextlogin.text = "Login successfully: " + message;

                // Optionally store the values in PlayerPrefs
                PlayerPrefs.SetString("Username", email);
                PlayerPrefs.SetString("Password", password);  // Be cautious storing passwords!
                PlayerPrefs.SetString("Token", token);
               // PlayerPrefs.SetInt("unlocklevel", winLevels - 1);
                PlayerPrefs.SetFloat("Meta", vrc);
                PlayerPrefs.SetInt("Guest", 0);
                PlayerPrefs.SetInt("firsttime", 1);

                PlayerPrefs.SetString("PlayerName", username);
                
                float gems = float.Parse(json["gems"].ToString());
                string userid = json["userId"].ToString();
                string useremail = json["email"].ToString();
                string walletAddress = json["walletAddress"].ToString();

                Debug.Log("Gems are:" + gems);
                PlayerPrefs.SetString("WalletAddress", walletAddress);
                PlayerPrefs.SetFloat("gems", gems);
                PlayerPrefs.SetString("UserId", userid);
                PlayerPrefs.SetString("userEmail", useremail);

                // Retrieve and store weapons array
                JArray weaponsArray = (JArray)json["weapons"];

                // Iterate through weapons array and store values
                for (int i = 0; i < weaponsArray.Count; i++)
                {
                    // Convert the weapon value to a string
                    string weaponValue = weaponsArray[i].ToString();
                    Debug.Log("Weapon Val is:" + weaponValue);
                    string weaponPrefKey = "Weapon_" + weaponValue; // Customize the prefix as per your need
                    PlayerPrefs.SetInt(weaponPrefKey, 1);
                }

                JArray charactersArray = (JArray)json["characters"];

                for (int i = 0; i < charactersArray.Count; i++)
                {
                    string characterValue = charactersArray[i].ToString();
                    Debug.Log("Character Val is:" + characterValue);
                    string characterPrefKey = "Character_" + characterValue;
                    PlayerPrefs.SetInt(characterPrefKey, 1);
                }

                JArray avatarsArray = (JArray)json["avatars"];

                for (int i = 0; i < avatarsArray.Count; i++)
                {
                    string avatarValue = avatarsArray[i].ToString();
                    Debug.Log("Character Val is:" + avatarValue);
                    string avatarPrefKey = "Avatar_" + avatarValue;
                    PlayerPrefs.SetInt(avatarPrefKey, 1);
                }

                JArray BannersArray = (JArray)json["banners"];

                for (int i = 0; i < BannersArray.Count; i++)
                {
                    string BannerValue = BannersArray[i].ToString();
                    Debug.Log("Character Val is:" + BannerValue);
                    string BannerPrefKey = "Banner_" + BannerValue;
                    PlayerPrefs.SetInt(BannerPrefKey, 1);
                }

                SceneFading.Instance.FadeOutAndLoadScene(StaticValue.SCENE_MENU);
            }
        }
    }
    public void retunvrclevel()
    {
        if (PlayerPrefs.GetInt("unlocklevel") == 1)
        {
            PlayerPrefs.SetInt("vrcLevel", 3);
            PlayerPrefs.SetInt("locklevel", 0);
           // PlayerPrefs.SetInt("RewardClaimed_" + PlayerPrefs.GetInt("vrcLevel"), 1);
        }

       else if (PlayerPrefs.GetInt("unlocklevel") == 2)
        {
            PlayerPrefs.SetInt("vrcLevel", 6);
            PlayerPrefs.SetInt("locklevel", 1);
           // PlayerPrefs.SetInt("RewardClaimed_" + PlayerPrefs.GetInt("vrcLevel"), 1);
        }

        else if (PlayerPrefs.GetInt("unlocklevel") == 3)
        {
            PlayerPrefs.SetInt("vrcLevel", 9);
            PlayerPrefs.SetInt("locklevel", 2);
          //  PlayerPrefs.SetInt("RewardClaimed_" + PlayerPrefs.GetInt("vrcLevel"), 1);
        }

        else if (PlayerPrefs.GetInt("unlocklevel") == 4)
        {
            PlayerPrefs.SetInt("vrcLevel", 12);
            PlayerPrefs.SetInt("locklevel", 3);
           // PlayerPrefs.SetInt("RewardClaimed_" + PlayerPrefs.GetInt("vrcLevel"), 1);
        }
        else if (PlayerPrefs.GetInt("unlocklevel") == 5)
        {
            PlayerPrefs.SetInt("vrcLevel", 15);
            PlayerPrefs.SetInt("locklevel", 4);
           // PlayerPrefs.SetInt("RewardClaimed_" + PlayerPrefs.GetInt("vrcLevel"), 1);
        }
        else if (PlayerPrefs.GetInt("unlocklevel") == 6)
        {
            PlayerPrefs.SetInt("vrcLevel", 18);
            PlayerPrefs.SetInt("locklevel", 5);
           // PlayerPrefs.SetInt("RewardClaimed_" + PlayerPrefs.GetInt("vrcLevel"), 1);
        }
        else if (PlayerPrefs.GetInt("unlocklevel") == 7)
        {
            PlayerPrefs.SetInt("vrcLevel", 21);
            PlayerPrefs.SetInt("locklevel", 6);
           // PlayerPrefs.SetInt("RewardClaimed_" + PlayerPrefs.GetInt("vrcLevel"), 1);
        }
        else if (PlayerPrefs.GetInt("unlocklevel") == 8)
        {
            PlayerPrefs.SetInt("vrcLevel", 24);
            PlayerPrefs.SetInt("locklevel", 7);
           // PlayerPrefs.SetInt("RewardClaimed_" + PlayerPrefs.GetInt("vrcLevel"), 1);
        }
        else if (PlayerPrefs.GetInt("unlocklevel") == 9)
        {
            PlayerPrefs.SetInt("vrcLevel", 27);
            PlayerPrefs.SetInt("locklevel", 8);
           // PlayerPrefs.SetInt("RewardClaimed_" + PlayerPrefs.GetInt("vrcLevel"), 1);
        }
        else if (PlayerPrefs.GetInt("unlocklevel") == 10)
        {
            PlayerPrefs.SetInt("vrcLevel", 30);
            PlayerPrefs.SetInt("locklevel", 9);
           // PlayerPrefs.SetInt("RewardClaimed_" + PlayerPrefs.GetInt("vrcLevel"), 1);
        }
        else if (PlayerPrefs.GetInt("unlocklevel") == 11)
        {
            PlayerPrefs.SetInt("vrcLevel", 33);
            PlayerPrefs.SetInt("locklevel", 10);
          //  PlayerPrefs.SetInt("RewardClaimed_" + PlayerPrefs.GetInt("vrcLevel"), 1);
        }
        else if (PlayerPrefs.GetInt("unlocklevel") == 12)
        {
            PlayerPrefs.SetInt("vrcLevel", 36);
            PlayerPrefs.SetInt("locklevel", 11);
          //  PlayerPrefs.SetInt("RewardClaimed_" + PlayerPrefs.GetInt("vrcLevel"), 1);
        }
        else if (PlayerPrefs.GetInt("unlocklevel") == 13)
        {
            PlayerPrefs.SetInt("vrcLevel", 39);
            PlayerPrefs.SetInt("locklevel", 12);
           // PlayerPrefs.SetInt("RewardClaimed_" + PlayerPrefs.GetInt("vrcLevel"), 1);
        }
        else if (PlayerPrefs.GetInt("unlocklevel") == 14)
        {
            PlayerPrefs.SetInt("vrcLevel", 42);
            PlayerPrefs.SetInt("locklevel", 13);
           // PlayerPrefs.SetInt("RewardClaimed_" + PlayerPrefs.GetInt("vrcLevel"), 1);
        }
        else if (PlayerPrefs.GetInt("unlocklevel") == 15)
        {
            PlayerPrefs.SetInt("vrcLevel", 45);
            PlayerPrefs.SetInt("locklevel", 14);
           // PlayerPrefs.SetInt("RewardClaimed_" + PlayerPrefs.GetInt("vrcLevel"), 1);
        }
        else if (PlayerPrefs.GetInt("unlocklevel") == 16)
        {
            PlayerPrefs.SetInt("vrcLevel", 48);
            PlayerPrefs.SetInt("locklevel", 15);
           // PlayerPrefs.SetInt("RewardClaimed_" + PlayerPrefs.GetInt("vrcLevel"), 1);
        }
        else if (PlayerPrefs.GetInt("unlocklevel") == 17)
        {
            PlayerPrefs.SetInt("vrcLevel", 51);
            PlayerPrefs.SetInt("locklevel", 16);
           // PlayerPrefs.SetInt("RewardClaimed_" + PlayerPrefs.GetInt("vrcLevel"), 1);
        }
        else if (PlayerPrefs.GetInt("unlocklevel") == 18)
        {
            PlayerPrefs.SetInt("vrcLevel", 54);
            PlayerPrefs.SetInt("locklevel", 17);
           // PlayerPrefs.SetInt("RewardClaimed_" + PlayerPrefs.GetInt("vrcLevel"), 1);
        }
        else if (PlayerPrefs.GetInt("unlocklevel") == 19)
        {
            PlayerPrefs.SetInt("vrcLevel", 57);
            PlayerPrefs.SetInt("locklevel", 18);
           // PlayerPrefs.SetInt("RewardClaimed_" + PlayerPrefs.GetInt("vrcLevel"), 1);
        }
        else if (PlayerPrefs.GetInt("unlocklevel") == 20)
        {
            PlayerPrefs.SetInt("vrcLevel", 60);
            PlayerPrefs.SetInt("locklevel", 19);
           // PlayerPrefs.SetInt("RewardClaimed_" + PlayerPrefs.GetInt("vrcLevel"), 1);
        }
        else if (PlayerPrefs.GetInt("unlocklevel") == 21)
        {
            PlayerPrefs.SetInt("vrcLevel", 63);
            PlayerPrefs.SetInt("locklevel", 20);
           // PlayerPrefs.SetInt("RewardClaimed_" + PlayerPrefs.GetInt("vrcLevel"), 1);
        }
        else if (PlayerPrefs.GetInt("unlocklevel") == 22)
        {
            PlayerPrefs.SetInt("vrcLevel", 66);
            PlayerPrefs.SetInt("locklevel", 21);
           // PlayerPrefs.SetInt("RewardClaimed_" + PlayerPrefs.GetInt("vrcLevel"), 1);
        }
        else if (PlayerPrefs.GetInt("unlocklevel") == 23)
        {
            PlayerPrefs.SetInt("vrcLevel", 69);
            PlayerPrefs.SetInt("locklevel", 22);
           // PlayerPrefs.SetInt("RewardClaimed_" + PlayerPrefs.GetInt("vrcLevel"), 1);
        }
        else if (PlayerPrefs.GetInt("unlocklevel") == 24)
        {
            PlayerPrefs.SetInt("vrcLevel", 72);
            PlayerPrefs.SetInt("locklevel", 23);
           // PlayerPrefs.SetInt("RewardClaimed_" + PlayerPrefs.GetInt("vrcLevel"), 1);
        }


        int reward = PlayerPrefs.GetInt("vrcLevel");
        for (int i = 0; i <= reward; i++)
        {
            if (IsDivisibleBy3(i))
            {
                PlayerPrefs.SetInt("RewardClaimed_" + i, 1);
            }
            
        }
        
    }

    bool IsDivisibleBy3(int value)
    {
       
        return value % 3 == 0;
    }
    public void login()
    {
        StartCoroutine(waitlogin());
        loginbutton.SetActive(false);
        forgetpasswordbutton.SetActive(false);
        loading.SetActive(true);

        if (PlayerPrefs.GetInt("jj") == 1)
        {
            StartCoroutine(loginUserCallapiFromRegister(emailLoginField.text, passwordLoginField.text));
        }
        else
        {
            StartCoroutine(loginUserCallapi(emailLoginField.text, passwordLoginField.text));
            firebaseDB.instance.refferalcodegenerate();
        }
       
    }

    public void onClickSound()
    {
        if (SoundManager.Instance)
            SoundManager.Instance.onClickSound();
    }

    private IEnumerator waitlogin()
    {
        yield return new WaitForSeconds(5f);
        regbutton.SetActive(true);
        loginbutton.SetActive(true);
        forgetpasswordbutton.SetActive(true);
        loading.SetActive(false);
        infotextlogin.text = "";
        infotextRegister.text = "";





    }

    IEnumerator offloadingandloginpenal()
    {
        yield return new WaitForSeconds(5f);
        UIManager.instance.openPopUpPenal();
    }


    public void backlogin()
    {
        UIManager.instance.LoginScreen();
    }
    public void guestLogin()
    {
        PlayerPrefs.SetString("PlayerName", "Guest user");
        PlayerPrefs.SetInt("Guest", 1);
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_EXPLOSIVE);
        SceneFading.Instance.FadeOutAndLoadScene(StaticValue.SCENE_MENU);

    }

    bool click = true;

    public void showpassword()
    {


        if (click)
        {
            passwordRegisterField.contentType = InputField.ContentType.Standard;

            passwordRegisterVerifyField.contentType = InputField.ContentType.Standard;

            passwordLoginField.contentType = InputField.ContentType.Standard;

            for (int i = 0; i < eyebutton.Length; i++)
            {
                eyebutton[i].GetComponent<Image>().sprite = eyesclose;
            }
            passwordRegisterField.ForceLabelUpdate();
            passwordRegisterVerifyField.ForceLabelUpdate();
            passwordLoginField.ForceLabelUpdate();


            click = false;
        }
        else
        {
            passwordRegisterField.contentType = InputField.ContentType.Password;
            passwordRegisterVerifyField.contentType = InputField.ContentType.Password;
            passwordLoginField.contentType = InputField.ContentType.Password;
            for (int i = 0; i < eyebutton.Length; i++)
            {
                eyebutton[i].GetComponent<Image>().sprite = eyesopen;
            }
            passwordRegisterField.ForceLabelUpdate();
            passwordRegisterVerifyField.ForceLabelUpdate();
            passwordLoginField.ForceLabelUpdate();
            click = true;
        }



    }

}


[System.Serializable]
public class ApiResponseRegister
{
    public bool success;
    public string message;
}
