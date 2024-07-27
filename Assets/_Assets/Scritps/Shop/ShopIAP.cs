using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;


public class ShopIAP : Singleton<ShopIAP>
{
    public GameObject gemPanel;
    public GameObject coinPanel;
    public GameObject essentialPanel;

    public GameObject bonusGem100;
    public GameObject bonusGem300;
    public GameObject bonusGem500;
    public GameObject bonusGem1000;
    public GameObject bonusGem2500;
    public GameObject bonusGem5000;

    public GameObject btnRestorePurchase;
    public GameObject btnRemoveAds;
    public GameObject historypenal;

    public Text[] priceLabels;
    public float VrcCoin;

    public List<IAPVrcPurchasing> vrcPurchasing;
    public List<gemsInformation> gemsDetail = new List<gemsInformation>();
    public bool IsShowing { get { return enabled; } }

    private static string depositAuth = "deposit";
    private string depositeapi = APIHolder.getBaseUrl() + depositAuth;/*"https://a978-72-255-51-32.ngrok-free.app/api/deposit";*/

    private static string gemsPackage = "gemsPackage";
    private static string getGemsPackage = APIHolder.getBaseUrl() + gemsPackage;

    private static string getGemsRoute = "auth/purchasegems";
    private static string getUpdatedGemsRoute = APIHolder.getBaseUrl() + getGemsRoute;


    private void Awake()
    {
        StartCoroutine(getGemsPackageFromAPI());
    }

