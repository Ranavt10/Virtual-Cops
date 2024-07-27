using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UpperBar : MonoBehaviour
{
    
    private void OnEnable()
    {
        LobbyManager.Instance.lastSelectedAvatar = PlayerPrefs.GetInt("avatarSelected");
        LobbyManager.Instance.lastSelectedBanner = PlayerPrefs.GetInt("bannerSelected");
    }
    // Start is called before the first frame update
    void Start()
    {
        LobbyManager.Instance.SideBarAvatarImg.sprite = LobbyManager.Instance.AvatarsForSideBar[LobbyManager.Instance.lastSelectedAvatar];
        LobbyManager.Instance.SideBarBannerImg.sprite = LobbyManager.Instance.BannersForSideBar[LobbyManager.Instance.lastSelectedBanner];
    }

    public void setUpperBar()
    {
        LobbyManager.Instance.SideBarAvatarImg.sprite = LobbyManager.Instance.AvatarsForSideBar[LobbyManager.Instance.lastSelectedAvatar];
        LobbyManager.Instance.SideBarBannerImg.sprite = LobbyManager.Instance.BannersForSideBar[LobbyManager.Instance.lastSelectedBanner];
    }
}
