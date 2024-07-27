
using UnityEngine;

public class ShareLink: MonoBehaviour
{

    public void share()
    {
        if (PlayerPrefs.GetInt("Guest") == 1)
        {
            //UnityAdsManager.instance.ShowNonRewardedAd();
            return;
        }
        else
        {
            firebaseDB.instance.jamilShareReferralLink();
        }
       
    }

}