    private IEnumerator purchasediamedtoVRC(string vrc, string diamed)
    {
        // Creating a wwwForm object to send the parameters
        WWWForm form = new WWWForm();

        form.AddField("vrc", vrc);
        form.AddField("diamond", diamed);

        // Creating the UnityWebRequest and sending it
        using (UnityWebRequest www = UnityWebRequest.Post(depositeapi, form))
        {
            // Adding a custom header with value from PlayerPrefs



            string headerValue = PlayerPrefs.GetString("Token");
            www.SetRequestHeader("Authorization", headerValue);

            // Send the request and wait for the response
            yield return www.SendWebRequest();

            // Check for errors
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);

            }
            else
            {
                Debug.Log("Request sent successfully!");
                Debug.Log("Received: " + www.downloadHandler.text);

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
                    gemsAPIResponse apiResponse = JsonUtility.FromJson<gemsAPIResponse>(jsonResult);

                    if (apiResponse != null && apiResponse.gemsPackage != null)
                    {
                        if (apiResponse.gemsPackage.Count > 0)
                        {
                            Debug.Log("Number of weapon items received: " + apiResponse.gemsPackage.Count);
                            gemsDetail = apiResponse.gemsPackage;
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


    void Start()
    {
        btnRemoveAds.SetActive(!ProfileManager.UserProfile.isRemoveAds);

        Invoke(nameof(setPrices), 0.5f);
    }

    [ContextMenu("Set Pricing")]
    public void setPrices()
    {
        for (int i = 0; i < vrcPurchasing.Count; i++)
        {
            vrcPurchasing[i].vrcText.text = gemsDetail[i].price.ToString() + " VRC";
            vrcPurchasing[i].gemsQuantity.text = gemsDetail[i].gems.ToString();
        }
    }

    #region COIN PACK

    public void ExchangeMedalToCoin(int medal)
    {
        int coin = 0;

        switch (medal)
        {
            case 25:
                coin = 2500;
                break;
        }

        if (GameDataNEW.playerResources.medal >= medal)
        {
            Popup.Instance.Show(
                content: string.Format("would you like to exchange <color=#ffff00ff>{0:n0}</color> medal to <color=#ffff00ff>{1:n0}</color> coins?", medal, coin),
                title: "confirmation",
                type: PopupType.YesNo,
                yesCallback: () =>
                {
                GameDataNEW.playerResources.ConsumeMedal(medal);
                GameDataNEW.playerResources.ReceiveCoin(coin);
                SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_PURCHASE_SUCCESS);
                 });
        }
        else
        {
            Popup.Instance.ShowToastMessage("not enough medals");
        }

            SoundManager.Instance.PlaySfxClick();
    }

    public void ExchangeGemToCoin(int gem)
{
    int coin = 0;

    switch (gem)
    {
        case 50:
            // 50 gems to 5000 coins
            coin = 5000;
            break;
        case 100:
            // 100 gems to 12000 coins
            coin = 12000;
            break;
        case 250:
            // 250 gems to 32500 coins
            coin = 32500;
            break;
        case 500:
            // 500 gems to 70000 coins
            coin = 70000;
            break;
        case 1000:
            // 1000 gems to 150000 coins
            coin = 150000;
            break;
    }

    if (GameDataNEW.playerResources.gem >= gem)
    {
        Popup.Instance.Show(
            content: string.Format("would you like to exchange <color=#00ffffff>{0:n0}</color> gems to <color=#ffff00ff>{1:n0}</color> coins?", gem, coin),
            title: "confirmation",
            type: PopupType.YesNo,
            yesCallback: () =>
            {
                GameDataNEW.playerResources.ConsumeGem(gem);
                GameDataNEW.playerResources.ReceiveCoin(coin);
                SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_PURCHASE_SUCCESS);

               
            });
    }
    else
    {
        Popup.Instance.Show(
            content: string.Format("not enough gems, would you like to buy some?"),
            title: "confirmation",
            type: PopupType.YesNo,
            yesCallback: () =>
            {
                OpenGemShop();
            });
    }
}

    #endregion

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
                Popup.Instance.ShowToastMessage(string.Format("Low Balance..."), ToastLength.Normal);
            }
            else
            {
                Debug.Log("Request sent successfully!");
                string jsonResponse = www.downloadHandler.text;
                Debug.Log("Received: " + jsonResponse);

                // Parse the JSON response
                var responseObject = JsonUtility.FromJson<gemIAPBoughtResponse>(jsonResponse);

                if (responseObject != null)
                {
                    Debug.Log("Message: " + responseObject.message);
                    Debug.Log("Value: " + responseObject.status);
                    Debug.Log("Gems are:" + responseObject.gems);
                    Debug.Log("Vrc are:" + responseObject.vrc);

                    PlayerPrefs.SetFloat("gems", responseObject.gems);
                    PlayerPrefs.SetFloat("Meta", responseObject.vrc);
                }
                else
                {
                    Debug.Log("Error parsing JSON response");
                }
            }
        }
    }

    public void BuyGem100()
    {
        if (PlayerPrefs.GetInt("Guest") != 1)
        {
            int id = 0;
            StartCoroutine(setGemsOnBought(id.ToString()));
            /*VrcCoin = PlayerPrefs.GetFloat("Meta");
            if (VrcCoin >= 0.2f)
            {
                ProcessBuyGem100();
                PlayerPrefs.SetFloat("Meta", VrcCoin - 0.2f);

                float BitCoin = PlayerPrefs.GetFloat("Meta");
                StartCoroutine(purchasediamedtoVRC("0.2", "100"));
                return;
            }
            else
            {
                Popup.Instance.ShowToastMessage(string.Format("Low Balance..."), ToastLength.Normal);
                return;
            }*/
        }
        else
        {
           // UnityAdsManager.instance.ShowNonRewardedAd();
            Popup.Instance.ShowToastMessage(string.Format("Login for Purchase Items..."), ToastLength.Normal);
            return;
        }
    }

    public void BuyGem300()
    {
        


        if (PlayerPrefs.GetInt("Guest") != 1)
        {
            /*VrcCoin = PlayerPrefs.GetFloat("Meta");
            if (VrcCoin >= 0.4f)
            {
                ProcessBuyGem300();
                PlayerPrefs.SetFloat("Meta", VrcCoin - 0.4f);

                float BitCoin = PlayerPrefs.GetFloat("Meta");
                StartCoroutine(purchasediamedtoVRC("0.4", "300"));
                return;
            }

            else
            {
                Popup.Instance.ShowToastMessage(string.Format("Low Balance..."), ToastLength.Normal);
                return;
            }*/
            int id = 1;
            StartCoroutine(setGemsOnBought(id.ToString()));

        }
        else
        {
           // UnityAdsManager.instance.ShowNonRewardedAd();
            Popup.Instance.ShowToastMessage(string.Format("Login for Purchase Items..."), ToastLength.Normal);
            return;
        }

    }

    public void BuyGem500()
    {
        if (PlayerPrefs.GetInt("Guest") != 1)
        {
            /* VrcCoin = PlayerPrefs.GetFloat("Meta");
             if (VrcCoin >= 0.8f)
             {
                 ProcessBuyGem500();
                 PlayerPrefs.SetFloat("Meta", VrcCoin - 0.8f);

                 float BitCoin = PlayerPrefs.GetFloat("Meta");
                 StartCoroutine(purchasediamedtoVRC("0.8", "500"));
                 return;
             }
             else
             {
                 Popup.Instance.ShowToastMessage(string.Format("Low Balance..."), ToastLength.Normal);
                 return;
             }*/
            int id = 2;
            StartCoroutine(setGemsOnBought(id.ToString()));
        }
        else
        {
            //UnityAdsManager.instance.ShowNonRewardedAd();
            Popup.Instance.ShowToastMessage(string.Format("Login for Purchase Items"), ToastLength.Normal);
            return;
        }
    }
    

