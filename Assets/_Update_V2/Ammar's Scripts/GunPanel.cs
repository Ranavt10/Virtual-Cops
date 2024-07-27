using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GunPanel : MonoBehaviour
{
    public string gunsType;
    public gunType _GunType;

    public WeaponSystemHandler[] PistolWeaponSystemHandler;

    public TextMeshProUGUI gunNameText;
    public Image WeaponShowpieceImage;
    public Image WeaponPanelDamageFiller;
    public Image WeaponPanelDistanceFiller;
    private int lastSelectedNo = 0;
    
    private void OnEnable()
    {
        lastSelectedNo = PlayerPrefs.GetInt(gunsType);
        setSelectedGun(lastSelectedNo);
    }

    public void setSelectedGun(int no)
    {
        for (int i = 0; i < PistolWeaponSystemHandler.Length; i++)
        {
            PistolWeaponSystemHandler[i].HighlightedImage.enabled = false;
            PistolWeaponSystemHandler[i].Gun.SetActive(false);
        }

        PistolWeaponSystemHandler[no].Gun.SetActive(true);
        PistolWeaponSystemHandler[no].HighlightedImage.enabled = true;

        WeaponShowpieceImage.sprite = PistolWeaponSystemHandler[no].showPieceImage;
        WeaponShowpieceImage.SetNativeSize();

        /*gunNameText.text = LobbyManager.Instance.totalWeaponDetails[no].weaponName;
        WeaponPanelDamageFiller.fillAmount = LobbyManager.Instance.totalWeaponDetails[no].Damage;
        WeaponPanelDistanceFiller.fillAmount = LobbyManager.Instance.totalWeaponDetails[no].Distance;*/

        if (_GunType == gunType.Pistol)
        {
            gunNameText.text = LobbyManager.Instance.PistolWeaponDetails[no].weaponName;
            WeaponPanelDamageFiller.fillAmount = LobbyManager.Instance.PistolWeaponDetails[no].Damage;
            WeaponPanelDistanceFiller.fillAmount = LobbyManager.Instance.PistolWeaponDetails[no].Distance;
        }
        else if (_GunType == gunType.AR)
        {
            gunNameText.text = LobbyManager.Instance.ARWeaponDetails[no].weaponName;
            WeaponPanelDamageFiller.fillAmount = LobbyManager.Instance.ARWeaponDetails[no].Damage;
            WeaponPanelDistanceFiller.fillAmount = LobbyManager.Instance.ARWeaponDetails[no].Distance;
        }
        else if (_GunType == gunType.Sniper)
        {
            gunNameText.text = LobbyManager.Instance.SniperWeaponDetails[no].weaponName;
            WeaponPanelDamageFiller.fillAmount = LobbyManager.Instance.SniperWeaponDetails[no].Damage;
            WeaponPanelDistanceFiller.fillAmount = LobbyManager.Instance.SniperWeaponDetails[no].Distance;
        }
        else if (_GunType == gunType.Shotgun)
        {
            gunNameText.text = LobbyManager.Instance.ShotGunWeaponDetails[no].weaponName;
            WeaponPanelDamageFiller.fillAmount = LobbyManager.Instance.ShotGunWeaponDetails[no].Damage;
            WeaponPanelDistanceFiller.fillAmount = LobbyManager.Instance.ShotGunWeaponDetails[no].Distance;
        }
        else if (_GunType == gunType.Smg)
        {
            gunNameText.text = LobbyManager.Instance.SMGWeaponDetails[no].weaponName;
            WeaponPanelDamageFiller.fillAmount = LobbyManager.Instance.SMGWeaponDetails[no].Damage;
            WeaponPanelDistanceFiller.fillAmount = LobbyManager.Instance.SMGWeaponDetails[no].Distance;
        }

        PlayerPrefs.SetInt(gunsType, no);
    }
}

[Serializable]
public enum gunType
{
    Pistol,
    AR,
    Sniper,
    Shotgun,
    Smg
}

[Serializable]
public class WeaponSystemHandler
{
    public Image HighlightedImage;
    public Sprite showPieceImage;
    public Button clickAbleBtn;
    public int gunNo;
    public GameObject Gun;
}
