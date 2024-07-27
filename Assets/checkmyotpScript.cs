using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

public class checkmyotpScript : MonoBehaviour
{
    public InputField otpinputText;
    public Text mailtext;
    public Text responseText;

    private static string verifyPass = "auth/verifyForgotPasswordOtp";

    private string apiUrl = APIHolder.getBaseUrl() + verifyPass;/*"https://a978-72-255-51-32.ngrok-free.app/api/auth/verifyForgotPasswordOtp";*/  
    

    private void OnEnable()
    {
        mailtext.text = PlayerPrefs.GetString("formail");
    }
    public void VerifyForgotPasswordOtp()
    {
        if (PlayerPrefs.GetInt("Guest") != 1)
        {


            StartCoroutine(VerifyForgotPasswordOtpCoroutine(mailtext.text, otpinputText.text));
        }

        else
        {
            //UnityAdsManager.instance.ShowNonRewardedAd();
            Popup.Instance.ShowToastMessage("Login For This");
        }
    }
    [System.Serializable]
    public class RequestData
    {
        public string email;
        public string otp;
    }

    private IEnumerator VerifyForgotPasswordOtpCoroutine(string emailValue, string otpValue)
    {
        RequestData data = new RequestData
        {
            email = emailValue,
            otp = otpValue
        };

        // Convert to JSON string
        string json = JsonUtility.ToJson(data);

        Debug.Log("Sending JSON: " + json);  // To verify the JSON format

        using (UnityWebRequest www = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                
                ServerResponse serverResponse = JsonUtility.FromJson<ServerResponse>(www.downloadHandler.text);
                responseText.text = serverResponse.message;

            }
            else
            {
                if (www.isHttpError || www.isNetworkError)
                {
                    ServerResponse serverResponse = JsonUtility.FromJson<ServerResponse>(www.downloadHandler.text);
                    responseText.text = serverResponse.message;
                }
                else
                {
                    Debug.Log("API Response: " + www.downloadHandler.text);

                    
                    ServerResponse serverResponse = JsonUtility.FromJson<ServerResponse>(www.downloadHandler.text);
                    if (serverResponse != null && serverResponse.success)
                    {
                        Debug.Log("Received token: " + serverResponse.token);
                        PlayerPrefs.SetString("fortokan", serverResponse.token);
                        responseText.text = serverResponse.message;
                        StartCoroutine(loadResetpasswordScene());
                    }
                }
            }
        }
    }



    private IEnumerator loadResetpasswordScene()
    {
        yield return new WaitForSeconds(2f);
        otpinputText.text = "";
        responseText.text = "";
        UIManager.instance.penalRestPasswordui();
    }
    public void loadLoginscene()
    {
       
        otpinputText.text = "";
        responseText.text = "";
        UIManager.instance.enterEmailUI();
    }
}
[System.Serializable]
public class ServerResponse
{
    public bool success;
    public string token;
    public string message;
}