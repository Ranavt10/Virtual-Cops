﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class HudSelectBooster : MonoBehaviour
{
    public GameObject popup;
    public RectTransform rectTransBoosters;

    [Header("MONEY")]
    public ResourcesChangeText changeTextPrefab;
    public Text textCoin;
    public Text textGem;

    [Header("BOOSTER")]
    public Text textBoosterName;
    public Text textBoosterDescription;
    public Text textIntroduce;

    //[Header("QUEST")]
    //public GameObject groupQuests;
    //public Text[] questDescriptions;

    [Header("STAGE INFO")]
    public Text textStageNameId;
    public Text textDifficulty;
    public Color32 colorDifficultyNormal;
    public Color32 colorDifficultyHard;
    public Color32 colorDifficultyCrazy;

    [Header("AMMO")]
    public Image imgGun;
    public Text textPriceAmmo;
    public Text textCurrentAmmo;
    public Text textMaxAmmo;
    public Button btnBuyFullAmmo;
    public GameObject labelNoSpecialGun;
    public Color32 colorFullAmmo;

    private int costBuyFullAmmo;
    private int maxAmmoCount;

    [Header("PLAYER INFO")]
    public Text textRamboLevel;
    public Text textGrenadeQuantity;
    public GameObject[] selectedBoosters;

    public Text PlayernameText;
    public RawImage playerimage;
    public GameObject guestavatar;

    private void Awake()
    {
        EventDispatcher.Instance.RegisterListener(EventID.SelectBooster, (sender, param) => OnSelectBooster((StaticBoosterData)param));
        EventDispatcher.Instance.RegisterListener(EventID.ConsumeCoin, (sender, param) => OnConsumeCoin((int)param));
        EventDispatcher.Instance.RegisterListener(EventID.BuyBooster, (sender, param) => OnBuyBooster((BoosterType)param));

        PlayernameText.text = PlayerPrefs.GetString("PlayerName");
        Texture2D loadedTexture = LoadImageFromLocalStorage("savedImage.png");
        if (loadedTexture != null)
        {
            playerimage.texture = loadedTexture;
            guestavatar.SetActive(false);
        }
        else
        {
            playerimage.gameObject.SetActive(false);
            guestavatar.SetActive(true);
        }

    }

    public Texture2D LoadImageFromLocalStorage(string filename = "savedImage.png")
    {
        string path = Path.Combine(Application.persistentDataPath, filename);

        if (File.Exists(path))
        {
            byte[] fileData = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData); // Yeh image data ko texture mein load karega

            return texture; // Loaded texture wapis karna hai
        }
        else
        {
            Debug.LogError("Image not found at: " + path);
            return null; // Image nahi mili toh null wapis karein
        }
    }

    public void Open()
    {
        popup.SetActive(true);

        SetLayout();
        SetCoinAndGem();
        SetPlayerInfo();
        SetBuyAmmoInfo();
        SetActiveBoosters();
        ShowGuideText(true);

        if (GameDataNEW.mode == GameMode.Campaign)
        {
            textStageNameId.gameObject.SetActive(true);
            textDifficulty.gameObject.SetActive(true);
            SetStageInfo();

            if (GameDataNEW.mode == GameMode.Campaign)
            {
                if (GameDataNEW.playerTutorials.IsCompletedStep(TutorialType.Booster) == false && ProfileManager.UserProfile.gunSpecialId == -1)
                {
                    UIController.Instance.tutorialGamePlay.ShowTutorialBooster();
                }
            }
        }
        else if (GameDataNEW.mode == GameMode.Survival)
        {
            //groupQuests.SetActive(false);
            textStageNameId.gameObject.SetActive(false);
            textDifficulty.gameObject.SetActive(false);
        }
    }

    public void Close()
    {
        popup.SetActive(false);
    }

    public void ClosePowerUpAndStartGame()
    {
        Close();

        for (int i = 0; i < GameDataNEW.selectingBoosters.Count; i++)
        {
            BoosterType type = GameDataNEW.selectingBoosters[i];

            GameDataNEW.playerBoosters.Consume(type, 1);
        }

        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_START_MISSION);
        GameDataNEW.selectingBoosters.Save();
        EventDispatcher.Instance.PostEvent(EventID.SelectBoosterDone);

        if (GameDataNEW.isShowingTutorial && string.Compare(GameDataNEW.currentStage.id, "1.1") == 0)
        {
            EventDispatcher.Instance.PostEvent(EventID.CompleteStep, TutorialType.Booster);
        }
    }

    public void Home()
    {
        UIController.Instance.BackToMainMenu();

        //if (!ProfileManager.UserProfile.isRemoveAds)
        //{
        //    AdMobController.Instance.ShowInterstitialAd(() =>
        //    {
        //        UIController.Instance.BackToMainMenu();
        //    });
        //}
        //else
        //{
        //    UIController.Instance.BackToMainMenu();
        //}
    }

    public void BuyFullAmmo()
    {
        if (costBuyFullAmmo <= 0)
        {
            Popup.Instance.ShowToastMessage("Gun has max ammo");
            return;
        }

        GameDataNEW.playerResources.ConsumeCoin(costBuyFullAmmo);
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_PURCHASE_SUCCESS);
        GameDataNEW.playerGuns.SetGunAmmo(ProfileManager.UserProfile.gunSpecialId, maxAmmoCount);

        textCurrentAmmo.text = maxAmmoCount.ToString("n0");
        textCurrentAmmo.color = colorFullAmmo;
        costBuyFullAmmo = 0;
        textPriceAmmo.text = costBuyFullAmmo.ToString("n0");

        EventDispatcher.Instance.PostEvent(EventID.BuyAmmo);
    }

    private void SetLayout()
    {
        if (ProfileManager.UserProfile.gunSpecialId == -1)
        {
            Vector3 v = rectTransBoosters.localPosition;
            v.x = 0f;
            rectTransBoosters.localPosition = v;

            labelNoSpecialGun.transform.parent.gameObject.SetActive(false);
        }
    }

    private void SetCoinAndGem()
    {
        textCoin.text = GameDataNEW.playerResources.coin.ToString("n0");
        textGem.text = GameDataNEW.playerResources.gem.ToString("n0");
    }

    private void OnConsumeCoin(int value)
    {
        textCoin.text = GameDataNEW.playerResources.coin.ToString("n0");

        ResourcesChangeText changeText = Header.poolTextChange.New();

        if (changeText == null)
        {
            changeText = Instantiate(changeTextPrefab) as ResourcesChangeText;
        }

        changeText.Active(false, value, textCoin.rectTransform.position, transform);
    }

    private void OnSelectBooster(StaticBoosterData data)
    {
        ShowGuideText(false);
        textBoosterName.text = data.boosterName;
        textBoosterDescription.text = data.description;

        SetActiveBoosters();
    }

    private void OnBuyBooster(BoosterType type)
    {
        if (type == BoosterType.Grenade)
        {
            int id = ProfileManager.UserProfile.grenadeId;

            if (id == StaticValue.GRENADE_ID_F1)
            {
                int quantity = GameDataNEW.playerGrenades.GetQuantityHave(id);
                textGrenadeQuantity.text = string.Format("x{0:n0}", quantity);
            }
        }

        StaticBoosterData data = GameDataNEW.staticBoosterData.GetData(type);

        ShowGuideText(false);
        textBoosterDescription.text = data.description;
        textBoosterName.text = data.boosterName;

        SetActiveBoosters();
    }

    private void ShowGuideText(bool isShow)
    {
        textIntroduce.gameObject.SetActive(isShow);
        textBoosterDescription.gameObject.SetActive(!isShow);
        textBoosterName.gameObject.SetActive(!isShow);
    }

    private void SetActiveBoosters()
    {
        for (int i = 0; i < selectedBoosters.Length; i++)
        {
            GameObject obj = selectedBoosters[i];
            BoosterType type = (BoosterType)(int.Parse(obj.name));

            if (GameDataNEW.selectingBoosters.Contains(type))
            {
                if (obj.activeInHierarchy == false)
                {
                    obj.SetActive(false);
                    obj.SetActive(true);
                }
            }
            else
            {
                obj.SetActive(false);
            }
        }
    }

    private void SetQuestInfo()
    {
        //questDescriptions[0].transform.parent.gameObject.SetActive(true);

        //for (int i = 0; i < 3; i++)
        //{
        //    string s = GameController.Instance.CampaignMap.quest.GetDescription(i).ToUpper();
        //    questDescriptions[i].text = s;
        //}
    }

    private void SetPlayerInfo()
    {
        // Rambo level
        int ramboId = ProfileManager.UserProfile.ramboId;
        //StaticRamboData ramboData = GameDataNEW.staticRamboData.GetData(ramboId);

        int ramboLevel = GameDataNEW.playerRambos.ContainsKey(ramboId) ? GameDataNEW.playerRambos[ramboId].level : 0;
        textRamboLevel.text = string.Format("Lv: {0}", ramboLevel);

        // Grenades
        int grenadeId = ProfileManager.UserProfile.grenadeId;
        int quantity = GameDataNEW.playerGrenades.GetQuantityHave(grenadeId);
        textGrenadeQuantity.text = string.Format("x{0:n0}", quantity);
    }

    private void SetStageInfo()
    {
        textStageNameId.text = string.Format("STAGE {0}", GameDataNEW.currentStage.id);

        textDifficulty.text = GameDataNEW.currentStage.difficulty.ToString();

        switch (GameDataNEW.currentStage.difficulty)
        {
            case Difficulty.Normal:
                textDifficulty.color = colorDifficultyNormal;
                break;

            case Difficulty.Hard:
                textDifficulty.color = colorDifficultyHard;
                break;

            case Difficulty.Crazy:
                textDifficulty.color = colorDifficultyCrazy;
                break;
        }

    }

    private void SetBuyAmmoInfo()
    {
        int specialGunId = ProfileManager.UserProfile.gunSpecialId;
        labelNoSpecialGun.SetActive(specialGunId == -1);
        imgGun.transform.parent.gameObject.SetActive(specialGunId != -1);

        if (specialGunId != -1)
        {
            imgGun.sprite = GameResourcesUtils.GetGunImage(specialGunId);
            imgGun.SetNativeSize();

            StaticGunData gunData = GameDataNEW.staticGunData.GetData(specialGunId);
            int gunSpecialLevel = GameDataNEW.playerGuns.GetGunLevel(specialGunId);
            int curAmmo = GameDataNEW.playerGuns.GetGunAmmo(specialGunId);
            SO_GunStats baseStats = GameDataNEW.staticGunData.GetBaseStats(specialGunId, gunSpecialLevel);
            costBuyFullAmmo = (baseStats.Ammo - curAmmo) * gunData.ammoPrice;
            maxAmmoCount = baseStats.Ammo;

            textMaxAmmo.text = maxAmmoCount.ToString("n0");
            textCurrentAmmo.text = curAmmo.ToString("n0");
            textCurrentAmmo.color = (maxAmmoCount - curAmmo) > 0 ? StaticValue.color32NotEnoughMoney : colorFullAmmo;

            textPriceAmmo.text = costBuyFullAmmo.ToString("n0");
            textPriceAmmo.color = GameDataNEW.playerResources.coin >= costBuyFullAmmo ? Color.white : StaticValue.colorNotEnoughMoney;

            btnBuyFullAmmo.enabled = GameDataNEW.playerResources.coin >= costBuyFullAmmo;
        }
    }
}
