using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class YouWinPanel : MonoBehaviour
{
    public Button claimBtn;

    private static string winningApiURL = APIHolder.getBaseUrl() + "multipleLevelWinning";

    private static string rewardApiURL = APIHolder.getBaseUrl() + "multipleLevelReward?";

    public int position;

    public int roomNo;

    MobileFPSGameManager mobileFPS;

    GameSceneManager gameSceneManager;

    public bool wasEnabled;

    public bool successfullyTransferred;

    public TPSShooter.PlayerBehaviour playerBehaviour;

    APIHolder aPIHolder;

    public TextMeshProUGUI rewardText;
    public TextMeshProUGUI positionText;

    private void OnEnable()
    {
        wasEnabled = true;

        aPIHolder = FindObjectOfType<APIHolder>();
        if (aPIHolder.isFreeRoom == false)
            StartCoroutine(getRewardAPI((roomNo + 1).ToString(), (position).ToString()));
        else
        {
            rewardText.text = "0 VRC";
            int posSecured = position;
            positionText.text = posSecured.ToString();
        }
    }

    private void OnDisable()
    {
        wasEnabled = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        mobileFPS = FindObjectOfType<MobileFPSGameManager>();
        gameSceneManager = FindObjectOfType<GameSceneManager>();
        for (int i = 0; i < mobileFPS.PlayerPositionToSpawn.Length; i++)
        {
            if (mobileFPS.PlayerPositionToSpawn[i].EnvironmentSetup != null)
            {
                roomNo = i;
                break;
            }
        }
        claimBtn.onClick.AddListener(() => GameOver());
        StartCoroutine(AutoClaimReward());
    }

    private IEnumerator AutoClaimReward()
    {
        yield return new WaitForSeconds(5f);
        claimBtn.interactable = false;
        GameOver();
    }

    private bool alreadyTakenReward;
    [ContextMenu("Game Over")]
    public void GameOver()
    {
        if (!alreadyTakenReward)
        {
            alreadyTakenReward = true;
            /*string purchaseItem = HaveToPurchasedCharacter.ToString();*/
            if (aPIHolder.isFreeRoom == false)
            {
                StartCoroutine(setGameOverPositions((roomNo + 1).ToString(), (position).ToString()));
            }
            else
            {
                if (mobileFPS)
                {
                    mobileFPS.ExitLobby();
                    mobileFPS.onExitFunctionCalled();
                }
                /*if (playerBehaviour)
                    playerBehaviour.ExitFunctionCalled();*/
            }
        }
    }

    private IEnumerator setGameOverPositions(string room, string position)
    {
        WWWForm form = new WWWForm();
        form.AddField("room", room);
        form.AddField("position", position);

        Debug.Log("Room is:" + room);

        using (UnityWebRequest www = UnityWebRequest.Post(winningApiURL, form))
        {
            string headerValue = PlayerPrefs.GetString("Token");
            www.SetRequestHeader("Authorization", headerValue);

            Debug.Log("Header Val:" + headerValue);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                if (mobileFPS)
                {
                    mobileFPS.ExitLobby();
                    mobileFPS.onExitFunctionCalled();
                }
                if (playerBehaviour)
                    playerBehaviour.ExitFunctionCalled();
                //Popup.Instance.ShowToastMessage(string.Format("Low Balance..."), ToastLength.Normal);
            }
            else
            {
                successfullyTransferred = true;
                Debug.Log("Request sent successfully!");
                string jsonResponse = www.downloadHandler.text;
                Debug.Log("Received: " + jsonResponse);

                // Parse the JSON response
                var responseObject = JsonUtility.FromJson<GameWinResponse>(jsonResponse);

                if (responseObject != null)
                {
                    Debug.Log("Message: " + responseObject.message);
                    Debug.Log("Value: " + responseObject.status);
                    Debug.Log("Gems are:" + responseObject.gems);

                    PlayerPrefs.SetFloat("gems", responseObject.gems);
                    PlayerPrefs.SetFloat("Meta", responseObject.vrc);

                    if (mobileFPS)
                    {
                        mobileFPS.ExitLobby();
                        mobileFPS.onExitFunctionCalled();
                    }
                    /*if (playerBehaviour)
                        playerBehaviour.ExitFunctionCalled();*/

                }
                else
                {
                    Debug.Log("Error parsing JSON response");
                    if (mobileFPS)
                    {
                        mobileFPS.ExitLobby();
                        mobileFPS.onExitFunctionCalled();
                    }
                    /*if (playerBehaviour)
                        playerBehaviour.ExitFunctionCalled();*/
                }
            }
        }
    }

    private IEnumerator getRewardAPI(string room, string position)
    {
        string RewardedAPIResponse = rewardApiURL + "room=" + room + "&" + "position=" + position;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(RewardedAPIResponse))
        {
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
                    GameWinRewardResponse apiResponse = JsonUtility.FromJson<GameWinRewardResponse>(jsonResult);

                    if (apiResponse != null)
                    {
                        rewardText.text = apiResponse.reward.ToString() + " VRC";
                        int posSecured = apiResponse.position;
                        positionText.text = posSecured.ToString();
                    }
                    else
                    {
                        Debug.LogError("Response or WeaponInfo is NULL");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to deserialize JSON: " + e.Message);
                }
            }
        }

    }

    private void OnApplicationQuit()
    {
        if (wasEnabled && !successfullyTransferred)
        {
            GameOver();
        }
    }
}

[Serializable]
public class GameWinResponse
{
    public bool status;
    public string message;
    public float gems;
    public float vrc;
}

[Serializable]
public class GameWinRewardResponse
{
    public int position;
    public int room;
    public int reward;
}
