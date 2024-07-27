using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Networking;

public class GameHistoryPanel : MonoBehaviour
{
    public static string GameHistory = APIHolder.getBaseUrl() + "getUserGameHistory";
    public static string GameRewardHistory = APIHolder.getBaseUrl() + "getUserWinningHistory";
    public GameObject[] totalHistory;
    public TextMeshProUGUI[] dateText;
    public TextMeshProUGUI[] timeText;
    public TextMeshProUGUI[] roomTypeText;
    public TextMeshProUGUI[] feesText;
    public TextMeshProUGUI[] positionText;
    public GameObject noHistoryToShow;

    public List<GameHistoryInformation> gameHistoryAPIResponse;
    public List<RewardHistoryInformation> rewardHistoryAPIResponse;

    public bool gameWinning;
    // Start is called before the first frame update
    private void OnEnable()
    {
        for (int i = 0; i < totalHistory.Length; i++)
        {
            totalHistory[i].SetActive(false);
        }
        noHistoryToShow.SetActive(false);

        if (!gameWinning)
            StartCoroutine(getGameHistory());
        else
            StartCoroutine(getGameRewardHistory());
    }

    private IEnumerator getGameHistory()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(GameHistory))
        {
            string myHeaderValue = PlayerPrefs.GetString("Token");
            print(myHeaderValue);
            webRequest.SetRequestHeader("Authorization", myHeaderValue);
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("API Error: " + webRequest.error);
            }
            else
            {
                string jsonResult = webRequest.downloadHandler.text;
                Debug.Log("Received JSON: " + jsonResult);

                try
                {
                    Debug.Log("Try");
                    GameHistoryAPI apiResponse = JsonUtility.FromJson<GameHistoryAPI>(jsonResult);

                    if (apiResponse != null && apiResponse.gameHistory.Count > 0)
                    {
                        Debug.Log("In Up");
                        if (apiResponse.count > 20)
                        {
                            Debug.Log("In here");
                            gameHistoryAPIResponse = apiResponse.gameHistory;
                            int counter = 20;
                            for (int i = 0; i < counter; i++)
                            {
                                totalHistory[i].SetActive(true);
                                string timestamp = gameHistoryAPIResponse[i].time;
                                string[] dateTimeParts = timestamp.Split('T');

                                string date = dateTimeParts[0];
                                string time = dateTimeParts[1].Split('.')[0];
                                dateText[i].text = date;
                                timeText[i].text = time;
                                roomTypeText[i].text = gameHistoryAPIResponse[i].room;
                                feesText[i].text = gameHistoryAPIResponse[i].fee.ToString() + " GEMS";
                            }
                            //Debug.Log("Number of weapon items received: " + apiResponse.weapon.Count);
                        }
                        else if (apiResponse.count < 20)
                        {
                            Debug.Log("In here");
                            gameHistoryAPIResponse = apiResponse.gameHistory;
                            int counter = apiResponse.count;
                            for (int i = 0; i < counter; i++)
                            {
                                totalHistory[i].SetActive(true);
                                string timestamp = gameHistoryAPIResponse[i].time;
                                string[] dateTimeParts = timestamp.Split('T');

                                string date = dateTimeParts[0];
                                string time = dateTimeParts[1].Split('.')[0];
                                dateText[i].text = date;
                                timeText[i].text = time;
                                roomTypeText[i].text = gameHistoryAPIResponse[i].room;
                                feesText[i].text = gameHistoryAPIResponse[i].fee.ToString() + " GEMS";
                            }
                        }
                    }
                    else
                    {
                        noHistoryToShow.SetActive(true);
                        Debug.LogError("Response or GameHistory is NULL");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to deserialize JSON: " + e.Message);
                }
            }
        }

    }

    private IEnumerator getGameRewardHistory()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(GameRewardHistory))
        {
            string myHeaderValue = PlayerPrefs.GetString("Token");
            print(myHeaderValue);
            webRequest.SetRequestHeader("Authorization", myHeaderValue);
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("API Error: " + webRequest.error);
            }
            else
            {
                string jsonResult = webRequest.downloadHandler.text;
                Debug.Log("Received JSON: " + jsonResult);

                try
                {
                    Debug.Log("Try");
                    GameRewardHistoryAPI apiResponse = JsonUtility.FromJson<GameRewardHistoryAPI>(jsonResult);

                    if (apiResponse != null && apiResponse.winningHistory.Count > 0)
                    {
                        Debug.Log("In Up");
                        if (apiResponse.count > 20)
                        {
                            Debug.Log("In here");
                            rewardHistoryAPIResponse = apiResponse.winningHistory;
                            int counter = 20;
                            for (int i = 0; i < counter; i++)
                            {
                                totalHistory[i].SetActive(true);
                                string timestamp = rewardHistoryAPIResponse[i].time;
                                string[] dateTimeParts = timestamp.Split('T');

                                string date = dateTimeParts[0];
                                string time = dateTimeParts[1].Split('.')[0];
                                dateText[i].text = date;
                                timeText[i].text = time;
                                roomTypeText[i].text = rewardHistoryAPIResponse[i].room;
                                int pos = rewardHistoryAPIResponse[i].position;
                                if(pos < 2)
                                {
                                    feesText[i].text = rewardHistoryAPIResponse[i].reward.ToString() + " VRC";
                                }
                                else
                                    feesText[i].text = rewardHistoryAPIResponse[i].reward.ToString() + " GEMS";
                                positionText[i].text = rewardHistoryAPIResponse[i].position.ToString(); ;
                            }
                            //Debug.Log("Number of weapon items received: " + apiResponse.weapon.Count);
                        }
                        else if (apiResponse.count < 20)
                        {
                            Debug.Log("In here");
                            rewardHistoryAPIResponse = apiResponse.winningHistory;
                            int counter = apiResponse.count;
                            for (int i = 0; i < counter; i++)
                            {
                                totalHistory[i].SetActive(true);
                                string timestamp = rewardHistoryAPIResponse[i].time;
                                string[] dateTimeParts = timestamp.Split('T');

                                string date = dateTimeParts[0];
                                string time = dateTimeParts[1].Split('.')[0];
                                dateText[i].text = date;
                                timeText[i].text = time;
                                roomTypeText[i].text = rewardHistoryAPIResponse[i].room;
                                int pos = rewardHistoryAPIResponse[i].position;
                                if (pos < 2)
                                {
                                    feesText[i].text = rewardHistoryAPIResponse[i].reward.ToString() + " VRC";
                                }
                                else
                                    feesText[i].text = rewardHistoryAPIResponse[i].reward.ToString() + " GEMS";
                                positionText[i].text = rewardHistoryAPIResponse[i].position.ToString(); ;
                            }
                        }
                        else
                        {
                            noHistoryToShow.SetActive(true);
                            Debug.LogError("Response or GameHistory is NULL");
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to deserialize JSON: " + e.Message);
                }
            }
        }

    }
}

[Serializable]
public class GameHistoryAPI
{
    public bool status;
    public string message;
    public int count;
    public List<GameHistoryInformation> gameHistory;
}

[Serializable]
public class GameRewardHistoryAPI
{
    public bool status;
    public string message;
    public int count;
    public List<RewardHistoryInformation> winningHistory;
}

[Serializable]
public class GameHistoryInformation
{
    public string room;
    public int fee;
    public string time;
}

[Serializable]
public class RewardHistoryInformation
{
    public string room;
    public int position;
    public int reward;
    public string time;
}