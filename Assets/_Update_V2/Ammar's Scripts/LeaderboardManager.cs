using System.Collections;
using Photon.Pun;
using System;
using System.Collections.Generic;

public class LeaderboardManager : MonoBehaviourPunCallbacks
{
    [Serializable]
    public class PlayerData
    {
        public string playerName;
        public string rank; // New field to store player's rank in the format "X/Y"
    }

    public List<PlayerData> players = new List<PlayerData>();
    private int totalPlayersInRoom = 20; // Assuming there are initially 20 players in the room

    private void Awake()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    // Update player statistics when a player is killed
    public void PlayerKilled(string playerName)
    {
        PlayerData player = players.Find(p => p.playerName == playerName);
        if (player != null)
        {
            int playerRank = players.Count + 1; // Rank is the total number of players alive
            player.rank = playerRank + "/" + totalPlayersInRoom;
            // Optionally, you can update the UI here to reflect the new rank
        }
    }

    // Called when a new player joins the room
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        DebugCustom.Log("Its Called");
        PlayerData player = new PlayerData();
        player.playerName = newPlayer.NickName;
        player.rank = (players.Count + 1) + "/" + totalPlayersInRoom;
        players.Add(player);
        // Optionally, you can update the UI here to reflect the new player and rank
    }

    // Called when a player leaves the room
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        PlayerData player = players.Find(p => p.playerName == otherPlayer.NickName);
        if (player != null)
        {
            players.Remove(player);
            // Optionally, you can update the UI here to remove the player from the leaderboard
        }
    }

    // Get a player's rank
    public string GetPlayerRank(string playerName)
    {
        PlayerData player = players.Find(p => p.playerName == playerName);
        if (player != null)
        {
            return player.rank;
        }
        return "N/A"; // Player not found
    }

    // Determine the winning leader based on specific criteria
    public string GetWinningLeader()
    {
        // Find the player with rank "1/totalPlayersInRoom"
        PlayerData winningPlayer = players.Find(p => p.rank == "1/" + totalPlayersInRoom);
        if (winningPlayer != null)
        {
            return winningPlayer.playerName;
        }
        return "No winner"; // Return default value if no winner is found
    }
}
