using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPanel : MonoBehaviour
{
    public Button _StoreBtn;
    public Button _WalletBtn;
    public Button _SettingBtn;
    public Button _WeaponBtn;
    public Button _CharactersBtn;
    public Button _ProfileBtn;
    public Button _StartBtn;
    // Start is called before the first frame update
    void Start()
    {
        _StoreBtn.onClick.AddListener(() => setActivePanel(LobbyManager.Instance.storePanel));
        _WalletBtn.onClick.AddListener(() => setActivePanel(LobbyManager.Instance.walletPanel));
        _WeaponBtn.onClick.AddListener(() => setActivePanel(LobbyManager.Instance.weaponPanel));
        _ProfileBtn.onClick.AddListener(() => setActivePanel(LobbyManager.Instance.profilePanel));
        _StartBtn.onClick.AddListener(() => setActivePanel(LobbyManager.Instance.roomPanel));
        _CharactersBtn.onClick.AddListener(() => setActivePanel(LobbyManager.Instance.charactersPanel));
    }

    private void setActivePanel(GameObject Panel)
    {
        if(Panel != null)
        {
            Panel.SetActive(true);
        }
    }
}
