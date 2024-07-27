using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StoreGunPanel : MonoBehaviour
{
    public string gunsType;
    public gunType _GunType;

    public WeaponSystemHandlerStore[] PistolWeaponSystemHandler;

    public Image WeaponShowpieceImage;
    private int lastSelectedNo = 0;

    public TextMeshProUGUI gunNameText;
    public TextMeshProUGUI gunDescriptionText;

    public Button purchaseBtn;
    public int HaveToPurchasedGun;

    private static string buyGunsAPI = "auth/purchaseWeapon";
    private static string purchaseGuns = APIHolder.getBaseUrl() + buyGunsAPI;

    private void Awake()
    {
        PlayerPrefs.SetInt("Weapon_" + 0, 1);
        PlayerPrefs.SetInt("Weapon_" + 1, 1);
    }

    private void OnEnable()
    {
        LobbyManager.Instance.Guns_Holder.SetActive(true);
        setWeaponsPrice();
        lastSelectedNo = PlayerPrefs.GetInt("StoreGuns");
        setSelectedGun(lastSelectedNo);
    }

    public void setWeaponsPrice()
    {
        Debug.Log("Length of Gun Details:" + LobbyManager.Instance.weaponDetails.Count);
        for(int i= 2; i < PistolWeaponSystemHandler.Length; i++)
        {
            PistolWeaponSystemHandler[i].priceHandlers.weaponPrice.text = LobbyManager.Instance.weaponDetails[i - 2].price.ToString();
        }
    }

    public void setSelectedGun(int no)
    {
        for (int i = 0; i < PistolWeaponSystemHandler.Length; i++)
        {
            PistolWeaponSystemHandler[i].HighlightedImage.enabled = false;
            PistolWeaponSystemHandler[i].Gun.SetActive(false);
            int index = i;

            if(PlayerPrefs.GetInt("Weapon_" + index, 0) == 1)
            {
                PistolWeaponSystemHandler[index].priceHandlers.parentPrice.SetActive(false);
            }
            else
            {
                PistolWeaponSystemHandler[index].priceHandlers.parentPrice.SetActive(true);
            }
        }

        if (PlayerPrefs.GetInt("Weapon_" + no, 0) == 1)
        {
            purchaseBtn.gameObject.SetActive(false);
            PlayerPrefs.SetInt("StoreGuns", no);

            lastSelectedNo = no;

            Debug.Log("Selected Gun is:" + no);
        }
        else
        {
            purchaseBtn.gameObject.SetActive(true);
        }

        gunNameText.text = PistolWeaponSystemHandler[no].gunName;
        gunDescriptionText.text = PistolWeaponSystemHandler[no].gunDescription;

        PistolWeaponSystemHandler[no].Gun.SetActive(true);
        PistolWeaponSystemHandler[no].HighlightedImage.enabled = true;

        WeaponShowpieceImage.sprite = PistolWeaponSystemHandler[no].showPieceImage;
        WeaponShowpieceImage.SetNativeSize();

        HaveToPurchasedGun = no;
    }

    public void PurchaseGun()
    {
        string purchaseItem = HaveToPurchasedGun.ToString();
        StartCoroutine(setGunsOnBought(purchaseItem));
    }


    private IEnumerator setGunsOnBought(string weaponId)
    {
        WWWForm form = new WWWForm();
        form.AddField("weaponId", weaponId);

        Debug.Log("WeaponId Value:" + weaponId);

        using (UnityWebRequest www = UnityWebRequest.Post(purchaseGuns, form))
        {
            string headerValue = PlayerPrefs.GetString("Token");
            www.SetRequestHeader("Authorization", headerValue);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                string jsonResponse = www.downloadHandler.text;
                //Debug.Log("Received Error Response: " + jsonResponse);
                var responseObject = JsonUtility.FromJson<GunsErrorResponse>(jsonResponse);
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
                var responseObject = JsonUtility.FromJson<buyGunsSchema>(jsonResponse);

                if (responseObject != null)
                {
                    Debug.Log("Message: " + responseObject.message);
                    Debug.Log("Value: " + responseObject.status);
                    Debug.Log("Gems are:" + responseObject.gems);

                    PlayerPrefs.SetFloat("gems", responseObject.gems);

                    for (int i = 0; i < responseObject.weapons.Length; i++)
                    {
                        string weaponValue = responseObject.weapons[i].ToString();
                        Debug.Log("Weapon Val is:" + weaponValue);
                        string weaponPrefKey = "Weapon_" + weaponValue; // Customize the prefix as per your need
                        PlayerPrefs.SetInt(weaponPrefKey, 1);
                    }

                    setSelectedGun(int.Parse(weaponId));
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
        if (LobbyManager.Instance)
        {
            LobbyManager.Instance.Guns_Holder.SetActive(false);
        }
    }
}

[Serializable]
public class WeaponSystemHandlerStore
{
    public Image HighlightedImage;
    public Sprite showPieceImage;
    public Button clickAbleBtn;
    public int gunNo;
    public PriceHandlerWeapons priceHandlers;
    public GameObject Gun;
    public string gunName;
    public string gunDescription;
}

[Serializable]
public class PriceHandlerWeapons
{
    public GameObject parentPrice;
    public TextMeshProUGUI weaponPrice;
}

[Serializable]
public class buyGunsSchema
{
    public bool status;
    public string message;
    public int gems;
    public int[] weapons;
}

[Serializable]
public class GunsErrorResponse
{
    public bool status;
    public string message;
}

