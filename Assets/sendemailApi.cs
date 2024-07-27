using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;



public class sendemailApi : MonoBehaviour
{
    private static string forgotPass = "auth/forgotPassword/";
    private static string baseUrl = APIHolder.getBaseUrl() + forgotPass;/*"https://a978-72-255-51-32.ngrok-free.app/api/auth/forgotPassword/";*/

    public InputField emailInput; // Reference to the InputField where the user enters their email
    public Text responseText; // Reference to a UI Text element to display the API response


    private void Start()
    {
        this.gameObject.GetComponent<Button>().interactable = true;
    }

    private void OnEnable()
    {
        this.gameObject.GetComponent<Button>().interactable = true;
    }
    public void SendForgotPasswordRequest()
    {
        if (PlayerPrefs.GetInt("Guest") != 1)
        {

            if (emailInput.text.Length > 0)
            {
                this.gameObject.GetComponent<Button>().interactable = false;
                string email = emailInput.text;
                PlayerPrefs.SetString("formail", emailInput.text);
                StartCoroutine(GetRequest(email));
            }
            else
            {
                responseText.text = "Please Enter Email";
            }
        }
        else
        {
            //UnityAdsManager.instance.ShowNonRewardedAd();
        }
    }

    private IEnumerator GetRequest(string email)
    {
        string url = baseUrl + WWW.EscapeURL(email); 

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {

                ApiResponsesendmail response = JsonUtility.FromJson<ApiResponsesendmail>(webRequest.downloadHandler.text);

                // Using the parsed data
                bool isSuccess = response.success;
                string message = response.message;
                
                responseText.text = message;
                this.gameObject.GetComponent<Button>().interactable = true;

            }
            else
            {
                ApiResponsesendmail response = JsonUtility.FromJson<ApiResponsesendmail>(webRequest.downloadHandler.text);

                // Using the parsed data
                bool isSuccess = response.success;
                string message = response.message;
                string responseText = message;
                if (isSuccess == true)
                {
                    PlayerPrefs.SetString("formail", email);
                    this.responseText.text = responseText;
                    StartCoroutine(loadotpScene());
                }

                
            }
        }
    }


    private IEnumerator loadotpScene()
    {
        this.gameObject.GetComponent<Button>().interactable = false;
        yield return new WaitForSeconds(2f);
        emailInput.text = "";
        responseText.text = "";
        this.gameObject.GetComponent<Button>().interactable = true;
        UIManager.instance.penalEnterOtp();
    }
    public void loadLoginscene()
    {
        
        emailInput.text = "";
        responseText.text = "";
        UIManager.instance.LoginScreen();
    }



}

[System.Serializable]
public class ApiResponsesendmail
{
    public bool success;
    public string message;
}
