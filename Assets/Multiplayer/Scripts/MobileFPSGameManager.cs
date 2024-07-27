using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEditor;
using System;
using UnityEngine.Networking;
using UnityEngine.Events;

public class MobileFPSGameManager : MonoBehaviourPunCallbacks
{
    public GameObject mobileInputCanvas;
    public string lobbySceneLoad;
    public static MobileFPSGameManager instance;

    public GameObject Player;

    [SerializeField]
    public GameObject[] playerPrefab;
    public GameObject[] playerpos;
    public spawnPointsSystem[] PlayerPositionToSpawn;
    public SoundManager sound;
    public int selectedplayer;

    public GameObject exitButton;

    public Button QuitMatchBtn;

    public float damage;

    public GameObject[] cameraHolder;
    public GameObject bigMapImage;

    public Button mapButton;
    public Button mapCloseBtn;

    public string[] assetBundleLinks;

    private firebaseDB firebaseDB;

    public GameObject GameOverPanel;
    public GameObject YouWinPanel;
    public GameObject RewardCanvas;

    public List<int> eliminationOrder;
    private bool gameEnded = false;

    public GameObject[] Canvases;

    private APIHolder aPIHolder;

    private static string consumeGems = APIHolder.getBaseUrl() + "multiplePlyerFees";

    // Define the maximum allowed latency (ping) in milliseconds
    public int maxLatency = 160;

    // Interval at which latency is checked (in seconds)
    public float checkInterval = 5.0f;
    private float nextCheckTime;

    public UnityEvent exitEvent = new UnityEvent();

    public GameObject playerCanvas;

    public int spawnPositionIndex;

    public UnityEvent uiHandler = new UnityEvent();
    public UnityEvent uiHandler1 = new UnityEvent();
    private void Awake()
    {
        instance = this;

        if (FindObjectOfType<firebaseDB>())
        {
            firebaseDB = FindObjectOfType<firebaseDB>();
        }
        CheckIndex();
        //spawnPositionIndex = (int)changedProps["playerIndex"];

        if (PhotonNetwork.CurrentRoom.MaxPlayers == 5)
        {
            //StartCoroutine(DownloadAssetBundleFromServer(assetBundleLinks[0]));
            firebaseDB.startInstantiating(0);
        }
        else if (PhotonNetwork.CurrentRoom.MaxPlayers == 10)
        {
            //StartCoroutine(DownloadAssetBundleFromServer(assetBundleLinks[1]));
            firebaseDB.startInstantiating(1);
        }
        else if (PhotonNetwork.CurrentRoom.MaxPlayers == 15)
        {
            //StartCoroutine(DownloadAssetBundleFromServer(assetBundleLinks[1]));
            firebaseDB.startInstantiating(2);
        }

    }
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        //selectedplayer = PlayerPrefs.GetInt("SelectedPlayer");
        selectedplayer = PlayerPrefs.GetInt("PlayerSelectedForLobby");
        //sound = FindObjectOfType<SoundManager>();
        if (FindObjectOfType<APIHolder>() != null)
            sound = FindObjectOfType<APIHolder>().soundManager;

        nextCheckTime = Time.time + checkInterval;

        if (sound != null)
            sound.gameObject.SetActive(false);

        mapButton.onClick.AddListener(() => setBigMapCamera(true));
        mapCloseBtn.onClick.AddListener(() => setBigMapCamera(false));
        Invoke(nameof(setExitButton), 5f);
        if (FindObjectOfType<APIHolder>() != null)
        {
            aPIHolder = FindObjectOfType<APIHolder>();
        }

