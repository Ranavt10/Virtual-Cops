using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class GameSceneManager : MonoBehaviourPunCallbacks
{
    public List<string> playerNames = new List<string>();
    public List<GameObject> playerObjects = new List<GameObject>();

    public List<int> eliminationOrder;
    private bool gameEnded = false;

    MobileFPSGameManager mobileFps;
    public TextMeshProUGUI totalUsersText;

    private void Start()
    {
        /*getTotalPlayersInTheLobby();*/
        mobileFps = FindObjectOfType<MobileFPSGameManager>();
#if (UNITY_ANDROID || UNITY_IOS)
        StartCoroutine(checkFirstPosition());
#endif
    }

    private bool setChecker;
    private IEnumerator checkFirstPosition()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.04f);
            if (totalUsersText)
            {
                if(PhotonNetwork.CurrentRoom != null)
                    totalUsersText.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
            }
                
#if !UNITY_EDITOR
            if (!setChecker)
            {
                if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
                {
                    mobileFps.exitButton.gameObject.SetActive(false);
                    for (int i = 0; i < mobileFps.Canvases.Length; i++)
                    {
                        mobileFps.Canvases[i].SetActive(false);
                    }
                    setChecker = true;
                }
            }

            if (setChecker)
            {
                yield return new WaitForSeconds(2f);
                if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
                {
                    yield return new WaitForSeconds(1f);
                    TPSShooter.PlayerBehaviour player = FindObjectOfType<TPSShooter.PlayerBehaviour>();
                    if (player != null)
                        player.Die();
                }
                setChecker = false;
            }
#endif
        }

    }

    public bool lastTwoLeft;

    public void setLastTwoLeft()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount <= 2)
        {
            lastTwoLeft = true;
        }
    }

    [PunRPC]
    public void AddPlayerToList(string playerName)
    {
        playerNames.Add(playerName);
    }

    [PunRPC]
    public int RemoveAndRearrange(string personToRemove)
    {
        int indexToRemove = FindIndexOfGameObject(personToRemove);

        if (indexToRemove == -1)
        {
            Debug.LogError("GameObject not found in the list.");
            return -1;
        }

        Debug.Log("Index is:" + indexToRemove);

        return indexToRemove;
    }

    [PunRPC]
    // Function to find the index of a GameObject in the list
    private int FindIndexOfGameObject(string person)
    {
        return playerNames.IndexOf(person);
    }

    [PunRPC]
    public void UpdateList(List<string> updatedList)
    {
        playerNames = updatedList;
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
    public void PlayerLeft(int viewID)
    {
        Debug.Log("Player Left Called");
        if (!eliminationOrder.Contains(viewID))
        {
            Debug.Log("Player Id is:" + viewID);
            eliminationOrder.Add(viewID);
            eliminationOrder.Reverse();
        }
    }

    [PunRPC]
    public void PlayerKilled(int viewID)
    {
        if (!eliminationOrder.Contains(viewID))
        {
            Debug.Log("Player Id is:" + viewID);
            eliminationOrder.Add(viewID);
            DetermineFinalRanks();
            CheckGameEnd();
        }
    }

    void CheckGameEnd()
    {
        /*if (eliminationOrder.Count >= PhotonNetwork.PlayerList.Length - 1 && !gameEnded)
        {
            gameEnded = true;
            OnGameEnd();
        }*/
        if (eliminationOrder.Count >= PhotonNetwork.PlayerList.Length - 1 && !gameEnded)
        {
            gameEnded = true;
            if (PhotonNetwork.PlayerList.Length - eliminationOrder.Count == 1)
            {
                // Only one player remains, they are the winner
                int lastStandingViewID = GetLastStandingPlayerViewID();
                if (lastStandingViewID != -1)
                {
                    // Call Die method for the last standing player
                    TPSShooter.PlayerBehaviour lastStandingPlayer = PhotonView.Find(lastStandingViewID).GetComponent<TPSShooter.PlayerBehaviour>();
                    lastStandingPlayer.Die();
                }
            }
            else
            {
                // More than one player remains, determine final ranks
                OnGameEnd();
            }
        }
    }

    int GetLastStandingPlayerViewID()
    {
        /*foreach (var player in PhotonNetwork.PlayerList)
        {
            PhotonView[] photonViews = player.TagObject as PhotonView[];
            if (photonViews != null)
            {
                foreach (var pv in photonViews)
                {
                    return pv.ViewID;*//*
                    if (!eliminationOrder.Contains(pv.ViewID))
                    {
                        
                    }*//*
                }
            }
        }
        return -1; // No last standing player found*/

        if (PhotonNetwork.PlayerList.Length == 1)
        {
            PhotonView[] photonViews = PhotonNetwork.PlayerList[0].TagObject as PhotonView[];
            if (photonViews != null && photonViews.Length > 0)
            {
                return photonViews[0].ViewID;
            }
        }

        // If there are multiple players remaining or no players, return -1
        return -1;
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
        eliminationOrder.Reverse(); // First killed will be last in the rank
        for (int i = 0; i < eliminationOrder.Count; i++)
        {
            int viewID = eliminationOrder[i];
            int rank = i;
            AssignRank(viewID, rank);
        }

        // The last remaining player is the winner
        foreach (var player in PhotonNetwork.PlayerList)
        {
            PhotonView[] photonViews = player.TagObject as PhotonView[];
            if (photonViews != null)
            {
                foreach (var pv in photonViews)
                {
                    if (!eliminationOrder.Contains(pv.ViewID))
                    {
                        AssignRank(pv.ViewID, 0);
                    }
                }
            }
        }
    }

    void AssignRank(int viewID, int rank)
    {
        PhotonView targetPhotonView = PhotonView.Find(viewID);
        if (targetPhotonView != null)
        {
            Debug.Log($"Assigning rank {rank} to PhotonView with ViewID {viewID}");
            targetPhotonView.RPC("ReceiveRank", RpcTarget.All, rank);
        }
        else
        {
            Debug.LogError($"PhotonView not found for ViewID: {viewID}");
        }
    }


    PhotonView FindPlayerPhotonView(int playerId)
    {
        TPSShooter.PlayerBehaviour[] players = FindObjectsOfType<TPSShooter.PlayerBehaviour>();
        foreach (var player in players)
        {
            if (player.photonView.Owner.ActorNumber == playerId)
            {
                Debug.Log($"Found PhotonView for playerId: {playerId} on {player.gameObject.name}");
                return player.photonView;
            }
        }

        Debug.LogError($"PhotonView not found for playerId: {playerId}");
        return null;
    }

    public int getTotalPlayersInTheLobby()
    {
        return PhotonNetwork.CurrentRoom.PlayerCount;
    }
}
