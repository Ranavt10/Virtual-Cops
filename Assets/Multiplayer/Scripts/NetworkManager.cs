using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    [Header("Connection Status")]
    public Text connectionStatusText;

    [Header("Game Options UI Panel")]
    public GameObject GameOptions_UI_Panel;

    [Header("Create Room UI Panel")]
    public GameObject CreateRoom_UI_Panel;
    public InputField roomNameInputField;

    public Dropdown maxPlayerInputField;

    [Header("Inside Room UI Panel")]
    public GameObject InsideRoom_UI_Panel;
    public Text roomInfoText;
    public GameObject playerListPrefab;
    public GameObject playerListContent;
    public GameObject startGameButton;

    [Header("Room List UI Panel")]
    public GameObject RoomList_UI_Panel;
    public GameObject roomListEntryPrefab;
    public GameObject roomListParentGameobject;

    [Header("Join Random Room UI Panel")]
    public GameObject JoinRandomRoom_UI_Panel;

    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, GameObject> roomListGameobjects;
    private Dictionary<int, GameObject> playerListGameobjects;



    [Header("Character Setting penal")]
    public GameObject menupenal;
    public GameObject characterselection;
    public GameObject dressselection;
    public GameObject gunSelection;
    public GameObject playerlobby;

    string roomName;
    public int maxplayer;

    APIHolder aPIHolder;

    public string regionSelected = "regionSelected";
    public string region = "asia";

    #region Unity Methods

    public void selectcharacter()
    {
        menupenal.SetActive(false);
        characterselection.SetActive(true);
        gunSelection.SetActive(false);
        dressselection.SetActive(false);
        playerlobby.SetActive(false);

    }
    public void selectDress()
    {
        menupenal.SetActive(false);
        dressselection.SetActive(true);
        gunSelection.SetActive(false);
        characterselection.SetActive(false);
        playerlobby.SetActive(false);
    }
    public void selectGuns()
    {
        menupenal.SetActive(false);
        gunSelection.SetActive(true);
        dressselection.SetActive(false);
        characterselection.SetActive(false);
        playerlobby.SetActive(false);

    }
    public void backmenupenal()
    {
        menupenal.SetActive(true);
        gunSelection.SetActive(false);
        dressselection.SetActive(false);
        characterselection.SetActive(false);
        playerlobby.SetActive(false);

    }

    // Start is called before the first frame update
    private void Start()
    {
        //ActivatePanel(GameOptions_UI_Panel.name);
        //PlayerPrefs.SetString("PlayerName", "guestuset");

        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListGameobjects = new Dictionary<string, GameObject>();

        PhotonNetwork.AutomaticallySyncScene = true;
        menupenal.SetActive(true);
        //OnLoginButtonClicked();
        maxplayer = 5;
        if (FindObjectOfType<APIHolder>())
            aPIHolder = FindObjectOfType<APIHolder>();

        if (PlayerPrefs.GetInt("FirstTimeRegionSelection", 0) == 0)
        {
            PlayerPrefs.SetString(regionSelected, "asia");
            PlayerPrefs.SetInt("FirstTimeRegionSelection", 1);
        }

        region = PlayerPrefs.GetString(regionSelected);
        ConnectToRegion(region);
    }

    public void selectedcharacterandopengame()
    {
        menupenal.SetActive(false);
        gunSelection.SetActive(false);
        dressselection.SetActive(false);
        characterselection.SetActive(false);
        //LobbyManager.Instance.roomPanel.SetActive(true);
        //playerlobby.SetActive(true);
        OnLoginButtonClicked();
    }

    public void dropdownlist()
    {
        switch (maxPlayerInputField.value)
        {
            case 0:
                maxplayer = 5;
                break;
            case 1:
                maxplayer = 10;
                break;
            case 2:
                maxplayer = 15;
                break;

        }
    }

    public void maxPlayersSet(int no)
    {
        maxplayer = no;
    }

    public void backtoHome()
    {
        SceneFading.Instance.FadeOutAndLoadScene(StaticValue.SCENE_MENU);
    }

    // Update is called once per frame
    private void Update()
    {
        connectionStatusText.text = "Connection status: " + PhotonNetwork.NetworkClientState;

        if(LobbyManager.Instance.totalUsersToBeShownInRoom.isActiveAndEnabled)
        {
            LobbyManager.Instance.totalUsersToBeShownInRoom.text =
                            "Players Joined/Total Players: " +
                            PhotonNetwork.CurrentRoom.PlayerCount + "/" +
                            PhotonNetwork.CurrentRoom.MaxPlayers;

        }
    }

    #endregion

    #region UI Callbacks
    public void OnLoginButtonClicked()
    {
        string playerName;
        if (PlayerPrefs.GetInt("Guest") != 1)
        {
            playerName = PlayerPrefs.GetString("PlayerName");
        }
        else
        {
            playerName = "GuestUser";
        }

        if (!string.IsNullOrEmpty(playerName))
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            Debug.Log("Playername is invalid!");
        }
    }

    float gemsQuantity;
    private string roomNameToBeShown;

    public void OnRoomCreateButtonClicked()
    {
        if (!aPIHolder.isFreeRoom)
        {
            aPIHolder.indexToBeSpawnedOn = 0;
            if (maxplayer == 5)
            {
                gemsQuantity = 50f;
                roomNameToBeShown = "_Silver_Room";
            }
            else if (maxplayer == 10)
            {
                gemsQuantity = 100f;
                roomNameToBeShown = "_Gold_Room";
            }
            else if (maxplayer == 15)
            {
                gemsQuantity = 150f;
                roomNameToBeShown = "_Diamond_Room";
            }

            if (PlayerPrefs.GetFloat("gems") >= gemsQuantity)
            {
                string roomName = roomNameInputField.text;

                if (string.IsNullOrEmpty(roomName))
                {
                    roomName = "Room " + PlayerPrefs.GetString("PlayerName") + " " + roomNameToBeShown + "_" + Random.Range(1000, 10000);

                }

                roomNameInputField.text = roomName;
                RoomOptions roomOptions = new RoomOptions();
                roomOptions.IsVisible = true;
                roomOptions.IsOpen = true;
                roomOptions.MaxPlayers = maxplayer;

                ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable { { "RoomStarted", false } };
                roomOptions.CustomRoomProperties = customProperties;
                roomOptions.CustomRoomPropertiesForLobby = new string[] { "RoomStarted" };
                print("max player in room" + maxplayer);

                PhotonNetwork.CreateRoom(roomName, roomOptions);

                LobbyManager.Instance.InsideRoomPanel.SetActive(true);
            }
            else
            {
                LobbyManager.Instance.popUpText.text = "You need " + gemsQuantity.ToString() + " Gems. You Don't Have Enough Gems To Create This Room.";
                LobbyManager.Instance.popUpPanel.SetActive(true);
            }
        }
        else
        {
            aPIHolder.indexToBeSpawnedOn = 0;
            string roomName = roomNameInputField.text;

            if (string.IsNullOrEmpty(roomName))
            {
                roomName = "Room " + PlayerPrefs.GetString("PlayerName") + Random.Range(1000, 10000) + "_FreeRoom";

            }

            roomNameInputField.text = roomName;
            string freeRoom = "freeRoom";
            ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable();
            roomProperties.Add("RoomType", freeRoom);

            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = maxplayer;
            roomOptions.CustomRoomProperties = roomProperties;
            roomOptions.CustomRoomPropertiesForLobby = new string[] { "RoomType" };
            print("max player in room" + maxplayer);

            PhotonNetwork.CreateRoom(roomName, roomOptions);

            LobbyManager.Instance.InsideRoomPanel.SetActive(true);
        }
    }

    public void StartRoom()
    {
        /*// Set the room visibility to false if the room has started
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("RoomStarted"))
        {
            bool roomStarted = (bool)PhotonNetwork.CurrentRoom.CustomProperties["RoomStarted"];
            if (roomStarted)
            {
                PhotonNetwork.CurrentRoom.IsVisible = false;
                PhotonNetwork.CurrentRoom.RemovedFromList = true;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }*/

        if (PhotonNetwork.InRoom)
        {
            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable { { "RoomStarted", true } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);

            // Set the room visibility to false
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.CurrentRoom.RemovedFromList = true;
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
    }

    public void OnCancelButtonClicked()
    {
        ActivatePanel(GameOptions_UI_Panel.name);
    }

    public void OnShowRoomListButtonClicked()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }

        ActivatePanel(RoomList_UI_Panel.name);
    }

    public void OnBackButtonClicked()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        ActivatePanel(GameOptions_UI_Panel.name);
    }

    public void OnLeaveGameButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnJoinRandomRoomButtonClicked()
    {
        ActivatePanel(JoinRandomRoom_UI_Panel.name);
        PhotonNetwork.JoinRandomRoom();

    }
    public void OnStartGameButtonClicked()
    {
        if (!aPIHolder.isFreeRoom)
        {
            if (maxplayer == 5)
            {
                if (PhotonNetwork.CurrentRoom.PlayerCount >= 5)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        if (PhotonNetwork.IsMasterClient)
                        {
                            StartRoom();
                        }
                        PhotonNetwork.LoadLevel("GameScene");
                    }
                }
                else
                {
                    LobbyManager.Instance.popUpText.text = "ROOM IS NOT FULL YET";
                    LobbyManager.Instance.popUpPanel.SetActive(true);
                    //Debug.Log("Not Enough People in the room to start.");
                }
            }
            else if (maxplayer == 10)
            {
                if (PhotonNetwork.CurrentRoom.PlayerCount >= 10)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        if (PhotonNetwork.IsMasterClient)
                        {
                            StartRoom();
                        }
                        PhotonNetwork.LoadLevel("GameScene");
                    }
                }
                else
                {
                    //Debug.Log("Not Enough People in the room to start.");
                    LobbyManager.Instance.popUpText.text = "ROOM IS NOT FULL YET";
                    LobbyManager.Instance.popUpPanel.SetActive(true);
                }
            }
            else if (maxplayer == 15)
            {
                if (PhotonNetwork.CurrentRoom.PlayerCount >= 15)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        if (PhotonNetwork.IsMasterClient)
                        {
                            StartRoom();
                        }
                        PhotonNetwork.LoadLevel("GameScene");
                    }
                }
                else
                {
                    //Debug.Log("Not Enough People in the room to start.");
                    LobbyManager.Instance.popUpText.text = "ROOM IS NOT FULL YET";
                    LobbyManager.Instance.popUpPanel.SetActive(true);
                }
            }
        }
        else
        {
            if (maxplayer == 5)
            {
                if (PhotonNetwork.CurrentRoom.PlayerCount >= 1)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        if (PhotonNetwork.IsMasterClient)
                        {
                            StartRoom();
                        }
                        PhotonNetwork.LoadLevel("GameScene");
                    }
                }
                else
                {
                    LobbyManager.Instance.popUpText.text = "ROOM IS NOT FULL YET";
                    LobbyManager.Instance.popUpPanel.SetActive(true);
                    //Debug.Log("Not Enough People in the room to start.");
                }
            }
            else if (maxplayer == 10)
            {
                if (PhotonNetwork.CurrentRoom.PlayerCount >= 5)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        if (PhotonNetwork.IsMasterClient)
                        {
                            StartRoom();
                        }
                        PhotonNetwork.LoadLevel("GameScene");
                    }
                }
                else
                {
                    //Debug.Log("Not Enough People in the room to start.");
                    LobbyManager.Instance.popUpText.text = "ROOM IS NOT FULL YET";
                    LobbyManager.Instance.popUpPanel.SetActive(true);
                }
            }
            else if (maxplayer == 15)
            {
                if (PhotonNetwork.CurrentRoom.PlayerCount >= 5)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        if (PhotonNetwork.IsMasterClient)
                        {
                            StartRoom();
                        }
                        PhotonNetwork.LoadLevel("GameScene");
                    }
                }
                else
                {
                    //Debug.Log("Not Enough People in the room to start.");
                    LobbyManager.Instance.popUpText.text = "ROOM IS NOT FULL YET";
                    LobbyManager.Instance.popUpPanel.SetActive(true);
                }
            }
        }
    }
    #endregion

    #region Photon Callbacks

    public void ConnectToRegion(string region)
    {
        PhotonNetwork.Disconnect(); // Ensure we're not connected to Photon before setting a new region
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = region;
        Debug.Log("Region Connected:" + PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnected()
    {
        Debug.Log("Connected to Internet");
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is connected to Photon");
        ActivatePanel(GameOptions_UI_Panel.name);
    }
    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " is created.");
    }

    private int playerCount;
    public override void OnJoinedRoom()
    {
        playerCount = 0;
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name);
        ActivatePanel(InsideRoom_UI_Panel.name);

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            startGameButton.SetActive(true);
        }
        else
        {
            startGameButton.SetActive(false);
        }

        roomInfoText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name + " " +
                            "Players/Max.players: " +
                            PhotonNetwork.CurrentRoom.PlayerCount + "/" +
                            PhotonNetwork.CurrentRoom.MaxPlayers;
        LobbyManager.Instance.totalUsersToBeShownInRoom.text =
                            "Players Joined/Total Players: " +
                            PhotonNetwork.CurrentRoom.PlayerCount + "/" +
                            PhotonNetwork.CurrentRoom.MaxPlayers;

        if (playerListGameobjects == null)
        {
            playerListGameobjects = new Dictionary<int, GameObject>();

        }

        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            GameObject playerListGameobject = Instantiate(playerListPrefab);
            playerListGameobject.transform.SetParent(playerListContent.transform);
            playerListGameobject.transform.localScale = Vector3.one;

            if (!aPIHolder.isFreeRoom)
            {
                if (maxplayer == 5)
                    playerListGameobject.transform.Find("PlayerNameText").GetComponent<Text>().text = player.NickName + "_Silver_Room";
                else if (maxplayer == 10)
                    playerListGameobject.transform.Find("PlayerNameText").GetComponent<Text>().text = player.NickName + "_Gold_Room";
                else if (maxplayer == 15)
                    playerListGameobject.transform.Find("PlayerNameText").GetComponent<Text>().text = player.NickName + "_Diamond_Room";
            }
            else
                playerListGameobject.transform.Find("PlayerNameText").GetComponent<Text>().text = player.NickName + "_Free_Room";
            if (player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                playerListGameobject.transform.Find("PlayerIndicator").gameObject.SetActive(true);

            }
            else
            {
                playerListGameobject.transform.Find("PlayerIndicator").gameObject.SetActive(false);

            }

            playerListGameobjects.Add(player.ActorNumber, playerListGameobject);
            Debug.Log("Total Players in room:" + playerListGameobjects.Count);

            playerCount++;

            AssignIndices();
        }
        Debug.Log("Total Player in Room joined are:" + playerCount);
    }

    [ContextMenu("Check Players")]
    public void checkPlayersINRoom()
    {
        Debug.Log("Total Players in room:" + playerListGameobjects.Count);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player myPlayer)
    {
        roomInfoText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name + " " +
                           "Players/Max.players: " +
                           PhotonNetwork.CurrentRoom.PlayerCount + "/" +
                           PhotonNetwork.CurrentRoom.MaxPlayers;


        GameObject playerListGameobject = Instantiate(playerListPrefab);
        playerListGameobject.transform.SetParent(playerListContent.transform);
        playerListGameobject.transform.localScale = Vector3.one;

        playerListGameobject.transform.Find("PlayerNameText").GetComponent<Text>().text = myPlayer.NickName;
        if (myPlayer.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            playerListGameobject.transform.Find("PlayerIndicator").gameObject.SetActive(true);
        }
        else
        {
            playerListGameobject.transform.Find("PlayerIndicator").gameObject.SetActive(false);
        }

        playerListGameobjects.Add(myPlayer.ActorNumber, playerListGameobject);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        roomInfoText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name + " " +
                           "Players/Max.players: " +
                           PhotonNetwork.CurrentRoom.PlayerCount + "/" +
                           PhotonNetwork.CurrentRoom.MaxPlayers;

        Destroy(playerListGameobjects[otherPlayer.ActorNumber].gameObject);
        playerListGameobjects.Remove(otherPlayer.ActorNumber);

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            startGameButton.SetActive(true);
        }

        AssignIndices();
    }

    private void AssignIndices()
    {
        int index = 0;

        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "playerIndex", index } });
            if (player == PhotonNetwork.LocalPlayer)
            {
                Debug.Log("Assigned player index: " + index);
            }
            index++;
        }
    }


    public override void OnLeftRoom()
    {
        ActivatePanel(GameOptions_UI_Panel.name);

        if (playerListGameobjects != null)
        {
            foreach (GameObject playerListGameobject in playerListGameobjects.Values)
            {
                if (playerListGameobject)
                    Destroy(playerListGameobject);
            }
            playerListGameobjects.Clear();
            playerListGameobjects = null;
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomListView();

        foreach (RoomInfo room in roomList)
        {
            Debug.Log(room.Name);
            if (!room.IsOpen || !room.IsVisible || room.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(room.Name))
                {
                    cachedRoomList.Remove(room.Name);
                }
            }
            else
            {
                if (cachedRoomList.ContainsKey(room.Name))
                {
                    cachedRoomList[room.Name] = room;
                }
                else
                {
                    cachedRoomList.Add(room.Name, room);
                }
            }
        }

        foreach (RoomInfo room in cachedRoomList.Values)
        {

            GameObject roomListEntryGameobject = Instantiate(roomListEntryPrefab);
            roomListEntryGameobject.transform.SetParent(roomListParentGameobject.transform);
            roomListEntryGameobject.transform.localScale = Vector3.one;
            roomListEntryGameobject.transform.Find("RoomNameText").GetComponent<Text>().text = room.Name;
            roomListEntryGameobject.transform.Find("RoomPlayersText").GetComponent<Text>().text = room.PlayerCount + " / " + room.MaxPlayers;
            roomListEntryGameobject.transform.Find("JoinRoomButton").GetComponent<Button>().onClick.AddListener(() => OnJoinRoomButtonClicked(room.Name));

            roomListGameobjects.Add(room.Name, roomListEntryGameobject);

        }
    }
    public override void OnLeftLobby()
    {
        ClearRoomListView();
        cachedRoomList.Clear();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);

        string roomName = "Room " + Random.Range(1000, 10000);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 15;

        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    #endregion

    #region Private Methods

    private float joinGemsQuantity;
    void OnJoinRoomButtonClicked(string _roomName)
    {
        if (LobbyManager.Instance)
            LobbyManager.Instance.clickSound();

        RoomInfo roomInfo = GetRoomInfo(_roomName);

        string roomType = roomInfo.CustomProperties["RoomType"] as string;

        if (roomType == "freeRoom")
        {
            aPIHolder.isFreeRoom = true;
            if (PhotonNetwork.InLobby)
            {
                PhotonNetwork.LeaveLobby();
            }

            PhotonNetwork.JoinRoom(_roomName);

            Invoke(nameof(onJoinedFromRoomList), 1f);
        }
        else
        {
            if (roomInfo != null)
            {
                if (roomInfo.MaxPlayers == 5)
                {
                    joinGemsQuantity = 50;
                }
                else if (roomInfo.MaxPlayers == 10)
                {
                    joinGemsQuantity = 100;
                }
                else if (roomInfo.MaxPlayers == 15)
                {
                    joinGemsQuantity = 150;
                }
            }

            if (aPIHolder.isFreeRoom == false)
            {
                if (PlayerPrefs.GetFloat("gems") >= joinGemsQuantity)
                {
                    if (PhotonNetwork.InLobby)
                    {
                        PhotonNetwork.LeaveLobby();
                    }

                    PhotonNetwork.JoinRoom(_roomName);

                    Invoke(nameof(onJoinedFromRoomList), 1f);
                }
                else
                {
                    LobbyManager.Instance.popUpText.text = "You need " + joinGemsQuantity.ToString() + " Gems. You Don't Have Enough Gems To Join This Room.";
                    LobbyManager.Instance.popUpPanel.SetActive(true);
                }
            }/*
            else
            {
                aPIHolder.isFreeRoom = true;
                if (PhotonNetwork.InLobby)
                {
                    PhotonNetwork.LeaveLobby();
                }

                PhotonNetwork.JoinRoom(_roomName);

                Invoke(nameof(onJoinedFromRoomList), 1f);
            }*/
        }

    }

    public RoomInfo GetRoomInfo(string roomName)
    {
        cachedRoomList.TryGetValue(roomName, out RoomInfo roomInfo);
        return roomInfo;
    }

    private void onJoinedFromRoomList()
    {
        LobbyManager.Instance.InsideRoomPanel.SetActive(true);
    }

    void ClearRoomListView()
    {
        foreach (var roomListGameobject in roomListGameobjects.Values)
        {
            Destroy(roomListGameobject);
        }

        roomListGameobjects.Clear();
    }

    #endregion

    #region Public Methods
    public void ActivatePanel(string panelToBeActivated)
    {
        if (panelToBeActivated != null)
        {
            GameOptions_UI_Panel.SetActive(panelToBeActivated.Equals(GameOptions_UI_Panel.name));
            CreateRoom_UI_Panel.SetActive(panelToBeActivated.Equals(CreateRoom_UI_Panel.name));
            InsideRoom_UI_Panel.SetActive(panelToBeActivated.Equals(InsideRoom_UI_Panel.name));
            RoomList_UI_Panel.SetActive(panelToBeActivated.Equals(RoomList_UI_Panel.name));
            JoinRandomRoom_UI_Panel.SetActive(panelToBeActivated.Equals(JoinRandomRoom_UI_Panel.name));
        }
    }

    #endregion


    #region Application Pause

    private bool wasPaused;

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            wasPaused = true;
            HandlePause();
        }
        else
        {
            if (wasPaused)
            {
                wasPaused = false;
                HandleResume();
            }
        }
    }

    private void HandlePause()
    {
        PhotonNetwork.Disconnect();
    }

    private void HandleResume()
    {
        SceneManager.LoadScene("LobbyScene");
        //PhotonNetwork.ConnectUsingSettings();
    }
    #endregion
}
