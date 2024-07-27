using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LobbyManager : Singleton<LobbyManager>
{
    public GameObject InsideRoomPanel;
    public static string playerSelected = "PlayerSelectedForLobby";
    public GameObject Characters_Holder;

    public Image SideBarAvatarImg;
    public Sprite[] AvatarsForSideBar;
    public int lastSelectedAvatar;

    public Image SideBarBannerImg;
    public Sprite[] BannersForSideBar;
    public int lastSelectedBanner;

    [SerializeField]
    private NetworkManager networkManager;
    [SerializeField]
    private characterselection characterSelection;
    public WeaponsDetails[] totalWeaponDetails;
    public WeaponsDetails[] PistolWeaponDetails;
    public WeaponsDetails[] ARWeaponDetails;
    public WeaponsDetails[] SniperWeaponDetails;
    public WeaponsDetails[] ShotGunWeaponDetails;
    public WeaponsDetails[] SMGWeaponDetails;
    public GameObject storePanel;
    public GameObject weaponPanel;
    public GameObject profilePanel;
    public GameObject roomPanel;
    public GameObject createRoomPanel;
    public GameObject walletPanel;
    public GameObject depositPanel;
    public GameObject charactersPanel;
    public TextMeshProUGUI vrcCoinsText;
    public TextMeshProUGUI userName;
    public TextMeshProUGUI gemCoin;

    private int selectedPlayer = 0;

    [SerializeField]
    private Image[] selectedRoomPersonsList;
    [SerializeField]
    private Image[] unSelectedRoomPersonsList;

    public GameObject[] PlayersSelectedForLobby;

    public StoreGunPanel storeGunPanel;

    public GameObject Guns_Holder;

    public ShopifyPanel shopifyPanel;

    private static string weaponsAPI = "weapons";

    private static string weaponsFetch = APIHolder.getBaseUrl() + weaponsAPI;

    private static string charactersAPI = "characterPackage";
    private static string charactersFetch = APIHolder.getBaseUrl() + charactersAPI;

    private static string gemsPackage1 = "gemsPackage";
    private static string getGemsPackage = APIHolder.getBaseUrl() + gemsPackage1;

    private static string getBannerAPI = APIHolder.getBaseUrl() + "bannerPackage";

    private static string getAvatarAPI = APIHolder.getBaseUrl() + "avatarPackage";

    private static readonly string GetVrcEndpoint = APIHolder.getBaseUrl() + "getVrc";

    public List<WeaponInformation> weaponDetails = new List<WeaponInformation>();
    public List<CharactersInformation> characterDetails = new List<CharactersInformation>();
    public List<AvatarInformation> avatarDetails = new List<AvatarInformation>();
    public List<BannerInformation> bannerDetails = new List<BannerInformation>();

    public AudioSource mainAudioSource;
    public AudioClip gunClickSound;
    public AudioClip weaponClickSound;
    public AudioClip buySound;
    public AudioClip clickClip;
    public AudioClip gemsEarnedClip;

    public GameObject popUpPanel;
    public TextMeshProUGUI popUpText;

    private SoundManager soundObj;

    private APIHolder aPIHolder;

    public TextMeshProUGUI totalUsersToBeShownInRoom;
    private void Awake()
    {
        var Manager = GameObject.FindObjectsOfType<LobbyManager>();

        if (Manager.Length > 1)
        {
            Destroy(this.gameObject);
        }

        if (FindObjectOfType<APIHolder>() != null)
            soundObj = FindObjectOfType<APIHolder>().soundManager;

        if (soundObj != null)
            soundObj.gameObject.SetActive(true);

        mainAudioSource.volume = ProfileManager.UserProfile.soundVolume;

        if(FindObjectOfType<APIHolder>())
            aPIHolder = FindObjectOfType<APIHolder>();

        PlayerPrefs.SetInt("Banner_" + 0, 1);
        PlayerPrefs.SetInt("Avatar_" + 0, 1);

        StartCoroutine(getWeaponsListAPI());

        StartCoroutine(getCharactersLisAPI());

        StartCoroutine(getGemsPackageFromAPI());

        StartCoroutine(getAvatarsLisAPI());

        StartCoroutine(getBannersLisAPI());
    }

    private void OnEnable()
    {
        Invoke(nameof(setCharactersSetup), 0.1f);

        selectedPlayer = PlayerPrefs.GetInt(playerSelected);

        Characters_Holder.SetActive(true);

        setCharacters(selectedPlayer);
    }

    public void setCharacters(int no)
    {
        for (int i = 0; i < PlayersSelectedForLobby.Length; i++)
        {
            PlayersSelectedForLobby[i].SetActive(false);
        }

        PlayersSelectedForLobby[no].SetActive(true);

        PlayerPrefs.SetInt(playerSelected, no);

        selectedPlayer = no;
    }

    private void setCharactersSetup()
    {
        networkManager.selectcharacter();
        //characterSelection.startSelection();
        networkManager.selectedcharacterandopengame();
    }

    private void Start()
    {
        setVrcEconomy();
        setUserName();
        FillUserData();
    }

    public void setVrcEconomy()
    {
        float vrc = PlayerPrefs.GetFloat("Meta");
        vrcCoinsText.text = vrc.ToString();
    }

    private void setUserName()
    {
        userName.text = PlayerPrefs.GetString("PlayerName");
    }

    public void FillUserData()
    {
        gemCoin.text = PlayerPrefs.GetFloat("gems").ToString();/*GameDataNEW.playerResources.gem.ToString("n0")*/
        Debug.Log("Gem Coins are:" + gemCoin.text);
    }

    public void setSelectedRoomButton(int no)
    {
        aPIHolder.isFreeRoom = false;
        for (int i = 0; i < unSelectedRoomPersonsList.Length; i++)
        {
            unSelectedRoomPersonsList[i].enabled = true;
        }

        for (int i = 0; i < selectedRoomPersonsList.Length; i++)
        {
            selectedRoomPersonsList[i].enabled = false;
        }

        selectedRoomPersonsList[no].enabled = true;
        unSelectedRoomPersonsList[no].enabled = false;
    }

    public void setSelectedFreeRoomButton(int no)
    {
        aPIHolder.isFreeRoom = true;
        for (int i = 0; i < unSelectedRoomPersonsList.Length; i++)
        {
            unSelectedRoomPersonsList[i].enabled = true;
        }

        for (int i = 0; i < selectedRoomPersonsList.Length; i++)
        {
            selectedRoomPersonsList[i].enabled = false;
        }

        selectedRoomPersonsList[no].enabled = true;
        unSelectedRoomPersonsList[no].enabled = false;
    }

    public void setSettingsPanel()
    {
        Popup.Instance.setting.gameObject.SetActive(true);
    }

    public string getWeaponsAPI()
    {
        return weaponsFetch;
    }

    private IEnumerator getWeaponsListAPI()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(getWeaponsAPI()))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("API Error: " + webRequest.error);
            }
            else
            {
                string jsonResult = webRequest.downloadHandler.text;
                Debug.Log("Received JSON: " + jsonResult);

                try
                {
                    WeaponsApiResponse apiResponse = JsonUtility.FromJson<WeaponsApiResponse>(jsonResult);

                    if (apiResponse != null && apiResponse.weapon != null)
                    {
                        if (apiResponse.weapon.Count > 0)
                        {
                            Debug.Log("Number of weapon items received: " + apiResponse.weapon.Count);
                            weaponDetails = apiResponse.weapon;
                        }
                        else
                        {
                            Debug.LogWarning("No weapon items received.");
                        }
                    }
                    else
                    {
                        Debug.LogError("Response or WeaponInfo is NULL");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to deserialize JSON: " + e.Message);
                }
            }
        }

    }

    private IEnumerator getCharactersLisAPI()
    {
        using(UnityWebRequest webRequest = UnityWebRequest.Get(charactersFetch))
        {
            yield return webRequest.SendWebRequest();

            if(webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("API Error: " + webRequest.error);
            }
            else
            {
                string jsonResult = webRequest.downloadHandler.text;

                Debug.Log("Received Result is:" + jsonResult);

                try
                {
                    CharactersApiResponse apiResponse = JsonUtility.FromJson<CharactersApiResponse>(jsonResult);

                    if(apiResponse != null && apiResponse.characters != null)
                    {
                        if(apiResponse.characters.Count > 0)
                        {
                            Debug.Log("Number of character items received: " + apiResponse.characters.Count);

                            characterDetails = apiResponse.characters;
                        }
                        else
                        {
                            Debug.LogWarning("No weapon items received.");
                        }
                    }
                    else
                    {
                        Debug.LogError("Response or WeaponInfo is NULL");
                    }
                }
                catch(Exception e)
                {
                    Debug.LogError("Failed to deserialize JSON: " + e.Message);
                }
            }
        }
    }

    private IEnumerator getGemsPackageFromAPI()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(getGemsPackage))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("API Error: " + webRequest.error);
            }
            else
            {
                string jsonResult = webRequest.downloadHandler.text;
                Debug.Log("Received JSON: " + jsonResult);

                try
                {
                    gemsVRCAPIResponse apiResponse = JsonUtility.FromJson<gemsVRCAPIResponse>(jsonResult);

                    if (apiResponse != null && apiResponse.gemsPackage != null)
                    {
                        if (apiResponse.gemsPackage.Count > 0)
                        {
                            Debug.Log("Number of weapon items received: " + apiResponse.gemsPackage.Count);
                            shopifyPanel.gemsDetail = apiResponse.gemsPackage;
                        }
                        else
                        {
                            Debug.LogWarning("No weapon items received.");
                        }
                    }
                    else
                    {
                        Debug.LogError("Response or WeaponInfo is NULL");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to deserialize JSON: " + e.Message);
                }
            }
        }
    }

    private IEnumerator getAvatarsLisAPI()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(getAvatarAPI))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("API Error: " + webRequest.error);
            }
            else
            {
                string jsonResult = webRequest.downloadHandler.text;

                Debug.Log("Received Result is:" + jsonResult);

                try
                {
                    AvatarApiResponse apiResponse = JsonUtility.FromJson<AvatarApiResponse>(jsonResult);

                    if (apiResponse != null && apiResponse.avatars != null)
                    {
                        if (apiResponse.avatars.Count > 0)
                        {
                            Debug.Log("Number of character items received: " + apiResponse.avatars.Count);

                            avatarDetails = apiResponse.avatars;
                        }
                        else
                        {
                            Debug.LogWarning("No weapon items received.");
                        }
                    }
                    else
                    {
                        Debug.LogError("Response or WeaponInfo is NULL");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to deserialize JSON: " + e.Message);
                }
            }
        }
    }

    private IEnumerator getBannersLisAPI()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(getBannerAPI))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("API Error: " + webRequest.error);
            }
            else
            {
                string jsonResult = webRequest.downloadHandler.text;

                Debug.Log("Received Result is:" + jsonResult);

                try
                {
                    BannerApiResponse apiResponse = JsonUtility.FromJson<BannerApiResponse>(jsonResult);

                    if (apiResponse != null && apiResponse.banners != null)
                    {
                        if (apiResponse.banners.Count > 0)
                        {
                            Debug.Log("Number of character items received: " + apiResponse.banners.Count);

                            bannerDetails = apiResponse.banners;
                        }
                        else
                        {
                            Debug.LogWarning("No weapon items received.");
                        }
                    }
                    else
                    {
                        Debug.LogError("Response or WeaponInfo is NULL");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to deserialize JSON: " + e.Message);
                }
            }
        }
    }

    public void setVRC()
    {
        StartCoroutine(GetDataFromAPI());
    }

    IEnumerator GetDataFromAPI()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(GetVrcEndpoint))
        {

            string myHeaderValue = PlayerPrefs.GetString("Token");
            webRequest.SetRequestHeader("Authorization", myHeaderValue);

            // Request bhejna aur response ka intezar karna
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                // JSON data prapt karke use parse karna
                ApiResponseInLobbyVRC response = JsonUtility.FromJson<ApiResponseInLobbyVRC>(webRequest.downloadHandler.text);
                Debug.Log("Received: " + webRequest.downloadHandler.text);

                // 'vrc' value ko PlayerPrefs mein save karna
                PlayerPrefs.SetFloat("Meta", response.vrc);
                PlayerPrefs.SetFloat("gems", response.gems);

                setVrcEconomy();
                FillUserData();
            }
        }
    }

    public void GunclickSound()
    {
        mainAudioSource.volume = ProfileManager.UserProfile.soundVolume;
        mainAudioSource.clip = gunClickSound;
        mainAudioSource.Play();
    }

    public void WeaponClickSound()
    {
        mainAudioSource.volume = ProfileManager.UserProfile.soundVolume;
        mainAudioSource.clip = weaponClickSound;
        mainAudioSource.Play();
    }

    public void PurchaseClickSound()
    {
        mainAudioSource.volume = ProfileManager.UserProfile.soundVolume;
        mainAudioSource.clip = buySound;
        mainAudioSource.Play();
    }

    public void clickSound()
    {
        mainAudioSource.volume = ProfileManager.UserProfile.soundVolume;
        mainAudioSource.clip = clickClip;
        mainAudioSource.Play();
    }

    public void GemsEarnedSound()
    {
        mainAudioSource.volume = ProfileManager.UserProfile.soundVolume;
        mainAudioSource.clip = gemsEarnedClip;
        mainAudioSource.Play();
    }

    public void loadMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }
}



// Helper class to deserialize JSON array
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}


[Serializable]
public class WeaponsDetails
{
    public string weaponName;
    public float Damage;
    public float Distance;
}

[Serializable]
public class WeaponInformation
{
    public string name;
    public int id;
    public int price;
}

[Serializable]
public class WeaponsApiResponse
{
    public List<WeaponInformation> weapon;
}

[Serializable]
public class CharactersInformation
{
    public int id;
    public string name;
    public int price;
}

[Serializable]
public class CharactersApiResponse
{
    public List<CharactersInformation> characters;
}

[Serializable]
public class AvatarInformation
{
    public int id;
    public string name;
    public int price;
}

[Serializable]
public class AvatarApiResponse
{
    public List<AvatarInformation> avatars;
}

[Serializable]
public class BannerInformation
{
    public int id;
    public string name;
    public int price;
}

[Serializable]
public class BannerApiResponse
{
    public List<BannerInformation> banners;
}

[Serializable]
public class ApiResponseInLobbyVRC
{
    public float vrc;
    public float gems;
}

public class HistoryTakenBy
{
    public string room;
    public int fee;
    public string time;
}
