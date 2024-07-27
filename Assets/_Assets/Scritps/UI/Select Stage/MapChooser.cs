using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MapChooser : MonoBehaviour
{
    public MapChooser MapChooserScript;
    public static WorldMapNavigation navigation;
    public StageInformation stageInfoController;

    [Header("MAP")]
    public int currentMapIndex = 0;
    public GameObject btnNextMap;
    public GameObject btnPreviousMap;
    public Text currentStar;
    public Text maxStar;
    public Image starProgress;
    public MapOverview[] mapOverviews;
    public CampaignBoxReward[] boxes;
    //public RectTransform[] mapRectTransforms;

    [Header("MAP PAGE")]
    public Sprite pageActive;
    public Sprite pageDeactive;
    public Image[] pageMap;

    private int totalMap;
    private int totalDifficulty;
    private string selectingStageId;
    private Difficulty currentDifficulty;

    public Button[] levelbutton;

    //private Vector2 leftPos = new Vector2(-725f, 0f);
    //private Vector2 displayPosition = Vector2.zero;
    //private Vector2 rightPos = new Vector2(725f, 0f);
    //private bool isTweening;
    //private float tweenSpeed = 0.5f;

    //public static StageData SelectingStageData;



    #region UNITY METHODS

    void Awake()
    {
        MapChooserScript.enabled = true;
        totalMap = Enum.GetNames(typeof(MapType)).Length;
        totalDifficulty = Enum.GetNames(typeof(Difficulty)).Length;

        for (int i = 0; i < mapOverviews.Length; i++)
        {
            mapOverviews[i].Init();
        }
    }
    //public void scrptonkro()
    //{
    //    MapChooserScript.enabled = true;
    //}
    void Start()
    {
        EventDispatcher.Instance.RegisterListener(EventID.ClickStageOnWorldMap, (sender, param) => ShowStageInformation((string)param));
        EventDispatcher.Instance.RegisterListener(EventID.ClaimCampaignBox, (sender, param) => OnClaimBoxReward((int)param));

        int level = PlayerPrefs.GetInt("locklevel");
        enablelevel(level);

    }

    void enablelevel(int level)
    {
        int unlocklevel = PlayerPrefs.GetInt("unlocklevel");
        Debug.Log("Unlock Level is:" + unlocklevel);
        for (int i = 0; i < levelbutton.Length; i++)
        {
            levelbutton[i].interactable = false;
        }
        
        if(unlocklevel >= level)
        {
            for (int i = 0; i <= unlocklevel; i++)
            {
                levelbutton[i].interactable = true;
            }
        }
    }

    void OnEnable()
    {
        string stage = string.Empty;
        MapType map;

        switch (navigation)
        {
            case WorldMapNavigation.None:
                stage = MapUtils.GetCurrentProgressStageId();
                map = MapUtils.GetMapType(stage);
                currentMapIndex = (int)map - 1;
                break;

            case WorldMapNavigation.NextStageFromGame:
                stage = MapUtils.GetNextStage(GameDataNEW.currentStage);
                map = MapUtils.GetMapType(stage);
                currentMapIndex = (int)map - 1;
                ShowStageInformation(stage);
                break;
        }

        navigation = WorldMapNavigation.None;
        UpdateWorldMapInformation();
    }

    void OnDisable()
    {
        if (stageInfoController)
            stageInfoController.Close();
    }

    #endregion

    public void NextMap()
    {
        SoundManager.Instance.PlaySfxClick();
        currentMapIndex++;
        UpdateWorldMapInformation();
    }

    public void PreviousMap()
    {
        SoundManager.Instance.PlaySfxClick();
        currentMapIndex--;
        UpdateWorldMapInformation();
    }

    private void ShowStageInformation(string stageId)
    {
        if (string.IsNullOrEmpty(stageId))
            return;

        stageInfoController.Open(stageId);
    }

    private void UpdateWorldMapInformation()
    {
        MapType mapType = GetMapType(currentMapIndex);

        for (int i = 0; i < totalMap; i++)
        {
            mapOverviews[i].Active(i == currentMapIndex);
            pageMap[i].sprite = i == currentMapIndex ? pageActive : pageDeactive;
        }

      
        int numberOfStages = MapUtils.GetNumberOfStage(mapType);
        int curStars = MapUtils.GetNumberOfStar(mapType);
        int totalStars = numberOfStages * totalDifficulty;
        maxStar.text = totalStars.ToString();
        currentStar.text = curStars.ToString();
        starProgress.fillAmount = Mathf.Clamp01((float)curStars / (float)(totalStars-numberOfStages));

        // Arrow
        btnNextMap.SetActive(currentMapIndex < mapOverviews.Length - 1);
        btnPreviousMap.SetActive(currentMapIndex > 0);

        // Box rewards
        if (GameDataNEW.playerCampaignRewardProgress.ContainsKey(mapType) == false)
        {
            GameDataNEW.playerCampaignRewardProgress.AddNewProgress(mapType);
        }

        this.StartActionEndOfFrame(LoadBoxRewardState);
    }

    private void LoadBoxRewardState()
    {
        MapType mapType = GetMapType(currentMapIndex);
        List<bool> rewardProgress = GameDataNEW.playerCampaignRewardProgress[mapType];
        int currentStar = MapUtils.GetNumberOfStar(mapType);

        for (int i = 0; i < boxes.Length; i++)
        {
            boxes[i].LoadState(currentStar, rewardProgress);
        }
    }

    private void OnClaimBoxReward(int index)
    {
        MapType mapType = GetMapType(currentMapIndex);

        List<RewardData> rewards = GameDataNEW.staticCampaignBoxRewardData.GetRewards(mapType, index);
        RewardUtils.Receive(rewards);
        Popup.Instance.ShowReward(rewards);
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_GET_REWARD);

        GameDataNEW.playerCampaignRewardProgress.ClaimReward(mapType, index);
        LoadBoxRewardState();

        
    }

    private MapType GetMapType(int mapIndex)
    {
        return (MapType)(mapIndex + 1);
    }
}
