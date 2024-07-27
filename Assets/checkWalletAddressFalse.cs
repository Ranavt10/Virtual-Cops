using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class checkWalletAddressFalse : MonoBehaviour
{
    private static string authSmartWallet = "auth/smartWallet/";
    private string checkWalletApi = APIHolder.getBaseUrl() + authSmartWallet;/*"https://a978-72-255-51-32.ngrok-free.app/api/auth/smartWallet/";*/
    public Text responseText;
    public GameObject createwalletpenal;
    public Text WalletAddress;
    public Dropdown dropdown;

    private void Start()
    {
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }
    private void OnEnable()
    {
        StartCoroutine(CheckWalletIsCreated());
    }

    IEnumerator CheckWalletIsCreated()
    {
        UnityWebRequest request = UnityWebRequest.Get(checkWalletApi);

        string myHeaderValue = PlayerPrefs.GetString("Token");
        print(myHeaderValue);
        request.SetRequestHeader("Authorization", myHeaderValue);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            string responseT = request.downloadHandler.text;
            print(responseT);
          
            yield return new WaitForSeconds(2f);
           if(responseT == "User smart wallet not exist")
            {
                createwalletpenal.SetActive(true);
                this.gameObject.SetActive(false);
            }
            else
            {
                WalletAddress.text = responseT;
                PlayerPrefs.SetString("Walletaddress", responseT);
            }

            
        }
    }

    private void OnDropdownValueChanged(int index)
    {
        // You can perform actions based on the selected value
        Debug.Log("Dropdown value changed to: " + dropdown.options[index].text);

        if(dropdown.options[index].text== "Crypto Wallet Address")
        {
            WalletAddress.text = PlayerPrefs.GetString("Walletaddress");
        }
        else if(dropdown.options[index].text== "Email")
        {
            WalletAddress.text = PlayerPrefs.GetString("userEmail");
        }
        else if(dropdown.options[index].text== "Pay ID")
        {
            WalletAddress.text = PlayerPrefs.GetString("UserId");
        }
    }

    public void CopyText()
    {
        if (WalletAddress != null && !string.IsNullOrEmpty(WalletAddress.text))
        {
            GUIUtility.systemCopyBuffer = WalletAddress.text;
            Debug.Log("Text copied to clipboard: " + WalletAddress.text);
        }
        else
        {
            Debug.LogWarning("Input field is null or empty. Cannot copy text.");
        }
    }
}
