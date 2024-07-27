using UnityEngine;
using System.Collections;
//using Facebook.Unity;
using UnityEngine.UI;
//using UnityEngine.Advertisements;

public class HudPause : MonoBehaviour
{
    public GameObject popupPause;
    public GameObject popupPauseCampaign;
    public GameObject popupPauseSurvival;
    public Text stageNameId;


    public void Open()
    {
        popupPause.SetActive(true);

        popupPauseCampaign.SetActive(GameDataNEW.mode == GameMode.Campaign);
        popupPauseSurvival.SetActive(GameDataNEW.mode == GameMode.Survival);

        if (GameDataNEW.mode == GameMode.Campaign)
        {
            stageNameId.text = string.Format("STAGE {0} - {1}", GameDataNEW.currentStage.id, GameDataNEW.currentStage.difficulty.ToString().ToUpper());
        }

        Pause();
    }

    public void Pause()
    {
        //jamilGoogleAllAds.Instance.InterstitialAdPlz();
       // UnityAdsManager.instance.ShowNonRewardedAd();
        GameController.Instance.modeController.PauseGame();
    }

    public void Leave()
    {
        if (GameDataNEW.mode == GameMode.Campaign)
        {
            Time.timeScale = 1f;

            //if (!ProfileManager.UserProfile.isRemoveAds)
            //{
            //    bool IsInterReady = Advertisements.Instance.IsInterstitialAvailable();
            //    if (IsInterReady)
            //    {
            //        Advertisements.Instance.ShowInterstitial(InterstitialClosed2);

            //    }
            //    else if (Advertisement.IsReady())
            //    {
            //        Advertisement.Show();
            //        UIController.Instance.BackToMainMenu();

            //    }

            //    //AdMobController.Instance.ShowInterstitialAd(() =>
            //    //{
            //    //    UIController.Instance.BackToMainMenu();
            //    //});
            //}
            //else
            //UnityAdsManager.instance.ShowNonRewardedAd();
            UIController.Instance.BackToMainMenu();
            //}
        }
        else if (GameDataNEW.mode == GameMode.Survival)
        {
            //if (AccessToken.CurrentAccessToken != null)
            //{
            //    Time.timeScale = 1f;

            //    Popup.Instance.Show(
            //        content: "do you really want to quit?\nyour score will be saved.",
            //        type: PopupType.YesNo,
            //        yesCallback: () =>
            //        {
            //            EventDispatcher.Instance.PostEvent(EventID.QuitSurvivalSession);
            //        });
            //}
            //else
            //{
           // UnityAdsManager.instance.ShowNonRewardedAd();
            UIController.Instance.BackToMainMenu();
            //}
        }
    }

    public void Retry()
    {
        if (GameDataNEW.mode == GameMode.Campaign)
        {
            Time.timeScale = 1f;

            //if (!ProfileManager.UserProfile.isRemoveAds)
            //{
            //    bool IsInterReady =  Advertisements.Instance.IsInterstitialAvailable();
            //    if(IsInterReady)
            //    {
            //        Advertisements.Instance.ShowInterstitial(InterstitialClosed);

            //    }
            //    else if(Advertisement.IsReady())
            //    {
            //        Advertisement.Show();
            //        UIController.Instance.Retry();

            //    }
            //    //AdMobController.Instance.ShowInterstitialAd(() =>
            //    //{
            //    //    UIController.Instance.Retry();
            //    //});
            //}
            //else
            //{
           // UnityAdsManager.instance.ShowNonRewardedAd();
            UIController.Instance.Retry();
            //}
        }
    }
   
    public void Resume()
    {
       // UnityAdsManager.instance.ShowNonRewardedAd();
        GameController.Instance.modeController.ResumeGame();
        popupPause.SetActive(false);
    }
}
