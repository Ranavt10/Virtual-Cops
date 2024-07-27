using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class StageInformation : MonoBehaviour
{
    public static StageInformation instance;
    public MapChooser MapChooserScript;
    public Text textStageNameId;
    public GameObject btnStartEnable;
    public GameObject btnStartDisable;
    public RewardElement[] rewardCells;
    public GameObject[] stars;
    public GameObject[] highlights;
    public GameObject[] locks;
    public GameObject[] ticks;

    private string stageId;
    private Difficulty selectingDifficulty;
    private Difficulty highestPlayableDifficulty;
    //jamil
    private int Vrcreward;
    int difficultyid;

    // PlayerPrefs.GetString("ID")
    //PlayerPrefs.GetInt("vrcLevel");
    private void Awake()
    {
        instance = this;
    }
    public void Open(string stageId)
    {
        MapChooserScript.enabled = true;
        this.stageId = stageId;

        textStageNameId.text = string.Format("STAGE {0}", stageId);
        highestPlayableDifficulty = MapUtils.GetHighestPlayableDifficulty(stageId);
        selectingDifficulty = highestPlayableDifficulty;

        List<bool> progress = GameDataNEW.playerCampaignStageProgress.GetProgress(stageId);

        for (int i = 0; i < 3; i++)
        {
            locks[i].SetActive(i > (int)highestPlayableDifficulty);
            ticks[i].SetActive(progress[i]);
        }

        int numberStar = MapUtils.GetNumberOfStar(stageId);
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].SetActive(i < numberStar);
        }

        SetInformation();

        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
   

   

    public void SelectDifficulty(int difficulty)
    {
        //if ((int)selectingDifficulty == difficulty)
        //    return;
        //jamil

       
            selectingDifficulty = (Difficulty)difficulty;
            difficultyid = (int)selectingDifficulty;
            SetInformation();
        
    }

    void checklevel()
    {
        int level = PlayerPrefs.GetInt("getrewardlevel");
        #region normal levels
        if (level == 1 && difficultyid==0)
        {
            print(Difficulty.Normal);
            PlayerPrefs.SetInt("vrcLevel", 1);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 1 && difficultyid==1)
        {
            print(Difficulty.Hard);
            PlayerPrefs.SetInt("vrcLevel", 2);
            print("VRCllllllllllll"+PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 1 && difficultyid==2)
        {
            print(Difficulty.Crazy);
            PlayerPrefs.SetInt("vrcLevel", 3);
            print("VRCllllllllllll"+PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 2 && difficultyid==0)
        {
            print(Difficulty.Normal);
            PlayerPrefs.SetInt("vrcLevel", 4);
            print("VRCllllllllllll"+PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 2 && difficultyid == 1)
        {
            print(Difficulty.Hard);
            PlayerPrefs.SetInt("vrcLevel", 5);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 2 && difficultyid == 2)
        {
            print(Difficulty.Crazy);
            PlayerPrefs.SetInt("vrcLevel", 6);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 3 && difficultyid == 0)
        {
            print(Difficulty.Normal);
            PlayerPrefs.SetInt("vrcLevel", 7);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 3 && difficultyid == 1)
        {
            print(Difficulty.Hard);
            PlayerPrefs.SetInt("vrcLevel", 8);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 3 && difficultyid == 2)
        {
            print(Difficulty.Crazy);
            PlayerPrefs.SetInt("vrcLevel", 9);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 4 && difficultyid == 0)
        {
            print(Difficulty.Normal);
            PlayerPrefs.SetInt("vrcLevel", 10);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 4 && difficultyid == 1)
        {
            print(Difficulty.Hard);
            PlayerPrefs.SetInt("vrcLevel", 11);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 4 && difficultyid == 2)
        {
            print(Difficulty.Crazy);
            PlayerPrefs.SetInt("vrcLevel", 12);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 5 && difficultyid == 0)
        {
            print(Difficulty.Normal);
            PlayerPrefs.SetInt("vrcLevel", 13);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 5 && difficultyid == 1)
        {
            print(Difficulty.Hard);
            PlayerPrefs.SetInt("vrcLevel", 14);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 5 && difficultyid == 2)
        {
            print(Difficulty.Crazy);
            PlayerPrefs.SetInt("vrcLevel", 15);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 6 && difficultyid == 0)
        {
            print(Difficulty.Normal);
            PlayerPrefs.SetInt("vrcLevel", 16);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 6 && difficultyid == 1)
        {
            print(Difficulty.Hard);
            PlayerPrefs.SetInt("vrcLevel", 17);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 6 && difficultyid == 2)
        {
            print(Difficulty.Crazy);
            PlayerPrefs.SetInt("vrcLevel", 18);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 7 && difficultyid == 0)
        {
            print(Difficulty.Normal);
            PlayerPrefs.SetInt("vrcLevel", 19);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 7 && difficultyid == 1)
        {
            print(Difficulty.Hard);
            PlayerPrefs.SetInt("vrcLevel", 20);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 7 && difficultyid == 2)
        {
            print(Difficulty.Crazy);
            PlayerPrefs.SetInt("vrcLevel", 21);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 8 && difficultyid == 0)
        {
            print(Difficulty.Normal);
            PlayerPrefs.SetInt("vrcLevel", 22);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 8 && difficultyid == 1)
        {
            print(Difficulty.Hard);
            PlayerPrefs.SetInt("vrcLevel", 23);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 8 && difficultyid == 2)
        {
            print(Difficulty.Crazy);
            PlayerPrefs.SetInt("vrcLevel", 24);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 9 && difficultyid == 0)
        {
            print(Difficulty.Normal);
            PlayerPrefs.SetInt("vrcLevel", 25);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 9 && difficultyid == 1)
        {
            print(Difficulty.Hard);
            PlayerPrefs.SetInt("vrcLevel", 26);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 9 && difficultyid == 2)
        {
            print(Difficulty.Crazy);
            PlayerPrefs.SetInt("vrcLevel", 27);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        } 
        else if (level == 10 && difficultyid == 0)
        {
            print(Difficulty.Normal);
            PlayerPrefs.SetInt("vrcLevel", 28);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 10 && difficultyid == 1)
        {
            print(Difficulty.Hard);
            PlayerPrefs.SetInt("vrcLevel", 29);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 10 && difficultyid == 2)
        {
            print(Difficulty.Crazy);
            PlayerPrefs.SetInt("vrcLevel", 30);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 11 && difficultyid == 0)
        {
            print(Difficulty.Normal);
            PlayerPrefs.SetInt("vrcLevel", 31);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 11 && difficultyid == 1)
        {
            print(Difficulty.Hard);
            PlayerPrefs.SetInt("vrcLevel", 32);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 11 && difficultyid == 2)
        {
            print(Difficulty.Crazy);
            PlayerPrefs.SetInt("vrcLevel", 33);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        } 
        else if (level == 12 && difficultyid == 0)
        {
            print(Difficulty.Normal);
            PlayerPrefs.SetInt("vrcLevel", 34);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 12 && difficultyid == 1)
        {
            print(Difficulty.Hard);
            PlayerPrefs.SetInt("vrcLevel", 35);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        } 
        else if (level == 12 && difficultyid == 2)
        {
            print(Difficulty.Crazy);
            PlayerPrefs.SetInt("vrcLevel", 36);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 13 && difficultyid == 0)
        {
            print(Difficulty.Normal);
            PlayerPrefs.SetInt("vrcLevel", 37);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 13 && difficultyid == 1)
        {
            print(Difficulty.Hard);
            PlayerPrefs.SetInt("vrcLevel", 38);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 13 && difficultyid == 2)
        {
            print(Difficulty.Crazy);
            PlayerPrefs.SetInt("vrcLevel", 39);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        } 
        else if (level == 14 && difficultyid == 0)
        {
            print(Difficulty.Normal);
            PlayerPrefs.SetInt("vrcLevel", 40);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 14 && difficultyid == 1)
        {
            print(Difficulty.Hard);
            PlayerPrefs.SetInt("vrcLevel", 41);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 14 && difficultyid == 2)
        {
            print(Difficulty.Crazy);
            PlayerPrefs.SetInt("vrcLevel", 42);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 15 && difficultyid == 0)
        {
            print(Difficulty.Normal);
            PlayerPrefs.SetInt("vrcLevel", 43);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 15 && difficultyid == 1)
        {
            print(Difficulty.Hard);
            PlayerPrefs.SetInt("vrcLevel", 44);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        } 
        else if (level == 15 && difficultyid == 2)
        {
            print(Difficulty.Crazy);
            PlayerPrefs.SetInt("vrcLevel", 45);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 16 && difficultyid == 0)
        {
            print(Difficulty.Normal);
            PlayerPrefs.SetInt("vrcLevel", 46);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 16 && difficultyid == 1)
        {
            print(Difficulty.Hard);
            PlayerPrefs.SetInt("vrcLevel", 47);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 16 && difficultyid == 2)
        {
            print(Difficulty.Crazy);
            PlayerPrefs.SetInt("vrcLevel", 48);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 17 && difficultyid == 0)
        {
            print(Difficulty.Normal);
            PlayerPrefs.SetInt("vrcLevel", 49);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 17 && difficultyid == 1)
        {
            print(Difficulty.Hard);
            PlayerPrefs.SetInt("vrcLevel", 50);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 17 && difficultyid == 2)
        {
            print(Difficulty.Crazy);
            PlayerPrefs.SetInt("vrcLevel", 51);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        } 
        else if (level == 18 && difficultyid == 0)
        {
            print(Difficulty.Normal);
            PlayerPrefs.SetInt("vrcLevel", 52);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 18 && difficultyid == 1)
        {
            print(Difficulty.Hard);
            PlayerPrefs.SetInt("vrcLevel", 53);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 18 && difficultyid == 2)
        {
            print(Difficulty.Crazy);
            PlayerPrefs.SetInt("vrcLevel", 54);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 19 && difficultyid == 0)
        {
            print(Difficulty.Normal);
            PlayerPrefs.SetInt("vrcLevel", 55);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 19 && difficultyid == 1)
        {
            print(Difficulty.Hard);
            PlayerPrefs.SetInt("vrcLevel", 56);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        } 
        else if (level == 19 && difficultyid == 2)
        {
            print(Difficulty.Crazy);
            PlayerPrefs.SetInt("vrcLevel", 57);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 20 && difficultyid == 0)
        {
            print(Difficulty.Normal);
            PlayerPrefs.SetInt("vrcLevel", 58);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 20 && difficultyid == 1)
        {
            print(Difficulty.Hard);
            PlayerPrefs.SetInt("vrcLevel", 59);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 20 && difficultyid == 2)
        {
            print(Difficulty.Crazy);
            PlayerPrefs.SetInt("vrcLevel", 60);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        } 
        else if (level == 21 && difficultyid == 0)
        {
            print(Difficulty.Normal);
            PlayerPrefs.SetInt("vrcLevel", 61);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        } 
        else if (level == 21 && difficultyid == 1)
        {
            print(Difficulty.Hard);
            PlayerPrefs.SetInt("vrcLevel", 62);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 21 && difficultyid == 2)
        {
            print(Difficulty.Crazy);
            PlayerPrefs.SetInt("vrcLevel", 63);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 22 && difficultyid == 0)
        {
            print(Difficulty.Normal);
            PlayerPrefs.SetInt("vrcLevel", 64);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 22 && difficultyid == 1)
        {
            print(Difficulty.Hard);
            PlayerPrefs.SetInt("vrcLevel", 65);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        } 
        else if (level == 22 && difficultyid == 2)
        {
            print(Difficulty.Crazy);
            PlayerPrefs.SetInt("vrcLevel", 66);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 23 && difficultyid == 0)
        {
            print(Difficulty.Normal);
            PlayerPrefs.SetInt("vrcLevel", 67);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 23 && difficultyid == 1)
        {
            print(Difficulty.Hard);
            PlayerPrefs.SetInt("vrcLevel", 68);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 23 && difficultyid == 2)
        {
            print(Difficulty.Crazy);
            PlayerPrefs.SetInt("vrcLevel", 69);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        } 
        else if (level == 24 && difficultyid == 0)
        {
            print(Difficulty.Normal);
            PlayerPrefs.SetInt("vrcLevel", 70);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        else if (level == 24 && difficultyid == 1)
        {
            print(Difficulty.Hard);
            PlayerPrefs.SetInt("vrcLevel", 71);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        } 
        else if (level == 24 && difficultyid == 2)
        {
            print(Difficulty.Crazy);
            PlayerPrefs.SetInt("vrcLevel", 72);
            print("VRCllllllllllll" + PlayerPrefs.GetInt("vrcLevel"));
        }
        #endregion



    }



    public void Play()
    {
        
            GameDataNEW.mode = GameMode.Campaign;
            StartMission();
      
        if (GameDataNEW.isShowingTutorial && string.Compare(stageId, "1.1") == 0)
        {
            EventDispatcher.Instance.PostEvent(EventID.CompleteStep, TutorialType.WorldMap);
        }
    }

    private void StartMission()
    {

        Vrcreward = PlayerPrefs.GetInt("vrcLevel");
        GameDataNEW.currentStage = new StageData(stageId, selectingDifficulty, Vrcreward);
        DebugCustom.Log(string.Format("Start stage={0}, difficulty={1}", GameDataNEW.currentStage.id, GameDataNEW.currentStage.difficulty));
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_START_MISSION);
        
            Loading.nextScene = StaticValue.SCENE_GAME_PLAY;
        
        
        Popup.Instance.loading.Show();
    }

    private void SetInformation()
    {
        // Highlight
        for (int i = 0; i < highlights.Length; i++)
        {
            highlights[i].SetActive(i == (int)selectingDifficulty);
        }

        // Rewards
        List<RewardData> rewards = new List<RewardData>();
        if (MapUtils.IsStagePassed(stageId, selectingDifficulty))
        {
            rewards = MapUtils.GetStaticRewards(stageId, selectingDifficulty);
        }
        else
        {
            rewards = MapUtils.GetFirstTimeRewards(stageId, selectingDifficulty);
        }

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

        btnStartEnable.SetActive(selectingDifficulty <= highestPlayableDifficulty);
        btnStartDisable.SetActive(selectingDifficulty > highestPlayableDifficulty);
        checklevel();
    }
}
