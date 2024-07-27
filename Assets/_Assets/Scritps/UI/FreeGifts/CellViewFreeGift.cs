using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;


public class CellViewFreeGift : MonoBehaviour
{
    public int times;
    public RewardElement[] rewards;
    public GameObject labelAchieved;
    public Button btnWatch;
    public Text countDown;

    private List<RewardData> _rewardData;

    void OnEnable()
    {
        if (ProfileManager.UserProfile.countViewAdsFreeCoin == times - 1)
        {
            GameDataNEW.durationNextGift -= Mathf.RoundToInt(Time.realtimeSinceStartup - GameDataNEW.timeCloseFreeGift);
            UpdateState();
        }
    }

    void OnDisable()
    {
        GameDataNEW.timeCloseFreeGift = Time.realtimeSinceStartup;
    }

    public void Init()
    {
        EventDispatcher.Instance.RegisterListener(EventID.ViewAdsGetFreeCoin, (sender, param) => UpdateState());

        List<RewardData> rewardData = GameDataNEW.staticFreeGiftData.GetRewards(times);
        _rewardData = rewardData;

        for (int i = 0; i < rewards.Length; i++)
        {
            RewardElement element = rewards[i];

            if (i < rewardData.Count)
            {
                element.gameObject.SetActive(true);
                element.SetInformation(rewardData[i]);
            }
            else
            {
                element.gameObject.SetActive(false);
            }
        }

        UpdateState();
    }

    public void UpdateState()
    {
        int countViewAdsFreeGift = ProfileManager.UserProfile.countViewAdsFreeCoin;

        if (countViewAdsFreeGift >= times)
        {
            labelAchieved.SetActive(true);
            btnWatch.gameObject.SetActive(false);
        }
        else
        {
            labelAchieved.SetActive(false);

            if (countViewAdsFreeGift == times - 1)
            {
                if (GameDataNEW.durationNextGift > 0)
                {
                    if (gameObject.activeInHierarchy)
                    {
                        btnWatch.gameObject.SetActive(false);
                        countDown.transform.parent.gameObject.SetActive(true);

                        StopAllCoroutines();

                        StartCoroutine(StartCountDown(() =>
                        {
                            countDown.transform.parent.gameObject.SetActive(false);
                            btnWatch.gameObject.SetActive(true);
                            btnWatch.interactable = true;
                        }));
                    }
                }
                else
                {
                    countDown.transform.parent.gameObject.SetActive(false);
                    btnWatch.gameObject.SetActive(true);
                    btnWatch.interactable = true;
                }
            }
            else
            {
                btnWatch.gameObject.SetActive(true);
                btnWatch.interactable = false;
            }
        }
    }

    public void ViewAds()
    {
        SoundManager.Instance.PlaySfxClick();
        btnWatch.interactable = false;

        if (ProfileManager.UserProfile.countViewAdsFreeCoin >= times)
            return;
      
        
    }
    //private void HandleShowResult(UnityEngine.Advertisements.ShowResult result)
    //{
    //    switch (result)
    //    {
    //        case UnityEngine.Advertisements.ShowResult.Finished:
    //            int countViewAds = ProfileManager.UserProfile.countViewAdsFreeCoin;
    //            countViewAds++;
    //            ProfileManager.UserProfile.countViewAdsFreeCoin.Set(countViewAds);

    //            RewardUtils.Receive(_rewardData);
    //            Popup.Instance.ShowReward(_rewardData);
    //            GameDataNEW.durationNextGift = 5;
    //            EventDispatcher.Instance.PostEvent(EventID.ViewAdsGetFreeCoin);
    //            SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_GET_REWARD);

               
    //            break;
    //        case UnityEngine.Advertisements.ShowResult.Skipped:
    //            btnWatch.interactable = true;
    //            break;
    //        case UnityEngine.Advertisements.ShowResult.Failed:
    //            btnWatch.interactable = true;
    //            break;
    //    }
    //}
    private void CompleteMethod(bool completed, string advertiser)
    {
        Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
        if (completed == true)
        {
            int countViewAds = ProfileManager.UserProfile.countViewAdsFreeCoin;
            countViewAds++;
            ProfileManager.UserProfile.countViewAdsFreeCoin.Set(countViewAds);

            RewardUtils.Receive(_rewardData);
            Popup.Instance.ShowReward(_rewardData);
            GameDataNEW.durationNextGift = 5;
            EventDispatcher.Instance.PostEvent(EventID.ViewAdsGetFreeCoin);
            SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_GET_REWARD);

           // FirebaseAnalyticsHelper.LogEvent("N_GetFreeGift", times);
        }
        else
        {
            btnWatch.interactable = true;
        }
    }
    private IEnumerator StartCountDown(Action callback)
    {
        while (GameDataNEW.durationNextGift > 0)
        {
            int min = GameDataNEW.durationNextGift / 60;
            int sec = GameDataNEW.durationNextGift % 60;

            countDown.text = string.Format("{0:D2}:{1:D2}", min, sec);

            yield return StaticValue.waitOneSec;

            GameDataNEW.durationNextGift--;

            if (GameDataNEW.durationNextGift == 0)
            {
                callback();
                break;
            }
        }
    }
}
