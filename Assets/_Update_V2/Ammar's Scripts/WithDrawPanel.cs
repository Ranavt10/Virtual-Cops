using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WithDrawPanel : MonoBehaviour
{
    public Button closeBtn;
    public Button[] selectionButtons;
    public GameObject[] panels;

    private static string witdrawRoute = APIHolder.getBaseUrl() + "withdraw";
    public InputField emailInputField;
    public InputField emailWithdrawlAmount;
    public InputField walletInputField;
    public InputField walletWithdrawlAmount;
    public InputField userIdInputField;
    public InputField userIdWithdrawlAmount;
    // Start is called before the first frame update
    void Start()
    {
        closeBtn.onClick.AddListener(() => gameObject.SetActive(false));

        for(int i=0; i< selectionButtons.Length; i++)
        {
            int no = i;
            selectionButtons[no].onClick.AddListener(() => panels[no].SetActive(true));
        }
    }

    public void WithdrawThroughEmail()
    {
        string email = emailInputField.text;
        string amount = emailWithdrawlAmount.text;
        StartCoroutine(setEmailWithdraw("EMAIL", amount, email));
    }

    public void WithdrawThroughWallet()
    {
        string wallet = walletInputField.text;
        string amount = walletWithdrawlAmount.text;
        StartCoroutine(setEmailWithdraw("CRYPTO", amount, wallet));
    }

    public void WithdrawThroughUserId()
    {
        string userId = userIdInputField.text;
        string amount = userIdWithdrawlAmount.text;
        StartCoroutine(setEmailWithdraw("PAYID", amount, userId));
    }


    private IEnumerator setEmailWithdraw(string payType, string vrcValue, string receiverId)
    {
        WWWForm form = new WWWForm();
        form.AddField("payType", payType);
        form.AddField("vrc", vrcValue);
        form.AddField("receiverId", receiverId);

        Debug.Log("PayIdeType Value:" + payType);

        using (UnityWebRequest www = UnityWebRequest.Post(witdrawRoute, form))
        {
            string headerValue = PlayerPrefs.GetString("Token");
            www.SetRequestHeader("Authorization", headerValue);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);

                string jsonResponse = www.downloadHandler.text;
                //Debug.Log("Received Error Response: " + jsonResponse);
                var responseObject = JsonUtility.FromJson<WithdrawErrorResponse>(jsonResponse);
                LobbyManager.Instance.popUpText.text = responseObject.message;
                LobbyManager.Instance.popUpPanel.SetActive(true);
                //Popup.Instance.ShowToastMessage(string.Format("Low Balance..."), ToastLength.Normal);
            }
            else
            {
                Debug.Log("Request sent successfully!");
                string jsonResponse = www.downloadHandler.text;
                Debug.Log("Received: " + jsonResponse);

                // Parse the JSON response
                var responseObject = JsonUtility.FromJson<WithdrawApiResponse>(jsonResponse);

                if (responseObject != null)
                {
                    PlayerPrefs.SetFloat("Meta", responseObject.vrc);

                    LobbyManager.Instance.popUpText.text = responseObject.message;
                    LobbyManager.Instance.popUpPanel.SetActive(true);

                    LobbyManager.Instance.setVrcEconomy();
                    LobbyManager.Instance.FillUserData();
                }
                else
                {
                    Debug.Log("Error parsing JSON response");
                }
            }
        }
    }
}

[Serializable]
public class WithdrawApiResponse
{
    public bool status;
    public string message;
    public float vrc;
}

[Serializable]
public class WithdrawErrorResponse
{
    public bool status;
    public string message;
}
