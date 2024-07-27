using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RewardHistory2 : MonoBehaviour
{
    public GameObject historyItemPrefab;
    public Transform contentPanel;

    public GameObject[] spawnedObjects;
    private static string userLevelTwo = "userLevelTwosHistory";
    private string Rewardlevelonehistoryapi = APIHolder.getBaseUrl() + userLevelTwo;/*"https://a978-72-255-51-32.ngrok-free.app/api/userLevelTwosHistory";*/

    public List<LevelOneUser1> RewardHistoryList = new List<LevelOneUser1>();



    private void OnEnable()
    {
        if (PlayerPrefs.GetInt("Guest") != 1)
        {
            StartCoroutine(FetchHistoryDataLevelone());
        }
        else
        {
            //UnityAdsManager.instance.ShowNonRewardedAd();
            Popup.Instance.ShowToastMessage("Login For History");
        }

        
    }
    private void OnDisable()
    {
        for (int i = 0; i < spawnedObjects.Length; i++)
        {
            Destroy(spawnedObjects[i]);
        }
        RewardHistoryList.Clear();

        Array.Clear(spawnedObjects, 0, spawnedObjects.Length);
    }

    void gameobjectSpwinhistory()
    {

        if(RewardHistoryList.Count > 0)
        {
            spawnedObjects = new GameObject[RewardHistoryList.Count];


            for (int i = 0; i < RewardHistoryList.Count; i++)
            {
                GameObject spawnedObject = Instantiate(historyItemPrefab, contentPanel);

                spawnedObjects[i] = spawnedObject;

            }

            int currentIndex = 0;

            foreach (LevelOneUser1 item in RewardHistoryList)
            {
                spawnedObjects[currentIndex].GetComponent<RewarddataController1>().indextext.text = item.serialNumber.ToString();
                spawnedObjects[currentIndex].GetComponent<RewarddataController1>().UserNametext.text = item.username.ToString();
                spawnedObjects[currentIndex].GetComponent<RewarddataController1>().Rewardtext.text = item.reward.ToString();
                spawnedObjects[currentIndex].GetComponent<RewarddataController1>().datetimetext.text = item.date.ToString();

                currentIndex++;
            }
        }

        else
        {
            Popup.Instance.ShowToastMessage("No User Avaiable");
        }
       
    }

    IEnumerator FetchHistoryDataLevelone()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(Rewardlevelonehistoryapi))
        {
            string myHeaderValue = PlayerPrefs.GetString("Token");
            www.SetRequestHeader("Authorization", myHeaderValue);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("API Error: " + www.error);
            }
            else
            {
                string jsonResult = www.downloadHandler.text;
                print(jsonResult);

                try
                {
                    ApiResponse1 response = JsonUtility.FromJson<ApiResponse1>(jsonResult);
                    print("jjjjjjjjjjjjjjjjjjjjj " + response);
                    // Check if the object or any property within it is null
                    if (response != null)
                    {
                        if (response.levelOneUsers != null)
                        {
                            RewardHistoryList.Clear();
                            foreach (LevelOneUser1 user in response.levelOneUsers)
                            {
                                // Log each property to check if any of them is null
                                Debug.Log("ID: " + user._id ?? "NULL");
                                Debug.Log("Username: " + user.username ?? "NULL");
                                //... and so on for other properties

                                RewardHistoryList.Add(user);
                            }
                        }
                        else
                        {
                            Debug.LogError("levelOneUsers is NULL");
                        }
                    }
                    else
                    {
                        Debug.LogError("Response is NULL");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to deserialize JSON: " + e.Message);
                }
            }
        }
        gameobjectSpwinhistory();
    }

}
[Serializable]
public class LevelOneUser1
{
    public string _id;
    public string username;
    public string createdAt;
    public string id;
    public string date;
    public int serialNumber;
    public float reward;
}

[Serializable]
public class ApiResponse1
{
    public bool success;
    public List<LevelOneUser1> levelOneUsers;

}





