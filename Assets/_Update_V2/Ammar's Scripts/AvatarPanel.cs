using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System;

public class AvatarPanel : MonoBehaviour
{
    public static string avatarSelected = "avatarSelected";

    public ProfilePanel profilePanel;

    public UpperBar upperBar;

    public Button purchaseBtn;

    public Sprite[] avatarSprites;

    private int lastSelectedAvatarNo = 0;

    public int HaveToPurchaseAvatar;

    public GameObject PricingBar;

    public TextMeshProUGUI priceText;

    public static string purchaseAvatar = APIHolder.getBaseUrl() + "auth/purchaseAvatar";

    // Start is called before the first frame update
    private void OnEnable()
    {
        LobbyManager.Instance.setVRC();
        PricingBar.SetActive(true);
        LogoBanner.Instance.BannerPanel.SetActive(false);

        LobbyManager.Instance.lastSelectedAvatar = PlayerPrefs.GetInt(avatarSelected);

        setAvatar(LobbyManager.Instance.lastSelectedAvatar);
    }

    public void setAvatar(int no)
    {
        upperBar.setUpperBar();
        LogoBanner.Instance.showCaseAvatarImage.gameObject.SetActive(true);
        LogoBanner.Instance.avatarPurchaseBtn.gameObject.SetActive(true);

        priceText.text = LobbyManager.Instance.avatarDetails[no].price.ToString();

        if (PlayerPrefs.GetInt("Avatar_" + no, 0) == 1)
        {
            PricingBar.SetActive(false);
            purchaseBtn.gameObject.SetActive(false);
            PlayerPrefs.SetInt(avatarSelected, no);

            LobbyManager.Instance.lastSelectedAvatar = no;
            LobbyManager.Instance.SideBarAvatarImg.sprite = LobbyManager.Instance.AvatarsForSideBar[LobbyManager.Instance.lastSelectedAvatar];
            profilePanel.iconImage.sprite = profilePanel.Icons[LobbyManager.Instance.lastSelectedAvatar];
        }
        else
        {
            PricingBar.SetActive(true);
            purchaseBtn.gameObject.SetActive(true);
        }
        LogoBanner.Instance.showCaseAvatarImage.sprite = avatarSprites[no];

        HaveToPurchaseAvatar = no;
    }

    public void PurchaseAvatar()
    {
        string purchaseItem = HaveToPurchaseAvatar.ToString();
        StartCoroutine(setBannersOnBought(purchaseItem));
    }


    private IEnumerator setBannersOnBought(string bannerId)
    {
        WWWForm form = new WWWForm();
        form.AddField("avatarId", bannerId);

        Debug.Log("WeaponId Value:" + bannerId);

        using (UnityWebRequest www = UnityWebRequest.Post(purchaseAvatar, form))
        {
            string headerValue = PlayerPrefs.GetString("Token");
            www.SetRequestHeader("Authorization", headerValue);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                string jsonResponse = www.downloadHandler.text;
                //Debug.Log("Received Error Response: " + jsonResponse);
                var responseObject = JsonUtility.FromJson<AvatarErrorResponse>(jsonResponse);
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
                var responseObject = JsonUtility.FromJson<buyAvatarsSchema>(jsonResponse);

                if (responseObject != null)
                {
                    Debug.Log("Message: " + responseObject.message);
                    Debug.Log("Value: " + responseObject.status);
                    Debug.Log("Gems are:" + responseObject.gems);

                    PlayerPrefs.SetFloat("gems", responseObject.gems);

                    for (int i = 0; i < responseObject.avatars.Length; i++)
                    {
                        string weaponValue = responseObject.avatars[i].ToString();
                        Debug.Log("Avatar Val is:" + weaponValue);
                        string weaponPrefKey = "Avatar_" + weaponValue; // Customize the prefix as per your need
                        PlayerPrefs.SetInt(weaponPrefKey, 1);
                    }

                    setAvatar(int.Parse(bannerId));

                    LobbyManager.Instance.popUpText.text = responseObject.message;
                    LobbyManager.Instance.popUpPanel.SetActive(true);
                    LobbyManager.Instance.GemsEarnedSound();

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
        LogoBanner.Instance.showCaseAvatarImage.gameObject.SetActive(false);
        LogoBanner.Instance.avatarPurchaseBtn.gameObject.SetActive(false);
    }
}

[Serializable]
public class buyAvatarsSchema
{
    public bool status;
    public string message;
    public int gems;
    public int[] avatars;
}

[Serializable]
public class AvatarErrorResponse
{
    public bool status;
    public string message;
}
