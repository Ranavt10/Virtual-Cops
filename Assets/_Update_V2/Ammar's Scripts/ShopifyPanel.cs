using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;

public class ShopifyPanel : MonoBehaviour
{
    public List<TextMeshProUGUI> gemsPackage;
    public List<TextMeshProUGUI> vrcPrice;

    public List<gemsVrcInformation> gemsDetail = new List<gemsVrcInformation>();

    private static string gemsPackage1 = "gemsPackage";
    private static string getGemsPackage = APIHolder.getBaseUrl() + gemsPackage1;

    private static string getGemsRoute = "auth/purchasegems";
    private static string getUpdatedGemsRoute = APIHolder.getBaseUrl() + getGemsRoute;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        setPrices();
    }

    public void setPrices()
    {
        for (int i = 0; i < gemsPackage.Count; i++)
        {
            vrcPrice[i].text = gemsDetail[i].price.ToString() + " VRC";
            gemsPackage[i].text = gemsDetail[i].gems.ToString();
        }
    }

    private IEnumerator setGemsOnBought(string packageId)
    {
        WWWForm form = new WWWForm();
        form.AddField("pacakgeId", packageId);

        Debug.Log("Gems Value:" + packageId);

        using (UnityWebRequest www = UnityWebRequest.Post(getUpdatedGemsRoute, form))
        {
            string headerValue = PlayerPrefs.GetString("Token");
            www.SetRequestHeader("Authorization", headerValue);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                string jsonResponse = www.downloadHandler.text;
                //Debug.Log("Received Error Response: " + jsonResponse);
                var responseObject = JsonUtility.FromJson<ShopErrorResponse>(jsonResponse);
                LobbyManager.Instance.popUpText.text = responseObject.message;
                LobbyManager.Instance.popUpPanel.SetActive(true);
                /*Popup.Instance.ShowToastMessage(string.Format("Low Balance..."), ToastLength.Normal);*/
            }
            else
            {
                Debug.Log("Request sent successfully!");
                string jsonResponse = www.downloadHandler.text;
                Debug.Log("Received: " + jsonResponse);

                // Parse the JSON response
                var responseObject = JsonUtility.FromJson<gemShopifyIAPBoughtResponse>(jsonResponse);

                if (responseObject != null)
                {
                    Debug.Log("Message: " + responseObject.message);
                    Debug.Log("Value: " + responseObject.status);
                    Debug.Log("Gems are:" + responseObject.gems);
                    Debug.Log("Vrc are:" + responseObject.vrc);

                    PlayerPrefs.SetFloat("gems", responseObject.gems);
                    PlayerPrefs.SetFloat("Meta", responseObject.vrc);

                    LobbyManager.Instance.GemsEarnedSound();

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

    public void BuyGems(int id)
    {
        if (PlayerPrefs.GetInt("Guest") != 1)
        {
            StartCoroutine(setGemsOnBought(id.ToString()));
        }
        else
        {
            // UnityAdsManager.instance.ShowNonRewardedAd();
            Popup.Instance.ShowToastMessage(string.Format("Login for Purchase Items..."), ToastLength.Normal);
            return;
        }
    }
}

[Serializable]
public class gemsVrcInformation
{
    public int id;
    public string name;
    public int gems;
    public float price;
}

[Serializable]
public class gemsVRCAPIResponse
{
    public List<gemsVrcInformation> gemsPackage;
}

[Serializable]
public class gemShopifyIAPBoughtResponse
{
    public bool status;
    public string message;
    public float gems;
    public float vrc;
}

[Serializable]
public class ShopErrorResponse
{
    public bool status;
    public string message;
}