        if (aPIHolder)
        {
            if (aPIHolder.isFreeRoom == false)
            {
                Invoke(nameof(setRoomAssistance), 2f);
            }
        }
        //Invoke(nameof(setInputCanvas), 0.7f);
    }

    /*private void Update()
    {
        UpdatePing();

        // Check latency at regular intervals
        if (Time.time >= nextCheckTime)
        {
            CheckPlayerLatency();
            nextCheckTime = Time.time + checkInterval;
        }

    }*/

    void UpdatePing()
    {
        int ping = PhotonNetwork.GetPing();
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
        {
            { "Ping", ping }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    public void CheckIndex()
    {
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            if(player == PhotonNetwork.LocalPlayer)
            {
                if (player.CustomProperties.ContainsKey("playerIndex"))
                {
                    spawnPositionIndex = (int)player.CustomProperties["playerIndex"];
                    break;
                }
            }
        }  
    }

    void CheckPlayerLatency()
    {
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.ContainsKey("Ping"))
            {
                int ping = (int)player.CustomProperties["Ping"];

                Debug.Log("Latency is:" + ping);
                if (ping > maxLatency)
                {
                    KickPlayer(player);
                }
            }
        }
    }

    void KickPlayer(Photon.Realtime.Player player)
    {
        if (player.IsLocal)
        {
            /*if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("MasterClient detected with high latency. Transferring MasterClient role...");
                TransferMasterClient();
            }*/
            //ExitLobby();
            if (!PhotonNetwork.IsConnectedAndReady)
            {
                Debug.LogError("Failed to leave room: Not connected to Photon server.");
                return;
            }
            isLeaving = true;
            Debug.Log("Local player leaving the room...");
            PhotonNetwork.LeaveLobby();
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }
        else
        {
            // Only the MasterClient can kick other players
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log($"Kicking player {player.NickName} due to high latency.");
                PhotonNetwork.CloseConnection(player);
            }
        }
        /*if(!isLeaving)
            ExitLobby();*/
        /*if (player.IsLocal)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                TransferMasterClient();
            }
            
        }
        else
        {
            // Only the MasterClient can kick other players
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CloseConnection(player);
            }
        }*/

    }

    public void TransferMasterClient()
    {
        Photon.Realtime.Player newMasterClient = null;
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            if (player != PhotonNetwork.LocalPlayer)
            {
                newMasterClient = player;

                if (newMasterClient != null)
                {
                    PhotonNetwork.SetMasterClient(newMasterClient);
                }
                break;
            }
        }
    }

    public void setRoomAssistance()
    {
        int index = 0;
        for (int i = 0; i < PlayerPositionToSpawn.Length; i++)
        {
            if (PlayerPositionToSpawn[i].EnvironmentSetup != null)
            {
                index = i;
                break;
            }
        }
#if !UNITY_EDITOR
        StartCoroutine(setGemsAccordingToRoom((index + 1).ToString()));
#endif

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.CurrentRoom.RemovedFromList = true;
            PhotonNetwork.CurrentRoom.IsOpen = false;
            //photonView.RPC("EnableAllPlayerColliders", RpcTarget.AllBuffered);
        }
    }

    private IEnumerator setGemsAccordingToRoom(string room)
    {
        WWWForm form = new WWWForm();
        form.AddField("room", room);

        using (UnityWebRequest www = UnityWebRequest.Post(consumeGems, form))
        {
            string headerValue = PlayerPrefs.GetString("Token");
            www.SetRequestHeader("Authorization", headerValue);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                //Popup.Instance.ShowToastMessage(string.Format("Low Balance..."), ToastLength.Normal);
            }
            else
            {
                Debug.Log("Request sent successfully!");
                string jsonResponse = www.downloadHandler.text;
                Debug.Log("Received: " + jsonResponse);

                // Parse the JSON response
                var responseObject = JsonUtility.FromJson<RoomCreationResponse>(jsonResponse);

                if (responseObject != null)
                {
                    Debug.Log("Message: " + responseObject.message);
                    Debug.Log("Value: " + responseObject.status);
                    Debug.Log("Gems are:" + responseObject.gems);

                    PlayerPrefs.SetFloat("gems", responseObject.gems);
                }
                else
                {
                    Debug.Log("Error parsing JSON response");
                }
            }
        }
    }


    public void setInputCanvas()
    {
        mobileInputCanvas.SetActive(true);
    }

    private void setExitButton()
    {
        exitButton.gameObject.SetActive(true);
    }

    private IEnumerator DownloadAssetBundleFromServer(string url)
    {
        GameObject go = null;

        using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogWarning("Error on the get request at : " + url + " " + www.error);
            }
            else
            {
                AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
                go = bundle.LoadAsset(bundle.GetAllAssetNames()[0]) as GameObject;
                bundle.Unload(false);
                yield return new WaitForEndOfFrame();
            }

            www.Dispose();
        }

        InstantiateGameobjectFromAssetsBundle(go);
    }

    private void InstantiateGameobjectFromAssetsBundle(GameObject go)
    {
        GameObject gameInfo = Instantiate(go);
        gameInfo.transform.position = Vector3.zero;
    }

    /*private bool isExit;
    private void Update()
    {
        if(PhotonNetwork.IsConnectedAndReady && !isExit)
        {
            isExit = true;
            exitButton.gameObject.SetActive(true);
        }
    }*/

    private bool isLeaving = false;

    public void ExitLobby()
    {
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            Debug.LogError("Failed to leave room: Not connected to Photon server.");
            return;
        }
        isLeaving = true;
        /*if (PhotonNetwork.IsMasterClient)
        {
            TransferMasterClient();
        }*/
        //PhotonNetwork.LeaveRoom(); // Leave the room
    }

    public override void OnLeftRoom()
    {
        if (isLeaving)
        {
            Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;

            // Iterate through each player and enable their colliders
            foreach (Photon.Realtime.Player player in players)
            {
                TPSShooter.PlayerBehaviour playerObject = FindPlayerObject(player);
                Debug.Log("Player Object is:" + playerObject.name);
                if (playerObject != null)
                {
                    playerObject.GetComponent<CharacterController>().enabled = true;
                }
            }
            isLeaving = false;

            if (Player)
            {
                Destroy(Player);
                PhotonNetwork.Destroy(Player);
                Player = null;
            }

            PhotonNetwork.LoadLevel(lobbySceneLoad); // Load the lobby scene after leaving the room
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;

        // Iterate through each player and enable their colliders
        foreach (Photon.Realtime.Player player in players)
        {
            TPSShooter.PlayerBehaviour playerObject = FindPlayerObject(player);
            Debug.Log("Player Object is:" + playerObject.name);
            if (playerObject != null)
            {
                playerObject.GetComponent<CharacterController>().enabled = true;
            }
        }

        Debug.Log("Player entered room: " + newPlayer.NickName);

        // Ensure all player colliders are enabled
        EnableAllPlayerColliders();
    }

    // Called when a player leaves the room
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        // Get all the remaining players in the room
        Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;

        // Iterate through each player and enable their colliders
        foreach (Photon.Realtime.Player player in players)
        {
            TPSShooter.PlayerBehaviour playerObject = FindPlayerObject(player);
            Debug.Log("Player Object is:" + playerObject.name);
            if (playerObject != null)
            {
                playerObject.GetComponent<CharacterController>().enabled = true;
            }
        }
        Debug.Log("Player left room: " + otherPlayer.NickName);

        // Ensure all player colliders are enabled
        EnableAllPlayerColliders();
    }

    private void EnableAllPlayerColliders()
    {
        foreach (var playerObject in FindObjectsOfType<TPSShooter.PlayerBehaviour>())
        {
            TPSShooter.PlayerBehaviour playerCollider = playerObject.GetComponentInChildren<TPSShooter.PlayerBehaviour>();
            if (playerCollider != null)
            {
                playerCollider.GetComponent<TPSShooter.PlayerBehaviour>().enabled = true;
            }
        }
    }


    // Method to find the game object associated with a player
    private TPSShooter.PlayerBehaviour FindPlayerObject(Photon.Realtime.Player player)
    {
        foreach (var playerObject in GameObject.FindObjectsOfType<TPSShooter.PlayerBehaviour>())
        {
            PhotonView photonView = playerObject.GetComponent<PhotonView>();
            if (photonView != null && photonView.Owner == player)
            {
                return playerObject;
            }
        }
        return null;
    }

    /*// Called when a player leaves the room
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log($"Player {otherPlayer.NickName} left the room");
        // Clean up player state
        CleanUpPlayerState(otherPlayer);
    }

    private void CleanUpPlayerState(Photon.Realtime.Player player)
    {
        // Implement any additional cleanup logic needed when a player leaves
        GameObject playerObject = FindPlayerObject(player);
        if (playerObject != null)
        {
            Destroy(playerObject);
        }
    }

    private GameObject FindPlayerObject(Photon.Realtime.Player player)
    {
        // Implement a method to find the game object associated with the player
        foreach (var playerObject in FindObjectsOfType<TPSShooter.PlayerBehaviour>())
        {
            if (playerObject.photonView.Owner == player)
            {
                return playerObject.gameObject;
            }
        }
        return null;
    }*/

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (isLeaving)
        {
            Debug.LogWarning("Disconnected while trying to leave room: " + cause);
            PhotonNetwork.LoadLevel(lobbySceneLoad); // Load the lobby scene even if disconnected
        }
    }

    /*[PunRPC]
    public void AddPlayerInList(GameObject personToAdd)
    {
        PlayersInTheRoom.Add(personToAdd);
    }

    [PunRPC]
    public void RemoveAndRearrange(GameObject personToRemove)
    {
        int indexToRemove = FindIndexOfGameObject(personToRemove);

        if (indexToRemove == -1)
        {
            Debug.LogError("GameObject not found in the list.");
            return;
        }

        Debug.Log("Index is:" + indexToRemove);

        *//*// Remove the GameObject at the given index
        Destroy(personToRemove);
        PlayersInTheRoom.RemoveAt(indexToRemove);*/

    /*// Now, adjust the list so that everyone after the removed person shifts one index to the left
    for (int i = indexToRemove; i < PlayersInTheRoom.Count; i++)
    {
        // If we're not at the end of the list, move the person one index to the left
        if (i + 1 < PlayersInTheRoom.Count)
        {
            PlayersInTheRoom[i] = PlayersInTheRoom[i + 1];
        }
    }*/

    /*// Remove the last person from the list
    PlayersInTheRoom.RemoveAt(PlayersInTheRoom.Count - 1);*//*
}

[PunRPC]
// Function to find the index of a GameObject in the list
private int FindIndexOfGameObject(GameObject person)
{
    return PlayersInTheRoom.IndexOf(person);
}

void PrintPeopleList()
{
    Debug.Log("Updated People List:");
    foreach (var person in PlayersInTheRoom)
    {
        Debug.Log(person.name);
    }
}*/

    public void setBigMapCamera(bool setActive)
    {
        bigMapImage.SetActive(setActive);
        if (PhotonNetwork.CurrentRoom.MaxPlayers == 5)
        {
            cameraHolder[0].SetActive(setActive);
        }
        else if (PhotonNetwork.CurrentRoom.MaxPlayers == 10)
        {
            cameraHolder[1].SetActive(setActive);
        }
        else
        {
            cameraHolder[2].SetActive(setActive);
        }
    }

    [PunRPC]
    public void playerLeft(int playerId)
    {
        if (!eliminationOrder.Contains(playerId))
        {
            eliminationOrder.Add(playerId);
        }

        eliminationOrder.Reverse();
    }

    [PunRPC]
    public void PlayerKilled(int playerId)
    {
        if (!eliminationOrder.Contains(playerId))
        {
            eliminationOrder.Add(playerId);
            DetermineFinalRanks();
            CheckGameEnd();
        }
    }

    void CheckGameEnd()
    {
        if (eliminationOrder.Count >= PhotonNetwork.PlayerList.Length - 1 && !gameEnded)
        {
            gameEnded = true;
            OnGameEnd();
        }
    }

    void OnGameEnd()
    {
        photonView.RPC("GameEnded", RpcTarget.All);
    }

    [PunRPC]
    void GameEnded()
    {
        DetermineFinalRanks();
    }

    void DetermineFinalRanks()
    {
        eliminationOrder.Reverse();
        for (int i = 0; i < eliminationOrder.Count; i++)
        {
            int playerId = eliminationOrder[i];
            int rank = i;
            AssignRank(playerId, rank);
        }

        // The last remaining player is the winner
        foreach (var player in PhotonNetwork.PlayerList)
        {
            int playerId = player.ActorNumber;
            if (!eliminationOrder.Contains(playerId))
            {
                AssignRank(playerId, 0);
            }
        }
    }

    void AssignRank(int playerId, int rank)
    {
        PhotonView photonView = PhotonView.Find(playerId);
        photonView.RPC("ReceiveRank", RpcTarget.All, rank);
    }

    #region On Application Pause Solution
    private bool wasPaused;

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            wasPaused = true;
        }
        else
        {
            if (wasPaused)
            {
                wasPaused = false;
                if (Player)
                {
                    Destroy(Player);
                    PhotonNetwork.Destroy(Player);
                    Player = null;
                }

                PhotonNetwork.LoadLevel(lobbySceneLoad); // Load the lobby scene after leaving the room
            }
        }
    }

    public void onExitFunctionCalled()
    {
        /*if (Player)
        {
            Destroy(Player);
            PhotonNetwork.Destroy(Player);
            Player = null;
        }*/
        exitEvent?.Invoke();
        /*PhotonNetwork.LeaveLobby();
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();*/
        //PhotonNetwork.LoadLevel(lobbySceneLoad);
    }
    #endregion
}

[System.Serializable]
public class spawnPointsSystem
{
    public GameObject[] playerPositionAccordingtoMap;
    public GameObject EnvironmentSetup;
}

#if UNITY_EDITOR
public class BuildAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    public static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = Application.dataPath + "/../AssetsBundles";
        try
        {
            BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}
#endif

[Serializable]
public class RoomCreationResponse
{
    public bool status;
    public string message;
    public float gems;
}