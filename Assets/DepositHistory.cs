using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class DepositHistory : MonoBehaviour
{
    public GameObject historyItemPrefab;
    public Transform contentPanel;

    public GameObject[] spawnedObjects;
    private static string userDeposit = "userDepositHistory";
    private string Depositshistoryapi = APIHolder.getBaseUrl() + userDeposit;/*"https://a978-72-255-51-32.ngrok-free.app/api/userDepositHistory";*/
    public List<depositHistoryItem> depositBalanceHistoryList = new List<depositHistoryItem>();


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
            depositBalanceHistoryList.Clear();

        Array.Clear(spawnedObjects, 0, spawnedObjects.Length);
        }
       
       
    }

    void gameobjectSpwinhistory()
    {

        if (depositBalanceHistoryList.Count > 0)
        {


            spawnedObjects = new GameObject[depositBalanceHistoryList.Count];


            for (int i = 0; i < depositBalanceHistoryList.Count; i++)
            {
                GameObject spawnedObject = Instantiate(historyItemPrefab, contentPanel);

                spawnedObjects[i] = spawnedObject;

            }

            int currentIndex = 0;

            foreach (depositHistoryItem item in depositBalanceHistoryList)
            {
                spawnedObjects[currentIndex].GetComponent<DepositBalancedatacontroller>().indextext.text = item.serialNumber.ToString();
                spawnedObjects[currentIndex].GetComponent<DepositBalancedatacontroller>().vrctext.text = item.amount.ToString();
                spawnedObjects[currentIndex].GetComponent<DepositBalancedatacontroller>().WalletAddress.text = item.vaultAddress.ToString();
                spawnedObjects[currentIndex].GetComponent<DepositBalancedatacontroller>().datetimetext.text = item.date.ToString();

                currentIndex++;
            }

        }
        else
        {
            Popup.Instance.ShowToastMessage("No Record Avaiable");
        }
    }

    //IEnumerator FetchHistoryData()
    //{
    //    using (UnityWebRequest www = UnityWebRequest.Get(Depositshistoryapi))
    //    {
    //        string myHeaderValue = PlayerPrefs.GetString("Token");
    //        www.SetRequestHeader("Authorization", myHeaderValue);

    //        yield return www.SendWebRequest();

    //        if (www.result != UnityWebRequest.Result.Success)
    //        {
    //            Debug.LogError("API Error: " + www.error);
    //        }
    //        else
    //        {
    //            string jsonResult = www.downloadHandler.text;
    //            print(jsonResult);

    //            try
    //            {
    //                ApiResponsedeposit response = JsonUtility.FromJson<ApiResponsedeposit>(jsonResult);

    //                if (response.success)
    //                {
    //                    depositBalanceHistoryList.Clear(); // Clear the existing history.
    //                                                // historyText.text = ""; // Clear the existing text.

    //                    foreach (depositHistoryItem history in response.DepositHistory)
    //                    {
    //                        Debug.Log("ID: " + history._id);
    //                        Debug.Log("amount: " + history.amount);
    //                        Debug.Log("vaultAddress: " + history.vaultAddress);
    //                        Debug.Log("serialNumber: " + history.serialNumber);
    //                        Debug.Log("Version: " + history.__v);

    //                        depositBalanceHistoryList.Add(history);



    //                    }

    //                }
    //                else
    //                {
    //                    Debug.LogError("API responded with error: " + response.message);
    //                }
    //            }
    //            catch (Exception e)
    //            {
    //                Debug.LogError("Failed to deserialize JSON: " + e.Message);
    //            }
    //        }
    //    }

    //    gameobjectSpwinhistory();
    //}





    IEnumerator FetchHistoryData()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(Depositshistoryapi))
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
                Debug.Log(jsonResult);

                try
                {
                    JObject response = JObject.Parse(jsonResult);

                    if ((bool)response["success"])
                    {
                        JArray depositHistoryArray = (JArray)response["userdepositHistory"];

                        // Clear the existing history
                        foreach (Transform child in contentPanel)
                        {
                            Destroy(child.gameObject);
                        }

                        // Instantiate new history items
                        for (int i = 0; i < depositHistoryArray.Count; i++)
                        {
                            JObject history = (JObject)depositHistoryArray[i];
                            GameObject spawnedObject = Instantiate(historyItemPrefab, contentPanel);
                            DepositBalancedatacontroller dataController = spawnedObject.GetComponent<DepositBalancedatacontroller>();

                            dataController.indextext.text = history["serialNumber"].ToString();
                            dataController.vrctext.text = history["amount"].ToString();
                            dataController.WalletAddress.text = history["vaultAddress"].ToString();
                            dataController.datetimetext.text = history["date"].ToString();
                        }
                    }
                    else
                    {
                        Debug.LogError("API responded with error: " + response["message"]);
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Failed to parse JSON: " + e.Message);
                }
            }
        }
    }

}

[System.Serializable]
public class depositHistoryItem
{

    public string _id;
    public float amount;
    public string vaultAddress;
    public string date;
    public string createdAt;
    public string updatedAt;
    public int __v;
    public int serialNumber;
}

[System.Serializable]
public class ApiResponsedeposit
{
    public bool success;
    public depositHistoryItem[] DepositHistory;
    public string message;
}