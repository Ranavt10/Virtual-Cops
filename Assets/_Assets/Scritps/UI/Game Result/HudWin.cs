using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;




public class HudWin : MonoBehaviour
{
    public Button btnRetry;
    public Button btnSelectStage;
    public Button btnHome;
    public Button btnNextStage;
    //public Button btnWatchAds;
    public Text textNotiButtonHome;
    public GameObject[] difficultyIcons;
    public GameObject[] stars;
    public RewardElement[] rewardCells;
    public Text MetaText;
    public TextMeshProUGUI rewardtext;
    public Animator winanimator;
    public string winreward;



    private List<RewardData> winRewards = new List<RewardData>();

    private static string levelWin = "levelWinning";
    private string levelwinningapi = APIHolder.getBaseUrl() + levelWin;/*"https://a978-72-255-51-32.ngrok-free.app/api/levelWinning";*/

    private firebaseDB firebaseDB;

    private void Awake()
    {
        firebaseDB = FindObjectOfType<firebaseDB>();
    }

    public void callapilll()
    {
        StartCoroutine(levelwinningRewardAdd("2"));
    }
    private IEnumerator levelwinningRewardAdd(string coin)
    {
        WWWForm form = new WWWForm();
        form.AddField("vrc", coin);

        using (UnityWebRequest www = UnityWebRequest.Post(levelwinningapi, form))
        {
            string headerValue = PlayerPrefs.GetString("Token");
            www.SetRequestHeader("Authorization", headerValue);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Request sent successfully!");
                string jsonResponse = www.downloadHandler.text;
                Debug.Log("Received: " + jsonResponse);

                // Parse the JSON response
                var responseObject = JsonUtility.FromJson<ResponseObjectWin>(jsonResponse);

                if (responseObject != null)
                {
                    Debug.Log("Message: " + responseObject.message);
                    Debug.Log("Value: " + responseObject.value);
                    winreward = responseObject.value;
                    MetaText.text = "YOUR REWARDS:" + winreward + " VRC";
                    rewardtext.text = MetaText.text;
                }
                else
                {
                    Debug.Log("Error parsing JSON response");
                }
            }
        }
    }

    // Create a class to represent the response object

    //private IEnumerator levelwinningRewardAdd(string coin)
    //{

    //    WWWForm form = new WWWForm();

    //    form.AddField("vrc", coin);


    //    using (UnityWebRequest www = UnityWebRequest.Post(levelwinningapi, form))
    //    {
    //        string headerValue = PlayerPrefs.GetString("Token");
    //        www.SetRequestHeader("Authorization", headerValue);


    //        yield return www.SendWebRequest();


    //        if (www.result != UnityWebRequest.Result.Success)
    //        {
    //            Debug.Log(www.error);

    //        }
    //        else
    //        {
    //            Debug.Log("Request sent successfully!");
    //            Debug.Log("Received: " + www.downloadHandler.text);

    //        }
    //    }
    //}

    IEnumerator WinAnimationplay()
    {
        yield return new WaitForSeconds(0.5f);
        winanimator.SetBool("Win", true);

    }
    public void Open(List<RewardData> rewards)
    {

        winRewards = rewards;
        gameObject.SetActive(true);

        StartCoroutine(WinAnimationplay());



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

        //ShowButtons(true);
        if (PlayerPrefs.GetInt("Guest") != 1)
        {
            checkstageandgiveReward();
            nextlevelunlock();

        }
        else
        {
            nextlevelunlock();
            MetaText.text = "Login For Win Reward";
            rewardtext.text = MetaText.text;
        }



        UIController.Instance.ActiveIngameUI(false);
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_TEXT_TYPING);


    }
    private void Update()
    {
        rewardtext.text = MetaText.text;
    }


    void nextlevelunlock()
    {
        int currentlevel = PlayerPrefs.GetInt("locklevel");
        int vrclevel = PlayerPrefs.GetInt("vrcLevel");
        int unlocklevel = PlayerPrefs.GetInt("unlocklevel");

        if (unlocklevel <= currentlevel)
        {
            if (currentlevel == 0 && vrclevel == 3)
            {
                PlayerPrefs.SetInt("unlocklevel", 1);
            }
            else if (currentlevel == 1 && vrclevel == 6)
            {
                PlayerPrefs.SetInt("unlocklevel", 2);
            }
            else if (currentlevel == 2 && vrclevel == 9)
            {
                PlayerPrefs.SetInt("unlocklevel", 3);
            }
            else if (currentlevel == 3 && vrclevel == 12)
            {
                PlayerPrefs.SetInt("unlocklevel", 4);
            }
            else if (currentlevel == 4 && vrclevel == 15)
            {
                PlayerPrefs.SetInt("unlocklevel", 5);
            }
            else if (currentlevel == 5 && vrclevel == 18)
            {
                PlayerPrefs.SetInt("unlocklevel", 6);
            }
            else if (currentlevel == 6 && vrclevel == 21)
            {
                PlayerPrefs.SetInt("unlocklevel", 7);
            }
            else if (currentlevel == 7 && vrclevel == 24)
            {
                PlayerPrefs.SetInt("unlocklevel", 8);
            }
            else if (currentlevel == 8 && vrclevel == 27)
            {
                PlayerPrefs.SetInt("unlocklevel", 9);
            }
            else if (currentlevel == 9 && vrclevel == 30)
            {
                PlayerPrefs.SetInt("unlocklevel", 10);
            }
            else if (currentlevel == 10 && vrclevel == 33)
            {
                PlayerPrefs.SetInt("unlocklevel", 11);
            }
            else if (currentlevel == 11 && vrclevel == 36)
            {
                PlayerPrefs.SetInt("unlocklevel", 12);
            }
            else if (currentlevel == 12 && vrclevel == 39)
            {
                PlayerPrefs.SetInt("unlocklevel", 13);
            }
            else if (currentlevel == 13 && vrclevel == 42)
            {
                PlayerPrefs.SetInt("unlocklevel", 14);
            }
            else if (currentlevel == 14 && vrclevel == 45)
            {
                PlayerPrefs.SetInt("unlocklevel", 15);
            }
            else if (currentlevel == 15 && vrclevel == 48)
            {
                PlayerPrefs.SetInt("unlocklevel", 16);
            }
            else if (currentlevel == 16 && vrclevel == 51)
            {
                PlayerPrefs.SetInt("unlocklevel", 17);
            }
            else if (currentlevel == 17 && vrclevel == 54)
            {
                PlayerPrefs.SetInt("unlocklevel", 18);
            }
            else if (currentlevel == 18 && vrclevel == 57)
            {
                PlayerPrefs.SetInt("unlocklevel", 19);
            }
            else if (currentlevel == 19 && vrclevel == 60)
            {
                PlayerPrefs.SetInt("unlocklevel", 20);
            }
            else if (currentlevel == 20 && vrclevel == 63)
            {
                PlayerPrefs.SetInt("unlocklevel", 21);
            }
            else if (currentlevel == 21 && vrclevel == 66)
            {
                PlayerPrefs.SetInt("unlocklevel", 22);
            }
            else if (currentlevel == 22 && vrclevel == 69)
            {
                PlayerPrefs.SetInt("unlocklevel", 23);
            }
            else if (currentlevel == 23 && vrclevel == 72)
            {
                PlayerPrefs.SetInt("unlocklevel", 24);
            }

        }


    }
    void checkstageandgiveReward()
    {

        int reward = PlayerPrefs.GetInt("vrcLevel");
        float meta = PlayerPrefs.GetFloat("Meta");
        float win = PlayerPrefs.GetFloat("win");

        if (reward == 1 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {

            //  PlayerPrefs.SetFloat("Meta", meta + 0.0010f);
            //  PlayerPrefs.SetFloat("win", win + 0.0010f);
            //  PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 2 More Levels For Reward";
            rewardtext.text = MetaText.text;


            // StartCoroutine(levelwinningRewardAdd("0.0010"));



        }
        else if (reward == 2 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 0.0015f);
            // PlayerPrefs.SetFloat("win", win + 0.0015f);
            // PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 1 More Level for Reward";
            rewardtext.text = MetaText.text;
            //StartCoroutine(levelwinningRewardAdd("0.0015"));
        }
        else if (reward == 3 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            PlayerPrefs.SetFloat("Meta", meta + 0.0045f);
            PlayerPrefs.SetFloat("win", win + 0.0045f);
            PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);


            StartCoroutine(levelwinningRewardAdd("1"));
        }
        else if (reward == 4 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 0.0025f);
            // PlayerPrefs.SetFloat("win", win + 0.0025f);
            // PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 2 More Levels For Reward";
            rewardtext.text = MetaText.text;

            // StartCoroutine(levelwinningRewardAdd("0.0025"));
        }
        else if (reward == 5 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 0.0030f);
            // PlayerPrefs.SetFloat("win", win + 0.0030f);
            // PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 1 More Levels For Reward";
            rewardtext.text = MetaText.text;

            // StartCoroutine(levelwinningRewardAdd("0.0030"));
        }

        else if (reward == 6 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            PlayerPrefs.SetFloat("Meta", meta + 0.0090f);
            PlayerPrefs.SetFloat("win", win + 0.0090f);
            PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);



            StartCoroutine(levelwinningRewardAdd("2"));
        }
        else if (reward == 7 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 0.0045f);
            // PlayerPrefs.SetFloat("win", win + 0.0045f);
            // PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 2 More Levels For Reward";
            rewardtext.text = MetaText.text;

            // StartCoroutine(levelwinningRewardAdd("0.0045"));
        }
        else if (reward == 8 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 0.0050f);
            // PlayerPrefs.SetFloat("win", win + 0.0050f);
            // PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 1 More Levels For Reward";
            rewardtext.text = MetaText.text;

            // StartCoroutine(levelwinningRewardAdd("0.0050"));
        }
        else if (reward == 9 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            PlayerPrefs.SetFloat("Meta", meta + 0.0135f);
            PlayerPrefs.SetFloat("win", win + 0.0135f);
            PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);


            StartCoroutine(levelwinningRewardAdd("3"));
        }
        else if (reward == 10 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 0.0060f);
            //  PlayerPrefs.SetFloat("win", win + 0.0060f);
            // PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 2 More Levels For Reward";
            rewardtext.text = MetaText.text;

            //  StartCoroutine(levelwinningRewardAdd("0.0060"));
        }
        else if (reward == 11 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 0.0070f);
            // PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 1 More Levels For Reward";
            rewardtext.text = MetaText.text;

            // StartCoroutine(levelwinningRewardAdd("0.0070"));
        }
        else if (reward == 12 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            PlayerPrefs.SetFloat("Meta", meta + 0.0180f);
            PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);


            StartCoroutine(levelwinningRewardAdd("4"));
        }
        else if (reward == 13 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            //PlayerPrefs.SetFloat("Meta", meta + 0.0090f);
            // PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 2 More Levels For Reward";
            rewardtext.text = MetaText.text;

            //StartCoroutine(levelwinningRewardAdd("0.0090"));
        }
        else if (reward == 14 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 0.0100f);
            // PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 1 More Levels For Reward";
            rewardtext.text = MetaText.text;

            // StartCoroutine(levelwinningRewardAdd("0.0100"));

        }
        else if (reward == 15 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            PlayerPrefs.SetFloat("Meta", meta + 0.0225f);
            PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);


            StartCoroutine(levelwinningRewardAdd("5"));

        }
        else if (reward == 16 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 0.0200f);
            // PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 2 More Levels For Reward";
            rewardtext.text = MetaText.text;

            // StartCoroutine(levelwinningRewardAdd("0.0200"));

        }
        else if (reward == 17 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 0.0250f);
            // PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 1 More Levels For Reward";
            rewardtext.text = MetaText.text;

            // StartCoroutine(levelwinningRewardAdd("0.0250"));

        }
        else if (reward == 18 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            PlayerPrefs.SetFloat("Meta", meta + 0.0270f);
            PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);


            StartCoroutine(levelwinningRewardAdd("6"));

        }
        else if (reward == 19 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 0.0350f);
            //PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 2 More Levels For Reward";
            rewardtext.text = MetaText.text;

            // StartCoroutine(levelwinningRewardAdd("0.0350"));

        }
        else if (reward == 20 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            //PlayerPrefs.SetFloat("Meta", meta + 0.0400f);
            // PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 1 More Levels For Reward";
            rewardtext.text = MetaText.text;

            // StartCoroutine(levelwinningRewardAdd("0.0400"));

        }
        else if (reward == 21 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            PlayerPrefs.SetFloat("Meta", meta + 0.0315f);
            PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);


            StartCoroutine(levelwinningRewardAdd("7"));

        }
        else if (reward == 22 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 0.0500f);
            //PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 2 More Levels For Reward";
            rewardtext.text = MetaText.text;

            //StartCoroutine(levelwinningRewardAdd("0.0500"));

        }
        else if (reward == 23 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 0.0550f);
            //PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 1 More Levels For Reward";
            rewardtext.text = MetaText.text;

            //StartCoroutine(levelwinningRewardAdd("0.0550"));

        }
        else if (reward == 24 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            PlayerPrefs.SetFloat("Meta", meta + 0.0360f);
            PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);


            StartCoroutine(levelwinningRewardAdd("8"));

        }
        else if (reward == 25 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 0.0650f);
            // PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 2 More Levels For Reward";
            rewardtext.text = MetaText.text;

            //  StartCoroutine(levelwinningRewardAdd("0.0650"));

        }
        else if (reward == 26 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 0.0700f);
            // PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 1 More Levels For Reward";
            rewardtext.text = MetaText.text;

            //  StartCoroutine(levelwinningRewardAdd("0.0700"));

        }
        else if (reward == 27 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            PlayerPrefs.SetFloat("Meta", meta + 0.0750f);
            PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);


            StartCoroutine(levelwinningRewardAdd("9"));

        }
        else if (reward == 28 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            //PlayerPrefs.SetFloat("Meta", meta + 0.0800f);
            // PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 2 More Levels For Reward";
            rewardtext.text = MetaText.text;

            // StartCoroutine(levelwinningRewardAdd("0.0800"));

        }
        else if (reward == 29 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 0.0850f);
            //  PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 1 More Levels For Reward";
            rewardtext.text = MetaText.text;

            // StartCoroutine(levelwinningRewardAdd("0.0850"));

        }
        else if (reward == 30 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            PlayerPrefs.SetFloat("Meta", meta + 0.0900f);
            PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);


            StartCoroutine(levelwinningRewardAdd("10"));

        }
        else if (reward == 31 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 0.0950f);
            // PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 2 More Levels For Reward";
            rewardtext.text = MetaText.text;

            // StartCoroutine(levelwinningRewardAdd("0.0950"));

        }
        else if (reward == 32 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 0.1000f);
            // PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 1 More Levels For Reward";
            rewardtext.text = MetaText.text;

            // StartCoroutine(levelwinningRewardAdd("0.1"));

        }
        else if (reward == 33 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            PlayerPrefs.SetFloat("Meta", meta + 0.15f);
            PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);


            StartCoroutine(levelwinningRewardAdd("11"));

        }
        else if (reward == 34 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 0.20f);
            // PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 2 More Levels For Reward";
            rewardtext.text = MetaText.text;

            // StartCoroutine(levelwinningRewardAdd("0.20"));

        }
        else if (reward == 35 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 0.25f);
            //PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 1 More Levels For Reward";
            rewardtext.text = MetaText.text;

            //StartCoroutine(levelwinningRewardAdd("0.25"));

        }
        else if (reward == 36 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            PlayerPrefs.SetFloat("Meta", meta + 0.30f);
            PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);


            StartCoroutine(levelwinningRewardAdd("12"));

        }
        else if (reward == 37 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            //  PlayerPrefs.SetFloat("Meta", meta + 0.35f);
            // PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 2 More Levels For Reward";
            rewardtext.text = MetaText.text;

            // StartCoroutine(levelwinningRewardAdd("0.35"));

        }
        else if (reward == 38 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 0.40f);
            //  PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 1 More Levels For Reward";
            rewardtext.text = MetaText.text;

            // StartCoroutine(levelwinningRewardAdd("0.40"));

        }
        else if (reward == 39 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + );
            PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);


            StartCoroutine(levelwinningRewardAdd("13"));

        }
        else if (reward == 40 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 0.50f);
            //  PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 2 More Levels For Reward";
            rewardtext.text = MetaText.text;

            //StartCoroutine(levelwinningRewardAdd("0.50"));

        }
        else if (reward == 41 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 0.55f);
            // PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 1 More Levels For Reward";
            rewardtext.text = MetaText.text;

            // StartCoroutine(levelwinningRewardAdd("0.55"));

        }
        else if (reward == 42 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            PlayerPrefs.SetFloat("Meta", meta + 0.60f);
            PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);


            StartCoroutine(levelwinningRewardAdd("14"));

        }
        else if (reward == 43 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 0.65f);
            //PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 2 More Levels For Reward";
            rewardtext.text = MetaText.text;


            // StartCoroutine(levelwinningRewardAdd("0.65"));

        }
        else if (reward == 44 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 0.70f);
            // PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 1 More Levels For Reward";
            rewardtext.text = MetaText.text;

            // StartCoroutine(levelwinningRewardAdd("0.70"));

        }
        else if (reward == 45 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            PlayerPrefs.SetFloat("Meta", meta + 0.75f);
            PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);


            StartCoroutine(levelwinningRewardAdd("15"));

        }
        else if (reward == 46 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            //PlayerPrefs.SetFloat("Meta", meta + 0.80f);
            //PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 2 More Levels For Reward";
            rewardtext.text = MetaText.text;

            // StartCoroutine(levelwinningRewardAdd("0.80"));

        }
        else if (reward == 47 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            //PlayerPrefs.SetFloat("Meta", meta + 0.85f);
            // PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 1 More Levels For Reward";
            rewardtext.text = MetaText.text;

            //StartCoroutine(levelwinningRewardAdd("0.85"));

        }
        else if (reward == 48 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            PlayerPrefs.SetFloat("Meta", meta + 0.90f);
            PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);


            StartCoroutine(levelwinningRewardAdd("16"));

        }
        else if (reward == 49 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 0.95f);
            // PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 2 More Levels For Reward";
            rewardtext.text = MetaText.text;

            // StartCoroutine(levelwinningRewardAdd("0.95"));

        }
        else if (reward == 50 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            //  PlayerPrefs.SetFloat("Meta", meta + 1f);
            //  PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 1 More Levels For Reward";
            rewardtext.text = MetaText.text;

            //  StartCoroutine(levelwinningRewardAdd("1"));

        }
        else if (reward == 51 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            PlayerPrefs.SetFloat("Meta", meta + 1f);
            PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);


            StartCoroutine(levelwinningRewardAdd("17"));

        }
        else if (reward == 52 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 1f);
            // PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 2 More Levels For Reward";
            rewardtext.text = MetaText.text;

            // StartCoroutine(levelwinningRewardAdd("1"));

        }
        else if (reward == 53 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            //  PlayerPrefs.SetFloat("Meta", meta + 1f);
            //  PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 1 More Levels For Reward";
            rewardtext.text = MetaText.text;

            // StartCoroutine(levelwinningRewardAdd("1"));

        }
        else if (reward == 54 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            PlayerPrefs.SetFloat("Meta", meta + 1f);
            PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);


            StartCoroutine(levelwinningRewardAdd("18"));

        }
        else if (reward == 55 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 1f);
            //PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 2 More Levels For Reward";
            rewardtext.text = MetaText.text;

            // StartCoroutine(levelwinningRewardAdd("1"));

        }
        else if (reward == 56 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            //PlayerPrefs.SetFloat("Meta", meta + 1f);
            //PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 1 More Levels For Reward";
            rewardtext.text = MetaText.text;

            //StartCoroutine(levelwinningRewardAdd("1"));

        }
        else if (reward == 57 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            PlayerPrefs.SetFloat("Meta", meta + 1f);
            PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);


            StartCoroutine(levelwinningRewardAdd("19"));

        }
        else if (reward == 58 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 1f);
            //  PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 2 More Levels For Reward";
            rewardtext.text = MetaText.text;

            // StartCoroutine(levelwinningRewardAdd("1"));

        }
        else if (reward == 59 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 1f);
            // PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 1 More Levels For Reward";
            rewardtext.text = MetaText.text;

            // StartCoroutine(levelwinningRewardAdd("1"));

        }
        else if (reward == 60 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            PlayerPrefs.SetFloat("Meta", meta + 1f);
            PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);


            StartCoroutine(levelwinningRewardAdd("20"));

        }
        else if (reward == 61 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            //PlayerPrefs.SetFloat("Meta", meta + 1f);
            // PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 2 More Levels For Reward";
            rewardtext.text = MetaText.text;

            // StartCoroutine(levelwinningRewardAdd("1"));

        }
        else if (reward == 62 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 1f);
            // PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 1 More Levels For Reward";
            rewardtext.text = MetaText.text;

            // StartCoroutine(levelwinningRewardAdd("1"));

        }
        else if (reward == 63 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            PlayerPrefs.SetFloat("Meta", meta + 1f);
            PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);


            StartCoroutine(levelwinningRewardAdd("21"));

        }
        else if (reward == 64 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 1f);
            //PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 2 More Levels For Reward";
            rewardtext.text = MetaText.text;

            // StartCoroutine(levelwinningRewardAdd("1"));

        }



        else if (reward == 65 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 1f);
            // PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 1 More Levels For Reward";
            rewardtext.text = MetaText.text;

            //StartCoroutine(levelwinningRewardAdd("1"));

        }
        else if (reward == 66 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            PlayerPrefs.SetFloat("Meta", meta + 1f);
            PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);


            StartCoroutine(levelwinningRewardAdd("22"));

        }
        else if (reward == 67 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 1f);
            // PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 2 More Levels For Reward";
            rewardtext.text = MetaText.text;

            //StartCoroutine(levelwinningRewardAdd("1"));

        }
        else if (reward == 68 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 1f);
            //  PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 1 More Levels For Reward";
            rewardtext.text = MetaText.text;

            // StartCoroutine(levelwinningRewardAdd("1"));

        }
        else if (reward == 69 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            PlayerPrefs.SetFloat("Meta", meta + 1f);
            PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "YOUR REWARDS:" + winreward + " VRC";
            rewardtext.text = MetaText.text;

            StartCoroutine(levelwinningRewardAdd("23"));

        }
        else if (reward == 70 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            //PlayerPrefs.SetFloat("Meta", meta + 1f);
            // PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 2 More Levels For Reward";
            rewardtext.text = MetaText.text;

            // StartCoroutine(levelwinningRewardAdd("1"));


        }
        else if (reward == 71 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            // PlayerPrefs.SetFloat("Meta", meta + 1f);
            // PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);
            MetaText.text = "Play 1 More Levels For Reward";
            rewardtext.text = MetaText.text;

            // StartCoroutine(levelwinningRewardAdd("1"));


        }
        else if (reward == 72 && PlayerPrefs.GetInt("RewardClaimed_" + reward) == 0)
        {
            PlayerPrefs.SetFloat("Meta", meta + 1f);
            PlayerPrefs.SetInt("RewardClaimed_" + reward, 1);


            StartCoroutine(levelwinningRewardAdd("24"));


        }
        else
        {
            MetaText.text = "You have already collect reward at this level";
            rewardtext.text = MetaText.text;
            //print("You have already collect reward at this level");
        }


    }

    public void SelectStage()
    {
        SoundManager.Instance.PlaySfxClick();
        //jamil GoogleAllAds.Instance.InterstitialAdPlz();

        //UnityAdsManager.instance.ShowNonRewardedAd();
        MainMenu.navigation = MainMenuNavigation.OpenWorldMap;
        MapChooser.navigation = WorldMapNavigation.None;
        SceneFading.Instance.FadeOutAndLoadScene(StaticValue.SCENE_MENU);
    }

    public void NextStage()
    {
        SoundManager.Instance.PlaySfxClick();
        MainMenu.navigation = MainMenuNavigation.OpenWorldMap;


        SceneFading.Instance.FadeOutAndLoadScene(StaticValue.SCENE_MENU);
    }

    public void BackToMainMenu()
    {
        //jamil  GoogleAllAds.Instance.InterstitialAdPlz();
        //  UnityAdsManager.instance.ShowNonRewardedAd();
        firebaseDB.cameBackFromGamePlay = true;
        SoundManager.Instance.PlaySfxClick();
        UIController.Instance.BackToMainMenu();
    }

    public void Retry()
    {
        Time.timeScale = 1f;


        SoundManager.Instance.PlaySfxClick();
        SceneFading.Instance.FadeOutAndLoadScene(StaticValue.SCENE_GAME_PLAY);
    }


}

[System.Serializable]
public class ResponseObjectWin
{
    public string message;
    public string value;
}