public void BuyGem1000()
    {
        if (PlayerPrefs.GetInt("Guest") != 1)
        {
            /*VrcCoin = PlayerPrefs.GetFloat("Meta");
            if (VrcCoin >= 0.12f)
            {
                ProcessBuyGem1000();
                PlayerPrefs.SetFloat("Meta", VrcCoin - 0.12f);

                float BitCoin = PlayerPrefs.GetFloat("Meta");
                StartCoroutine(purchasediamedtoVRC("0.12", "1000"));
                return;
            }
            else
            {
                Popup.Instance.ShowToastMessage(string.Format("Low Balance..."), ToastLength.Normal);
                return;
            }*/

            int id = 3;
            StartCoroutine(setGemsOnBought(id.ToString()));
        }
        else
        {
            //UnityAdsManager.instance.ShowNonRewardedAd();
            Popup.Instance.ShowToastMessage(string.Format("Login for Purchase Items"), ToastLength.Normal);
            return;
        }
    }

    public void BuyGem2500()
    {
        if (PlayerPrefs.GetInt("Guest") != 1)
        {
            /*VrcCoin = PlayerPrefs.GetFloat("Meta");
            if (VrcCoin >= 0.24f)
            {
                ProcessBuyGem2500();
                PlayerPrefs.SetFloat("Meta", VrcCoin - 0.24f);

                float BitCoin = PlayerPrefs.GetFloat("Meta");
                StartCoroutine(purchasediamedtoVRC("0.24", "2500"));
                return;
            }
            else
            {
                Popup.Instance.ShowToastMessage(string.Format("Low Balance..."), ToastLength.Normal);
                return;
            }*/

            int id = 4;
            StartCoroutine(setGemsOnBought(id.ToString()));
        }

        else
        {
            //UnityAdsManager.instance.ShowNonRewardedAd();
            Popup.Instance.ShowToastMessage(string.Format("Login for Purchase Items"), ToastLength.Normal);
            return;
        }
    }

    public void BuyGem5000()
    {
        if (PlayerPrefs.GetInt("Guest") != 1)
        {
            /*VrcCoin = PlayerPrefs.GetFloat("Meta");
        if (VrcCoin >= 0.30f)
        {
            ProcessBuyGem5000();
            PlayerPrefs.SetFloat("Meta", VrcCoin - 0.30f);

            float BitCoin = PlayerPrefs.GetFloat("Meta");
            StartCoroutine(purchasediamedtoVRC("0.30", "5000"));
            return;
        }
        else
        {
            Popup.Instance.ShowToastMessage(string.Format("Low Balance..."), ToastLength.Normal);
            return;
        }*/

            int id = 5;
            StartCoroutine(setGemsOnBought(id.ToString()));
        }

        else
        {
            //UnityAdsManager.instance.ShowNonRewardedAd();
            Popup.Instance.ShowToastMessage(string.Format("Login for Purchase Items"), ToastLength.Normal);
            return;
        }
    }

    public void BuyEverybodyFavorite()
    {

        ProcessBuyEverybodyFavorite();
        return;

      //  InAppPurchaseController.Instance.BuyProductID(ProductDefine.EVERY_FAVORITE.productId);
    }

    public void BuyDragonBreath()
    {

        ProcessBuyDragonBreath();
        return;

       // InAppPurchaseController.Instance.BuyProductID(ProductDefine.DRAGON_BREATH.productId);
    }

    public void BuyLetThereBeFire()
    {

        ProcessBuyLetThereBeFire();
        return;

       // InAppPurchaseController.Instance.BuyProductID(ProductDefine.LET_THERE_BE_FIRE.productId);
    }

    public void BuySnippingForDummies()
    {

        ProcessBuySnippingForDummies();
        return;

       // InAppPurchaseController.Instance.BuyProductID(ProductDefine.SNIPPING_FOR_DUMMIES.productId);
    }

    public void BuyTaserLaser()
    {

        ProcessBuyTaserLaser();
        return;

       // InAppPurchaseController.Instance.BuyProductID(ProductDefine.TASER_LASER.productId);
    }

    public void BuyShockingSale()
    {

        ProcessBuyShockingSale();
        return;

       // InAppPurchaseController.Instance.BuyProductID(ProductDefine.SHOCKING_SALE.productId);
    }

    public void BuyUpgradeEnthusiast()
    {

        ProcessBuyUpgradeEnthusiast();
        return;

        //InAppPurchaseController.Instance.BuyProductID(ProductDefine.UPGRADE_ENTHUSIAST.productId);
    }

    public void BuyBattleEssentials1()
    {
        if (PlayerPrefs.GetInt("Guest") != 1)
        {
            VrcCoin = PlayerPrefs.GetFloat("Meta");
        if (VrcCoin >= 1f)
        {
            ProcessBuyBattleEssentials_1();
            PlayerPrefs.SetFloat("Meta", VrcCoin - 1f);

            float BitCoin = PlayerPrefs.GetFloat("Meta");
                StartCoroutine(purchasediamedtoVRC("1", "BuyBattleEssentialsPack1"));
                return;
        }
        else
        {
            Popup.Instance.ShowToastMessage(string.Format("Low Balance..."), ToastLength.Normal);
            return;
        }
    }

        else
        {
            //UnityAdsManager.instance.ShowNonRewardedAd();
            Popup.Instance.ShowToastMessage(string.Format("Login for Purchase Items"), ToastLength.Normal);
            return;
        }
    }

    public void BuyBattleEssentials2()
    {
        if (PlayerPrefs.GetInt("Guest") != 1)
        {
            VrcCoin = PlayerPrefs.GetFloat("Meta");
        if (VrcCoin >= 1.5f)
        {
            ProcessBuyBattleEssentials_2();
            PlayerPrefs.SetFloat("Meta", VrcCoin - 1.5f);

            float BitCoin = PlayerPrefs.GetFloat("Meta");
            StartCoroutine(purchasediamedtoVRC("1.5", "BuyBattleEssentialsPack2"));
            return;
        }
        else
        {
            Popup.Instance.ShowToastMessage(string.Format("Low Balance..."), ToastLength.Normal);
            return;
        }

        }

        else
        {
            //UnityAdsManager.instance.ShowNonRewardedAd();
            Popup.Instance.ShowToastMessage(string.Format("Login for Purchase Items"), ToastLength.Normal);
            return;
        }



    }

    public void BuyBattleEssentials3()
    {
        if (PlayerPrefs.GetInt("Guest") != 1)
        {
            VrcCoin = PlayerPrefs.GetFloat("Meta");
        if (VrcCoin >= 2f)
        {
            ProcessBuyBattleEssentials_3();
            PlayerPrefs.SetFloat("Meta", VrcCoin - 2f);

            float BitCoin = PlayerPrefs.GetFloat("Meta");
            StartCoroutine(purchasediamedtoVRC("2", "BuyBattleEssentialsPack3"));
            return;
        }
        else
        {
            Popup.Instance.ShowToastMessage(string.Format("Low Balance..."), ToastLength.Normal);
            return;
        }
    }

        else
        {
            //UnityAdsManager.instance.ShowNonRewardedAd();
            Popup.Instance.ShowToastMessage(string.Format("Login for Purchase Items"), ToastLength.Normal);
            return;
        }
    }

    public void BuyStarterPack()
    {

        ProcessBuyStarterPack();
        return;

    }

    public void BuyRemoveAds()
    {
        if (PlayerPrefs.GetInt("Guest") != 1)
        {
            VrcCoin = PlayerPrefs.GetFloat("Meta");
        if (VrcCoin >= 5f)
        {
            ProcessBuyRemoveAds();
            PlayerPrefs.SetFloat("Meta", VrcCoin - 5f);

            float BitCoin = PlayerPrefs.GetFloat("Meta");
            StartCoroutine(purchasediamedtoVRC("5", "RemoveAds"));
            return;
        }
        else
        {
            Popup.Instance.ShowToastMessage(string.Format("Earn 5 coin for Remove Ads"), ToastLength.Normal);
            return;
        }
        }

        else
        {
            //UnityAdsManager.instance.ShowNonRewardedAd();
            Popup.Instance.ShowToastMessage(string.Format("Login for Purchase Items"), ToastLength.Normal);
            return;
        }
    }

    #region PANEL CONTROL

    public void OpenGemShop()
{
    //starterBoard.SetActive(false);
    //gemAndCoinBoard.SetActive(true);
    gemPanel.SetActive(true);
        historypenal.SetActive(false);
        coinPanel.SetActive(false);
    essentialPanel.SetActive(false);

    CheckLabelBonusGem();

    SoundManager.Instance.PlaySfxClick();
}

    public void OpenCoinShop()
{
    //starterBoard.SetActive(false);
    //gemAndCoinBoard.SetActive(true);
    gemPanel.SetActive(false);
    coinPanel.SetActive(true);
        historypenal.SetActive(false);
        essentialPanel.SetActive(false);

    SoundManager.Instance.PlaySfxClick();
}

    public void OpenEssentialShop()
{
  
    gemPanel.SetActive(false);
    coinPanel.SetActive(false);
        historypenal.SetActive(false);
        essentialPanel.SetActive(true);

    SoundManager.Instance.PlaySfxClick();
}


    public void Shophistory()
    {

        gemPanel.SetActive(false);
        coinPanel.SetActive(false);
        essentialPanel.SetActive(false);
        historypenal.SetActive(true);

        SoundManager.Instance.PlaySfxClick();
    }
    public void Hide()
{
    gameObject.SetActive(false);
}

