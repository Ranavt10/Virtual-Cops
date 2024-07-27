using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class WithdrawHistory : MonoBehaviour
{
    public GameObject historyItemPrefab;
    public Transform contentPanel;

    public GameObject[] spawnedObjects;
    private static string withdrawHistory = "withdrawHistory";
    private string withdrawshistoryapi = APIHolder.getBaseUrl() + withdrawHistory;/*"https://a978-72-255-51-32.ngrok-free.app/api/withdrawHistory";*/
    public List<WithdrawHistoryItem> depositHistoryList = new List<WithdrawHistoryItem>();

    private void OnEnable()
    {
        if (PlayerPrefs.GetInt("Guest") != 1)
        {
            StartCoroutine(FetchHistoryData());
        }
        else
        {
            Popup.Instance.ShowToastMessage("Login For History");
        }
    }
    private void OnDisable()
    {
        if (PlayerPrefs.GetInt("Guest") != 1)
        {
       
        for (int i = 0; i < spawnedObjects.Length; i++)
        {
            Destroy(spawnedObjects[i]);
        }
        depositHistoryList.Clear();

        Array.Clear(spawnedObjects, 0, spawnedObjects.Length);
        }
        
       
    }

    void gameobjectSpwinhistory()
    {

        if (depositHistoryList.Count > 0)
        {


            spawnedObjects = new GameObject[depositHistoryList.Count];


            for (int i = 0; i < depositHistoryList.Count; i++)
            {
                GameObject spawnedObject = Instantiate(historyItemPrefab, contentPanel);

                spawnedObjects[i] = spawnedObject;

            }

            int currentIndex = 0;

            foreach (WithdrawHistoryItem item in depositHistoryList)
            {
                spawnedObjects[currentIndex].GetComponent<Withdrawdatacontroller>().indextext.text = item.serialNumber.ToString();
                spawnedObjects[currentIndex].GetComponent<Withdrawdatacontroller>().vrctext.text = item.vrc.ToString();
                spawnedObjects[currentIndex].GetComponent<Withdrawdatacontroller>().WalletAddress.text = item.vaultAddress.ToString();
                spawnedObjects[currentIndex].GetComponent<Withdrawdatacontroller>().datetimetext.text = item.date.ToString();

                currentIndex++;
            }

        }
        else
        {
            Popup.Instance.ShowToastMessage("No Record Avaiable");
        }
    }

    IEnumerator FetchHistoryData()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(withdrawshistoryapi))
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
                    ApiResponsewithdraw response = JsonUtility.FromJson<ApiResponsewithdraw>(jsonResult);

                    if (response.success)
                    {
                        depositHistoryList.Clear(); // Clear the existing history.
                                                    // historyText.text = ""; // Clear the existing text.

                        foreach (WithdrawHistoryItem history in response.withdrawHistory)
                        {
                            Debug.Log("ID: " + history._id);
                            Debug.Log("VRC: " + history.vrc);
                            Debug.Log("Address: " + history.vaultAddress);
                            Debug.Log("Serial: " + history.serialNumber);
                            Debug.Log("Version: " + history.__v);

                            depositHistoryList.Add(history);



                        }

                    }
                    else
                    {
                        Debug.LogError("API responded with error: " + response.message);
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

[System.Serializable]
public class WithdrawHistoryItem
{

    public string _id;
    public float vrc;
    public string vaultAddress;
    public string date;
    public string createdAt;
    public string updatedAt;
    public int __v;
    public int serialNumber;
}

[System.Serializable]
public class ApiResponsewithdraw
{
    public bool success;
    public WithdrawHistoryItem[] withdrawHistory;
    public string message;
}