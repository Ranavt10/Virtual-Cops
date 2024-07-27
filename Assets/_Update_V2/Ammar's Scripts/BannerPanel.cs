using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System;

public class BannerPanel : MonoBehaviour
{
    public static string bannerSelected = "bannerSelected";

    public ProfilePanel profilePanel;

    public UpperBar upperBar;

    public Sprite[] bannerSprites;

    private int lastSelectedBannerNo = 0;

    public Button purchaseBtn;

    public int HaveToPurchaseBanner;

    public GameObject PricingBar;

    public TextMeshProUGUI priceText;

    public static string purchaseBanner = APIHolder.getBaseUrl() + "auth/purchaseBanner";
    // Start is called before the first frame update
    private void OnEnable()
    {
        LobbyManager.Instance.setVRC();
        PricingBar.SetActive(true);
        LogoBanner.Instance.LogoPanel.SetActive(false);

        LobbyManager.Instance.lastSelectedBanner = PlayerPrefs.GetInt(bannerSelected);

        setSelectedBanner(LobbyManager.Instance.lastSelectedBanner);
    }

    public void setSelectedBanner(int no)
    {
        upperBar.setUpperBar();
        LogoBanner.Instance.bannerPurchaseBtn.gameObject.SetActive(true);
        LogoBanner.Instance.showCaseBannerImage.gameObject.SetActive(true);

        priceText.text = LobbyManager.Instance.bannerDetails[no].price.ToString();

        if (PlayerPrefs.GetInt("Banner_" + no, 0) == 1)
        {
            PricingBar.SetActive(false);
            purchaseBtn.gameObject.SetActive(false);
            PlayerPrefs.SetInt(bannerSelected, no);

            LobbyManager.Instance.lastSelectedBanner = no; 
            LobbyManager.Instance.SideBarBannerImg.sprite = LobbyManager.Instance.BannersForSideBar[LobbyManager.Instance.lastSelectedBanner];
            profilePanel.bannerImage.sprite = profilePanel.banners[LobbyManager.Instance.lastSelectedBanner];
        }
        else
        {
            PricingBar.SetActive(true);
            purchaseBtn.gameObject.SetActive(true);
        }

        LogoBanner.Instance.showCaseBannerImage.sprite = bannerSprites[no];

        HaveToPurchaseBanner = no;
    }

    public void PurchaseBanner()
    {
        string purchaseItem = HaveToPurchaseBanner.ToString();
        StartCoroutine(setBannersOnBought(purchaseItem));
    }


    private IEnumerator setBannersOnBought(string bannerId)
    {
        WWWForm form = new WWWForm();
        form.AddField("bannerId", bannerId);

        Debug.Log("WeaponId Value:" + bannerId);

        using (UnityWebRequest www = UnityWebRequest.Post(purchaseBanner, form))
        {
            string headerValue = PlayerPrefs.GetString("Token");
            www.SetRequestHeader("Authorization", headerValue);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                string jsonResponse = www.downloadHandler.text;
                //Debug.Log("Received Error Response: " + jsonResponse);
                var responseObject = JsonUtility.FromJson<BannerErrorResponse>(jsonResponse);
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
                var responseObject = JsonUtility.FromJson<buyBannersSchema>(jsonResponse);

                if (responseObject != null)
                {
                    Debug.Log("Message: " + responseObject.message);
                    Debug.Log("Value: " + responseObject.status);
                    Debug.Log("Gems are:" + responseObject.gems);

                    PlayerPrefs.SetFloat("gems", responseObject.gems);

                    for (int i = 0; i < responseObject.banners.Length; i++)
                    {
                        string weaponValue = responseObject.banners[i].ToString();
                        Debug.Log("Banner Val is:" + weaponValue);
                        string weaponPrefKey = "Banner_" + weaponValue; // Customize the prefix as per your need
                        PlayerPrefs.SetInt(weaponPrefKey, 1);
                    }

                    setSelectedBanner(int.Parse(bannerId));
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

    private void OnDisable()
    {
        if (LogoBanner.Instance)
        {
            LogoBanner.Instance.bannerPurchaseBtn.gameObject.SetActive(false);
            LogoBanner.Instance.showCaseBannerImage.gameObject.SetActive(false);
        }
    }
}

[Serializable]
public class buyBannersSchema
{
    public bool status;
    public string message;
    public int gems;
    public int[] banners;
}

[Serializable]
public class BannerErrorResponse
{
    public bool status;
    public string message;
}
