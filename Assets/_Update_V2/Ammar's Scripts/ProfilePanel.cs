using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ProfilePanel : MonoBehaviour
{
    public string changNameRoute = APIHolder.getBaseUrl() + "auth/updateName";
    public GameObject PriceHolder;
    public Button LogoBtn;
    public Button BannerBtn;

    public Button closeBtn;

    private bool avatarSet = false;
    private bool bannerSet = false;

    public List<Sprite> Icons;
    public List<Sprite> banners;

    public Image iconImage;
    public Image bannerImage;

    public InputField userNameField;
    public InputField userIdField;

    private string userIdCopy;

    private void OnEnable()
    {
        LobbyManager.Instance.setVRC();
        string email = PlayerPrefs.GetString("PlayerName");
        userNameField.text = email;
        string userId = PlayerPrefs.GetString("UserId");
        userIdField.text = userId;
        userIdCopy = userId;

        userNameField.interactable = false;
        userIdField.interactable = false;
        LobbyManager.Instance.lastSelectedAvatar = PlayerPrefs.GetInt("avatarSelected");
        LobbyManager.Instance.lastSelectedBanner = PlayerPrefs.GetInt("bannerSelected");
        PriceHolder.SetActive(false);
        iconImage.sprite = Icons[LobbyManager.Instance.lastSelectedAvatar];
        bannerImage.sprite = banners[LobbyManager.Instance.lastSelectedBanner];
    }
    // Start is called before the first frame update
    void Start()
    {
        closeBtn.onClick.AddListener(() => setThisPanel());
    }

    public void setAvatarPanel()
    {
        if (!avatarSet)
        {
            avatarSet = true;
            bannerSet = false;
            LogoBanner.Instance.LogoPanel.SetActive(true);
        }
        else
        {
            avatarSet = false;
            bannerSet = false;
            LogoBanner.Instance.LogoPanel.SetActive(false);
        }
    }

    public void setBannerPanel()
    {
        if (!bannerSet)
        {
            bannerSet = true;
            avatarSet = false;
            LogoBanner.Instance.BannerPanel.SetActive(true);
        }
        else
        {
            bannerSet = false;
            avatarSet = false;
            LogoBanner.Instance.BannerPanel.SetActive(false);
        }
    }

    private void setThisPanel()
    {
        this.gameObject.SetActive(false);
    }

    public void copyUserId()
    {
        GUIUtility.systemCopyBuffer = userIdCopy;
    }

    private IEnumerator setUserName(string username)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);

        Debug.Log("username Value:" + username);

        using (UnityWebRequest www = UnityWebRequest.Put(changNameRoute, form.data))
        {
            string headerValue = PlayerPrefs.GetString("Token");
            www.SetRequestHeader("Authorization", headerValue);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                string jsonResponse = www.downloadHandler.text;
                //Debug.Log("Received Error Response: " + jsonResponse);
                var responseObject = JsonUtility.FromJson<CharactersErrorResponse>(jsonResponse);
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
                var responseObject = JsonUtility.FromJson<UpdateUserName>(jsonResponse);

                if (responseObject != null)
                {
                    PlayerPrefs.SetString("PlayerName", responseObject.username);
                    LobbyManager.Instance.userName.text = responseObject.username;
                    //setCharacters(int.Parse(characterId));
                    /*LobbyManager.Instance.GemsEarnedSound();
                    LobbyManager.Instance.popUpText.text = responseObject.message;
                    LobbyManager.Instance.popUpPanel.SetActive(true);*/
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
public class UpdateUserName
{
    public string username;
}
