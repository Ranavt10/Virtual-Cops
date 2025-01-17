﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System;

public class MapUtils
{
    private static Dictionary<string, MapData> mapDatas = new Dictionary<string, MapData>();


    public static MapData GetMapData(string nameId)
    {
        if (mapDatas.ContainsKey(nameId))
        {
            return mapDatas[nameId];
        }
        else
        {
            string path = StaticValue.PATH_JSON_MAP_ENEMY_DATA + nameId;
            TextAsset textAsset = Resources.Load<TextAsset>(path);
            MapData mapData = JsonConvert.DeserializeObject<MapData>(textAsset.text);

            mapDatas.Add(nameId, mapData);

            return mapData;
        }
    }

    public static Map GetMapPrefab(string stageId)
    {
        return Resources.Load<Map>(StaticValue.PATH_MAP_PREFAB + stageId);
    }

    public static MapType GetMapType(string stageId)
    {
        string s = stageId.Split('.').First();
        int mapId = int.Parse(s);

        return (MapType)mapId;
    }

    public static string GetMapName(MapType mapType)
    {
        string name = string.Empty;

        switch (mapType)
        {
            case MapType.Map_1_Desert:
                name = StaticValue.NAME_MAP_1_DESERT;
                break;

            case MapType.Map_2_Lab:
                name = StaticValue.NAME_MAP_2_LAB;
                break;

            case MapType.Map_3_Jungle:
                name = StaticValue.NAME_MAP_3_JUNGLE;
                break;
        }

        return name;
    }

    public static string GetNextStage(StageData currentStage)
    {
        int s1 = int.Parse(currentStage.id.Split('.').First());
        int s2 = int.Parse(currentStage.id.Split('.').Last());

        string nextStageId = string.Empty;

        if (IsLastStageInMap(currentStage.id))
        {
            MapType mapType = GetMapType(currentStage.id);

            if (IsLastMap(mapType))
            {
                nextStageId = currentStage.id;
            }
            else
            {
                nextStageId = string.Format("{0}.{1}", s1 + 1, 1);
            }
        }
        else
        {
            nextStageId = string.Format("{0}.{1}", s1, s2 + 1);
        }

        return nextStageId;
    }

    public static string GetCurrentProgressStageId()
    {
       
        string id = string.Empty;

        if (GameDataNEW.playerCampaignStageProgress.Count <= 0)
        {
            // id = "1.1";
            id = PlayerPrefs.GetString("ID");
            
            // PlayerPrefs.GetFloat(ID) = id;
        }
        else
        {
            
            string highestStagePassed = GameDataNEW.playerCampaignStageProgress.Last().Key;
            int s1 = int.Parse(highestStagePassed.Split('.').First());
            int s2 = int.Parse(highestStagePassed.Split('.').Last());

            if (IsLastStageInMap(highestStagePassed))
            {
                MapType mapType = GetMapType(highestStagePassed);

                if (IsLastMap(mapType))
                {
                    id = highestStagePassed;
                }
                else
                {
                    id = string.Format("{0}.{1}", s1 + 1, 1);
                }
            }
            else
            {
                id = string.Format("{0}.{1}", s1, s2 + 1);
            }
        }
        Debug.Log("currrrrrrrrrrrrrrrrrrrrrrrrrrrr  " + id);
        return id;
    }

    public static Difficulty GetHighestPlayableDifficulty(string stageId)
    {
        Difficulty difficulty = Difficulty.Normal;

        foreach (KeyValuePair<string, List<bool>> progress in GameDataNEW.playerCampaignStageProgress)
        {
            if (string.Compare(progress.Key, stageId) == 0)
            {
                for (int i = 0; i < progress.Value.Count; i++)
                {
                    if (progress.Value[i] == true && i >= (int)difficulty)
                    {
                        int dif = Mathf.Clamp(i + 1, 0, 2);
                        difficulty = (Difficulty)dif;
                    }
                }
            }
        }

        return difficulty;
    }

