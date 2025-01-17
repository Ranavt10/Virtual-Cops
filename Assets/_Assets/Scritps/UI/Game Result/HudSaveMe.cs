﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class HudSaveMe : MonoBehaviour
{
    public Button btnReviveByGem;
    public Button btnWatchAds;
    public Text textPrice;
    public Text textCountDown;
    public Image progress;
    public RectTransform head;
    public Color32 colorEnoughCoin;

    private int timeOut = 10;

    public void Open(float curProgress)
    {
        UIController.Instance.ActiveIngameUI(false);

        textPrice.text = StaticValue.COST_REVIVE_BY_GEM.ToString("n0");
        bool isEnoughGem = GameDataNEW.playerResources.gem >= StaticValue.COST_REVIVE_BY_GEM;
        textPrice.color = isEnoughGem ? colorEnoughCoin : StaticValue.color32NotEnoughMoney;
        btnReviveByGem.enabled = isEnoughGem;

        progress.fillAmount = curProgress;
        Vector2 v = head.anchoredPosition;
        v.x = curProgress * 500f;
        head.anchoredPosition = v;

        gameObject.SetActive(true);

        StartCoroutine(CountDown());
    }

    public void Close()
    {
        UIController.Instance.ActiveIngameUI(true);
        SoundManager.Instance.PlaySfxClick();
        StopAllCoroutines();
        gameObject.SetActive(false);
        EventDispatcher.Instance.PostEvent(EventID.GameEnd, false);
    }

    //public void WatchAdsToRevive()
    //{
    //    SoundManager.Instance.PlaySfxClick();
    //    btnWatchAds.interactable = false;
    //    bool IsrewardReady = Advertisements.Instance.IsRewardVideoAvailable();
    //    if(IsrewardReady)
    //    {
    //        Advertisements.Instance.ShowRewardedVideo(CompleteMethod);
    //    }
    //    else if  (Advertisement.IsReady("rewardedVideo"))
    //        {
    //            var options = new ShowOptions { resultCallback = HandleShowResult };
    //            Advertisement.Show("rewardedVideo", options);
    //        }

    //}
    //private void HandleShowResult(UnityEngine.Advertisements.ShowResult result)
    //{
    //    switch (result)
    //    {
    //        case UnityEngine.Advertisements.ShowResult.Finished:
    //            Debug.Log("The ad was successfully shown.");
    //            ReviveByAds();
    //            break;
    //        case UnityEngine.Advertisements.ShowResult.Skipped:
    //            btnWatchAds.interactable = true;
    //            break;
    //        case UnityEngine.Advertisements.ShowResult.Failed:
    //            btnWatchAds.interactable = true;
    //            break;
    //    }
    //}
    private void CompleteMethod(bool completed, string advertiser)
    {
        Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
        if (completed == true)
        {
            ReviveByAds();
        }
        else
        {
            btnWatchAds.interactable = true;
        }
    }

    private void ReviveByAds()
    {
        gameObject.SetActive(false);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        GameController.Instance.SetActiveAllUnits(true);
        EventDispatcher.Instance.PostEvent(EventID.ReviveByAds);

       // FirebaseAnalyticsHelper.LogEvent("N_ReviveByAds", string.Format("{0}-{1}", GameDataNEW.currentStage.id, GameDataNEW.currentStage.difficulty));
    }

    public void ReviveByGem()
    {
        SoundManager.Instance.PlaySfxClick();

        if (GameDataNEW.playerResources.gem >= StaticValue.COST_REVIVE_BY_GEM)
        {
            gameObject.SetActive(false);

            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;

            GameController.Instance.SetActiveAllUnits(true);
            GameDataNEW.playerResources.ConsumeGem(StaticValue.COST_REVIVE_BY_GEM);

            EventDispatcher.Instance.PostEvent(EventID.ReviveByGem);

          //  FirebaseAnalyticsHelper.LogEvent("N_ReviveByGem", string.Format("{0}-{1}", GameDataNEW.currentStage.id, GameDataNEW.currentStage.difficulty));
        }
    }

    private IEnumerator CountDown()
    {
        int remaining = timeOut;

        while (remaining > 0)
        {
            textCountDown.text = remaining.ToString();
            textCountDown.gameObject.SetActive(false);
            textCountDown.gameObject.SetActive(true);

            remaining--;
            yield return StaticValue.waitOneSec;
        }

        Close();
    }
}
