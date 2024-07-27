using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CreateWalletApi : MonoBehaviour
{
    private static string createWallet = "auth/createWallet";
    private string CreatewalletAddressnew = APIHolder.getBaseUrl() + createWallet; /*"https://a978-72-255-51-32.ngrok-free.app/api/auth/createWallet";*/
    public Text responseText;
    public GameObject DepositPenal;

    public void CRWalletaddress()
    {
        StartCoroutine(CreatedWalletAddress());
    }
    IEnumerator CreatedWalletAddress()
    {
        UnityWebRequest request = UnityWebRequest.PostWwwForm(CreatewalletAddressnew, "");

        string myHeaderValue = PlayerPrefs.GetString("Token");
        
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

                DepositPenal.SetActive(true);
                this.gameObject.SetActive(false);
        }
    }
}