    public static List<RewardData> GetFirstTimeRewards(string stageId, Difficulty difficulty)
    {
        for (int i = 0; i < GameDataNEW.staticCampaignStageData.Count; i++)
        {
            StaticCampaignStageData stageData = GameDataNEW.staticCampaignStageData[i];

            if (string.Compare(stageData.stageNameId, stageId) == 0)
            {
                return stageData.firstTimeRewards[difficulty];
            }
        }

        return null;
    }

    public static List<RewardData> GetStaticRewards(string stageId, Difficulty difficulty)
    {
        for (int i = 0; i < GameDataNEW.staticCampaignStageData.Count; i++)
        {
            StaticCampaignStageData stageData = GameDataNEW.staticCampaignStageData[i];

            if (string.Compare(stageData.stageNameId, stageId) == 0)
            {
                return stageData.rewards[difficulty];
            }
        }

        return null;
    }

    public static bool IsStagePassed(string stageId, Difficulty difficulty)
    {
        bool isPassed = GameDataNEW.playerCampaignStageProgress.ContainsKey(stageId)
            && GameDataNEW.playerCampaignStageProgress[stageId][(int)difficulty] == true;

        return isPassed;
    }

    public static void UnlockCampaignProgress(StageData stageData)
    {
       

        if (IsStagePassed(stageData.id, stageData.difficulty) == false)
        {
            if (GameDataNEW.playerCampaignStageProgress.ContainsKey(stageData.id))
            {
                GameDataNEW.playerCampaignStageProgress[stageData.id][(int)stageData.difficulty] = true;
            }
            else
            {
                List<bool> progress = new List<bool>(3);

                for (int i = 0; i < 3; i++)
                {
                    progress.Add(i == (int)stageData.difficulty);
                }

                GameDataNEW.playerCampaignStageProgress.Add(stageData.id, progress);
            }

            GameDataNEW.playerCampaignStageProgress.Save();
        }
    }

    public static int GetNumberOfStage(MapType map)
    {
        

        int count = 0;

        for (int i = 0; i < GameDataNEW.staticCampaignStageData.Count; i++)
        {
            string s = GameDataNEW.staticCampaignStageData[i].stageNameId.Split('.').First();

            if (int.Parse(s) == (int)map)
            {
                count++;
            }
        }

        return count;
    }

    public static int GetNumberOfStar(MapType map)
    {
        int count = 0;

        foreach (KeyValuePair<string, List<bool>> stage in GameDataNEW.playerCampaignStageProgress)
        {
            if (GetMapType(stage.Key) == map)
            {
                for (int i = 0; i < stage.Value.Count; i++)
                {
                    if (stage.Value[i])
                    {
                        count++;
                    }
                }
            }
        }

        return count;
    }

    public static int GetNumberOfStar(string stageId)
    {
        int count = 0;

        foreach (KeyValuePair<string, List<bool>> stage in GameDataNEW.playerCampaignStageProgress)
        {
            if (string.Compare(stage.Key, stageId) == 0)
            {
                for (int i = 0; i < stage.Value.Count; i++)
                {
                    if (stage.Value[i])
                    {
                        count++;
                    }
                }
            }
        }

        return count;
    }

    private static bool IsLastDifficulty(Difficulty difficulty)
    {
        int totalDifficulty = Enum.GetNames(typeof(Difficulty)).Length;

        return (int)difficulty == totalDifficulty - 1;
    }

    private static bool IsLastMap(MapType mapType)
    {
        int totalMap = Enum.GetNames(typeof(MapType)).Length;

        return (int)mapType == totalMap;
    }

    private static bool IsLastStageInMap(string stageId)
    {
        MapType mapType = GetMapType(stageId);
        int numberStage = GetNumberOfStage(mapType);

        string s = stageId.Split('.').Last();

        return (int.Parse(s) == numberStage);
    }
}
