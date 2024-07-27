using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class swaptoUseAmount : MonoBehaviour
{
    private static string swapCoin = "auth/swapCoin";
    private string swapapi = APIHolder.getBaseUrl() + swapCoin;/*"https://a978-72-255-51-32.ngrok-free.app/api/auth/swapCoin";*/
   
    public InputField amounttext;
    public Text Totalbalance;
    
    public Text messageBox;

    public GameObject transferbtn;
    public GameObject closebtn;
   
    
    private IEnumerator WithdrawAmountApi(string coin)
    {
        // Creating a wwwForm object to send the parameters
        WWWForm form = new WWWForm();

        
        form.AddField("amount", coin);


        // Creating the UnityWebRequest and sending it
        using (UnityWebRequest www = UnityWebRequest.Post(swapapi, form))
        {
            // Adding a custom header with value from PlayerPrefs



            string headerValue = PlayerPrefs.GetString("Token");
            www.SetRequestHeader("Authorization", headerValue);

            // Send the request and wait for the response
            yield return www.SendWebRequest();

            // Check for errors
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                messageBox.text = www.downloadHandler.text;
                transferbtn.SetActive(true);
                closebtn.SetActive(true);

            }
            else
            {
                Debug.Log("Request sent successfully!");
                Debug.Log("Received: " + www.downloadHandler.text);
                messageBox.text = www.downloadHandler.text;
                transferbtn.SetActive(true);
                closebtn.SetActive(true);
               
                amounttext.text = "";
                float balance = PlayerPrefs.GetFloat("withdrawable");
                PlayerPrefs.SetFloat("withdrawable", balance - float.Parse(coin));
                PlayerPrefs.SetFloat("Meta", balance + float.Parse(coin));
            }
        }
    }


    private void Update()
    {
        Totalbalance.text= PlayerPrefs.GetFloat("withdrawable").ToString();
    }
    public void trasferamount()
    {
        if (PlayerPrefs.GetInt("Guest") != 1)
        {
            StartCoroutine(WithdrawAmountApi(amounttext.text));
           
            messageBox.text = "Please Wait For Complete Your Transation...";
        }
        else
        {
            
            Popup.Instance.ShowToastMessage("Login For Withdraw");
        }
    }

}
