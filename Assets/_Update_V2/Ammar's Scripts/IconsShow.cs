using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class IconsShow : MonoBehaviourPun, IPunObservable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Only synchronize the guns if this is the owner of the player GameObject
        if (photonView.IsMine)
        {
            // Update the positions and rotations of the guns
            foreach (Transform gunTransform in transform)
            {
                photonView.RPC("SyncIconTransform", RpcTarget.Others, gunTransform.position, gunTransform.rotation);
            }
        }
    }

    [PunRPC]
    private void SyncIconTransform(Vector3 position, Quaternion rotation)
    {
        // Update the position and rotation of the guns for other players
        foreach (Transform gunTransform in transform)
        {
            gunTransform.position = position;
            gunTransform.rotation = rotation;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // Implement serialization if needed
    }
}