#endregion

    #region PRIVATE METHODS

    private void CheckLabelBonusGem()
    {
        bonusGem100.SetActive(!ProfileManager.UserProfile.isFirstBuyGem100);
        bonusGem300.SetActive(!ProfileManager.UserProfile.isFirstBuyGem300);
        bonusGem500.SetActive(!ProfileManager.UserProfile.isFirstBuyGem500);
        bonusGem1000.SetActive(!ProfileManager.UserProfile.isFirstBuyGem1000);
        bonusGem2500.SetActive(!ProfileManager.UserProfile.isFirstBuyGem2500);
        bonusGem5000.SetActive(!ProfileManager.UserProfile.isFirstBuyGem5000);
    }

    #endregion

    #region PROCESS NEW IAP

    private void ProcessBuyGem100()
    {
     int gem = ProfileManager.UserProfile.isFirstBuyGem100 ? 100 : 200;
     GameDataNEW.playerResources.ReceiveGem(gem);
     Popup.Instance.ShowToastMessage(string.Format("Received {0} gems", gem), ToastLength.Normal);
     ProfileManager.UserProfile.isFirstBuyGem100.Set(true);
     CheckLabelBonusGem();
    
    }

    private void ProcessBuyGem300()
{
    int gem = ProfileManager.UserProfile.isFirstBuyGem100 ? 315 : 630;
    GameDataNEW.playerResources.ReceiveGem(gem);
    Popup.Instance.ShowToastMessage(string.Format("Received {0} gems", gem), ToastLength.Normal);
    ProfileManager.UserProfile.isFirstBuyGem300.Set(true);
    CheckLabelBonusGem();

   // FirebaseAnalyticsHelper.LogEvent("N_BuyGem", 300);
}

    private void ProcessBuyGem500()
{
    int gem = ProfileManager.UserProfile.isFirstBuyGem100 ? 550 : 1100;
    GameDataNEW.playerResources.ReceiveGem(gem);
    Popup.Instance.ShowToastMessage(string.Format("Received {0} gems", gem), ToastLength.Normal);
    ProfileManager.UserProfile.isFirstBuyGem500.Set(true);
    CheckLabelBonusGem();

    //FirebaseAnalyticsHelper.LogEvent("N_BuyGem", 500);
}

    private void ProcessBuyGem1000()
{
    int gem = ProfileManager.UserProfile.isFirstBuyGem100 ? 1250 : 2500;
    GameDataNEW.playerResources.ReceiveGem(gem);
    Popup.Instance.ShowToastMessage(string.Format("Received {0} gems", gem), ToastLength.Normal);
    ProfileManager.UserProfile.isFirstBuyGem1000.Set(true);
    CheckLabelBonusGem();

   // FirebaseAnalyticsHelper.LogEvent("N_BuyGem", 1000);
}

    private void ProcessBuyGem2500()
{
    int gem = ProfileManager.UserProfile.isFirstBuyGem100 ? 3750 : 7500;
    GameDataNEW.playerResources.ReceiveGem(gem);
    Popup.Instance.ShowToastMessage(string.Format("Received {0} gems", gem), ToastLength.Normal);
    ProfileManager.UserProfile.isFirstBuyGem2500.Set(true);
    CheckLabelBonusGem();

    //FirebaseAnalyticsHelper.LogEvent("N_BuyGem", 2500);
}

    private void ProcessBuyGem5000()
{
    int gem = ProfileManager.UserProfile.isFirstBuyGem100 ? 10000 : 20000;
    GameDataNEW.playerResources.ReceiveGem(gem);
    Popup.Instance.ShowToastMessage(string.Format("Received {0} gems", gem), ToastLength.Normal);
    ProfileManager.UserProfile.isFirstBuyGem5000.Set(true);
    CheckLabelBonusGem();

   // FirebaseAnalyticsHelper.LogEvent("N_BuyGem", 5000);
}

    private void ProcessBuyEverybodyFavorite()
{
    if (ProfileManager.UserProfile.isPurchasedPackEverybodyFavorite)
    {
        return;
    }

    TextAsset textAsset = Resources.Load<TextAsset>("JSON/IAP/iap_pack_everybody_favorite");
    List<RewardData> rewards = JsonConvert.DeserializeObject<List<RewardData>>(textAsset.text);
    RewardUtils.Receive(rewards);
    Popup.Instance.ShowReward(rewards);

    ProfileManager.UserProfile.isPurchasedPackEverybodyFavorite.Set(true);

    EventDispatcher.Instance.PostEvent(EventID.BuySpecialOffer);

    //FirebaseAnalyticsHelper.LogEvent("N_Buy_EverybodyFavorite");
}

    private void ProcessBuyDragonBreath()
{
    if (ProfileManager.UserProfile.isPurchasedPackDragonBreath)
    {
        return;
    }

    TextAsset textAsset = Resources.Load<TextAsset>("JSON/IAP/iap_pack_dragon_breath");
    List<RewardData> rewards = JsonConvert.DeserializeObject<List<RewardData>>(textAsset.text);
    RewardUtils.Receive(rewards);
    Popup.Instance.ShowReward(rewards);

    ProfileManager.UserProfile.isPurchasedPackDragonBreath.Set(true);

    EventDispatcher.Instance.PostEvent(EventID.BuySpecialOffer);

    //FirebaseAnalyticsHelper.LogEvent("N_Buy_DragonBreath");
}

    private void ProcessBuyLetThereBeFire()
{
    if (ProfileManager.UserProfile.isPurchasedPackLetThereBeFire)
    {
        return;
    }

    TextAsset textAsset = Resources.Load<TextAsset>("JSON/IAP/iap_pack_let_there_be_fire");
    List<RewardData> rewards = JsonConvert.DeserializeObject<List<RewardData>>(textAsset.text);
    RewardUtils.Receive(rewards);
    Popup.Instance.ShowReward(rewards);

    ProfileManager.UserProfile.isPurchasedPackLetThereBeFire.Set(true);

    EventDispatcher.Instance.PostEvent(EventID.BuySpecialOffer);

   // FirebaseAnalyticsHelper.LogEvent("N_Buy_LetBeFire");
}

    private void ProcessBuySnippingForDummies()
{
    if (ProfileManager.UserProfile.isPurchasedPackSnippingForDummies)
    {
        return;
    }

    TextAsset textAsset = Resources.Load<TextAsset>("JSON/IAP/iap_pack_snipping_for_dummies");
    List<RewardData> rewards = JsonConvert.DeserializeObject<List<RewardData>>(textAsset.text);
    RewardUtils.Receive(rewards);
    Popup.Instance.ShowReward(rewards);

    ProfileManager.UserProfile.isPurchasedPackSnippingForDummies.Set(true);

    EventDispatcher.Instance.PostEvent(EventID.BuySpecialOffer);

    //FirebaseAnalyticsHelper.LogEvent("N_Buy_SnippingDummies");
}

    private void ProcessBuyTaserLaser()
{
    if (ProfileManager.UserProfile.isPurchasedPackTaserLaser)
    {
        return;
    }

    TextAsset textAsset = Resources.Load<TextAsset>("JSON/IAP/iap_pack_taser_laser");
    List<RewardData> rewards = JsonConvert.DeserializeObject<List<RewardData>>(textAsset.text);
    RewardUtils.Receive(rewards);
    Popup.Instance.ShowReward(rewards);

    ProfileManager.UserProfile.isPurchasedPackTaserLaser.Set(true);

    EventDispatcher.Instance.PostEvent(EventID.BuySpecialOffer);

   // FirebaseAnalyticsHelper.LogEvent("N_Buy_TaserLaser");
}

    private void ProcessBuyShockingSale()
{
    if (ProfileManager.UserProfile.isPurchasedPackShockingSale)
    {
        return;
    }

    TextAsset textAsset = Resources.Load<TextAsset>("JSON/IAP/iap_pack_shocking_sale");
    List<RewardData> rewards = JsonConvert.DeserializeObject<List<RewardData>>(textAsset.text);
    RewardUtils.Receive(rewards);
    Popup.Instance.ShowReward(rewards);

    ProfileManager.UserProfile.isPurchasedPackShockingSale.Set(true);

    EventDispatcher.Instance.PostEvent(EventID.BuySpecialOffer);

   // FirebaseAnalyticsHelper.LogEvent("N_Buy_ShockingSale");
}

    private void ProcessBuyUpgradeEnthusiast()
{
    if (ProfileManager.UserProfile.isPurchasedPackUpgradeEnthusiast)
    {
        return;
    }

    TextAsset textAsset = Resources.Load<TextAsset>("JSON/IAP/iap_pack_upgrade_enthusiast");
    List<RewardData> rewards = JsonConvert.DeserializeObject<List<RewardData>>(textAsset.text);
    RewardUtils.Receive(rewards);
    Popup.Instance.ShowReward(rewards);

    ProfileManager.UserProfile.isPurchasedPackUpgradeEnthusiast.Set(true);

    EventDispatcher.Instance.PostEvent(EventID.BuySpecialOffer);

   // FirebaseAnalyticsHelper.LogEvent("N_Buy_Enthusiast");
}

    private void ProcessBuyBattleEssentials_1()
{
    TextAsset textAsset = Resources.Load<TextAsset>("JSON/IAP/iap_pack_battle_essentitals_1");
    List<RewardData> rewards = JsonConvert.DeserializeObject<List<RewardData>>(textAsset.text);
    RewardUtils.Receive(rewards);
    Popup.Instance.ShowReward(rewards);

   // FirebaseAnalyticsHelper.LogEvent("N_Buy_Essential_1");
}

    private void ProcessBuyBattleEssentials_2()
{
    TextAsset textAsset = Resources.Load<TextAsset>("JSON/IAP/iap_pack_battle_essentitals_2");
    List<RewardData> rewards = JsonConvert.DeserializeObject<List<RewardData>>(textAsset.text);
    RewardUtils.Receive(rewards);
    Popup.Instance.ShowReward(rewards);

   // FirebaseAnalyticsHelper.LogEvent("N_Buy_Essential_2");
}

    private void ProcessBuyBattleEssentials_3()
{
    TextAsset textAsset = Resources.Load<TextAsset>("JSON/IAP/iap_pack_battle_essentitals_3");
    List<RewardData> rewards = JsonConvert.DeserializeObject<List<RewardData>>(textAsset.text);
    RewardUtils.Receive(rewards);
    Popup.Instance.ShowReward(rewards);

   // FirebaseAnalyticsHelper.LogEvent("N_Buy_Essential_3");
}

    public void ProcessBuyStarterPack()
{
    if (ProfileManager.UserProfile.isPurchasedStarterPack)
    {
        return;
    }

    TextAsset textAsset = Resources.Load<TextAsset>("JSON/IAP/iap_pack_starter");
    List<RewardData> rewards = JsonConvert.DeserializeObject<List<RewardData>>(textAsset.text);
    RewardUtils.Receive(rewards);
    Popup.Instance.ShowReward(rewards);

    ProfileManager.UserProfile.isPurchasedStarterPack.Set(true);

    EventDispatcher.Instance.PostEvent(EventID.BuyStarterPack);

   // FirebaseAnalyticsHelper.LogEvent("N_Buy_Starter_Pack");
}

    public void ProcessBuyRemoveAds()
{
    if (ProfileManager.UserProfile.isRemoveAds)
    {
        return;
    }

    btnRemoveAds.SetActive(false);

    Popup.Instance.Show(
        content: "Your purchase was successful.\nYou now no longer receive ads.",
        title: "Remove ads");

    ProfileManager.UserProfile.isRemoveAds.Set(true);

   // FirebaseAnalyticsHelper.LogEvent("N_Buy_RemoveAds");
}

    #endregion

}

[Serializable]
public class IAPVrcPurchasing
{
    public Button vrcButtons;
    public Text vrcText;
    public TextMeshProUGUI gemsQuantity;
}

[Serializable]
public class gemsInformation
{
    public int id;
    public string name;
    public int gems;
    public float price;
}

[Serializable]
public class gemsAPIResponse
{
    public List<gemsInformation> gemsPackage;
}

[Serializable]
public class gemIAPBoughtResponse
{
    public bool status;
    public string message;
    public float gems;
    public float vrc;
}
