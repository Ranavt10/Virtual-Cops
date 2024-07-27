using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class MinimapCameraSetup : MonoBehaviourPunCallbacks
{
    public GameObject mySign;
    public GameObject EnemySign;
    public Camera minimapCamera; // Reference to the minimap camera
    public RawImage minimapRawImage; // Reference to the RawImage displaying the minimap

    public RenderTexture minimapRenderTexture;

    private bool shootStart = false;

    private Coroutine miniMapEnemyCo;

    [SerializeField]
    private TPSShooter.PlayerBehaviour playerBehaviour;

    [SerializeField]
    private GameObject localPlayer;

    public GameObject playerIconPrefab; // Player icon prefab for minimap
    private Dictionary<int, GameObject> playerIcons = new Dictionary<int, GameObject>(); // Dictionary to store player icons
    private GameObject localPlayerIcon;

    void Start()
    {
        if (photonView.IsMine)
        {
            InitializeMinimap();
            localPlayerIcon = PhotonNetwork.Instantiate(playerIconPrefab.name,minimapRawImage.transform.position, minimapRawImage.transform.rotation);
            localPlayerIcon.transform.SetParent(minimapRawImage.transform);
            localPlayerIcon.transform.eulerAngles = new Vector3(0, 0, 0);
            localPlayerIcon.transform.localScale = Vector3.one;
            localPlayerIcon.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            localPlayerIcon.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            localPlayerIcon.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            localPlayerIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.5f, 0.5f);
            localPlayerIcon.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            localPlayerIcon.SetActive(true);
            //Invoke(nameof(setImage), 2f);
            //playerIcons.Add(PhotonNetwork.LocalPlayer.ActorNumber, localPlayerIcon);
        }/*

        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            InstantiatePlayerIcon(player);
        }*/
    }

    [ContextMenu("Set Image")]
    private void setImage()
    {
        localPlayerIcon.transform.localPosition = new Vector3(0, 0, 0);
    }

    void Update()
    {
        // Update minimap icons for all players
        //UpdatePlayerIcons();
    }

    void InstantiatePlayerIcon(Photon.Realtime.Player player)
    {
        if (!playerIcons.ContainsKey(player.ActorNumber))
        {
            localPlayerIcon = PhotonNetwork.Instantiate(playerIconPrefab.name, minimapRawImage.transform.position, minimapRawImage.transform.rotation);
            localPlayerIcon.transform.SetParent(minimapRawImage.transform);
            localPlayerIcon.transform.localPosition = new Vector3(0, 0, 0);
            localPlayerIcon.transform.eulerAngles = new Vector3(0, 0, 0);
            localPlayerIcon.transform.localScale = Vector3.one;
            localPlayerIcon.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            localPlayerIcon.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            localPlayerIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.5f, 0.5f);
            localPlayerIcon.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            //localPlayerIcon.transform.localPosition = new Vector3(0, 0, 1.7f);
            localPlayerIcon.SetActive(true);
            //Invoke(nameof(setImage), 2f);
            //playerIcons.Add(player.ActorNumber, playerIcon);
        }
    }

    /*void UpdatePlayerIcons()
    {
        // Update position of player icons
        foreach (var entry in playerIcons)
        {
            int playerId = entry.Key;
            GameObject playerIcon = entry.Value;
            Photon.Realtime.Player player = PhotonNetwork.CurrentRoom.GetPlayer(playerId);

            if (player != null)
            {
                Vector3 playerPosition = GetPlayerPosition(player);
                playerIcon.transform.position = playerPosition;
            }
        }
    }*/

    void UpdatePlayerIcons()
    {
        // Update position of player icons
        foreach (var entry in playerIcons)
        {
            int playerId = entry.Key;
            GameObject playerIcon = entry.Value;
            Photon.Realtime.Player player = PhotonNetwork.CurrentRoom.GetPlayer(playerId);

            if (player != null)
            {
                Vector3 playerPosition = GetPlayerPosition(player);
                playerIcon.transform.position = playerPosition;

                // Update position of local player's icon as well
                if (player.IsLocal)
                {
                    localPlayerIcon.transform.position = playerPosition;
                }
            }
        }
    }

    Vector3 GetPlayerPosition(Photon.Realtime.Player player)
    {
        // Get the PhotonView of the player
        PhotonView playerPhotonView = player.TagObject as PhotonView;

        // Check if the PhotonView exists and is valid
        if (playerPhotonView != null)
        {
            // If the player is local, return the position of the local player's GameObject
            if (player.IsLocal)
            {
                return localPlayerIcon.transform.position;
            }
            else
            {
                // Return the position of the player's GameObject
                return playerPhotonView.gameObject.transform.position;
            }
        }
        else
        {
            // If PhotonView is not available or valid, return a default position
            Debug.LogWarning("PhotonView not found or invalid for player: " + player.ActorNumber);
            return Vector3.zero; // Return a default position (you can modify this as needed)
        }
    }
    // Called when remote player leaves the room
    /*public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        // Remove player icon from minimap when they leave
        int playerId = otherPlayer.ActorNumber;
        if (playerIcons.ContainsKey(playerId))
        {
            Destroy(playerIcons[playerId]);
            playerIcons.Remove(playerId);
        }
    }*/

    /*// Called when a new player joins the room
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        // Instantiate icon for the new player
        InstantiatePlayerIcon(newPlayer);
    }*/

    void InitializeMinimap()
    {
        minimapRenderTexture = new RenderTexture(256, 256, 16, UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_UNorm);
        if (minimapCamera && minimapRawImage)
        {
            minimapCamera.targetTexture = minimapRenderTexture;
            minimapRawImage.texture = minimapRenderTexture;
        }
        else
        {
            Debug.LogError("Minimap components are not properly assigned.");
        }
    }
    /*void Start()
    {
        localPlayer = gameObject;
        
        if (photonView.IsMine)
        {
            InitializeMinimap();
            *//*photonView.RPC(nameof(ActivateMySign), RpcTarget.AllBuffered);*//*
            //mySign.SetActive(true);
        }
        else
        {
            *//*minimapCamera.enabled = false;
            minimapRawImage.enabled = false;*//*

        }
    }

    void InitializeMinimap()
    {
        minimapRenderTexture = new RenderTexture(256, 256, 16, UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_UNorm);
        if (minimapCamera && minimapRawImage)
        {
            minimapCamera.targetTexture = minimapRenderTexture;
            minimapRawImage.texture = minimapRenderTexture;
        }
        else
        {
            Debug.LogError("Minimap components are not properly assigned.");
        }
    }

    [PunRPC]
    public void ShowEnemyOnMinimap(int playerID)
    {
        GameObject player = PhotonView.Find(playerID).gameObject;
        if (player != null)
        {
            ActiveState(player);
        }
        else
        {
            Debug.LogError("Player not found for playerID: " + playerID);
        }
    }

    private Coroutine _co;
    private void ActiveState(GameObject player)
    {
        var minimapSetup = player.GetComponent<MinimapCameraSetup>();
        if (minimapSetup != null)
        {
            minimapSetup.EnemySign.SetActive(true);
            Debug.Log("Minimap Camera");

            if (_co != null)
            {
                StopCoroutine(_co);
                _co = null;
            }

            _co = StartCoroutine(SetPlayerIcon(minimapSetup));
        }
    }

    private IEnumerator SetPlayerIcon(MinimapCameraSetup minimapSetup)
    {
        yield return new WaitForSeconds(2f);
        minimapSetup.EnemySign.SetActive(false);
    }

    public void FireWeapon()
    {
        photonView.RPC(nameof(ShowEnemyOnMinimap), RpcTarget.AllBuffered, photonView.ViewID);
    }

    [PunRPC]
    public void ActivateMySign()
    {
        mySign.SetActive(true);  // Activate mySign for other players
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        *//*if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
        }*//*
    }

    void OnDestroy()
    {
        if (minimapRenderTexture != null)
        {
            minimapRenderTexture.Release();
            Destroy(minimapRenderTexture);
        }
    }*/

    /*void Start()
    {
        localPlayer = gameObject;
        if (photonView.IsMine)
        {
            // Only initialize the minimap for the local player
            InitializeMinimap();
            mySign.SetActive(true);
        }
        else
        {
            // Disable the minimap components for non-local players
            minimapCamera.enabled = false;
            minimapRawImage.enabled = false;
        }
    }

    void InitializeMinimap()
    {
        // Create a unique RenderTexture for the minimap
        minimapRenderTexture = new RenderTexture(256, 256, 16, UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_UNorm); // Adjust the resolution as needed

        // Ensure the minimap camera and raw image are properly set up
        if (minimapCamera && minimapRawImage)
        {
            // Assign the render texture to the minimap camera
            minimapCamera.targetTexture = minimapRenderTexture;

            // Assign the render texture to the raw image
            minimapRawImage.texture = minimapRenderTexture;

            // Adjust the camera position and rotation to match the player's position
            *//*minimapCamera.transform.SetParent(transform);*/
    /*minimapCamera.transform.localPosition = new Vector3(0, 10, 0); // Adjust height as needed
    minimapCamera.transform.localRotation = Quaternion.Euler(90, 0, 0); // Pointing downwards*//*
}
else
{
    Debug.LogError("Minimap components are not properly assigned.");
}
}

[PunRPC]
public void ShowEnemyOnMinimap(int playerID)
{
GameObject player = PhotonView.Find(playerID).gameObject;

ActiveState(player);
//if (!photonView.IsMine)
{
    //mySign.SetActive(false);
    //EnemySign.SetActive(true);
    *//*if (!shootStart)
    {
        Debug.Log("NotifyFiring called on a remote client.");
        shootStart = true;
        Debug.Log("In Sign Activation");

    }*/

    /*CancelInvoke(nameof(HideEnemyIndicator));
    Invoke(nameof(HideEnemyIndicator), 2f);*/

    /*{
        if (miniMapEnemyCo == null)
        {
            Debug.Log("Starting CheckForFiringCoroutine.");
            miniMapEnemyCo = StartCoroutine(ShowEnemyIconCoroutine());
        }
    }*//*
}
}

private Coroutine _co;
private void ActiveState(GameObject player)
{
*//*if(player != localPlayer)*//*
{
    if (player.GetComponent<MinimapCameraSetup>() != null)
    {
        player.GetComponent<MinimapCameraSetup>().EnemySign.SetActive(true);
        Debug.Log("Minimap Camera");

        if(_co != null)
        {
            StopCoroutine(_co);
            _co = null;
        }

        StartCoroutine(setPlayerIcon(player));
    }

    else if (player.GetComponent<TPSShooter.PlayerBehaviour>() != null)
    {
        player.GetComponent<TPSShooter.PlayerBehaviour>().minimapCamera.EnemySign.SetActive(true);
        Debug.Log("Player Behavior");
    }
}
}

private IEnumerator setPlayerIcon(GameObject player)
{
yield return new WaitForSeconds(2f);
if(player.GetComponent<MinimapCameraSetup>() != null)
{
    player.GetComponent<MinimapCameraSetup>().EnemySign.SetActive(false);
}
}
*//*private IEnumerator ShowEnemyIconCoroutine()
{
    Debug.Log("CheckForFiringCoroutine started.");
    while (true)
    {
        yield return new WaitForSeconds(1f);
        if (shootStart && !playerBehaviour.IsFire)
        {
            Debug.Log("Enemy has fired, displaying icon.");
            // Instantiate the red icon at the enemy's position
            shootStart = false;
            Debug.Log("In Sign DeActivation");
            yield return new WaitForSeconds(0.5f);
            mySign.SetActive(true);
            EnemySign.SetActive(false);
        }
    }

}*//*

void HideEnemyIndicator()
{
    shootStart = false;
    mySign.SetActive(true);
    EnemySign.SetActive(false);
}

public void FireWeapon()
{
    photonView.RPC(nameof(ShowEnemyOnMinimap), RpcTarget.AllBuffered, photonView.ViewID);
    *//*if (photonView.IsMine)
    {
        // Notify other players that this player has fired

    }*//*
}

void OnDestroy()
{
    // Clean up the render texture when the object is destroyed
    if (minimapRenderTexture != null)
    {
        minimapRenderTexture.Release();
        Destroy(minimapRenderTexture);
    }
}*/
}
