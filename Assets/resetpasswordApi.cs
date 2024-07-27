using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class resetpasswordApi : MonoBehaviour
{
    public InputField newpasswordText;
    public InputField ConfirmpasswordText;
    
    public Text responseText;
    public Sprite eyesopen;
    public Sprite eyesclose;
    public Button eyebutton;
    private static string changePassword = "auth/changePassword";
    private string passwordreset = APIHolder.getBaseUrl() + changePassword;/*"https://a978-72-255-51-32.ngrok-free.app/api/auth/changePassword";*/



    public void submitnewpassword()
    {
        if (PlayerPrefs.GetInt("Guest") != 1)
        {
            if (newpasswordText.text.Length > 0)
            {
                 StartCoroutine(ResetPassowordapicall(newpasswordText.text));
            }
            else
            {
            responseText.text = "Enter New Passoword";
            }
        
       }
       else
        {
           // UnityAdsManager.instance.ShowNonRewardedAd();
            Popup.Instance.ShowToastMessage("Register For Resetpassword");
        }
    }

    private IEnumerator ResetPassowordapicall(string password)
    {
        if (newpasswordText.text == ConfirmpasswordText.text)
        {

            WWWForm form = new WWWForm();

            form.AddField("password", password);


        using (UnityWebRequest www = UnityWebRequest.Post(passwordreset, form))
        {
            string headerValue = PlayerPrefs.GetString("fortokan");
            print("tokan is:- " + headerValue);
            www.SetRequestHeader("Authorization", headerValue);


            yield return www.SendWebRequest();


            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                   

                    RestResponse Response = JsonUtility.FromJson<RestResponse>(www.downloadHandler.text);
                    bool isSuccess = Response.success;
                    string message = Response.message;
                    responseText.text = message;
                }
            else
            {
                Debug.Log("Request sent successfully!");
                Debug.Log("Received: " + www.downloadHandler.text);
                    RestResponse Response = JsonUtility.FromJson<RestResponse>(www.downloadHandler.text);
                    bool isSuccess = Response.success;
                    string message = Response.message;
                    if (isSuccess == true)
                    {
                        StartCoroutine(loadLoginscene());
                    }
                    responseText.text = message;
                   
            }
        }
        }
        else
        {
            responseText.text = "Password Not Matched";
           
        }
    }
    
    private IEnumerator loadLoginscene()
    {
        yield return new WaitForSeconds(2f);
        newpasswordText.text = "";
        ConfirmpasswordText.text = "";
        responseText.text = "";
        UIManager.instance.LoginScreen();
    }

    public void loadbackscene()
    {
        newpasswordText.text = "";
        ConfirmpasswordText.text = "";
        responseText.text = "";
        UIManager.instance.LoginScreen();
    }



    bool click = true;

    public void showpassword()
    {


        if (click)
        {
            newpasswordText.contentType = InputField.ContentType.Standard;

            ConfirmpasswordText.contentType = InputField.ContentType.Standard;

            //eyebutton.GetComponent<Image>().sprite = eyesclose;

            newpasswordText.ForceLabelUpdate();
            ConfirmpasswordText.ForceLabelUpdate();
           

            click = false;
        }
        else
        {
            newpasswordText.contentType = InputField.ContentType.Password;
            ConfirmpasswordText.contentType = InputField.ContentType.Password;
            eyebutton.GetComponent<Image>().sprite = eyesopen;
            newpasswordText.ForceLabelUpdate();
            ConfirmpasswordText.ForceLabelUpdate();
            
            click = true;
        }



    }
}

public class RestResponse
{
    public bool success;
    public string message;
}
