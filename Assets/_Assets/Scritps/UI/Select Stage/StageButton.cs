using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class StageButton : MonoBehaviour
{

    public static StageButton instance; 
    public string stageNameId;
    public Button icon;
    public Sprite iconLock;
    public Sprite iconUnlock;
    public Sprite starLock;
    public Sprite starUnlock;
    public Text textStageName;
   // public GameObject focus;
    public Image[] stars;

    private bool isLock;
    // public MapChooser MapChooserScript;


    private void Awake()
    {
        instance = this;
    }
    public void SelectStage()
    {
        SoundManager.Instance.PlaySfxClick();

        if (isLock)
            return;

        EventDispatcher.Instance.PostEvent(EventID.ClickStageOnWorldMap, stageNameId);

        if (GameDataNEW.isShowingTutorial && string.Compare(stageNameId, "1.1") == 0)
        {
            EventDispatcher.Instance.PostEvent(EventID.SubStepSelectStage);
        }
    }

    

    
    public void stage1_1()
    {
        PlayerPrefs.SetString("ID", "1.1");
        PlayerPrefs.SetInt("getrewardlevel", 1);
        PlayerPrefs.SetInt("locklevel",0);
        SelectStage();
    }
    public void stage1_2()
    {
        PlayerPrefs.SetString("ID", "1.2");
        PlayerPrefs.SetInt("getrewardlevel", 2);
        PlayerPrefs.SetInt("locklevel", 1);
        SelectStage();
    }
    public void stage1_3()
    {
        PlayerPrefs.SetString("ID", "1.3");
        PlayerPrefs.SetInt("getrewardlevel", 3);
        PlayerPrefs.SetInt("locklevel", 2);
        //  MapChooserScript.enabled = true;
        SelectStage();
    }
    public void stage1_4()
    {
        PlayerPrefs.SetString("ID", "1.4");
        PlayerPrefs.SetInt("getrewardlevel", 4);
        PlayerPrefs.SetInt("locklevel", 3);

        SelectStage();
    }
    public void stage1_5()
    {
        PlayerPrefs.SetString("ID", "1.5");
        PlayerPrefs.SetInt("getrewardlevel", 5);
        PlayerPrefs.SetInt("locklevel", 4);
        SelectStage();
    }
    public void stage1_6()
    {
        PlayerPrefs.SetString("ID", "1.6");
        PlayerPrefs.SetInt("getrewardlevel", 6);
        PlayerPrefs.SetInt("locklevel", 5);
        SelectStage();
    }
    public void stage1_7()
    {
        PlayerPrefs.SetString("ID", "1.7");
        PlayerPrefs.SetInt("getrewardlevel", 7);
        PlayerPrefs.SetInt("locklevel", 6);
        SelectStage();
    }
    public void stage1_8()
    {
        PlayerPrefs.SetString("ID", "1.8");
        PlayerPrefs.SetInt("getrewardlevel", 8);
        PlayerPrefs.SetInt("locklevel", 7);
        SelectStage();
    }
    public void stage2_1()
    {
        PlayerPrefs.SetString("ID", "2.1");
        PlayerPrefs.SetInt("getrewardlevel", 9);
        PlayerPrefs.SetInt("locklevel", 8);
        SelectStage();
    }
    public void stage2_2()
    {
        PlayerPrefs.SetString("ID", "2.2");
        PlayerPrefs.SetInt("getrewardlevel", 10);
        PlayerPrefs.SetInt("locklevel", 9);
        SelectStage();
    }
    public void stage2_3()
    {
        PlayerPrefs.SetString("ID", "2.3");
        PlayerPrefs.SetInt("getrewardlevel", 11);
        PlayerPrefs.SetInt("locklevel", 10);
        SelectStage();
    }
    public void stage2_4()
    {
        PlayerPrefs.SetString("ID", "2.4");
        PlayerPrefs.SetInt("getrewardlevel", 12);
        PlayerPrefs.SetInt("locklevel", 11);
        SelectStage();
    }
    public void stage2_5()
    {
        PlayerPrefs.SetString("ID", "2.5");
        PlayerPrefs.SetInt("getrewardlevel", 13);
        PlayerPrefs.SetInt("locklevel", 12);
        SelectStage();
    }
    public void stage2_6()
    {
        PlayerPrefs.SetString("ID", "2.6");
        PlayerPrefs.SetInt("getrewardlevel", 14);
        PlayerPrefs.SetInt("locklevel", 13);
        SelectStage();
    }
    public void stage2_7()
    {
        PlayerPrefs.SetString("ID", "2.7");
        PlayerPrefs.SetInt("getrewardlevel", 15);
        PlayerPrefs.SetInt("locklevel",14);
        SelectStage();
    }
    public void stage2_8()
    {
        PlayerPrefs.SetString("ID", "2.8");
        PlayerPrefs.SetInt("getrewardlevel", 16);
        PlayerPrefs.SetInt("locklevel", 15);
        SelectStage();
    }
    public void stage3_1()
    {
        PlayerPrefs.SetString("ID", "3.1");
        PlayerPrefs.SetInt("getrewardlevel", 17);
        PlayerPrefs.SetInt("locklevel", 16);
        SelectStage();
    }
    public void stage3_2()
    {
        PlayerPrefs.SetString("ID", "3.2");
        PlayerPrefs.SetInt("getrewardlevel", 18);
        PlayerPrefs.SetInt("locklevel", 17);
        SelectStage();
    }
    public void stage3_3()
    {
        PlayerPrefs.SetString("ID", "3.3");
        PlayerPrefs.SetInt("getrewardlevel", 19);
        PlayerPrefs.SetInt("locklevel", 18);
        SelectStage();
    }
    public void stage3_4()
    {
        PlayerPrefs.SetString("ID", "3.4");
        PlayerPrefs.SetInt("getrewardlevel", 20);
        PlayerPrefs.SetInt("locklevel", 19);
        SelectStage();
    }
    public void stage3_5()
    {
        PlayerPrefs.SetString("ID", "3.5");
        PlayerPrefs.SetInt("getrewardlevel", 21);
        PlayerPrefs.SetInt("locklevel", 20);
        SelectStage();
    }
    public void stage3_6()
    {
        PlayerPrefs.SetString("ID", "3.6");
        PlayerPrefs.SetInt("getrewardlevel", 22);
        PlayerPrefs.SetInt("locklevel", 21);
        SelectStage();
    }
    public void stage3_7()
    {
        PlayerPrefs.SetString("ID", "3.7");
        PlayerPrefs.SetInt("getrewardlevel", 23);
        PlayerPrefs.SetInt("locklevel", 22);
        SelectStage();
    }
    public void stage3_8()
    {
        PlayerPrefs.SetString("ID", "3.8");
        PlayerPrefs.SetInt("getrewardlevel", 24);
        PlayerPrefs.SetInt("locklevel", 23);
        SelectStage();
    }
    public void Load()
    {
        textStageName.text = stageNameId;

        if (MapUtils.IsStagePassed(stageNameId, Difficulty.Normal))
        {
            icon.image.sprite = iconUnlock;

            List<bool> progress = GameDataNEW.playerCampaignStageProgress[stageNameId];

            int numberDifficultyPassed = 0;

            for (int i = 0; i < progress.Count; i++)
            {
                if (progress[i])
                {
                    numberDifficultyPassed++;
                }
            }

            ActiveStars(numberDifficultyPassed);
            isLock = false;
        }
        else
        {
            icon.image.sprite = iconLock;
            ActiveStars(0);
            isLock = false;
        }

        // Current focus stage
        string focusStageId = MapUtils.GetCurrentProgressStageId();
        MapType focusMapType = MapUtils.GetMapType(focusStageId);
        MapType map = MapUtils.GetMapType(stageNameId);

        if (string.Compare(focusStageId, stageNameId) == 0)
        {
            if (MapUtils.IsStagePassed(focusStageId, Difficulty.Normal) == false)
            {
                icon.image.sprite = iconUnlock;
                ActiveStars(0);
                isLock = false;
            }

           // focus.SetActive(true);
        }
        else if ((int)map < (int)focusMapType)
        {
            if (MapUtils.IsStagePassed(stageNameId, Difficulty.Normal) == false)
            {
                icon.image.sprite = iconUnlock;
                ActiveStars(0);
                isLock = false;
            }

           // focus.SetActive(false);
        }
        else
        {
           // focus.SetActive(false);
        }

        icon.image.SetNativeSize();
    }

    private void ActiveStars(int number)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].sprite = i < number ? starUnlock : starLock;
            stars[i].SetNativeSize();
        }
    }
}

