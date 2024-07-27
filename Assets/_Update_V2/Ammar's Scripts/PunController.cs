using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PunController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameSceneManager gameManager;

    public int positionIndex;

    private TPSShooter.PlayerBehaviour playerBehaviour;

    void Awake()
    {
        // Find the GameManager in the scene
        gameManager = FindObjectOfType<GameSceneManager>();
    }


    [ContextMenu("Add Player")]
    public void AddToGameManagerList()
    {
        // Send the player's name instead of GameObject reference
        gameManager.photonView.RPC("AddPlayerToList", RpcTarget.All, PhotonNetwork.NickName);

        if(PhotonNetwork.CurrentRoom.PlayerCount < 2 && PhotonNetwork.CurrentRoom.PlayerCount > 0)
        {
            playerBehaviour = FindObjectOfType<TPSShooter.PlayerBehaviour>();

            playerBehaviour.getPunController().AddToGameManagerList();
        }
    }

    // Example method to remove this player from the GameManager's list
    public void RemoveFromGameManagerList()
    {
        // Send the player's name instead of GameObject reference
        gameManager.photonView.RPC("RemoveAndRearrange", RpcTarget.All, PhotonNetwork.NickName);
    }

    [PunRPC]
    public void ReceivePositionIndex(int index)
    {
        // Store the received index
        positionIndex = index;

        // Now you can use the index as needed
        Debug.Log("Received index: " + positionIndex);
    }

    [ContextMenu("Request Position")]
    public void RequestPosition()
    {
        photonView.RPC("RequestPositionIndex", RpcTarget.MasterClient, PhotonNetwork.NickName);
    }

    [PunRPC]
    private void RequestPositionIndex(string playerName)
    {
        // Find the GameManager in the scene
        GameSceneManager gameManager = GameObject.FindObjectOfType<GameSceneManager>();

        // Call the RemoveAndRearrange RPC method on the GameManager and receive the index
        int index = gameManager.RemoveAndRearrange(playerName);

        // Send the index back to the player
        photonView.RPC("ReceivePositionIndex", RpcTarget.All, index);
    }
}
