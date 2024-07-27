//using Facebook.Unity;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public Text textVersion;
    public Slider sound;
    public Slider music;
    public GameObject SettingsBG;

    void OnEnable()
    {
        //if (FB.IsLoggedIn)
        //{
        //    textVersion.text = "ID: " + AccessToken.CurrentAccessToken.UserId;
        //}
        if(SettingsBG)
            SettingsBG.SetActive(true);
        sound.value = ProfileManager.UserProfile.soundVolume;
        music.value = ProfileManager.UserProfile.musicVolume;
    }

    void OnDisable()
    {
        ProfileManager.UserProfile.soundVolume.Set(sound.value);
        ProfileManager.UserProfile.musicVolume.Set(music.value);
        ProfileManager.SaveAll();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnAdjustSoundVolume()
    {
        SoundManager.Instance.AdjustSoundVolume(sound.value);
    }

    public void OnAdjustMusicVolume()
    {
        SoundManager.Instance.AdjustMusicVolume(music.value);
    }

    //public void BackUpData()
    //{
    //    Hide();

    //    Popup.Instance.Show("Do you want to save current game data?", "CONFIRMATION", PopupType.YesNo, () =>
    //    {
    //        if (FB.IsLoggedIn)
    //        {
    //            ProcessBackupData();
    //        }
    //        else
    //        {
    //            FbController.Instance.LoginWithReadPermission(success =>
    //            {
    //                if (success)
    //                {
    //                    ProcessBackupData();
    //                }
    //                else
    //                {
    //                    Popup.Instance.ShowToastMessage("Login Facebook failed", ToastLength.Long);
    //                }
    //            });
    //        }
    //    });
    //}

    //public void AutoBackupData()
    //{
    //    if (FB.IsLoggedIn)
    //    {
    //        string fbId = AccessToken.CurrentAccessToken.UserId;

    //        FireBaseDatabase.Instance.AuthenWithFacebook(fbId, AccessToken.CurrentAccessToken.TokenString, authUser =>
    //        {
    //            if (authUser != null)
    //            {
    //                FireBaseDatabase.Instance.SaveUserData(fbId, complete =>
    //                {
    //                    DebugCustom.Log("Auto backup data: " + complete);
    //                });
    //            }
    //        });
    //    }
    //}

    //public void RestoreData()
    //{
    //    Hide();

    //    Popup.Instance.Show(
    //        "Do you want to replace current game data with previous one?",
    //        "CONFIRMATION",
    //        PopupType.YesNo,
    //        yesCallback: () =>
    //        {
    //            Popup.Instance.ShowInstantLoading();

    //            if (FB.IsLoggedIn)
    //            {
    //                ProcessRestoreData();
    //            }
    //            else
    //            {
    //                FbController.Instance.LoginWithReadPermission(success =>
    //                {
    //                    if (success)
    //                    {
    //                        ProcessRestoreData();
    //                    }
    //                    else
    //                    {
    //                        Popup.Instance.HideInstantLoading();
    //                        Popup.Instance.ShowToastMessage("Login Facebook failed", ToastLength.Long);
    //                    }
    //                });
    //            }
    //        },
    //        noCallback: () =>
    //        {
    //            Show();
    //        });
    //}

    //private void ProcessBackupData()
    //{
    //    string fbId = AccessToken.CurrentAccessToken.UserId;

    //    FireBaseDatabase.Instance.AuthenWithFacebook(fbId, AccessToken.CurrentAccessToken.TokenString, authUser =>
    //    {
    //        if (authUser != null)
    //        {
    //            FireBaseDatabase.Instance.SaveUserData(fbId, complete =>
    //            {
    //                if (complete)
    //                {
    //                    Popup.Instance.ShowToastMessage("Save game data successfully", ToastLength.Long);
    //                }
    //                else
    //                {
    //                    Popup.Instance.ShowToastMessage("Save game data error, please try again later. Sorry for this inconvinient", ToastLength.Long);
    //                }
    //            });
    //        }
    //        else
    //        {
    //            Popup.Instance.ShowToastMessage("Authentication failed", ToastLength.Long);
    //        }
    //    });
    //}

    //private void ProcessRestoreData()
    //{
    //    string fbId = AccessToken.CurrentAccessToken.UserId;

    //    FireBaseDatabase.Instance.AuthenWithFacebook(fbId, AccessToken.CurrentAccessToken.TokenString, authUser =>
    //    {
    //        if (authUser != null)
    //        {
    //            FireBaseDatabase.Instance.GetUserData(fbId, inventory =>
    //            {
    //                Popup.Instance.HideInstantLoading();

    //                if (inventory != null)
    //                {
    //                    ProfileManager.Load(inventory);
    //                    ProfileManager.SaveAll();
    //                    SceneManager.LoadScene(StaticValue.SCENE_LOGIN);
    //                }
    //                else
    //                {
    //                    DebugCustom.Log("Inventory empty");
    //                }
    //            });
    //        }
    //        else
    //        {
    //            Popup.Instance.HideInstantLoading();
    //            Popup.Instance.ShowToastMessage("Authentication failed", ToastLength.Long);
    //        }
    //    });
    //}


    #region CHEATING FOR TEST

    public void MaxData()
    {
        string s = Resources.Load<TextAsset>("JSON/TMP/max_campaign_progress").text;
        ProfileManager.UserProfile.playerCampaignProgessData.Set(string.Empty);
        ProfileManager.UserProfile.playerCampaignStageProgessData.Set(s);
        GameDataNEW.playerCampaignStageProgress = JsonConvert.DeserializeObject<_PlayerCampaignStageProgressData>(s);
        DebugCustom.Log("MaxPlayerCampaignProgess=" + JsonConvert.SerializeObject(GameDataNEW.playerCampaignStageProgress));

        s = Resources.Load<TextAsset>("JSON/TMP/max_campaign_reward_progress").text;
        ProfileManager.UserProfile.playerCampaignRewardProgessData.Set(s);
        GameDataNEW.playerCampaignRewardProgress = JsonConvert.DeserializeObject<_PlayerCampaignRewardProgressData>(s);
        DebugCustom.Log("MaxPlayerCampaignRewardProgess=" + JsonConvert.SerializeObject(GameDataNEW.playerCampaignRewardProgress));

        s = Resources.Load<TextAsset>("JSON/TMP/max_grenade_data").text;
        ProfileManager.UserProfile.playerGrenadeData.Set(s);
        GameDataNEW.playerGrenades = JsonConvert.DeserializeObject<_PlayerGrenadeData>(s);
        DebugCustom.Log("MaxPlayerGrenadeData=" + JsonConvert.SerializeObject(GameDataNEW.playerGrenades));

        s = Resources.Load<TextAsset>("JSON/TMP/max_gun_data").text;
        ProfileManager.UserProfile.playerGunData.Set(s);
        GameDataNEW.playerGuns = JsonConvert.DeserializeObject<_PlayerGunData>(s);
        DebugCustom.Log("MaxPlayerGunData=" + JsonConvert.SerializeObject(GameDataNEW.playerGuns));

        s = Resources.Load<TextAsset>("JSON/TMP/max_melee_weapon_data").text;
        ProfileManager.UserProfile.playerMeleeWeaponData.Set(s);
        GameDataNEW.playerMeleeWeapons = JsonConvert.DeserializeObject<_PlayerMeleeWeaponData>(s);
        DebugCustom.Log("MaxPlayerMeleeWeaponData=" + JsonConvert.SerializeObject(GameDataNEW.playerMeleeWeapons));

        s = Resources.Load<TextAsset>("JSON/TMP/max_resources_data").text;
        ProfileManager.UserProfile.playerResourcesData.Set(s);
        GameDataNEW.playerResources = JsonConvert.DeserializeObject<_PlayerResourcesData>(s);
        DebugCustom.Log("MaxPlayerResourcesData=" + JsonConvert.SerializeObject(GameDataNEW.playerResources));

        s = Resources.Load<TextAsset>("JSON/TMP/max_rambo_data").text;
        ProfileManager.UserProfile.playerRamboData.Set(s);
        GameDataNEW.playerRambos = JsonConvert.DeserializeObject<_PlayerRamboData>(s);
        DebugCustom.Log("MaxPlayerRamboData=" + JsonConvert.SerializeObject(GameDataNEW.playerRambos));

        s = Resources.Load<TextAsset>("JSON/TMP/max_booster_data").text;
        ProfileManager.UserProfile.playerBoosterData.Set(s);
        GameDataNEW.playerBoosters = JsonConvert.DeserializeObject<_PlayerBoosterData>(s);
        DebugCustom.Log("MaxPlayerBoosterData=" + JsonConvert.SerializeObject(GameDataNEW.playerBoosters));

        ProfileManager.SaveAll();

        gameObject.SetActive(false);
        SceneFading.Instance.FadeOutAndLoadScene(StaticValue.SCENE_MENU, false);
    }

    public void ResetData()
    {
        ProfileManager.DeleteAll();

        string s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_RESOURCES_DATA).text;
        ProfileManager.UserProfile.playerResourcesData.Set(s);
        GameDataNEW.playerResources = JsonConvert.DeserializeObject<_PlayerResourcesData>(s);
        DebugCustom.Log("PlayerResources=" + JsonConvert.SerializeObject(GameDataNEW.playerResources));

        s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_RAMBO_DATA).text;
        ProfileManager.UserProfile.playerRamboData.Set(s);
        ProfileManager.UserProfile.ramboId.Set(StaticValue.RAMBO_ID_JOHN);
        GameDataNEW.playerRambos = JsonConvert.DeserializeObject<_PlayerRamboData>(s);
        DebugCustom.Log("PlayerRambos=" + JsonConvert.SerializeObject(GameDataNEW.playerRambos));

        s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_GUN_DATA).text;
        ProfileManager.UserProfile.playerGunData.Set(s);
        ProfileManager.UserProfile.gunNormalId.Set(StaticValue.GUN_ID_UZI);
        ProfileManager.UserProfile.gunSpecialId.Set(-1);
        GameDataNEW.playerGuns = JsonConvert.DeserializeObject<_PlayerGunData>(s);
        DebugCustom.Log("PlayerGuns=" + JsonConvert.SerializeObject(GameDataNEW.playerGuns));

        s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_GRENADE_DATA).text;
        ProfileManager.UserProfile.playerGrenadeData.Set(s);
        ProfileManager.UserProfile.grenadeId.Set(StaticValue.GRENADE_ID_F1);
        GameDataNEW.playerGrenades = JsonConvert.DeserializeObject<_PlayerGrenadeData>(s);
        DebugCustom.Log("PlayerGrenades=" + JsonConvert.SerializeObject(GameDataNEW.playerGrenades));

        s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_MELEE_WEAPON_DATA).text;
        ProfileManager.UserProfile.playerMeleeWeaponData.Set(s);
        ProfileManager.UserProfile.meleeWeaponId.Set(StaticValue.MELEE_WEAPON_ID_KNIFE);
        GameDataNEW.playerMeleeWeapons = JsonConvert.DeserializeObject<_PlayerMeleeWeaponData>(s);
        DebugCustom.Log("PlayerMeleeWeapons=" + JsonConvert.SerializeObject(GameDataNEW.playerMeleeWeapons));

        s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_CAMPAIGN_STAGE_PROGRESS_DATA).text;
        ProfileManager.UserProfile.playerCampaignStageProgessData.Set(s);
        ProfileManager.UserProfile.playerCampaignProgessData.Set(string.Empty);
        GameDataNEW.playerCampaignStageProgress = JsonConvert.DeserializeObject<_PlayerCampaignStageProgressData>(s);
        DebugCustom.Log("PlayerCampaignProgress=" + JsonConvert.SerializeObject(GameDataNEW.playerCampaignStageProgress));

        s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_CAMPAIGN_REWARD_PROGRESS_DATA).text;
        ProfileManager.UserProfile.playerCampaignRewardProgessData.Set(s);
        GameDataNEW.playerCampaignRewardProgress = JsonConvert.DeserializeObject<_PlayerCampaignRewardProgressData>(s);
        DebugCustom.Log("PlayerCampaignRewardProgress=" + JsonConvert.SerializeObject(GameDataNEW.playerCampaignRewardProgress));

        s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_BOOSTER_DATA).text;
        ProfileManager.UserProfile.playerBoosterData.Set(s);
        GameDataNEW.playerBoosters = JsonConvert.DeserializeObject<_PlayerBoosterData>(s);
        DebugCustom.Log("PlayerBoosters=" + JsonConvert.SerializeObject(GameDataNEW.playerBoosters));

        s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_SELECTING_BOOSTER).text;
        ProfileManager.UserProfile.playerSelectingBooster.Set(s);
        GameDataNEW.selectingBoosters = JsonConvert.DeserializeObject<_PlayerSelectingBooster>(s);
        DebugCustom.Log("PlayerSelectingBooster=" + JsonConvert.SerializeObject(GameDataNEW.selectingBoosters));

        ProfileManager.SaveAll();

        gameObject.SetActive(false);
        SceneFading.Instance.FadeOutAndLoadScene(StaticValue.SCENE_MENU, false);
    }

    #endregion
}
