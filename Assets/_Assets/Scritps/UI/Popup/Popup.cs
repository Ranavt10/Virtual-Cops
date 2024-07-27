using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Popup : Singleton<Popup>
{
    public Setting setting;
   // public Rate rate;
    public Loading loading;
    public InstantLoading instantLoading;

    [Header("NOTICE POPUP")]
    public GameObject noticePopup;
    public GameObject btnYes;
    public GameObject btnNo;
    public GameObject btnOk;
    public Text textTitle;
    public Text textContent;

    [Header("TOAST MESSAGE")]
    public Animation toastAnim;
    public Text textToastContent;
    public GameObject toastmessageBox;

    [Header("REWARD")]
    public Text textRewardContent;
    public GameObject rewardPopup;
    public RewardElement[] rewardCells;

    public Image[] YellowFiller;
    public Image[] selectedButton;
    public GameObject[] selectedPanel;

    private int lastSelectedPanel;

    private string deleteUserRoute = APIHolder.getBaseUrl() + "auth/deletAccount";

    //[Header("RATING")]
    //public GameObject popupRate;

    public bool IsShowing { get { return noticePopup.activeSelf || setting.gameObject.activeSelf; } }
    public bool IsInstantLoading { get { return instantLoading.gameObject.activeSelf; } }

    private UnityAction yesCallback;
    private UnityAction noCallback;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

#if UNITY_EDITOR || UNITY_STANDALONE

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            GameDataNEW.playerProfile.ReceiveExp(50000);
        }
    }

#endif

    public void Show(
        string content,
        string title = "NOTICE",
        PopupType type = PopupType.Ok,
        UnityAction yesCallback = null,
        UnityAction noCallback = null)
    {
        textContent.text = content.ToUpper();
        textTitle.text = title.ToUpper();
        this.yesCallback = yesCallback;
        this.noCallback = noCallback;

        btnNo.gameObject.SetActive(type == PopupType.YesNo);
        btnYes.gameObject.SetActive(type == PopupType.YesNo);
        btnOk.gameObject.SetActive(type != PopupType.YesNo);

        textContent.gameObject.SetActive(true);
        rewardPopup.SetActive(false);
        //noticePopup.SetActive(true);//jamil is change into noticepopup false from true;

        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_SHOW_DIALOG, -20f);
    }

    public void ShowToastMessage(string content, ToastLength length = ToastLength.Normal)
    {
        toastmessageBox.SetActive(true);
        textToastContent.text = content.ToUpper();

        if (length == ToastLength.Normal)
        {
            toastAnim.Play("toast_message");
        }
        else
        {
            toastAnim.Play("toast_message_long");
        }
    }

    public void ShowReward(List<RewardData> rewards, string content = null, UnityAction yesCallback = null)
    {
        textTitle.text = "CONGRATULATIONS";
        textContent.gameObject.SetActive(false);
        textRewardContent.text = content == null ? "YOU'VE GOT REWARDS!" : content;

        btnNo.gameObject.SetActive(false);
        btnYes.gameObject.SetActive(false);
        btnOk.gameObject.SetActive(true);

        this.yesCallback = yesCallback;

        for (int i = 0; i < rewardCells.Length; i++)
        {
            RewardElement cell = rewardCells[i];

            cell.gameObject.SetActive(false);
            cell.gameObject.SetActive(i < rewards.Count);

            if (i < rewards.Count)
            {
                RewardData rw = rewards[i];
                cell.SetInformation(rw);
            }
        }

        rewardPopup.SetActive(true);
        //noticePopup.SetActive(true);

        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_GET_REWARD);
    }

    //public void ShowRateUs()
    //{
    //    rate.Show();
    //}

    public void ShowInstantLoading(int timeout = 15)
    {
        instantLoading.Show(timeout);
    }

    public void HideInstantLoading()
    {
        instantLoading.Hide();
    }

    public void Yes()
    {
        if (yesCallback != null)
            yesCallback();

        Hide();
        SoundManager.Instance.PlaySfxClick();
    }

    public void No()
    {
        if (noCallback != null)
            noCallback();

        Hide();
        SoundManager.Instance.PlaySfxClick();
    }

    public void Hide()
    {
        yesCallback = null;
        noCallback = null;

        noticePopup.SetActive(false);
        setting.gameObject.SetActive(false);
    }

    public void setSelectedItem(int num)
    {
        for(int i = 0; i < YellowFiller.Length; i++)
        {
            YellowFiller[i].enabled = false;
        }

        for(int i = 0; i < selectedButton.Length; i++)
        {
            selectedButton[i].color = new Color(1, 1, 1, 1);
        }

        for(int i=0; i< selectedPanel.Length; i++)
        {
            selectedPanel[i].SetActive(false);
        }

        YellowFiller[num].enabled = true;
        selectedButton[num].color = new Color(0, 0, 0, 1);
        selectedPanel[num].SetActive(true);

        PlayerPrefs.SetInt("SettingPanelSelected", num);

        lastSelectedPanel = num;
    }

    public void setSelectedItemTurnOff()
    {
        for (int i = 0; i < selectedPanel.Length; i++)
        {
            selectedPanel[i].SetActive(false);
        }
    }

    public void DeleteAccount()
    {
        StartCoroutine(deleteAccount());
    }
    
    private IEnumerator deleteAccount()
    {
        using (UnityWebRequest www = UnityWebRequest.Delete(deleteUserRoute))
        {
            string headerValue = PlayerPrefs.GetString("Token");
            www.SetRequestHeader("Authorization", headerValue);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                string jsonResponse = www.downloadHandler.text;
                //Debug.Log("Received Error Response: " + jsonResponse);
                var responseObject = JsonUtility.FromJson<DeleteAccountHolder>(jsonResponse);
                //Popup.Instance.ShowToastMessage(string.Format("Low Balance..."), ToastLength.Normal);
            }
            else
            {
                SceneManager.LoadScene(0);
                PlayerPrefs.DeleteAll();
                setSelectedItemTurnOff();
                setting.SettingsBG.SetActive(false);
                PlayerPrefs.SetInt("ResourcesLoading", 5);
                /*Debug.Log("DownloadText:" + www.downloadHandler.text);
                string jsonResponse = www.downloadHandler.text;

                // Parse the JSON response
                var responseObject = JsonUtility.FromJson<DeleteAccountHolder>(jsonResponse);

                Debug.Log("Response: " + responseObject.message);
                Debug.Log("Success: " + responseObject.success);*/

                /*if (responseObject != null)
                {
                    
                }
                else
                {
                    Debug.Log("Error parsing JSON response");
                }*/
            }
        }
    }
}

[Serializable]
public class DeleteAccountHolder
{
    public bool success;
    public string message;
}