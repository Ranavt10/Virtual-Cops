using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class Login : MonoBehaviour
{
    public Text textVersion;

    private void Awake()
    {
        LoadStaticData();
        LoadPlayerData();
        //LoadIapRewardData();
    }

    private void Start()
    {
        textVersion.text = "v" + MasterInfo.Instance.Version;
    }

    public void PlayAsGuest()
    {
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_EXPLOSIVE);
        // SceneFading.Instance.FadeOutAndLoadScene(StaticValue.SCENE_MENU);
        if (PlayerPrefs.GetInt("firsttime") == 0)
        {
            SceneFading.Instance.FadeOutAndLoadScene(StaticValue.Reg_scene);

        }
        else
        {
            Debug.Log("Login");
            //SceneFading.Instance.FadeOutAndLoadScene(StaticValue.Reg_scene);
            firebaseDB.instance.login();

        }

    }


    #region LOAD JSON DATA

    private void LoadStaticData()
    {
        LoadStaticGunData();
        LoadStaticGrenadeData();
        LoadStaticMeleeWeaponData();
        LoadStaticCampaignStageData();
        LoadStaticCampaignBoxRewardData();
        LoadStaticRecommendGunData();
        LoadStaticRamboData();
        LoadStaticRamboSkillData();
        LoadStaticBoosterData();
        LoadStaticDailyQuestData();
        LoadStaticAchievementData();
        LoadStaticRankData();
        LoadStaticFreeGiftData();
        LoadStaticTournamentRankData();

        LoadQuestDescription();
        LoadRankName();
        LoadGunValueGem();
        LoadCampaignStageLevelData();
    }

    private void LoadStaticGunData()
    {
        if (GameDataNEW.staticGunData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_STATIC_GUN_DATA);
           // GameDataNEW.staticGunData = JsonConvert.DeserializeObject<_StaticGunData>(textAsset.text);

            Dictionary<int, StaticGunData> dict = JsonConvert.DeserializeObject<Dictionary<int, StaticGunData>>(textAsset.text);
            List<KeyValuePair<int, StaticGunData>> myList = dict.ToList();
            myList = myList.OrderBy(x => x.Value.isSpecialGun).ThenBy(x => x.Value.index).ToList();
            dict = myList.ToDictionary(x => x.Key, x => x.Value);
            string s = JsonConvert.SerializeObject(dict);
            GameDataNEW.staticGunData = JsonConvert.DeserializeObject<_StaticGunData>(s);
        }

        DebugCustom.Log("StaticGunData=" + JsonConvert.SerializeObject(GameDataNEW.staticGunData));
    }

    private void LoadStaticGrenadeData()
    {
        if (GameDataNEW.staticGrenadeData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_STATIC_GRENADE_DATA);
            GameDataNEW.staticGrenadeData = JsonConvert.DeserializeObject<_StaticGrenadeData>(textAsset.text);
        }

        DebugCustom.Log("StaticGrenadeData=" + JsonConvert.SerializeObject(GameDataNEW.staticGrenadeData));
    }

    private void LoadStaticMeleeWeaponData()
    {
        if (GameDataNEW.staticMeleeWeaponData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_STATIC_MELEE_WEAPON_DATA);
            GameDataNEW.staticMeleeWeaponData = JsonConvert.DeserializeObject<_StaticMeleeWeaponData>(textAsset.text);
        }

        DebugCustom.Log("StaticMeleeWeaponData=" + JsonConvert.SerializeObject(GameDataNEW.staticMeleeWeaponData));
    }

    private void LoadStaticCampaignStageData()
    {
        if (GameDataNEW.staticCampaignStageData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_STATIC_CAMPAIGN_STAGE_DATA);
            GameDataNEW.staticCampaignStageData = JsonConvert.DeserializeObject<_StaticCampaignStageData>(textAsset.text);
        }

        DebugCustom.Log("StaticCampaignStageData=" + JsonConvert.SerializeObject(GameDataNEW.staticCampaignStageData));
    }

    private void LoadStaticCampaignBoxRewardData()
    {
        if (GameDataNEW.staticCampaignBoxRewardData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_STATIC_CAMPAIGN_BOX_REWARD_DATA);
            GameDataNEW.staticCampaignBoxRewardData = JsonConvert.DeserializeObject<_StaticCampaignBoxRewardData>(textAsset.text);
        }

        DebugCustom.Log("StaticCampaignBoxRewardData=" + JsonConvert.SerializeObject(GameDataNEW.staticCampaignBoxRewardData));
    }

    private void LoadStaticRamboData()
    {
        if (GameDataNEW.staticRamboData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_STATIC_RAMBO_DATA);
            GameDataNEW.staticRamboData = JsonConvert.DeserializeObject<_StaticRamboData>(textAsset.text);
        }

        DebugCustom.Log("StaticRamboData=" + JsonConvert.SerializeObject(GameDataNEW.staticRamboData));
    }

    private void LoadStaticRamboSkillData()
    {
        if (GameDataNEW.staticRamboSkillData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_STATIC_RAMBO_SKILL_DATA);
            GameDataNEW.staticRamboSkillData = JsonConvert.DeserializeObject<_StaticRamboSkillData>(textAsset.text);
        }

        DebugCustom.Log("StaticRamboSkillData=" + JsonConvert.SerializeObject(GameDataNEW.staticRamboSkillData));
    }

    private void LoadStaticBoosterData()
    {
        if (GameDataNEW.staticBoosterData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_STATIC_BOOSTER_DATA);
            GameDataNEW.staticBoosterData = JsonConvert.DeserializeObject<_StaticBoosterData>(textAsset.text);
        }

        DebugCustom.Log("StaticBoosterData=" + JsonConvert.SerializeObject(GameDataNEW.staticBoosterData));
    }

    private void LoadStaticDailyQuestData()
    {
        if (GameDataNEW.staticDailyQuestData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_STATIC_DAILY_QUEST_DATA);
            GameDataNEW.staticDailyQuestData = JsonConvert.DeserializeObject<_StaticDailyQuestData>(textAsset.text);
        }

        DebugCustom.Log("StaticDailyQuestData=" + JsonConvert.SerializeObject(GameDataNEW.staticDailyQuestData));
    }

    private void LoadStaticAchievementData()
    {
        if (GameDataNEW.staticAchievementData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_STATIC_ACHIEVEMENT_DATA);
            GameDataNEW.staticAchievementData = JsonConvert.DeserializeObject<_StaticAchievementData>(textAsset.text);
        }

        DebugCustom.Log("StaticAchievementData=" + JsonConvert.SerializeObject(GameDataNEW.staticAchievementData));
    }

    private void LoadStaticRankData()
    {
        if (GameDataNEW.staticRankData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_STATIC_RANK_DATA);
            GameDataNEW.staticRankData = JsonConvert.DeserializeObject<_StaticRankData>(textAsset.text);
        }

        DebugCustom.Log("StaticRankData=" + JsonConvert.SerializeObject(GameDataNEW.staticRankData));
    }

    private void LoadStaticFreeGiftData()
    {
        if (GameDataNEW.staticFreeGiftData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_STATIC_FREE_GIFT_DATA);
            GameDataNEW.staticFreeGiftData = JsonConvert.DeserializeObject<_StaticFreeGiftData>(textAsset.text);
        }

        DebugCustom.Log("StaticFreeGiftData=" + JsonConvert.SerializeObject(GameDataNEW.staticFreeGiftData));
    }

    private void LoadStaticTournamentRankData()
    {
        if (GameDataNEW.staticTournamentRankData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_STATIC_TOURNAMENT_RANK_DATA);
            GameDataNEW.staticTournamentRankData = JsonConvert.DeserializeObject<_StaticTournamentRankData>(textAsset.text);
        }

        DebugCustom.Log("StaticTournamentRankData=" + JsonConvert.SerializeObject(GameDataNEW.staticTournamentRankData));

        if (GameDataNEW.tournamentTopRankRewards == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_TOURNAMENT_TOP_RANK_REWARD);
            GameDataNEW.tournamentTopRankRewards = JsonConvert.DeserializeObject<Dictionary<int, List<RewardData>>>(textAsset.text);
        }

        DebugCustom.Log("TournamentRankRewards=" + JsonConvert.SerializeObject(GameDataNEW.tournamentTopRankRewards));
    }

    private void LoadStaticRecommendGunData()
    {
        if (GameDataNEW.staticRecommendGunData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_STATIC_RECOMMEND_GUN_DATA);
            GameDataNEW.staticRecommendGunData = JsonConvert.DeserializeObject<_StaticRecommendGunData>(textAsset.text);
            DebugCustom.Log("RecommendGunData=" + JsonConvert.SerializeObject(GameDataNEW.staticRecommendGunData));
        }
    }

    private void LoadQuestDescription()
    {
        if (GameDataNEW.questDescriptions == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_STAGE_QUEST_DESCRIPTION);
            GameDataNEW.questDescriptions = JsonConvert.DeserializeObject<Dictionary<string, string>>(textAsset.text);
        }
    }

    private void LoadRankName()
    {
        if (GameDataNEW.rankNames == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_RANK_NAME);
            GameDataNEW.rankNames = JsonConvert.DeserializeObject<Dictionary<int, string>>(textAsset.text);
        }
    }

    private void LoadGunValueGem()
    {
        if (GameDataNEW.gunValueGem == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_EXCHANGE_WEAPON_TO_GEM);
            GameDataNEW.gunValueGem = JsonConvert.DeserializeObject<Dictionary<int, int>>(textAsset.text);
        }
    }

    private void LoadCampaignStageLevelData()
    {
        if (GameDataNEW.campaignStageLevelData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_CAMPAIGN_STAGE_LEVEL_DATA);
            GameDataNEW.campaignStageLevelData = JsonConvert.DeserializeObject<Dictionary<string, int>>(textAsset.text);
        }
    }

    #endregion


    #region PLAYER DATA

    private void LoadPlayerData()
    {
        LoadPlayerProfile();
        LoadPlayerResourcesData();
        LoadPlayerRamboData();
        LoadPlayerRamboSkillData();
        LoadPlayerGunData();
        LoadPlayerGrenadeData();
        LoadPlayerMeleeWeaponData();
        LoadPlayerCampaignProgressData();
        LoadPlayerCampaignRewardProgressData();
        LoadPlayerBoosterData();
        LoadPlayerSelectingBooster();
        LoadPlayerDailyQuestData();
        LoadPlayerAchievementData();
        LoadPlayerTutorialData();

        ProfileManager.SaveAll();
    }

    private void LoadPlayerProfile()
    {
        string s;

        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerProfile))
        {
            s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_PROFILE).text;
            ProfileManager.UserProfile.playerProfile.Set(s);
        }
        else
        {
            s = ProfileManager.UserProfile.playerProfile;
        }

        GameDataNEW.playerProfile = JsonConvert.DeserializeObject<_PlayerProfile>(s);
        DebugCustom.Log("PlayerProfile=" + JsonConvert.SerializeObject(GameDataNEW.playerProfile));
    }

    private void LoadPlayerResourcesData()
    {
        string s;

        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerResourcesData))
        {
            s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_RESOURCES_DATA).text;
            ProfileManager.UserProfile.playerResourcesData.Set(s);
        }
        else
        {
            s = ProfileManager.UserProfile.playerResourcesData;
        }

        GameDataNEW.playerResources = JsonConvert.DeserializeObject<_PlayerResourcesData>(s);
        DebugCustom.Log("PlayerResources=" + JsonConvert.SerializeObject(GameDataNEW.playerResources));
    }

    private void LoadPlayerRamboData()
    {
        string s;

        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerRamboData))
        {
            s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_RAMBO_DATA).text;
            ProfileManager.UserProfile.playerRamboData.Set(s);
            ProfileManager.UserProfile.ramboId.Set(StaticValue.RAMBO_ID_JOHN);
        }
        else
        {
            s = ProfileManager.UserProfile.playerRamboData;
        }

        GameDataNEW.playerRambos = JsonConvert.DeserializeObject<_PlayerRamboData>(s);
        DebugCustom.Log("PlayerRambos=" + JsonConvert.SerializeObject(GameDataNEW.playerRambos));
    }

    private void LoadPlayerRamboSkillData()
    {
        string s;

        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerRamboSkillData))
        {
            s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_RAMBO_SKILL_DATA).text;
            ProfileManager.UserProfile.playerRamboSkillData.Set(s);
        }
        else
        {
            s = ProfileManager.UserProfile.playerRamboSkillData;
        }

        GameDataNEW.playerRamboSkills = JsonConvert.DeserializeObject<_PlayerRamboSkillData>(s);
        DebugCustom.Log("PlayerRamboSkills=" + JsonConvert.SerializeObject(GameDataNEW.playerRamboSkills));
    }

    private void LoadPlayerGunData()
    {
        string s;

        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerGunData))
        {
            s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_GUN_DATA).text;
            ProfileManager.UserProfile.playerGunData.Set(s);
            ProfileManager.UserProfile.gunNormalId.Set(StaticValue.GUN_ID_UZI);
            ProfileManager.UserProfile.gunSpecialId.Set(-1);
        }
        else
        {
            s = ProfileManager.UserProfile.playerGunData;
        }

        GameDataNEW.playerGuns = JsonConvert.DeserializeObject<_PlayerGunData>(s);
        DebugCustom.Log("PlayerGuns=" + JsonConvert.SerializeObject(GameDataNEW.playerGuns));
    }

    private void LoadPlayerGrenadeData()
    {
        string s;

        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerGrenadeData))
        {
            s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_GRENADE_DATA).text;
            ProfileManager.UserProfile.playerGrenadeData.Set(s);
            ProfileManager.UserProfile.grenadeId.Set(StaticValue.GRENADE_ID_F1);
        }
        else
        {
            s = ProfileManager.UserProfile.playerGrenadeData;
        }

        GameDataNEW.playerGrenades = JsonConvert.DeserializeObject<_PlayerGrenadeData>(s);
        DebugCustom.Log("PlayerGrenades=" + JsonConvert.SerializeObject(GameDataNEW.playerGrenades));

        // Remove Dragon-Nades
        int grenadeId = ProfileManager.UserProfile.grenadeId;
        if (GameDataNEW.staticGrenadeData.ContainsKey(grenadeId) == false)
        {
            GameDataNEW.playerGrenades.RemoveGrenade(grenadeId);
            ProfileManager.UserProfile.grenadeId.Set(StaticValue.GRENADE_ID_F1);
        }
    }

    private void LoadPlayerMeleeWeaponData()
    {
        string s;

        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerMeleeWeaponData))
        {
            s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_MELEE_WEAPON_DATA).text;
            ProfileManager.UserProfile.playerMeleeWeaponData.Set(s);
            ProfileManager.UserProfile.meleeWeaponId.Set(StaticValue.MELEE_WEAPON_ID_KNIFE);
        }
        else
        {
            s = ProfileManager.UserProfile.playerMeleeWeaponData;
        }

        GameDataNEW.playerMeleeWeapons = JsonConvert.DeserializeObject<_PlayerMeleeWeaponData>(s);
        DebugCustom.Log("PlayerMeleeWeapons=" + JsonConvert.SerializeObject(GameDataNEW.playerMeleeWeapons));
    }

    private void LoadPlayerCampaignProgressData()
    {
        string s;

        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerCampaignProgessData))
        {
            if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerCampaignStageProgessData))
            {
                s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_CAMPAIGN_STAGE_PROGRESS_DATA).text;
                ProfileManager.UserProfile.playerCampaignStageProgessData.Set(s);
            }
            else
            {
                s = ProfileManager.UserProfile.playerCampaignStageProgessData;
            }

            GameDataNEW.playerCampaignStageProgress = JsonConvert.DeserializeObject<_PlayerCampaignStageProgressData>(s);
        }
        else
        {
            GameDataNEW.playerCampaignProgress = JsonConvert.DeserializeObject<_PlayerCampaignProgressData>(ProfileManager.UserProfile.playerCampaignProgessData);
            DebugCustom.Log("PlayerCampaignProgress OLD=" + JsonConvert.SerializeObject(GameDataNEW.playerCampaignProgress));

            GameDataNEW.playerCampaignStageProgress = new _PlayerCampaignStageProgressData();

            foreach (KeyValuePair<Difficulty, PlayerCampaignProgressData> difficulty in GameDataNEW.playerCampaignProgress)
            {
                foreach (KeyValuePair<string, List<bool>> progress in difficulty.Value.stageProgress)
                {
                    if (GameDataNEW.playerCampaignStageProgress.ContainsKey(progress.Key))
                    {
                        GameDataNEW.playerCampaignStageProgress[progress.Key][(int)difficulty.Key] = true;
                    }
                    else
                    {
                        List<bool> listResult = new List<bool>();

                        for (int i = 0; i < 3; i++)
                        {
                            listResult.Add(i == (int)difficulty.Key);
                        }

                        GameDataNEW.playerCampaignStageProgress.Add(progress.Key, listResult);
                    }
                }
            }

            ProfileManager.UserProfile.playerCampaignProgessData.Set(string.Empty);
            ProfileManager.UserProfile.playerCampaignStageProgessData.Set(JsonConvert.SerializeObject(GameDataNEW.playerCampaignStageProgress));
        }

        DebugCustom.Log("PlayerCampaignProgress=" + JsonConvert.SerializeObject(GameDataNEW.playerCampaignStageProgress));
    }

    private void LoadPlayerCampaignRewardProgressData()
    {
        string s;

        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerCampaignRewardProgessData))
        {
            s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_CAMPAIGN_REWARD_PROGRESS_DATA).text;
            ProfileManager.UserProfile.playerCampaignRewardProgessData.Set(s);
        }
        else
        {
            s = ProfileManager.UserProfile.playerCampaignRewardProgessData;
        }

        GameDataNEW.playerCampaignRewardProgress = JsonConvert.DeserializeObject<_PlayerCampaignRewardProgressData>(s);
        DebugCustom.Log("PlayerCampaignRewardProgress=" + JsonConvert.SerializeObject(GameDataNEW.playerCampaignRewardProgress));
    }

    private void LoadPlayerBoosterData()
    {
        string s;

        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerBoosterData))
        {
            s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_BOOSTER_DATA).text;
            ProfileManager.UserProfile.playerBoosterData.Set(s);
        }
        else
        {
            s = ProfileManager.UserProfile.playerBoosterData;
        }

        GameDataNEW.playerBoosters = JsonConvert.DeserializeObject<_PlayerBoosterData>(s);
        DebugCustom.Log("PlayerBoosters=" + JsonConvert.SerializeObject(GameDataNEW.playerBoosters));
    }

    private void LoadPlayerSelectingBooster()
    {
        string s;

        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerSelectingBooster))
        {
            s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_SELECTING_BOOSTER).text;
            ProfileManager.UserProfile.playerSelectingBooster.Set(s);
        }
        else
        {
            s = ProfileManager.UserProfile.playerSelectingBooster;
        }

        GameDataNEW.selectingBoosters = JsonConvert.DeserializeObject<_PlayerSelectingBooster>(s);
        DebugCustom.Log("PlayerSelectingBooster=" + JsonConvert.SerializeObject(GameDataNEW.selectingBoosters));
    }

    private void LoadPlayerDailyQuestData()
    {
        string s;

        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerDailyQuestData))
        {
            s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_DAILY_QUEST_DATA).text;
            ProfileManager.UserProfile.playerDailyQuestData.Set(s);
        }
        else
        {
            s = ProfileManager.UserProfile.playerDailyQuestData;
        }

        GameDataNEW.playerDailyQuests = JsonConvert.DeserializeObject<_PlayerDailyQuestData>(s);
        DebugCustom.Log("PlayerDailyQuestData=" + JsonConvert.SerializeObject(GameDataNEW.playerDailyQuests));
    }

    private void LoadPlayerAchievementData()
    {
        string s;

        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerAchievementData))
        {
            s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_ACHIEVEMENT_DATA).text;
            ProfileManager.UserProfile.playerAchievementData.Set(s);
        }
        else
        {
            s = ProfileManager.UserProfile.playerAchievementData;
        }

        GameDataNEW.playerAchievements = JsonConvert.DeserializeObject<_PlayerAchievementData>(s);

        // Remove unused achievement 
        List<AchievementType> tmp = new List<AchievementType>();

        foreach (KeyValuePair<AchievementType, PlayerAchievementData> achievement in GameDataNEW.playerAchievements)
        {
            bool isNotUse = true;

            for (int i = 0; i < GameDataNEW.staticAchievementData.Count; i++)
            {
                StaticAchievementData staticData = GameDataNEW.staticAchievementData[i];

                if (staticData.type == achievement.Key)
                {
                    isNotUse = false;
                    break;
                }
            }

            if (isNotUse)
                tmp.Add(achievement.Key);
        }

        for (int i = 0; i < tmp.Count; i++)
        {
            GameDataNEW.playerAchievements.Remove(tmp[i]);
        }

        DebugCustom.Log("PlayerAchievementData=" + JsonConvert.SerializeObject(GameDataNEW.playerAchievements));
    }

    private void LoadPlayerTutorialData()
    {
        string s;

        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerTutorialData))
        {
            s = Resources.Load<TextAsset>(StaticValue.PATH_JSON_NEW_PLAYER_TUTORIAL_DATA).text;
            ProfileManager.UserProfile.playerTutorialData.Set(s);
        }
        else
        {
            s = ProfileManager.UserProfile.playerTutorialData;
        }

        GameDataNEW.playerTutorials = JsonConvert.DeserializeObject<_PlayerTutorialData>(s);

        if (GameDataNEW.playerCampaignStageProgress.Count > 0)
        {
            GameDataNEW.playerTutorials.SetComplete(TutorialType.WorldMap);
            GameDataNEW.playerTutorials.SetComplete(TutorialType.Booster);
            GameDataNEW.playerTutorials.SetComplete(TutorialType.ActionInGame);
        }

        if (GameDataNEW.playerRambos.GetRamboLevel(ProfileManager.UserProfile.ramboId) > 1)
        {
            GameDataNEW.playerTutorials.SetComplete(TutorialType.Character);
        }

        bool isUziLevel1 = GameDataNEW.playerGuns.GetGunLevel(StaticValue.GUN_ID_UZI) == 1;
        bool isNoHaveKamePower = GameDataNEW.playerGuns.ContainsKey(StaticValue.GUN_ID_KAME_POWER) == false;

        if (!isUziLevel1 || !isNoHaveKamePower)
        {
            GameDataNEW.playerTutorials.SetComplete(TutorialType.Weapon);
        }

        DebugCustom.Log("PlayerTutorialData=" + JsonConvert.SerializeObject(GameDataNEW.playerTutorials));
    }

    #endregion


    #region IAP DATA


    #endregion

    public void PrivacyPolicy()
    {
        Application.OpenURL("https://virtualcops.vzsolution.com/");
    }
}
