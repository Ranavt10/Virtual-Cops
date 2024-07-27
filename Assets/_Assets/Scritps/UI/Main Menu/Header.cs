using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Header : MonoBehaviour
{
    public Text playerName;
    public Text rankName;
    public Image rankIcon;
    public Text level;
    public RectTransform levelBar;
    public Text coin;
    public Text gem;
    public Text medal;
    public Text metatext;
    public ResourcesChangeText changeTextPrefab;

    public static ObjectPooling<ResourcesChangeText> poolTextChange = new ObjectPooling<ResourcesChangeText>();


    private void Start()
    {
        EventDispatcher.Instance.RegisterListener(EventID.ReceiveExp, (sender, param) => UpdatePlayerInfo());
        EventDispatcher.Instance.RegisterListener(EventID.ReceiveCoin, (sender, param) => ChangeValueCoin(true, (int)param));
        EventDispatcher.Instance.RegisterListener(EventID.ReceiveGem, (sender, param) => ChangeValueGem(true, (int)param));
        EventDispatcher.Instance.RegisterListener(EventID.ReceiveMedal, (sender, param) => ChangeValueMedal(true, (int)param));
        EventDispatcher.Instance.RegisterListener(EventID.ConsumeCoin, (sender, param) => ChangeValueCoin(false, (int)param));
        EventDispatcher.Instance.RegisterListener(EventID.ConsumeGem, (sender, param) => ChangeValueGem(false, (int)param));
        EventDispatcher.Instance.RegisterListener(EventID.ConsumeMedal, (sender, param) => ChangeValueMedal(false, (int)param));

        playerName.text = PlayerPrefs.GetString("PlayerName");
        FillData();
    }

    private void Update()
    {
        FillData();
    }

    public void FillData()
    {
        UpdateCoinText();
        UpdateGemText();
        UpdateMedalText();
        UpdatePlayerInfo();
        metaCoin();
    }


    #region Player

    private void UpdatePlayerInfo()
    {
        int playerLevel = GameDataNEW.playerProfile.level;

        level.text = string.Format("RANK LEVEL: {0}", playerLevel);
        rankName.text = GameDataNEW.staticRankData.GetRankName(playerLevel).ToUpper();
        rankIcon.sprite = GameResourcesUtils.GetRankImage(playerLevel);

        bool isMaxLevel = playerLevel >= GameDataNEW.staticRankData.Count;

        if (isMaxLevel)
        {
            Vector2 v = levelBar.sizeDelta;
            v.x = 147f;
            levelBar.sizeDelta = v;
        }
        else
        {
            int curExp = GameDataNEW.playerProfile.exp;
            int nextLevelExp = GameDataNEW.staticRankData.GetExpOfLevel(playerLevel + 1);
            float size = Mathf.Clamp(((float)curExp / (float)nextLevelExp) * 147f, 15f, 147f);
            Vector2 v = levelBar.sizeDelta;
            v.x = size;
            levelBar.sizeDelta = v;
        }
    }

    #endregion


    #region Coin
    private void UpdateCoinText()
    {
        coin.text = GameDataNEW.playerResources.coin.ToString("n0");
    }

    public void metaCoin()
    {
       // PlayerPrefs.SetFloat("Meta", 5);
        metatext.text = PlayerPrefs.GetFloat("Meta").ToString() +" VRC";
    }
    private void ChangeValueCoin(bool isReceive, int value)
    {
        UpdateCoinText();

        ResourcesChangeText changeText = poolTextChange.New();

        if (changeText == null)
        {
            changeText = Instantiate(changeTextPrefab) as ResourcesChangeText;
        }

        changeText.Active(isReceive, value, coin.rectTransform.position, transform);
    }
    #endregion


    #region Gem
    private void UpdateGemText()
    {
        gem.text = PlayerPrefs.GetFloat("gems").ToString();/*GameDataNEW.playerResources.gem.ToString("n0");*/
        Debug.Log("Withdrawable gems are:" + PlayerPrefs.GetFloat("withdrawable"));
    }

    private void ChangeValueGem(bool isReceive, int value)
    {
        UpdateGemText();

        ResourcesChangeText changeText = poolTextChange.New();

        if (changeText == null)
        {
            changeText = Instantiate(changeTextPrefab) as ResourcesChangeText;
        }

        changeText.Active(isReceive, value, gem.rectTransform.position, transform);
    }
    #endregion

    #region Medal
    private void UpdateMedalText()
    {
        medal.text = GameDataNEW.playerResources.medal.ToString("n0");
    }

    private void ChangeValueMedal(bool isReceive, int value)
    {
        UpdateMedalText();

        ResourcesChangeText changeText = poolTextChange.New();

        if (changeText == null)
        {
            changeText = Instantiate(changeTextPrefab) as ResourcesChangeText;
        }

        changeText.Active(isReceive, value, medal.rectTransform.position, transform);
    }
    #endregion
}
